using System.Net.Sockets;
using System.Text;

namespace Connection
{
    public class SocketClient
    {
        /// <summary>
        /// Size of buffer array
        /// </summary>
        public int BufferSize { get; set; } = 256;

        /// <summary>
        /// Default encoding of messages
        /// </summary>
        public Encoding MessageEncoding { get; set; } = Encoding.Unicode;

        /// <summary>
        /// Code to stop communication
        /// </summary>
        public const string STOP_CODE = "/out";

        /// <summary>
        /// Ok code
        /// </summary>
        public static string OkCode { get; set; } = "/ok";

        /// <summary>
        /// Fail code
        /// </summary>
        public static string FailCode { get; set; } = "/fail";

        /// <summary>
        /// Authorization code
        /// </summary>
        public static string AuthorizationCode { get; set; } = "/login";

        /// <summary>
        /// Registration code
        /// </summary>
        public static string RegistrationCode { get; set; } = "/register";

        public static string ResultCode { get; set; } = "/result";

        public static string WinCode { get; set; } = "/win";

        public static string LostCode { get; set; } = "/lost";

        /// <summary>
        /// Send a message to the stream
        /// </summary>
        /// <param name="stream">Network stream</param>
        /// <param name="message">Message to send</param>
        public void SendMessage(NetworkStream stream, string message)
        {
            byte[] data = MessageEncoding.GetBytes(message);

            //using (BinaryWriter writer = new BinaryWriter(stream))
            {
                BinaryWriter writer = new BinaryWriter(stream);
                writer.Write(data.Length);
                writer.Write(data);
            }
            
            //stream.Write(data, 0, data.Length);
        }

        /// <summary>
        /// Get message from the streem
        /// </summary>
        /// <param name="stream">Network stream</param>
        /// <returns>Received message</returns>
        public string ReceiveMessage(NetworkStream stream)
        {
            string msg = String.Empty;
            //using (BinaryReader reader = new BinaryReader(stream))
            {
                BinaryReader reader = new BinaryReader(stream);
                int length = reader.ReadInt32();
                byte[] data = new byte[length];
                reader.Read(data, 0 , length);
                msg = MessageEncoding.GetString(data, 0, length);
            }

            return msg;

            //StringBuilder sb = new StringBuilder();
            //byte[] data = new byte[BufferSize];
            //int count;

            //do
            //{
            //    count = stream.Read(data, 0, BufferSize);
            //    sb.Append(MessageEncoding.GetString(data, 0, count));
            //} while (stream.DataAvailable);

            //return sb.ToString();
        }
    }
}