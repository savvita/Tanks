using Connection;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text.Json;
using TankLibrary;

namespace Server.Model
{
    public class SessionModel
    {
        private static int id = 0;

        public int ID { get; }
        public BattlefieldModel? Battlefield { get; set; }

        public List<ClientModel>? Clients { get; set; }

        public int Count
        {
            get => Clients.Count;
        }

        public SessionModel()
        {
            ID = id++;
            Clients = new List<ClientModel>();
        }

        /// <summary>
        /// Handle battle with new data
        /// </summary>
        /// <param name="msg">Data from the client</param>
        /// <returns>New data</returns>
        public string? HandleBattle(string msg)
        {
            try
            {
                List<TankManModel>? tankmen = JsonSerializer.Deserialize<List<TankManModel>>(msg);

                if (tankmen != null)
                {
                    Battlefield.Tankmen = tankmen;
                    Battlefield.HandleBattle();

                    string? res = JsonSerializer.Serialize<List<TankManModel>>(Battlefield.Tankmen);

                    return res;
                }
            }
            catch { }

            return null;
        }

        public void SendTanks()
        {
            string? msg = JsonSerializer.Serialize<List<TankManModel>>(Battlefield.Tankmen);
            BroadcastMessage(msg);
        }

        public void BroadcastMessage(string? message)
        {
            for (int i = 0; i < Clients.Count; i++)
            {
                if (Clients[i].Name == null)
                {
                    continue;
                }

                try
                {
                    SocketClient.SendMessage(Clients[i].Stream, message);
                }
                catch { }
            }
        }

        public void JoinBattle(TankManModel tankman, int width, int height)
        {
            Random random = new Random();
            bool isCorrect;

            do
            {
                tankman.Tank.Location = new Point(random.Next(width - tankman.Tank.Rectangle.Width),
                    random.Next(height - tankman.Tank.Rectangle.Height));

                bool isIntersect = false;
                for (int i = 0; i < Battlefield.Tankmen.Count; i++)
                {
                    if (tankman.Tank.Rectangle.IntersectsWith(Battlefield.Tankmen[i].Tank.Rectangle))
                    {
                        isIntersect = true;
                        break;
                    }

                }

                isCorrect = !isIntersect;
            } while (!isCorrect);


            if (tankman.Tank.Bullet != null)
            {
                tankman.Tank.Bullet.Location = tankman.Tank.Muzzle;
            }

            Battlefield.Tankmen.Add(tankman);

            if (Battlefield.Tankmen.Count > 1)
            {
                Battlefield.IsGameStarted = true;
            }

            SendTanks();
        }


    }
}
