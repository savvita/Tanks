using Connection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server.Model
{
    public class ServerModel
    {
        private TcpListener? listener;
        private readonly int port;
        private readonly SocketClient chat = new SocketClient();

        /// <summary>
        /// List of clients connected to the server
        /// </summary>
        private readonly List<ClientModel> clients = new List<ClientModel>();
        public List<ClientModel> Clients
        {
            get => clients;
        }

        #region Events
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

        public ServerModel(int port = 8008)
        {
            this.port = port;
        }


        /// <summary>
        /// Listen to the port
        /// </summary>
        public void Listen()
        {
            try
            {
                listener = new TcpListener(IPAddress.Any, port);
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

                    
                    //string[] cols = authorization.Split(',');

                    string code = chat.ReceiveMessage(stream);
                    chat.SendMessage(stream, SocketClient.OkCode);

                    if (code.Equals(SocketClient.RegistrationCode))
                    {
                        string authorization = chat.ReceiveMessage(stream);

                        string[] cols = authorization.Split(',');

                        if (RegisterClient(cols[0], cols[1]))
                        {
                            chat.SendMessage(stream, SocketClient.OkCode);


                            ClientModel client = new ClientModel(tcpClient, this)
                            {
                                Name = cols[0]
                            };

                            users.SetTotal(client.Name);

                            client.Proceed();
                        }
                        else
                        {
                            chat.SendMessage(stream, SocketClient.FailCode);
                        }
                    }
                    else if (code.Equals(SocketClient.AuthorizationCode))
                    {
                        string authorization = chat.ReceiveMessage(stream);

                        string[] cols = authorization.Split(',');

                        if (AuthorizateClient(cols[0], cols[1]))
                        {
                            chat.SendMessage(stream, SocketClient.OkCode);

                            ClientModel client = new ClientModel(tcpClient, this)
                            {
                                Name = cols[0]
                            };

                            users.SetTotal(client.Name);

                            client.Proceed();
                        }
                        else
                        {
                            chat.SendMessage(stream, SocketClient.FailCode);
                        }
                    }
                }
                catch (Exception ex)
                {
                    OnGotError($"[{DateTime.Now}] {ex.Message}");
                }
            }
        }

        public void SetWin(ClientModel client)
        {
            users.SetWin(client.Name);
        }

        private Users users = new Users();

        private bool AuthorizateClient(string login, string password)
        {
            return users.IsUserRegistered(login, password);

        }

        private bool RegisterClient(string login, string password)
        {
            return users.RegisterUser(login, password);
        }

        public List<string?>? GetAllLastData()
        {
            if (Clients == null)
                return null;

            List<string?>? data = new List<string?>();
            for (int i = 0; i < Clients.Count; i++)
            {
                if(Clients[i].LastData != null)
                    data.Add(Clients[i].LastData);
            }

            return data;
        }

        /// <summary>
        /// Add new client model to the list of active clients
        /// </summary>
        /// <param name="client">Client to add</param>
        public void AddClient(ClientModel client)
        {
            clients.Add(client);

            ClientConnected?.Invoke(client);
        }

        /// <summary>
        /// Remove client from the list
        /// </summary>
        /// <param name="id">Id of client to remove</param>
        public void RemoveClient(string id)
        {
            ClientModel? client = GetClientById(id);

            if (client != null)
            {
                clients.Remove(client);
                ClientDisconnected?.Invoke(client);
            }
        }

        /// <summary>
        /// Get a client by its Id
        /// </summary>
        /// <param name="id">Id of a client</param>
        /// <returns>Client with specified Id</returns>
        public ClientModel? GetClientById(string id)
        {
            return clients.Where(x => x.Id.Equals(id)).FirstOrDefault();
        }

        public void BroadcastMessage(string senderId, string message)
        {
            for (int i = 0; i < clients.Count; i++)
            {
                if (!clients[i].Id.Equals(senderId))
                {
                    try
                    {
                         chat.SendMessage(clients[i].Stream, message);
                    }
                    catch (Exception ex)
                    {
                        OnGotError($"[{DateTime.Now}] {clients[i].Id} : {ex.Message}" + Environment.NewLine);
                    }
                }
            }
        }

        /// <summary>
        /// Close all connections
        /// </summary>
        public void Close()
        {
            BroadcastMessage("", SocketClient.STOP_CODE);
            for (int i = 0; i < clients.Count; i++)
            {
                clients[i].Close();
            }
            users.SaveUsers();
            clients.Clear();
            listener?.Stop();
        }

        /// <summary>
        /// Raise an event GotError
        /// </summary>
        /// <param name="message">Error message</param>
        public void OnGotError(string message)
        {
            GotError?.Invoke(message);
        }

        /// <summary>
        /// Send message from the server to the client
        /// </summary>
        /// <param name="id">Id of the client to send a message to</param>
        /// <param name="message">Message</param>
        public void SendMessage(string id, string? message)
        {
            try
            {
                ClientModel? client = GetClientById(id);

                if (client != null && message != null)
                {
                    chat.SendMessage(client.Stream, message);
                }
            }
            catch { }
        }
    }
}

