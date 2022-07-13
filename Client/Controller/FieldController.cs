using Client.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Client.Controller
{
    public class FieldController
    {
        public TankController TankController { get; set; }
        public TankModel? Enemy { get; set; } = null;

        private ClientModel client;

        public FieldController()
        {
            TankController = new TankController(new SpriteImageModel(
                Properties.Resources.Tank,
                new ImageBounds(0, 564, 350, 258),
                new ImageBounds(0, 354, 350, 210),
                new ImageBounds(0, 0, 211, 350),
                new ImageBounds(211, 0, 211, 350)));

            client = new ClientModel("127.0.0.1", 8008);

            Connect();
        }

        public bool IsMovePossible()
        {
            return false;
        }

        /// <summary>
        /// Connect to the server
        /// </summary>
        private void Connect()
        {
            bool result = client.Connect();

            if (result)
            {
                string? msg = JsonSerializer.Serialize<TankModel>(TankController.Tank);
                client.SendMessage(msg);
                Thread listeningThread = new Thread(ReceivingMessages)
                {
                    IsBackground = true
                };
                listeningThread.Start();

                Thread sendingThread = new Thread(SendingMessages)
                {
                    IsBackground = true
                };
                sendingThread.Start();
            }
        }

        /// <summary>
        /// Receiving messages unless the connection is not failed or stopped
        /// </summary>
        private void ReceivingMessages()
        {
            bool isSuccess;
            do
            {
                try
                {
                    string msg = client.ReceiveMessage(out isSuccess);

                    Enemy = JsonSerializer.Deserialize<TankModel>(msg);
                    
                    if(Enemy == null)
                    {
                        continue;
                    }

                    if(Enemy.IsFire && TankController.Tank.TankRectangle.Contains(Enemy.Bullet.Location))
                    {
                        Enemy.Bullet.IsFlying = false;
                        TankController.Tank.IsAlive = false;
                    }

                    if(TankController.Tank.IsFire && Enemy.TankRectangle.Contains(TankController.Tank.Bullet.Location))
                    {
                        TankController.Tank.Bullet.IsFlying = false;
                        Enemy.IsAlive = false;
                    }
                }
                catch { }

            } while (true);
        }

        private void SendingMessages()
        {
            while (true)
            {
                try
                {
                    string? msg = JsonSerializer.Serialize<TankModel>(TankController.Tank);
                    client.SendMessage(msg);
                }
                catch { }
                Thread.Sleep(500);
            }
        }
    }
}
