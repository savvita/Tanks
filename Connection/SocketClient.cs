using System.Net.Sockets;
using System.Text;

namespace Connection
{
    public static class SocketClient
    {
        /// <summary>
        /// Port to connect
        /// </summary>
        public static int Port { get; } = 8008;

        /// <summary>
        /// IP of the server
        /// </summary>
        public static string Host { get; } = "127.0.0.1";

        /// <summary>
        /// Default encoding of messages
        /// </summary>
        public static Encoding MessageEncoding { get; } = Encoding.Unicode;

        /// <summary>
        /// Stop code
        /// </summary>
        public static string StopCode { get; } = "/stop";

        /// <summary>
        /// Ok code
        /// </summary>
        public static string OkCode { get; } = "/ok";

        /// <summary>
        /// Fail code
        /// </summary>
        public static string FailCode { get; } = "/fail";

        /// <summary>
        /// Authorization code
        /// </summary>
        public static string AuthorizationCode { get; } = "/login";

        /// <summary>
        /// Registration code
        /// </summary>
        public static string RegistrationCode { get; } = "/register";

        /// <summary>
        /// Code to set the winner
        /// </summary>
        public static string WinCode { get; } = "/win";

        /// <summary>
        /// Code to set the looser 
        /// </summary>
        public static string LostCode { get; } = "/lost";

        /// <summary>
        /// Code to set the values from the shop 
        /// </summary>
        public static string ShopCode { get; } = "/shop";

        /// <summary>
        /// Code to join the game
        /// </summary>
        public static string StartCode { get; } = "/start";

        /// <summary>
        /// Code to leave the battle
        /// </summary>
        public static string LeaveCode { get; } = "/leave";

        /// <summary>
        /// Send a message to the stream
        /// </summary>
        /// <param name="stream">Network stream</param>
        /// <param name="message">Message to send</param>
        public static void SendMessage(NetworkStream? stream, string message)
        {
            if(stream == null)
            {
                return;
            }

            byte[] data = MessageEncoding.GetBytes(message);

            BinaryWriter writer = new BinaryWriter(stream);
            writer.Write(data.Length);
            writer.Write(data);
        }

        /// <summary>
        /// Get message from the streem
        /// </summary>
        /// <param name="stream">Network stream</param>
        /// <returns>Received message</returns>
        public static string ReceiveMessage(NetworkStream? stream)
        {
            if(stream == null)
            {
                throw new NullReferenceException("Stream cannot be null");
            }

            string msg;

            BinaryReader reader = new BinaryReader(stream);

            int length = reader.ReadInt32();

            byte[] data = new byte[length];
            reader.Read(data, 0 , length);

            msg = MessageEncoding.GetString(data, 0, length);

            return msg;
        }
    }
}