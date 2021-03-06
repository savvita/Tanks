using Connection;
using System;
using System.Net.Sockets;
using TankLibrary;

namespace Server.Model
{
    public class ClientModel
    {
        private readonly ServerModel server;
        private readonly TcpClient? tcpClient;
        private readonly UserModel user;

        public TankManModel Tankman { get; set; }

        /// <summary>
        /// Unique name of the client
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Network stream of this client
        /// </summary>
        public NetworkStream? Stream { get; private set; }

        /// <summary>
        /// Game session
        /// </summary>
        public SessionModel? Session { get; set; }

        public ClientModel(UserModel user, TcpClient tcpClient, ServerModel server)
        {
            this.user = user;
            this.Name = user.Login;
            this.server = server;

            Tankman = new TankManModel()
            {
                Name = user.Login
            };

            
            this.tcpClient = tcpClient;

            try
            {
                this.Stream = tcpClient.GetStream();
            }
            catch { }

            Tankman.Tank = new TankModel();


            if (Tankman.Tank != null)
            {
                Tankman.Tank.Health = GlobalSettings.Health;
                Tankman.Tank.Damage = GlobalSettings.Damage;
            }
        }
        

        /// <summary>
        /// Proceed the client
        /// </summary>
        public void Proceed()
        {
            if(Stream == null)
            {
                return;
            }

            string msg;

            do
            {
                try
                {
                    msg = SocketClient.ReceiveMessage(Stream);

                    if(msg.Equals(SocketClient.ShopCode))
                    {
                        HandleShopCommand();
                    }

                    else if(msg.Equals(SocketClient.StartCode))
                    {
                        HandleStartCommand();
                    }

                    else if(msg.Equals(SocketClient.LeaveCode))
                    {
                        Session?.LeaveBattle(this);
                    }

                    else if (msg.Equals(SocketClient.StopCode))
                    {
                        break;
                    }

                    else if (msg != string.Empty && Session != null)
                    {
                        string? res = Session.HandleBattle(msg);
                        Session.BroadcastMessage(res);
                    }

                }
                catch (Exception ex)
                {
                    server.OnGotError($"[{DateTime.Now}] {Name} : {ex.Message}");
                    break;
                }
            } while (true);

            server.RemoveClient(Name);

            Close();
        }

        /// <summary>
        /// Handle command to start battle
        /// </summary>
        private void HandleStartCommand()
        {
            user.TotalGames++;

            int width = int.Parse(SocketClient.ReceiveMessage(Stream));
            int height = int.Parse(SocketClient.ReceiveMessage(Stream));

            TankModel? tank = server.ReceiveTankModel(Stream);

            if (tank != null && Tankman.Tank != null)
            {
                tank.Health = Tankman.Tank.Health;
                tank.Damage = Tankman.Tank.Damage;
                tank.IsAlive = true;
            }

            Tankman.Tank = tank;

            server.JoinBattle(this, width, height);
        }

        /// <summary>
        /// Handle command to shopping
        /// </summary>
        private void HandleShopCommand()
        {
            if (Tankman.Tank != null)
            {
                string health = SocketClient.ReceiveMessage(Stream);
                Tankman.Tank.Health += int.Parse(health);

                string damage = SocketClient.ReceiveMessage(Stream);
                Tankman.Tank.Damage += int.Parse(damage);

                string coins = SocketClient.ReceiveMessage(Stream);
                user.Coins -= int.Parse(coins);

                SocketClient.SendMessage(Stream, user.Coins.ToString());
                SocketClient.SendMessage(Stream, Tankman.Tank.Health.ToString());
                SocketClient.SendMessage(Stream, Tankman.Tank.Damage.ToString());
            }
        }

        /// <summary>
        /// Close all connections
        /// </summary>
        public void Close()
        {
            tcpClient?.Close();
            Stream?.Close();
        }
    }
}

