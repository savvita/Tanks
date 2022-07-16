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

        private TankManModel tankman;

        /// <summary>
        /// Unique name of the client
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Network stream of this client
        /// </summary>
        public NetworkStream? Stream { get; private set; }

        private UserModel user;

        public ClientModel(UserModel user, TcpClient tcpClient, ServerModel server)
        {
            this.user = user;
            this.Name = user.Login;
            this.server = server;

            tankman = new TankManModel()
            {
                Name = user.Login
            };

            
            this.tcpClient = tcpClient;

            try
            {
                this.Stream = tcpClient.GetStream();
            }
            catch
            {
            }

            //tankman.Tank = server.ReceiveTankModel(Stream);
            tankman.Tank = new TankModel();


            if (tankman.Tank != null)
            {
                tankman.Tank.Health = GlobalSettings.Health;
                tankman.Tank.Damage = GlobalSettings.Damage;
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
                        if (tankman.Tank != null)
                        {
                            string health = SocketClient.ReceiveMessage(Stream);
                            tankman.Tank.Health += int.Parse(health);

                            string damage = SocketClient.ReceiveMessage(Stream);
                            tankman.Tank.Damage += int.Parse(damage);

                            string coins = SocketClient.ReceiveMessage(Stream);
                            user.Coins -= int.Parse(coins);

                            SocketClient.SendMessage(Stream, user.Coins.ToString());
                            SocketClient.SendMessage(Stream, tankman.Tank.Health.ToString());
                            SocketClient.SendMessage(Stream, tankman.Tank.Damage.ToString());
                        }
                    }

                    else if(msg.Equals(SocketClient.StartCode))
                    {
                        server.AddClient(this);

                        int width = int.Parse(SocketClient.ReceiveMessage(Stream));
                        int height = int.Parse(SocketClient.ReceiveMessage(Stream));

                        //tankman.Tank = server.ReceiveTankModel(Stream);

                        TankModel? tank = server.ReceiveTankModel(Stream);
                        if(tank != null)
                        {
                            tank.Health = tankman.Tank.Health;
                            tank.Damage = tankman.Tank.Damage;
                            tank.IsAlive = true;
                        }

                        tankman.Tank = tank;

                        server.JoinBattle(tankman, width, height);
                        server.SendTanks();
                    }

                    else if (msg.Equals(SocketClient.StopCode))
                    {
                        break;
                    }

                    else if (msg != string.Empty)
                    {
                        string? res = server.HandleBattle(msg);
                        server.BroadcastMessage(Name, res);
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
        /// Close all connections
        /// </summary>
        public void Close()
        {
            tcpClient?.Close();
            Stream?.Close();
        }
    }
}

