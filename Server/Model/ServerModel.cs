using Connection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading;
using TankLibrary;

namespace Server.Model
{
    public class ServerModel : INotifyPropertyChanged
    {
        private TcpListener? listener;
        private Users users = new Users();
        //private BattlefieldModel battlefield;

        /// <summary>
        /// List of clients connected to the server
        /// </summary>
        private ObservableCollection<ClientModel>? clients;
        public ObservableCollection<ClientModel>? Clients
        {
            get => clients;
            private set
            {
                clients = value;
                OnPropertyChanged(nameof(Clients));
            }
        }

        private ObservableCollection<SessionModel>? sessions;
        public ObservableCollection<SessionModel>? Sessions
        {
            get => sessions;
            private set
            {
                sessions = value;
                OnPropertyChanged(nameof(Sessions));
            }
        }

        #region Events
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Raise when during the connection was an error
        /// </summary>
        public event Action<string>? GotError;

        /// <summary>
        /// Raise when the server is started to listening
        /// </summary>
        public Action<string>? ServerStarted;

        /// <summary>
        /// Raise when new client is connected to the server
        /// </summary>
        public Action<ClientModel>? ClientConnected;

        /// <summary>
        /// Raise when the client is disconnected
        /// </summary>
        public Action<ClientModel>? ClientDisconnected;
        #endregion

        public ServerModel()
        {
            Clients = new ObservableCollection<ClientModel>();
            Sessions = new ObservableCollection<SessionModel>();

            AddNewSession();
        }

        private void AddNewSession()
        {
            SessionModel session = new SessionModel();
            session.Battlefield = new BattlefieldModel();
            session.Battlefield.Lost += OnLost;
            session.Battlefield.Won += OnWon;

            Sessions?.Add(session);
        }

        /// <summary>
        /// Listen to the port
        /// </summary>
        public void Listen()
        {
            try
            {
                listener = new TcpListener(IPAddress.Any, SocketClient.Port);
                listener.Start();

                ServerStarted?.Invoke($"[{DateTime.Now}] Server started");

                do
                {
                    TcpClient tcpClient = listener.AcceptTcpClient();

                    Thread thread = new Thread((obj) => HandleClient(tcpClient))
                    {
                        IsBackground = true
                    };

                    thread.Start();

                } while (true);
            }
            catch (Exception ex)
            {
                OnGotError($"[{DateTime.Now}] {ex.Message}");
                Close();
            }
        }

