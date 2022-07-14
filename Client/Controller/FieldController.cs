using Client.Model;
using System.Text.Json;

namespace Client.Controller
{
    public class FieldController
    {
        public TankController TankController { get; set; }

        //public TankModel? Enemy { get; set; } = null;
        public List<TankModel>? Enemies { get; set; } = new List<TankModel>();

        private ClientModel client;

        public FieldController(Rectangle fieldBounds)
        {
            TankController = new TankController(new SpriteImageModel(
                Properties.Resources.Tank,
                new ImageBounds(0, 564, 350, 258),
                new ImageBounds(0, 354, 350, 210),
                new ImageBounds(0, 0, 211, 350),
                new ImageBounds(211, 0, 211, 350)));

            TankController.FieldBounds = fieldBounds;

            client = new ClientModel("127.0.0.1", 8008);

            //Connect();
        }

        private bool IsMovePossible(Point location, Directions direction)
        {
            if (Enemies == null)
            {
                return true;
            }

            for (int i = 0; i < Enemies.Count; i++)
            {
                if (new Rectangle(location, TankController.Tank.Size).IntersectsWith(Enemies[i].TankRectangle))
                {
                    return false;
                }
            }

            return true;
            //if(new Rectangle(location, TankController.Tank.Size).IntersectsWith(Enemy.TankRectangle))
            //{
            //    return false;
            //}

            //return true;
        }

        public void Move(Directions direction)
        {
            Point location = TankController.GetNextLocation(direction);

            if (!IsMovePossible(location, direction))
            {
                location = TankController.Tank.Location;
            }

            switch (direction)
            {
                case Directions.Left:
                    TankController.MoveLeft(location);
                    break;
                case Directions.Right:
                    TankController.MoveRight(location);
                    break;
                case Directions.Up:
                    TankController.MoveUp(location);
                    break;
                case Directions.Down:
                    TankController.MoveDown(location);
                    break;
            }
        }

        /// <summary>
        /// Connect to the server
        /// </summary>
        public void Connect()
        {
            bool result = client.Connect();

            if (result)
            {
                try
                {
                    bool isSuccess;
                    string? init = client.ReceiveMessage(out isSuccess);

                    string[] enemies = init.Split('|');

                    for (int i = 0; i < enemies.Count(); i++)
                    {
                        TankModel? model = JsonSerializer.Deserialize<TankModel>(enemies[i]);

                        if(model != null)
                            Enemies.Add(model);
                    }

                    if (Enemies != null)
                    {
                        Random random = new Random();

                        bool isCorrect = false;

                        do
                        {
                            TankController.Tank.Location = new Point(random.Next(TankController.FieldBounds.Width - TankController.Tank.TankRectangle.Width),
                                random.Next(TankController.FieldBounds.Height - TankController.Tank.TankRectangle.Height));


                            bool isIntersect = false;
                            for (int i = 0; i < Enemies.Count; i++)
                            {
                                if (TankController.Tank.TankRectangle.IntersectsWith(Enemies[i].TankRectangle))
                                {
                                    isIntersect = true;
                                    break;
                                }

                                isCorrect = !isIntersect;
                            }

                        } while (!isCorrect);

                        int ch = random.Next(4);

                        switch (ch)
                        {
                            case 0:
                                TankController.MoveLeft(TankController.Tank.Location);
                                break;
                            case 1:
                                TankController.MoveRight(TankController.Tank.Location);
                                break;
                            case 2:
                                TankController.MoveUp(TankController.Tank.Location);
                                break;
                            case 3:
                                TankController.MoveDown(TankController.Tank.Location);
                                break;
                        }
                    }
                    //bool isSuccess;
                    //string? init = client.ReceiveMessage(out isSuccess);

                    //Enemy = JsonSerializer.Deserialize<TankModel>(init);

                    //if (Enemy != null)
                    //{
                    //    TankController.Tank.Location = new Point(TankController.FieldBounds.Width - TankController.Tank.TankRectangle.Width,
                    //        TankController.FieldBounds.Height - TankController.Tank.TankRectangle.Height);

                    //    TankController.MoveLeft(TankController.Tank.Location);

                    //}
                }
                catch { }

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

                    TankModel? enemy = JsonSerializer.Deserialize<TankModel>(msg);

                    if (enemy == null)
                    {
                        continue;
                    }

                    if (Enemies == null)
                        continue;

                    if (Enemies.Count(x => x.Name.Equals(enemy.Name)) == 0)
                    {
                        Enemies.Add(enemy);
                    }

                    for (int i = 0; i < Enemies.Count; i++)
                    {
                        if (Enemies[i].Name != null && Enemies[i].Name.Equals(enemy.Name))
                        {
                            Enemies[i] = enemy;
                        }


                        if (Enemies[i].IsFire && TankController.Tank.TankRectangle.Contains(Enemies[i].Bullet.Location))
                        {
                            Enemies[i].Bullet.IsFlying = false;
                            TankController.Tank.Health -= Enemies[i].Damage;
                            TankController.Tank.IsHit = true;

                            if (TankController.Tank.Health <= 0)
                            {
                                TankController.Tank.IsAlive = false;
                            }
                        }

                        if (TankController.Tank.IsFire && Enemies[i].TankRectangle.Contains(TankController.Tank.Bullet.Location))
                        {
                            TankController.Tank.Bullet.IsFlying = false;

                            Enemies[i].Health -= TankController.Tank.Damage;
                            Enemies[i].IsHit = true;
                            if (Enemies[i].Health <= 0)
                            {
                                Enemies[i].IsAlive = false;
                            }
                        }
                    }
                }
                catch { }

                //try
                //{
                //    string msg = client.ReceiveMessage(out isSuccess);

                //    Enemy = JsonSerializer.Deserialize<TankModel>(msg);

                //    if(Enemy == null)
                //    {
                //        continue;
                //    }

                //    if(Enemy.IsFire && TankController.Tank.TankRectangle.Contains(Enemy.Bullet.Location))
                //    {
                //        Enemy.Bullet.IsFlying = false;
                //        TankController.Tank.Health -= Enemy.Damage;
                //        TankController.Tank.IsHit = true;

                //        if (TankController.Tank.Health <= 0)
                //        {
                //            TankController.Tank.IsAlive = false;
                //        }
                //    }

                //    if(TankController.Tank.IsFire && Enemy.TankRectangle.Contains(TankController.Tank.Bullet.Location))
                //    {
                //        TankController.Tank.Bullet.IsFlying = false;

                //        Enemy.Health -= TankController.Tank.Damage;
                //        Enemy.IsHit = true;
                //        if (Enemy.Health <= 0)
                //        {
                //            Enemy.IsAlive = false;
                //        }
                //    }
                //}
                //catch { }

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
