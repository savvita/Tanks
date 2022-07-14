using Connection;
using System;
using System.ComponentModel;
using System.Net.Sockets;
using System.Runtime.CompilerServices;

namespace Server.Model
{
    public class ClientModel : INotifyPropertyChanged
    {
        private readonly TcpClient tcpClient;
        private readonly SocketClient client = new SocketClient();
        private readonly ServerModel server;

        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Unique identifier of the client
        /// </summary>
        public string Id { get; private set; }

        /// <summary>
        /// Name of the client. May not be unique
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Network stream of this client
        /// </summary>
        public NetworkStream? Stream { get; private set; }

        public ClientModel(TcpClient tcpClient, ServerModel server)
        {
            Id = Guid.NewGuid().ToString();

            this.tcpClient = tcpClient;
            this.server = server;

            try
            {
                Stream = tcpClient.GetStream();
            }
            catch (Exception ex)
            {
                server.OnGotError($"[{DateTime.Now}] {Id} : {ex.Message}");
            }
        }

        private void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public void Proceed()
        {
            string msg;

            if (EnterChat())
            {
                do
                {
                    try
                    {
                        msg = client.ReceiveMessage(Stream);

                        if (msg.Equals(SocketClient.STOP_CODE))
                        {
                            break;
                        }

                        LastData = msg;

                        if (msg != string.Empty)
                        {
                            server.BroadcastMessage(Id, msg);
                        }

                    }
                    catch (Exception ex)
                    {
                        server.OnGotError($"[{DateTime.Now}] {Id} : {ex.Message}");
                        break;
                    }
                } while (true);

                server.RemoveClient(Id);

                Close();
            }
        }

        public string? LastData { get; set; }

        /// <summary>
        /// Enter the chat, get a name
        /// </summary>
        /// <returns>True if no error during connection otherwise false</returns>
        private bool EnterChat()
        {
            try
            {
                server.AddClient(this);

                if (server.Clients.Count == 1)
                {
                    server.SendMessage(this.Id, "noenemies");
                }
                else
                {
                    server.SendMessage(this.Id, String.Join('|', server.GetAllLastData()));
                }
            }
            catch (Exception ex)
            {
                server.OnGotError($"[{DateTime.Now}] {Id} : {ex.Message}");
                return false;
            }

            return true;
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