        /// <summary>
        /// Handle client connection
        /// </summary>
        /// <param name="obj">TCP client</param>
        private void HandleClient(object? obj)
        {
            if (obj is TcpClient tcpClient)
            {
                try
                {
                    NetworkStream stream = tcpClient.GetStream();

                    string code = SocketClient.ReceiveMessage(stream);

                    if (code.Equals(SocketClient.RegistrationCode))
                    {
                        string authorization = SocketClient.ReceiveMessage(stream);

                        string[] cols = authorization.Split(',');

                        UserModel? user = users.RegisterUser(cols[0], cols[1]);

                        if (user != null)
                        {
                            SocketClient.SendMessage(stream, SocketClient.OkCode);
                            ClientModel client = new ClientModel(user, tcpClient, this);

                            AcceptClient(cols[0], stream);
                            client.Proceed();
                        }
                        else
                        {
                            SocketClient.SendMessage(stream, SocketClient.FailCode);
                        }
                    }
                    else if (code.Equals(SocketClient.AuthorizationCode))
                    {
                        string authorization = SocketClient.ReceiveMessage(stream);

                        string[] cols = authorization.Split(',');

                        UserModel? user = users.GetUserByName(cols[0]);

                        if (user != null)
                        {
                            SocketClient.SendMessage(stream, SocketClient.OkCode);
                            ClientModel client = new ClientModel(user, tcpClient, this);

                            AcceptClient(cols[0], stream);

                            client?.Proceed();
                        }
                        else
                        {
                            SocketClient.SendMessage(stream, SocketClient.FailCode);
                        }
                    }
                }
                catch (Exception ex)
                {
                    OnGotError($"[{DateTime.Now}] {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Acccept new client
        /// </summary>
        /// <param name="name">Name of the client</param>
        /// <param name="stream">Network stream of the new client</param>
        private void AcceptClient(string name, NetworkStream stream)
        {
            if (clients == null)
            {
                return;
            }


            int coins = users.GetCoins(name);
            SocketClient.SendMessage(stream, coins.ToString());
            SocketClient.SendMessage(stream, GlobalSettings.Health.ToString());
            SocketClient.SendMessage(stream, GlobalSettings.Damage.ToString());

            users.SetTotalGames(name);
        }

        /// <summary>
        /// Add new client to the list of active clients
        /// </summary>
        /// <param name="client">Client to add</param>
        public void AddClient(ClientModel client)
        {
            if (clients == null)
            {
                return;
            }

            clients.Add(client);
            ClientConnected?.Invoke(client);
        }

        /// <summary>
        /// Remove client from the list
        /// </summary>
        /// <param name="name">Name of client to remove</param>
        public void RemoveClient(string? name)
        {
            ClientModel? client = GetClientByName(name);

            if (client != null)
            {
               // battlefield.RemoveTankMan(name);
                clients!.Remove(client);
                ClientDisconnected?.Invoke(client);
            }
        }

        /// <summary>
        /// Get a client by its name
        /// </summary>
        /// <param name="name">Name of a client</param>
        /// <returns>Client</returns>
        private ClientModel? GetClientByName(string? name)
        {
            if(clients == null || name == null)
            {
                return null;
            }

            return clients.Where(x => x.Name != null && x.Name.Equals(name)).FirstOrDefault();
        }

        /// <summary>
        /// Send the message to all the users except for the original sender
        /// </summary>
        /// <param name="sender">Name of the sender</param>
        /// <param name="message">Message</param>
        public void BroadcastMessage(string? message)
        {
            if (clients == null || message == null)
            {
                return;
            }

            for (int i = 0; i < clients.Count; i++)
            {
                if (clients[i].Name == null)
                {
                    continue;
                }

                try
                {
                    SocketClient.SendMessage(clients[i].Stream, message);
                }
                catch (Exception ex)
                {
                    OnGotError($"[{DateTime.Now}] {clients[i].Name} : {ex.Message}");
                }

            }
        }

        /// <summary>
        /// Close all connections
        /// </summary>
        public void Close()
        {
            BroadcastMessage(SocketClient.StopCode);

            if (clients != null)
            {
                for (int i = 0; i < clients.Count; i++)
                {
                    clients[i].Close();
                }
            }

            users.SaveUsers();
            clients?.Clear();
            listener?.Stop();
        }

        public TankModel? ReceiveTankModel(NetworkStream stream)
        {
            string? msg = SocketClient.ReceiveMessage(stream);

            if(msg == null)
            {
                return null;
            }

            try
            {
                TankModel? tankModel = JsonSerializer.Deserialize<TankModel>(msg);
                return tankModel;
            }
            catch { }

            return null;
        }

        /// <summary>
        /// Add a client to the battle
        /// </summary>
        /// <param name="tankman">Tankman of the client</param>
        public void JoinBattle(ClientModel client, int width, int height)
        {
            if(Sessions == null)
            {
                return;
            }

            SessionModel? session = Sessions.LastOrDefault();

            if (session == null || session.Clients == null)
            {
                return;
            }

            if (session.Clients.Count >= GlobalSettings.MaxPlayers)
            {
                AddNewSession();
                session = Sessions.LastOrDefault();
            }

            //if (session == null || session.Clients.Count >= GlobalSettings.MaxPlayers)
            //{
            //    return;
            //}

            session.Clients.Add(client);
            client.Session = session;



            session.JoinBattle(client.Tankman, width, height);
        }

        ///// <summary>
        ///// Handle battle with new data
        ///// </summary>
        ///// <param name="msg">Data from the client</param>
        ///// <returns>New data</returns>
        //public string? HandleBattle(string msg)
        //{
        //    try
        //    {
        //        List<TankManModel>? tankmen = JsonSerializer.Deserialize<List<TankManModel>>(msg);

        //        if(tankmen != null)
        //        {
        //            battlefield.Tankmen = tankmen;
        //            battlefield.HandleBattle();

        //            string? res = JsonSerializer.Serialize<List<TankManModel>>(battlefield.Tankmen);

        //            return res;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        OnGotError($"[{DateTime.Now}] : {ex.Message}");
        //    }

        //    return null;
        //}

        //public void SendTanks()
        //{
        //    string? msg = JsonSerializer.Serialize<List<TankManModel>>(battlefield.Tankmen);
        //    BroadcastMessage("", msg);
        //}

        /// <summary>
        /// Raise an event GotError
        /// </summary>
        /// <param name="message">Error message</param>
        public void OnGotError(string message)
        {
            GotError?.Invoke(message);
        }

        private void OnLost(TankManModel tankman)
        {
            SendMessage(tankman.Name, SocketClient.LostCode);
        }

        private void OnWon(TankManModel tankman)
        {
            SendMessage(tankman.Name, SocketClient.WinCode);
            users.SetWinner(tankman.Name);
        }

        /// <summary>
        /// Send message from the server to the client
        /// </summary>
        /// <param name="name">Name of the client to send a message to</param>
        /// <param name="message">Message</param>
        public void SendMessage(string? name, string? message)
        {
            if(name == null)
            {
                return;
            }

            try
            {
                ClientModel? client = GetClientByName(name);

                if (client != null && message != null)
                {
                    SocketClient.SendMessage(client.Stream, message);
                }
            }
            catch { }
        }

        private void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}

