using Client.Model;
using TankLibrary;

namespace Client.Controller
{
    public class TankController
    {
        private Func<Point, Rectangle>? currentMove;

        public TankManModel TankMan { get; }

        /// <summary>
        /// Bounds of the field
        /// </summary>
        public Rectangle FieldBounds { get; set; }

        /// <summary>
        /// Describe sprite image this tank
        /// </summary>
        public SpriteImageModel? SpriteImage { get; private set; }

        public TankController(SpriteImageModel spriteImage)
        {
            TankMan = new TankManModel();
            TankMan.Tank = new TankModel();

            TankMan.Tank.IsAlive = true;
            TankMan.Tank.Bullet = new TankLibrary.BulletModel();

            SpriteImage = spriteImage;

            TankMan.Tank.Size = new Size(SpriteImage.MovingRightImageBounds.Width / 4, SpriteImage.MovingRightImageBounds.Height / 4);

            TankMan.Tank.ImageBounds = SpriteImage.MovingRightImageBounds.Rectangle;
            currentMove = MoveRight;

            TankMan.Tank.Muzzle = new Point(TankMan.Tank.Location.X + TankMan.Tank.Size.Width, TankMan.Tank.Location.Y + TankMan.Tank.Size.Height / 2);

            if (TankMan.Tank.Bullet != null)
            {
                TankMan.Tank.Bullet.Location = TankMan.Tank.Muzzle;
            }
        }

        
        /// <summary>
        /// Make a fire
        /// </summary>
        //public void Fire()
        //{
        //    if (TankMan.Tank == null || TankMan.Tank.Bullet == null)
        //    {
        //        return;
        //    }

        //    if (TankMan.Tank.IsFire || !TankMan.Tank.IsAlive)
        //    {
        //        return;
        //    }

        //    TankMan.Tank.IsFire = true;
        //    TankMan.Tank.Bullet.IsFlying = true;

        //    if (currentMove == MoveRight)
        //    {
        //        MoveBullet(TankMan.Tank.Bullet.MoveRight);
        //    }
        //    else if (currentMove == MoveLeft)
        //    {
        //        MoveBullet(TankMan.Tank.Bullet.MoveLeft);
        //    }
        //    else if (currentMove == MoveUp)
        //    {
        //        MoveBullet(TankMan.Tank.Bullet.MoveUp);
        //    }
        //    else if (currentMove == MoveDown)
        //    {
        //        MoveBullet(TankMan.Tank.Bullet.MoveDown);
        //    }
        //}

        public Action? GetFireMoving()
        {
            if (TankMan.Tank == null || TankMan.Tank.Bullet == null)
            {
                return null;
            }

            if (TankMan.Tank.IsFire || !TankMan.Tank.IsAlive)
            {
                return null;
            }

            TankMan.Tank.IsFire = true;
            TankMan.Tank.Bullet.IsFlying = true;

            if (currentMove == MoveRight)
            {
                return TankMan.Tank.Bullet.MoveRight;
            }
            else if (currentMove == MoveLeft)
            {
                return TankMan.Tank.Bullet.MoveLeft;
            }
            else if (currentMove == MoveUp)
            {
                return TankMan.Tank.Bullet.MoveUp;
            }
            else if (currentMove == MoveDown)
            {
                return TankMan.Tank.Bullet.MoveDown;
            }

            return null;
        }

        /// <summary>
        /// Move the bullet
        /// </summary>
        /// <param name="move">Function describing a movement of the bullet</param>
        //private void MoveBullet(Action move)
        //{
        //    if(TankMan.Tank == null || TankMan.Tank.Bullet == null)
        //    {
        //        return;
        //    }

        //    Task.Factory.StartNew(() =>
        //    {
        //        while (FieldBounds.Contains(TankMan.Tank.Bullet.Location))
        //        {
        //            if (!TankMan.Tank.Bullet.IsFlying)
        //            {
        //                break;
        //            }

