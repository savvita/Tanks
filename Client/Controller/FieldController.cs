using Client.Model;
using Connection;
using System.Text.Json;
using TankLibrary;

namespace Client.Controller
{
    public class FieldController
    {
        private ClientModel client;

        private TankController controller { get; set; }

        public List<TankManModel>? TankMen { get; private set; }

        public FieldController(Rectangle fieldBounds, ClientModel client)
        {
            controller = new TankController(new SpriteImageModel(
                Properties.Resources.Tank,
                new ImageBounds(0, 564, 350, 258),
                new ImageBounds(0, 354, 350, 210),
                new ImageBounds(0, 0, 211, 350),
                new ImageBounds(211, 0, 211, 350)));

            controller.FieldBounds = fieldBounds;

            this.client = client;
        }

        private TankManModel? GetMe()
        {
            for (int i = 0; i < TankMen.Count; i++)
            {
                if (TankMen[i].Name == null)
                {
                    continue;
                }

                if (TankMen[i].Name!.Equals(client.Name))
                {
                    return TankMen[i];
                }
            }

            return null;
        }

        private bool IsMovePossible(Point location, Directions direction)
        {
            if(TankMen == null)
            {
                return false;
            }
            if (TankMen.Count <= 1)
            {
                return true;
            }

            TankManModel? me = GetMe();

            if (me == null || me.Tank == null)
            {
                return false;
            }

            for (int i = 0; i < TankMen.Count; i++)
            {
                if (TankMen[i] == me)
                {
                    continue;
                }

                if (TankMen[i].Tank == null)
                {
                    continue;
                }

                if (new Rectangle(location, me.Tank.Size).IntersectsWith(TankMen[i].Tank!.Rectangle))
                {
                    return false;
                }
            }

            return true;
        }

        public void Move(Directions direction)
        {
            TankManModel? me = GetMe();

            if(me == null)
            {
                return;
            }

            me.Tank.Bullet = controller.TankMan.Tank.Bullet;

            controller.TankMan.Tank = me.Tank;

            Point location = controller.GetNextLocation(direction);

            if (!IsMovePossible(location, direction) && controller.TankMan.Tank != null)
            {
                location = controller.TankMan.Tank.Location;
            }

            switch (direction)
            {
                case Directions.Left:
                    controller.MoveLeft(location);
                    break;
                case Directions.Right:
                    controller.MoveRight(location);
                    break;
                case Directions.Up:
                    controller.MoveUp(location);
                    break;
                case Directions.Down:
                    controller.MoveDown(location);
                    break;
            }

            SendLocations();
        }

        public void Fire()
        {
            TankManModel? me = GetMe();

            if (me == null)
            {
                return;
            }

            controller.TankMan.Tank = me.Tank;

            Action? move = controller.GetFireMoving();

            if(move != null)
            {
                MoveBullet(move);
            }
        }

        private void MoveBullet(Action move)
        {
            if (controller.TankMan.Tank == null || controller.TankMan.Tank.Bullet == null)
            {
                return;
            }

            Task.Factory.StartNew(() =>
            {
                TankManModel? me;

                while (controller.FieldBounds.Contains(controller.TankMan.Tank.Bullet.Location))
                {
                    me = GetMe();

                    if (me == null)
                    {
                        return;
                    }

                    me.Tank.Bullet = controller.TankMan.Tank.Bullet;


                    if (!controller.TankMan.Tank.Bullet.IsFlying)
                    {
                        break;
                    }

                    move();
                    SendLocations();

                    Thread.Sleep(100);
                }

                me = GetMe();

                if (me == null)
                {
                    return;
                }

                controller.TankMan.Tank = me.Tank;

                controller.TankMan.Tank.IsFire = false;
                controller.TankMan.Tank.Bullet.IsFlying = false;
                controller.TankMan.Tank.Bullet.Location = controller.TankMan.Tank.Muzzle;

            });
        }

        /// <summary>
        /// Connect to the server
        /// </summary>
        public void Connect()
        {
            try
            {
                if(controller.TankMan.Tank == null)
                {
                    return;
                }


                Random random = new Random();
                int ch = random.Next(4);

                switch (ch)
                {
                    case 0:
                        controller.MoveLeft(controller.TankMan.Tank.Location);
                        break;
                    case 1:
                        controller.MoveRight(controller.TankMan.Tank.Location);
                        break;
                    case 2:
                        controller.MoveUp(controller.TankMan.Tank.Location);
                        break;
                    case 3:
                        controller.MoveDown(controller.TankMan.Tank.Location);
                        break;
                }


                client.SendMessage(SocketClient.StartCode);
                client.SendMessage(controller.FieldBounds.Width.ToString());
                client.SendMessage(controller.FieldBounds.Height.ToString());


                bool isSuccess;

                try
                {
                    string tank = JsonSerializer.Serialize<TankModel>(controller.TankMan.Tank);
                    client.SendMessage(tank);

                    string msg = client.ReceiveMessage(out isSuccess);

                    TankMen = JsonSerializer.Deserialize<List<TankManModel>>(msg);
                }
                catch { }

            }
            catch { }

            Thread listeningThread = new Thread(ReceivingMessages)
            {
                IsBackground = true
            };
            listeningThread.Start();
        }

        public void Close()
        {
            client.Close();
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

                    if (msg.Equals(SocketClient.WinCode))
                    {
                        Win?.Invoke();
                    }
                    else if (msg.Equals(SocketClient.LostCode))
                    {
                        Lost?.Invoke();
                    }
                    else
                    {
                        TankMen = JsonSerializer.Deserialize<List<TankManModel>>(msg);

                        TankManModel? me = GetMe();

                        if (me == null)
                        {
                            return;
                        }

                        me.Tank.Bullet = controller.TankMan.Tank.Bullet;
                        controller.TankMan.Tank = me.Tank;
                    }
                }
                catch { }

            } while (true);
        }

        public event Action? Win;
        public event Action? Lost;


        private void SendLocations()
        {

            try
            {
                string? msg = JsonSerializer.Serialize<List<TankManModel>>(TankMen);
                client.SendMessage(msg);
            }
            catch { }
        }
    }
}
