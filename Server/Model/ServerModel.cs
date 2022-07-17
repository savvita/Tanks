using Connection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
        private List<SessionModel>? sessions;

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

        /// <summary>
        /// Raise when list of registered users changed
        /// </summary>
        public event Action? UsersChanged;

        /// <summary>
        /// Raise when list of the sessions changed
        /// </summary>
        public event Action? SessionsChanged;
        #endregion

        public ServerModel()
        {
            Clients = new ObservableCollection<ClientModel>();
            sessions = new List<SessionModel>();

            users.UserAdded += () => { UsersChanged?.Invoke(); };

            AddNewSession();
        }

        /// <summary>
        /// Get all the registered users
        /// </summary>
        /// <returns>List of the users</returns>
        public List<UserModel>? GetAllUsers() => users.GetAllUsers();

        /// <summary>
        /// Get all the game sessions
        /// </summary>
        /// <returns>List of sessions</returns>
        public List<SessionModel>? GetAllSessions() => sessions;

        /// <summary>
        /// Add new game session
        /// </summary>
        private void AddNewSession()
        {
            SessionModel session = new SessionModel
            {
                Battlefield = new BattlefieldModel()
            };
            session.Battlefield.Lost += OnLost;
            session.Battlefield.Won += OnWon;

            sessions?.Add(session);
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
                            clients?.Add(client);

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
                        ClientModel? activeClient = GetClientByName(cols[0]);

                        if (activeClient != null)
                        {
                            SocketClient.SendMessage(stream, SocketClient.FailCode);
                        }
                        else
                        {
                            UserModel? user = users.GetUserByName(cols[0]);

                            if (user != null)
                            {
                                SocketClient.SendMessage(stream, SocketClient.OkCode);
                                ClientModel client = new ClientModel(user, tcpClient, this);
                                clients?.Add(client);

                                AcceptClient(cols[0], stream);

                                client?.Proceed();
                            }
                            else
                            {
                                SocketClient.SendMessage(stream, SocketClient.FailCode);
                            }
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
            int games = users.GetTotalGames(name);
            SocketClient.SendMessage(stream, games.ToString());

            int won = users.GetWonGames(name);
            SocketClient.SendMessage(stream, won.ToString());
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
                if(client.Session == null)
                {
                    return;
                }

                client.Session.Battlefield?.Tankmen.Remove(client.Tankman);
                client.Session.Clients?.Remove(client);
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
        /// Send the message to all the users
        /// </summary>
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

        /// <summary>
        /// Receive a tank
        /// </summary>
        /// <param name="stream">Network stream to receive tank from</param>
        /// <returns>Tank</returns>
        public TankModel? ReceiveTankModel(NetworkStream? stream)
        {
            if(stream == null)
            {
                return null;
            }

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
        /// <param name="client">Client to be added</param>
        /// <param name="width">Width of the field</param>
        /// <param name="height">Height of the field</param>
        public void JoinBattle(ClientModel client, int width, int height)
        {
            if(sessions == null)
            {
                return;
            }

            SessionModel? session = sessions.LastOrDefault();

            if (session == null || session.Clients == null)
            {
                return;
            }

            if (session.Clients.Count >= GlobalSettings.MaxPlayers)
            {
                AddNewSession();
                session = sessions.LastOrDefault();
            }

            if(session!.Battlefield == null || session.Battlefield.IsFinished)
            {
                AddNewSession();
                session = sessions.LastOrDefault();
            }

            if(session!.Clients == null)
            {
                session.Clients = new List<ClientModel>();
            }

            session.Clients.Add(client);
            client.Session = session;

            session.JoinBattle(client.Tankman, width, height);

            SessionsChanged?.Invoke();
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
    }
}

