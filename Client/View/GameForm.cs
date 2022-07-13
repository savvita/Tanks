using Client.Controller;
using Client.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client.View
{
    public partial class GameForm : Form
    {
        #region Graphics
        private Bitmap bufferedImage;
        private Bitmap tankImage;
        private Bitmap fireImage;
        private Graphics graphics;
        #endregion

        //private TankController controller;
        private FieldController controller;
        public GameForm()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.BackgroundImageLayout = ImageLayout.Center;

            this.bufferedImage = new Bitmap(this.Width, this.Height);
            this.graphics = Graphics.FromImage(bufferedImage);

            this.tankImage = new Bitmap(Properties.Resources.Tank);
            this.tankImage.MakeTransparent(Color.White);
            this.tankImage.MakeTransparent();

            controller = new FieldController(new Rectangle(0, 0, this.Width - 10, this.Height - 40));

            //this.fireImage = new Bitmap(Properties.Resources.Fire);
            //this.fireImage.MakeTransparent(Color.White);
            //this.fireImage.MakeTransparent(Color.FromArgb(238, 238, 238));

            Thread thread = new Thread(Drawing)
            {
                IsBackground = true
            };

            thread.Start();

        }

        //public GameForm(FieldController controller) : this()
        //{
        //    //this.controller = controller;
        //    //this.controller.TankController.FieldBounds = new Rectangle(0, 0, this.Width - 10, this.Height - 40);

        //    this.tankImage = new Bitmap(Properties.Resources.Tank);
        //    this.tankImage.MakeTransparent(Color.White);
        //    this.tankImage.MakeTransparent();

        //    //this.fireImage = new Bitmap(Properties.Resources.Fire);
        //    //this.fireImage.MakeTransparent(Color.White);
        //    //this.fireImage.MakeTransparent(Color.FromArgb(238, 238, 238));

        //    Thread thread = new Thread(Drawing)
        //    {
        //        IsBackground = true
        //    };

        //    thread.Start();
        //}

        private void Drawing()
        {
            int fireCount = 0;
            int fireEnemyCount = 0;
            int bangCount = 0;
            int bangEnemyCount = 0;
            while (true)
            {
                this.graphics.Clear(this.BackColor);

                DrawTank(controller.TankController.Tank);

                if (controller.Enemy != null)
                {
                    DrawTank(controller.Enemy);

                    if (controller.Enemy.IsFire)
                    {
                        if (fireEnemyCount < 3)
                        {
                            DrawFire(controller.Enemy, fireEnemyCount);
                            fireEnemyCount++;
                        }

                        this.graphics.FillEllipse(Brushes.Green, new Rectangle(controller.Enemy.Bullet.Location, new Size(5, 5)));
                    }
                    else
                    {
                        fireEnemyCount = 0;
                    }

                    if (!controller.Enemy.IsAlive)
                    {
                        if (bangEnemyCount < 5)
                        {
                            DrawBang(controller.Enemy, bangEnemyCount);

                            bangEnemyCount++;
                        }
                    }
                }

                if (controller.TankController.Tank.IsFire)
                {
                    if (fireCount < 3)
                    {
                        DrawFire(controller.TankController.Tank, fireCount);
                        fireCount++;
                    }

                    this.graphics.FillEllipse(Brushes.Green, new Rectangle(controller.TankController.Tank.Bullet.Location, new Size(5, 5)));
                }

                if (!controller.TankController.Tank.IsAlive)
                {
                    if (bangCount < 5)
                    {
                        DrawBang(controller.TankController.Tank, bangCount);
                        bangCount++;
                    }
                }



                this.BackgroundImage = bufferedImage;
                this.Invalidate();

                Thread.Sleep(200);
            }
        }

        private void DrawFire(TankModel tank, int k)
        {
            int width = 10 * (k + 1);
            int height = 10 * (k + 1);

            Point center = new Point(tank.Muzzle.X - width / 2, tank.Muzzle.Y - height / 2);

            this.graphics.FillEllipse(Brushes.Red, new Rectangle(center, new Size(width, height)));
        }

        private void DrawTank(TankModel tank)
        {
            if (tank.IsAlive)
            {
                this.graphics.DrawImage(Properties.Resources.Tank, tank.TankRectangle,
                    tank.TankImageBounds, GraphicsUnit.Pixel);
            }
            else
            {
                this.graphics.DrawImage(Properties.Resources.DeadTankSprite, tank.TankRectangle,
                    tank.TankImageBounds, GraphicsUnit.Pixel);
            }
        }
        private void DrawBang(TankModel tank, int k)
        {
            int width = 15 * (k + 1);
            int height = 15 * (k + 1);

            Point center = new Point(tank.Location.X + tank.TankRectangle.Width / 2 - width / 2,
                tank.Location.Y + tank.TankRectangle.Height / 2 - height / 2);

            this.graphics.DrawImage(Properties.Resources.Bang,
                                new Rectangle(center, new Size(width, height)),
                                new Rectangle(0, 0, 200, 200), GraphicsUnit.Pixel);
        }

        private void GameForm_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Left)
            {
                controller.Move(Directions.Left);
            }
            else if(e.KeyCode == Keys.Right)
            {
                controller.Move(Directions.Right);
            }
            else if (e.KeyCode == Keys.Up)
            {
                controller.Move(Directions.Up);
            }
            else if (e.KeyCode == Keys.Down)
            {
                controller.Move(Directions.Down);
            }
            else if(e.KeyCode == Keys.Space)
            {
                controller.TankController.Fire();
            }
        }

        private void GameForm_MouseClick(object sender, MouseEventArgs e)
        {
            //controller.TankController.Fire();
        }
    }
}
