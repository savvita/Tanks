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

        /// <summary>
        /// ID of the session
        /// </summary>
        public int ID { get; }

        /// <summary>
        /// Battlefield of the session
        /// </summary>
        public BattlefieldModel? Battlefield { get; set; }

        /// <summary>
        /// Clients playing at this session
        /// </summary>
        public List<ClientModel>? Clients { get; set; }

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
            if(Battlefield == null)
            {
                return null;
            }

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

        /// <summary>
        /// Send current info about tanks at the battlefield
        /// </summary>
        public void SendTanks()
        {
            if(Battlefield == null)
            {
                return;
            }

            string? msg = JsonSerializer.Serialize<List<TankManModel>>(Battlefield.Tankmen);
            BroadcastMessage(msg);
        }

        /// <summary>
        /// Send the message to all the clients at this session
        /// </summary>
        /// <param name="message">Message to send</param>
        public void BroadcastMessage(string? message)
        {
            if(Clients == null)
            {
                return;
            }

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

        /// <summary>
        /// Join to the battle at this session
        /// </summary>
        /// <param name="tankman">Tankman to join</param>
        /// <param name="width">Width of the field</param>
        /// <param name="height">Height of the field</param>
        public void JoinBattle(TankManModel tankman, int width, int height)
        {
            if(Battlefield == null)
            {
                return;
            }

            if(tankman.Tank == null)
            {
                return;
            }

            SetTankLocation(tankman.Tank, width, height);

            if (tankman.Tank.Bullet != null)
            {
                tankman.Tank.Bullet.Location = tankman.Tank.Muzzle;
            }

            Battlefield.Tankmen.Add(tankman);

            if (Battlefield.Tankmen.Count > 1)
            {
                Battlefield.IsInPlay = true;
            }

            SendTanks();
        }

        /// <summary>
        /// Find free location and set it to the tank
        /// </summary>
        /// <param name="tank">Tank to be placed</param>
        /// <param name="width">Width of the field</param>
        /// <param name="height">Height of the field</param>
        private void SetTankLocation(TankModel tank, int width, int height)
        {
            if (Battlefield == null)
            {
                return;
            }

            Random random = new Random();
            bool isCorrect;
            do
            {
                tank.Location = new Point(random.Next(width - tank.Rectangle.Width),
                    random.Next(height - tank.Rectangle.Height));

                bool isIntersect = false;
                for (int i = 0; i < Battlefield.Tankmen.Count; i++)
                {
                    if(Battlefield.Tankmen[i].Tank == null)
                    {
                        continue;
                    }

                    if (tank.Rectangle.IntersectsWith(Battlefield.Tankmen[i].Tank!.Rectangle))
                    {
                        isIntersect = true;
                        break;
                    }

                }

                isCorrect = !isIntersect;
            } while (!isCorrect);
        }

        /// <summary>
        /// Leave the battle
        /// </summary>
        /// <param name="client"></param>
        public void LeaveBattle(ClientModel client)
        {
            Battlefield?.Tankmen.Remove(client.Tankman);
            Clients?.Remove(client);
        }
    }
}
