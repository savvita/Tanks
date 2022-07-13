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
        public TankModel Tank { get; private set; }

        public Rectangle FieldBounds;

        /// <summary>
        /// Describe sprite image this tank
        /// </summary>
        public SpriteImageModel SpriteImage { get; protected set; }

        /// <summary>
        /// Bounds of the tank
        /// </summary>
        public Rectangle TankRectangle
        {
            get => new Rectangle(Tank.Location, Tank.Size);
        }

        public Rectangle TankImageBounds { get; private set; }

        public TankController(SpriteImageModel spriteImage)
        {
            Tank = new TankModel();

            SpriteImage = spriteImage;

            Tank.Image = SpriteImage.Image;
            Tank.Image.MakeTransparent();

            Tank.Size = new Size(SpriteImage.MovingRightImageBounds.Width / 4, SpriteImage.MovingRightImageBounds.Height / 4);

            TankImageBounds = SpriteImage.MovingRightImageBounds.Rectangle;
            currentMove = MoveRight;
        }

        private Func<Rectangle> currentMove;

        public void Fire()
        {
            if(Tank.IsFire)
            {
                return;
            }

            Tank.IsFire = true;

            if(currentMove == MoveRight)
            {
                Task.Factory.StartNew(() =>
                {
                    while (FieldBounds.Contains(Tank.Bullet.Location))
                    {
                        Tank.Bullet.MoveRight();
                        Thread.Sleep(100);
                    }

                    Tank.IsFire = false;

                });
            }
            else if (currentMove == MoveLeft)
            {
                Task.Factory.StartNew(() =>
                {
                    while (FieldBounds.Contains(Tank.Bullet.Location))
                    {
                        Tank.Bullet.MoveLeft();
                        Thread.Sleep(500);
                    }

                    Tank.IsFire = false;

                });
            }
            else if (currentMove == MoveUp)
            {
                Task.Factory.StartNew(() =>
                {
                    while (FieldBounds.Contains(Tank.Bullet.Location))
                    {
                        Tank.Bullet.MoveUp();
                        Thread.Sleep(500);
                    }

                    Tank.IsFire = false;

                });
            }
            else if (currentMove == MoveDown)
            {
                Task.Factory.StartNew(() =>
                {
                    while (FieldBounds.Contains(Tank.Bullet.Location))
                    {
                        Tank.Bullet.MoveDown();
                        Thread.Sleep(500);
                    }

                    Tank.IsFire = false;

                });
            }
        }

        public Rectangle MoveLeft()
        {
            if (Tank.Location.X - Tank.Speed >= FieldBounds.Location.X)
            {
                Tank.Location = new Point(Tank.Location.X - Tank.Speed, Tank.Location.Y);
            }
            else
            {
                Tank.Location = new Point(FieldBounds.Location.X, Tank.Location.Y);
            }

            Tank.Size = new Size(SpriteImage.MovingLeftImageBounds.Width / 4, SpriteImage.MovingLeftImageBounds.Height / 4);

            Rectangle rect = SpriteImage.MovingLeftImageBounds.Rectangle;

            TankImageBounds = rect;

            Tank.Muzzle = new Point(Tank.Location.X, Tank.Location.Y + Tank.Size.Height / 2);

            if (!Tank.IsFire)
            {
                Tank.Bullet.Location = Tank.Muzzle;
            }

            currentMove = MoveLeft;

            return rect;
        }

        public Rectangle MoveRight()
        {
            if (Tank.Location.X + Tank.Speed + Tank.Size.Width <= FieldBounds.Location.X + FieldBounds.Width)
            {
                Tank.Location = new Point(Tank.Location.X + Tank.Speed, Tank.Location.Y);
            }
            else
            {
                Tank.Location = new Point(FieldBounds.Location.X + FieldBounds.Width - Tank.Size.Width, Tank.Location.Y);
            }

            Tank.Size = new Size(SpriteImage.MovingRightImageBounds.Width / 4, SpriteImage.MovingRightImageBounds.Height / 4);

            Rectangle rect = SpriteImage.MovingRightImageBounds.Rectangle;

            TankImageBounds = rect;

            Tank.Muzzle = new Point(Tank.Location.X + Tank.Size.Width, Tank.Location.Y + Tank.Size.Height / 2);

            if (!Tank.IsFire)
            {
                Tank.Bullet.Location = Tank.Muzzle;
            }

            currentMove = MoveRight;

            return rect;
        }

        public Rectangle MoveUp()
        {
            if (Tank.Location.Y - Tank.Speed >= FieldBounds.Location.Y)
            {
                Tank.Location = new Point(Tank.Location.X, Tank.Location.Y - Tank.Speed);
            }
            else
            {
                Tank.Location = new Point(Tank.Location.X, FieldBounds.Location.Y);
            }

            Tank.Size = new Size(SpriteImage.MovingUpImageBounds.Width / 4, SpriteImage.MovingUpImageBounds.Height / 4);

            Rectangle rect = SpriteImage.MovingUpImageBounds.Rectangle;

            TankImageBounds = rect;

            Tank.Muzzle = new Point(Tank.Location.X + Tank.Size.Width / 2, Tank.Location.Y);

            if (!Tank.IsFire)
            {
                Tank.Bullet.Location = Tank.Muzzle;
            }

            currentMove = MoveUp;

            return rect;
        }

        public Rectangle MoveDown()
        {
            if (Tank.Location.Y + Tank.Speed + Tank.Size.Height <= FieldBounds.Location.Y + FieldBounds.Height)
            {
                Tank.Location = new Point(Tank.Location.X, Tank.Location.Y + Tank.Speed);
            }
            else
            {
                Tank.Location = new Point(Tank.Location.X, FieldBounds.Location.Y + FieldBounds.Height - Tank.Size.Height);
            }

            Tank.Size = new Size(SpriteImage.MovingDownImageBounds.Width / 4, SpriteImage.MovingDownImageBounds.Height / 4);

            Rectangle rect = SpriteImage.MovingDownImageBounds.Rectangle;

            TankImageBounds = rect;

            Tank.Muzzle = new Point(Tank.Location.X + Tank.Size.Width / 2, Tank.Location.Y + Tank.Size.Height);
            
            if(!Tank.IsFire)
            {
                Tank.Bullet.Location = Tank.Muzzle;
            }

            currentMove = MoveDown;

            return rect;
        }
    }
}
