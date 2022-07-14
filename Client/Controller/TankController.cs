using Client.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Controller
{
    public class TankController
    {
        public TankModel Tank { get; set; }

        public Rectangle FieldBounds { get; set; }

        /// <summary>
        /// Describe sprite image this tank
        /// </summary>
        public SpriteImageModel SpriteImage { get; protected set; }



        public TankController(SpriteImageModel spriteImage)
        {
            Tank = new TankModel();
            Tank.Bullet = new BulletModel();

            SpriteImage = spriteImage;

            Tank.Image = SpriteImage.Image;
            Tank.Image.MakeTransparent();

            Tank.Size = new Size(SpriteImage.MovingRightImageBounds.Width / 4, SpriteImage.MovingRightImageBounds.Height / 4);

            Tank.TankImageBounds = SpriteImage.MovingRightImageBounds.Rectangle;
            currentMove = MoveRight;

            Tank.Muzzle = new Point(Tank.Location.X + Tank.Size.Width, Tank.Location.Y + Tank.Size.Height / 2);

            Tank.Bullet.Location = Tank.Muzzle;

        }

        private Func<Point, Rectangle> currentMove;

        public void Fire()
        {
            if (Tank.IsFire || !Tank.IsAlive)
            {
                return;
            }

            Tank.IsFire = true;
            Tank.Bullet.IsFlying = true;

            if (currentMove == MoveRight)
            {
                MoveBullet(Tank.Bullet.MoveRight);
            }
            else if (currentMove == MoveLeft)
            {
                MoveBullet(Tank.Bullet.MoveLeft);
            }
            else if (currentMove == MoveUp)
            {
                MoveBullet(Tank.Bullet.MoveUp);
            }
            else if (currentMove == MoveDown)
            {
                MoveBullet(Tank.Bullet.MoveDown);
            }
        }

        private void MoveBullet(Action move)
        {
            Task.Factory.StartNew(() =>
            {
                while (FieldBounds.Contains(Tank.Bullet.Location))
                {
                    if (!Tank.Bullet.IsFlying)
                    {
                        break;
                    }

                    move();
                    Thread.Sleep(100);
                }

                Tank.IsFire = false;
                Tank.Bullet.IsFlying = false;
                Tank.Bullet.Location = Tank.Muzzle;

            });
        }

        public Point GetNextLocation(Directions direction)
        {
            Point point = new Point();
            switch(direction)
            {
                case Directions.Left:
                    if (Tank.Location.X - Tank.Speed >= FieldBounds.Location.X)
                    {
                        point = new Point(Tank.Location.X - Tank.Speed, Tank.Location.Y);
                    }
                    else
                    {
                        point = new Point(FieldBounds.Location.X, Tank.Location.Y);
                    }
                    break;

                case Directions.Right:
                    if (Tank.Location.X + Tank.Speed + Tank.Size.Width <= FieldBounds.Location.X + FieldBounds.Width)
                    {
                        point = new Point(Tank.Location.X + Tank.Speed, Tank.Location.Y);
                    }
                    else
                    {
                        point = new Point(FieldBounds.Location.X + FieldBounds.Width - Tank.Size.Width, Tank.Location.Y);
                    }
                    break;

                case Directions.Up:
                    if (Tank.Location.Y - Tank.Speed >= FieldBounds.Location.Y)
                    {
                        point = new Point(Tank.Location.X, Tank.Location.Y - Tank.Speed);
                    }
                    else
                    {
                        point = new Point(Tank.Location.X, FieldBounds.Location.Y);
                    }
                    break;

                case Directions.Down:
                    if (Tank.Location.Y + Tank.Speed + Tank.Size.Height <= FieldBounds.Location.Y + FieldBounds.Height)
                    {
                        point = new Point(Tank.Location.X, Tank.Location.Y + Tank.Speed);
                    }
                    else
                    {
                        point = new Point(Tank.Location.X, FieldBounds.Location.Y + FieldBounds.Height - Tank.Size.Height);
                    }
                    break;
            }

            return point;
        }

        public Rectangle MoveLeft(Point location)
        {
            if (!Tank.IsAlive)
            {
                return Tank.TankRectangle;
            }

            Tank.Location = location;

            Tank.Size = new Size(SpriteImage.MovingLeftImageBounds.Width / 4, SpriteImage.MovingLeftImageBounds.Height / 4);

            Rectangle rect = SpriteImage.MovingLeftImageBounds.Rectangle;

            Tank.TankImageBounds = rect;

            Tank.Muzzle = new Point(Tank.Location.X, Tank.Location.Y + Tank.Size.Height / 2);

            if (!Tank.IsFire)
            {
                Tank.Bullet.Location = Tank.Muzzle;
            }

            currentMove = MoveLeft;

            return rect;
        }

        public Rectangle MoveRight(Point location)
        {
            if (!Tank.IsAlive)
            {
                return Tank.TankRectangle;
            }

            Tank.Location = location;

            Tank.Size = new Size(SpriteImage.MovingRightImageBounds.Width / 4, SpriteImage.MovingRightImageBounds.Height / 4);

            Rectangle rect = SpriteImage.MovingRightImageBounds.Rectangle;

            Tank.TankImageBounds = rect;

            Tank.Muzzle = new Point(Tank.Location.X + Tank.Size.Width, Tank.Location.Y + Tank.Size.Height / 2);

            if (!Tank.IsFire)
            {
                Tank.Bullet.Location = Tank.Muzzle;
            }

            currentMove = MoveRight;

            return rect;
        }

        public Rectangle MoveUp(Point location)
        {
            if (!Tank.IsAlive)
            {
                return Tank.TankRectangle;
            }

            Tank.Location = location;

            Tank.Size = new Size(SpriteImage.MovingUpImageBounds.Width / 4, SpriteImage.MovingUpImageBounds.Height / 4);

            Rectangle rect = SpriteImage.MovingUpImageBounds.Rectangle;

            Tank.TankImageBounds = rect;

            Tank.Muzzle = new Point(Tank.Location.X + Tank.Size.Width / 2, Tank.Location.Y);

            if (!Tank.IsFire)
            {
                Tank.Bullet.Location = Tank.Muzzle;
            }

            currentMove = MoveUp;

            return rect;
        }

        public Rectangle MoveDown(Point location)
        {
            if (!Tank.IsAlive)
            {
                return Tank.TankRectangle;
            }

            Tank.Location = location;

            Tank.Size = new Size(SpriteImage.MovingDownImageBounds.Width / 4, SpriteImage.MovingDownImageBounds.Height / 4);

            Rectangle rect = SpriteImage.MovingDownImageBounds.Rectangle;

            Tank.TankImageBounds = rect;

            Tank.Muzzle = new Point(Tank.Location.X + Tank.Size.Width / 2, Tank.Location.Y + Tank.Size.Height);

            if (!Tank.IsFire)
            {
                Tank.Bullet.Location = Tank.Muzzle;
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
