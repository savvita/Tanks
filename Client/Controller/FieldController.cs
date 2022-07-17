using Client.Model;
using Connection;
using System.Text.Json;
using TankLibrary;

namespace Client.Controller
{
    public class FieldController
    {
        private readonly ClientModel? client;
        private readonly TankController? controller;

        private readonly CancellationToken token;
        private readonly CancellationTokenSource source;

        /// <summary>
        /// Raise when one of the tankmen wins
        /// </summary>
        public event Action? Win;

        /// <summary>
        /// Raise when one of the tankmen lost
        /// </summary>
        public event Action? Lost;

        /// <summary>
        /// Tankmen in the battle
        /// </summary>
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

            source = new CancellationTokenSource();
            token = source.Token;
        }

        /// <summary>
        /// Find a tank of the client
        /// </summary>
        /// <returns></returns>
        private TankManModel? GetMe()
        {
            if (TankMen == null || client == null)
            {
                return null;
            }

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

        /// <summary>
        /// Defint if the move is possible to the new location
        /// </summary>
        /// <param name="location">New location to check</param>
        /// <returns>True if move is possible otherwise false</returns>
        private bool IsMovePossible(Point location)
        {
            if (TankMen == null)
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

        /// <summary>
        /// Move tank to the specified direction
        /// </summary>
        /// <param name="direction">Direction in whick move to</param>
        public void Move(Directions direction)
        {
            TankManModel? me = GetMe();

            if (me == null || me.Tank == null)
            {
                return;
            }

            if (controller == null)
            {
                return;
            }

            if (controller.TankMan.Tank != null)
            {
                me.Tank.Bullet = controller.TankMan.Tank.Bullet;
            }

            controller.TankMan.Tank = me.Tank;

            Point location = controller.GetNextLocation(direction);

            if (!IsMovePossible(location) && controller.TankMan.Tank != null)
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

        /// <summary>
        /// Make a fire
        /// </summary>
        public void Fire()
        {
            TankManModel? me = GetMe();

            if (me == null)
            {
                return;
            }

            if (controller == null)
            {
                return;
            }

            controller.TankMan.Tank = me.Tank;

            Action? move = controller.GetFireMoving();

            if (move != null)
            {
                MoveBullet(move);
            }
        }

        private void MoveBullet(Action move)
        {
            if (controller == null)
            {
                return;
            }

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

                    if (me == null || me.Tank == null)
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

                RestoreBulletLocation();

            });
        }

        /// <summary>
        /// Restore location of the bullet after the firing
        /// </summary>
        private void RestoreBulletLocation()
        {
            TankManModel? me = GetMe();

            if (me == null || me.Tank == null)
            {
                return;
            }

            if (controller == null || controller.TankMan == null || controller.TankMan.Tank == null)
            {
                return;
            }

            me.Tank.Bullet = controller.TankMan.Tank.Bullet;
            controller.TankMan.Tank = me.Tank;

            controller.TankMan.Tank.IsFire = false;

            if (controller.TankMan.Tank.Bullet != null)
            {
                controller.TankMan.Tank.Bullet.IsFlying = false;
                controller.TankMan.Tank.Bullet.Location = controller.TankMan.Tank.Muzzle;
            }
        }

        /// <summary>
        /// Join to the battle
        /// </summary>
        public void ConnectToBattle()
        {
            try
            {
                InitializeTank();
            }
            catch { }

            Task task = new Task(ReceivingMessages, token);
            task.Start();
        }

        /// <summary>
        /// Initialize a tank at the beginning
        /// </summary>
        private void InitializeTank()
        {
            if (client == null)
            {
                return;
            }

            if (controller == null || controller.TankMan.Tank == null)
            {
                return;
            }

            SetRandomStartDirection();

            client.SendMessage(SocketClient.StartCode);

            client.SendMessage(controller.FieldBounds.Width.ToString());
            client.SendMessage(controller.FieldBounds.Height.ToString());

            try
            {
                string tank = JsonSerializer.Serialize<TankModel>(controller.TankMan.Tank);
                client.SendMessage(tank);

                string msg = client.ReceiveMessage(out bool _);

                TankMen = JsonSerializer.Deserialize<List<TankManModel>>(msg);
            }
            catch { }
        }

        /// <summary>
        /// Set random direction of the tank at the beginning
        /// </summary>
        private void SetRandomStartDirection()
        {
            if(controller == null || controller.TankMan.Tank == null)
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
        }

        /// <summary>
        /// Leave the battle
        /// </summary>
        public void LeaveBattle()
        {
            client?.SendMessage(SocketClient.LeaveCode);
            source.Cancel();
        }


        /// <summary>
        /// Receiving messages unless the connection is not failed or stopped
        /// </summary>
        private void ReceivingMessages()
        {
            do
            {
                if(client == null)
                {
                    break;
                }

                if (token.IsCancellationRequested)
                {
                    break;
                }

                try
                {
                    string msg = client.ReceiveMessage(out bool _);

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

                        if (me == null || me.Tank == null)
                        {
                            return;
                        }

                        if(controller == null)
                        {
                            return;
                        }

                        if (controller.TankMan.Tank != null)
                        {
                            me.Tank.Bullet = controller.TankMan.Tank.Bullet;
                        }
                        controller.TankMan.Tank = me.Tank;
                    }
                }
                catch { }

            } while (true);
        }


        /// <summary>
        /// Send new locations of the tanks
        /// </summary>
        private void SendLocations()
        {
            if(TankMen == null)
            {
                return;
            }

            try
            {
                string? msg = JsonSerializer.Serialize<List<TankManModel>>(TankMen);
                client?.SendMessage(msg);
            }
            catch { }
        }
    }
}