        //            move();
        //            Thread.Sleep(100);
        //        }

        //        TankMan.Tank.IsFire = false;
        //        TankMan.Tank.Bullet.IsFlying = false;
        //        TankMan.Tank.Bullet.Location = TankMan.Tank.Muzzle;

        //    });
        //}

        /// <summary>
        /// Get the next location of the tank
        /// </summary>
        /// <param name="direction">Direction of the moving</param>
        /// <returns>New location</returns>
        public Point GetNextLocation(Directions direction)
        {
            Point point = new Point();

            if(TankMan.Tank == null)
            {
                return point;
            }

            switch(direction)
            {
                case Directions.Left:
                    if (TankMan.Tank.Location.X - TankMan.Tank.Speed >= FieldBounds.Location.X)
                    {
                        point = new Point(TankMan.Tank.Location.X - TankMan.Tank.Speed, TankMan.Tank.Location.Y);
                    }
                    else
                    {
                        point = new Point(FieldBounds.Location.X, TankMan.Tank.Location.Y);
                    }
                    break;

                case Directions.Right:
                    if (TankMan.Tank.Location.X + TankMan.Tank.Speed + TankMan.Tank.Size.Width <= FieldBounds.Location.X + FieldBounds.Width)
                    {
                        point = new Point(TankMan.Tank.Location.X + TankMan.Tank.Speed, TankMan.Tank.Location.Y);
                    }
                    else
                    {
                        point = new Point(FieldBounds.Location.X + FieldBounds.Width - TankMan.Tank.Size.Width, TankMan.Tank.Location.Y);
                    }
                    break;

                case Directions.Up:
                    if (TankMan.Tank.Location.Y - TankMan.Tank.Speed >= FieldBounds.Location.Y)
                    {
                        point = new Point(TankMan.Tank.Location.X, TankMan.Tank.Location.Y - TankMan.Tank.Speed);
                    }
                    else
                    {
                        point = new Point(TankMan.Tank.Location.X, FieldBounds.Location.Y);
                    }
                    break;

                case Directions.Down:
                    if (TankMan.Tank.Location.Y + TankMan.Tank.Speed + TankMan.Tank.Size.Height <= FieldBounds.Location.Y + FieldBounds.Height)
                    {
                        point = new Point(TankMan.Tank.Location.X, TankMan.Tank.Location.Y + TankMan.Tank.Speed);
                    }
                    else
                    {
                        point = new Point(TankMan.Tank.Location.X, FieldBounds.Location.Y + FieldBounds.Height - TankMan.Tank.Size.Height);
                    }
                    break;
            }

            return point;
        }

        /// <summary>
        /// Move a tank to the left
        /// </summary>
        /// <param name="location">New location</param>
        /// <returns>New rectangle of the tank</returns>
        public Rectangle MoveLeft(Point location)
        {
            if(TankMan.Tank == null)
            {
                return new Rectangle();
            }

            if (!TankMan.Tank.IsAlive)
            {
                return TankMan.Tank.Rectangle;
            }

            TankMan.Tank.Location = location;

            Rectangle rect;

            if (SpriteImage != null)
            {

                TankMan.Tank.Size = new Size(SpriteImage.MovingLeftImageBounds.Width / 4, SpriteImage.MovingLeftImageBounds.Height / 4);

                rect = SpriteImage.MovingLeftImageBounds.Rectangle;
            }
            else
            {
                rect = new Rectangle();
            }

            TankMan.Tank.ImageBounds = rect;
            TankMan.Tank.Muzzle = new Point(TankMan.Tank.Location.X, TankMan.Tank.Location.Y + TankMan.Tank.Size.Height / 2);

            if (TankMan.Tank.Bullet != null && !TankMan.Tank.IsFire)
            {
                TankMan.Tank.Bullet.Location = TankMan.Tank.Muzzle;
            }

            currentMove = MoveLeft;

            return rect;
        }


