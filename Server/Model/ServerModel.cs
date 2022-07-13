using Connection;
using System;
using System.Collections.Generic;
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

                    ClientModel client = new ClientModel(tcpClient, this);

                    Thread thread = new Thread(client.Proceed)
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
    }
}

