using Connection;
using System.Net.Sockets;

namespace Client.Model
{
    public class ClientModel
    {
        private readonly int port;
        private readonly string host;
        private TcpClient? tcpClient;
        private NetworkStream? stream;
        private readonly SocketClient client = new SocketClient();

        /// <summary>
        /// Name of the client
        /// </summary>
        public string? Name { get; set; }

        public ClientModel(string host, int port)
        {
            this.host = host;
            this.port = port;
        }

        /// <summary>
        /// Connect to the server
        /// </summary>
        /// <returns>True if connection is set otherwise false</returns>
        public bool Connect()
        {
            bool result;
            tcpClient = new TcpClient();

            try
            {
                tcpClient.Connect(host, port);
                stream = tcpClient.GetStream();
                result = true;
            }
            catch
            {
                result = false;
            }

            return result;
        }

        /// <summary>
        /// Send a message
        /// </summary>
        /// <param name="message">Message to send</param>
        /// <returns>True if no error otherwise false</returns>
        public bool SendMessage(string? message)
        {
            if (message == null)
            {
                return false;
            }

            bool result;
            try
            {
                client.SendMessage(stream, message);
                result = true;
            }
            catch
            {
                result = false;
                Disconnect();
            }

            return result;
        }

        /// <summary>
        /// Receive a message from the server
        /// </summary>
        /// <returns>Received message</returns>
        public string ReceiveMessage(out bool isSuccess)
        {
            string msg;
            try
            {
                msg = client.ReceiveMessage(stream);

                if (msg.Equals(SocketClient.STOP_CODE))
                {
                    msg = $"[{DateTime.Now}] Server: Disconnected";
                    isSuccess = false;
                    Disconnect();
                }
                else
                {
                    isSuccess = true;
                }
            }
            catch
            {
                msg = $"[{DateTime.Now}] Server: Connection lost";
                isSuccess = false;
                Close();
            }

            return msg;
        }

        /// <summary>
        /// Send a stop message and close all connections
        /// </summary>
        public void Close()
        {
            if (stream != null)
            {
                SendMessage(SocketClient.STOP_CODE);
            }

            Disconnect();
        }

        /// <summary>
        /// Close all connections
        /// </summary>
        public void Disconnect()
        {
            if (stream != null)
            {
                stream.Close();
            }

            if (tcpClient != null)
            {
                tcpClient.Close();
            }
        }
        /// <summary>
        /// Authorizate at the server
        /// </summary>
        /// <param name="login">Login</param>
        /// <param name="password">Password</param>
        /// <returns>True if authorization is successful otherwise false</returns>
        public bool Authorizate(string login, string password)
        {
            bool connected = Connect();

            if (connected)
            {
                try
                {
                    client.SendMessage(stream, SocketClient.AuthorizationCode);

                    return GetAuthorizationResponse(login, password);
                }
                catch { }
            }
            return false;
        }

        /// <summary>
        /// Register a new user
        /// </summary>
        /// <param name="login">Login</param>
        /// <param name="password">Password</param>
        /// <returns>True if registration is successful otherwise false</returns>
        public bool Register(string login, string password)
        {
            bool connected = Connect();

            if (connected)
            {
                try
                {
                    client.SendMessage(stream, SocketClient.RegistrationCode);

                    return GetAuthorizationResponse(login, password);
                }
                catch { }
            }
            return false;
        }

        private bool GetAuthorizationResponse(string login, string password)
        {
            if (stream == null)
            {
                return false;
            }

            client.ReceiveMessage(stream);
            client.SendMessage(stream, string.Join(',', login, password));

            string response = client.ReceiveMessage(stream);

            if (!response.Equals(SocketClient.FailCode))
            {
                this.Name = login;
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