        /// <summary>
        /// Move a tank to the right
        /// </summary>
        /// <param name="location">New location</param>
        /// <returns>New rectangle of the tank</returns>
        public Rectangle MoveRight(Point location)
        {
            if (TankMan.Tank == null)
            {
                return new Rectangle();
            }

            if (!TankMan.Tank.IsAlive)
            {
                return TankMan.Tank.Rectangle;
            }

            TankMan.Tank.Location = location;

            Rectangle rect;

            if (SpriteImage != null)
            {
                TankMan.Tank.Size = new Size(SpriteImage.MovingRightImageBounds.Width / 4, SpriteImage.MovingRightImageBounds.Height / 4);

                rect = SpriteImage.MovingRightImageBounds.Rectangle;
            }
            else
            {
                rect = new Rectangle();
            }

            TankMan.Tank.ImageBounds = rect;

            TankMan.Tank.Muzzle = new Point(TankMan.Tank.Location.X + TankMan.Tank.Size.Width, TankMan.Tank.Location.Y + TankMan.Tank.Size.Height / 2);

            if (TankMan.Tank.Bullet != null && !TankMan.Tank.IsFire)
            {
                TankMan.Tank.Bullet.Location = TankMan.Tank.Muzzle;
            }

            currentMove = MoveRight;

            return rect;
        }

        /// <summary>
        /// Move a tank to the up
        /// </summary>
        /// <param name="location">New location</param>
        /// <returns>New rectangle of the tank</returns>
        public Rectangle MoveUp(Point location)
        {
            if (TankMan.Tank == null)
            {
                return new Rectangle();
            }

            if (!TankMan.Tank.IsAlive)
            {
                return TankMan.Tank.Rectangle;
            }

            TankMan.Tank.Location = location;

            Rectangle rect;

            if (SpriteImage != null)
            {
                TankMan.Tank.Size = new Size(SpriteImage.MovingUpImageBounds.Width / 4, SpriteImage.MovingUpImageBounds.Height / 4);

                rect = SpriteImage.MovingUpImageBounds.Rectangle;
            }
            else
            {
                rect = new Rectangle();
            }

            TankMan.Tank.ImageBounds = rect;

            TankMan.Tank.Muzzle = new Point(TankMan.Tank.Location.X + TankMan.Tank.Size.Width / 2, TankMan.Tank.Location.Y);

            if (TankMan.Tank.Bullet != null && !TankMan.Tank.IsFire)
            {
                TankMan.Tank.Bullet.Location = TankMan.Tank.Muzzle;
            }

            currentMove = MoveUp;

            return rect;
        }


        /// <summary>
        /// Move a tank to the down
        /// </summary>
        /// <param name="location">New location</param>
        /// <returns>New rectangle of the tank</returns>
        public Rectangle MoveDown(Point location)
        {
            if (TankMan.Tank == null)
            {
                return new Rectangle();
            }

            if (!TankMan.Tank.IsAlive)
            {
                return TankMan.Tank.Rectangle;
            }

            TankMan.Tank.Location = location;

            Rectangle rect;

            if (SpriteImage != null)
            {

                TankMan.Tank.Size = new Size(SpriteImage.MovingDownImageBounds.Width / 4, SpriteImage.MovingDownImageBounds.Height / 4);

                rect = SpriteImage.MovingDownImageBounds.Rectangle;
            }
            else
            {
                rect = new Rectangle();
            }

            TankMan.Tank.ImageBounds = rect;

            TankMan.Tank.Muzzle = new Point(TankMan.Tank.Location.X + TankMan.Tank.Size.Width / 2, TankMan.Tank.Location.Y + TankMan.Tank.Size.Height);

            if (TankMan.Tank.Bullet != null && !TankMan.Tank.IsFire)
            {
                TankMan.Tank.Bullet.Location = TankMan.Tank.Muzzle;
            }

            currentMove = MoveDown;

            return rect;
        }

    }
}

public enum Directions
{
    Left, 
    Right, 
    Up, 
    Down
}
