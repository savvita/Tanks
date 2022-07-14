using Client.Controller;
using Client.Model;

namespace Client.View
{
    public partial class GameForm : Form
    {
        #region Graphics
        private Bitmap bufferedImage;
        private Bitmap tankImage;
        private Bitmap deadTankImage;
        private Bitmap bangImage;
        private Graphics graphics;
        #endregion

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

            this.deadTankImage = new Bitmap(Properties.Resources.DeadTankSprite);
            this.deadTankImage.MakeTransparent(Color.White);
            this.deadTankImage.MakeTransparent();


            this.bangImage = new Bitmap(Properties.Resources.Bang);
            this.bangImage.MakeTransparent(Color.White);
            this.bangImage.MakeTransparent();

            


        }

        public GameForm(ClientModel client) : this()
        {
            context = SynchronizationContext.Current;
            controller = new FieldController(new Rectangle(0, 0, this.Width - 10, this.Height - 40), client);
            controller.TankController.Tank.Name = client.Name;
            controller.Lost += Controller_Lost;
            controller.Win += Controller_Win;
            controller.Connect();


            Thread thread = new Thread(Drawing)
            {
                IsBackground = true
            };

            thread.Start();
        }

        private void Controller_Lost()
        {
            context.Send(SetLost, null);
        }

        private void Controller_Win()
        {

            context.Send(SetWin, null);
        }

        private void SetWin(object? obj)
        {
            Label result = new Label();
            result.Text = "You win!";
            result.Font = new Font("Segoe UI", 32, FontStyle.Bold);
            result.Size = new Size(400, 200);
            result.Location = new Point((this.ClientRectangle.Width - result.Width) / 2, (this.ClientRectangle.Height - result.Height) / 2);

            this.Controls.Add(result);
        }

        private void SetLost(object? obj)
        {
            Label result = new Label();
            result.Text = "You lost (((";
            result.Font = new Font("Segoe UI", 32, FontStyle.Bold);
            result.Size = new Size(400, 200);
            result.Location = new Point((this.ClientRectangle.Width - result.Width) / 2, (this.ClientRectangle.Height - result.Height) / 2);

            this.Controls.Add(result);
        }

        private SynchronizationContext context;

        //public GameForm(string name) : this()
        //{
        //    controller.TankController.Tank.Name = name;
        //    controller.Connect();
        //}

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

        private int[] fireCount = new int[5];
        private int[] bangCount = new int[5];

        private int fireTankCount = 0;
        private int bangTankCount = 0;

        private void Drawing()
        {
            //int fireCount = 0;
            //int fireEnemyCount = 0;
            //int bangCount = 0;
            //int bangEnemyCount = 0;


            while (true)
            {
                this.graphics.Clear(this.BackColor);

                DrawTank(controller.TankController.Tank);

                if (controller.Enemies != null)
                {
                    for (int i = 0; i < controller.Enemies.Count; i++)
                    {
                        DrawTank(controller.Enemies[i]);

                        if (controller.Enemies[i].IsFire)
                        {
                            if (fireCount[i] < 3)
                            {
                                DrawFire(controller.Enemies[i], fireCount[i]);
                                fireCount[i]++;
                            }

                            this.graphics.FillEllipse(Brushes.Black, new Rectangle(controller.Enemies[i].Bullet.Location, new Size(7, 7)));
                        }
                        else
                        {
                            fireCount[i] = 0;
                        }

                        if (controller.Enemies[i].IsHit)
                        {
                            if (bangCount[i] < 3)
                            {
                                DrawBang(controller.Enemies[i], bangCount[i]);

                                bangCount[i]++;
                            }
                            else
                            {
                                bangCount[i] = 0;
                                controller.Enemies[i].IsHit = false;
                            }
                        }
                    }
                }

                if (controller.TankController.Tank.IsFire)
                {
                    if (fireTankCount < 3)
                    {
                        DrawFire(controller.TankController.Tank, fireTankCount);
                        fireTankCount++;
                    }

                    this.graphics.FillEllipse(Brushes.Black, new Rectangle(controller.TankController.Tank.Bullet.Location, new Size(7, 7)));
                }
                else
                {
                    fireTankCount = 0;
                }

                if (controller.TankController.Tank.IsHit)
                {
                    if (bangTankCount < 3)
                    {
                        DrawBang(controller.TankController.Tank, bangTankCount);
                        bangTankCount++;
                    }
                    else
                    {
                        bangTankCount = 0;
                        controller.TankController.Tank.IsHit = false;
                    }
                }

                //===================================
                //if (controller.Enemy != null)
                //{
                //    DrawTank(controller.Enemy);

                //    if (controller.Enemy.IsFire)
                //    {
                //        if (fireEnemyCount < 3)
                //        {
                //            DrawFire(controller.Enemy, fireEnemyCount);
                //            fireEnemyCount++;
                //        }

                //        this.graphics.FillEllipse(Brushes.Black, new Rectangle(controller.Enemy.Bullet.Location, new Size(7, 7)));
                //    }
                //    else
                //    {
                //        fireEnemyCount = 0;
                //    }

                //    if (controller.Enemy.IsHit)
                //    {
                //        if (bangEnemyCount < 3)
                //        {
                //            DrawBang(controller.Enemy, bangEnemyCount);

                //            bangEnemyCount++;
                //        }
                //        else
                //        {
                //            bangEnemyCount = 0;
                //            controller.Enemy.IsHit = false;
                //        }
                //    }
                //}

                //if (controller.TankController.Tank.IsFire)
                //{
                //    if (fireCount < 3)
                //    {
                //        DrawFire(controller.TankController.Tank, fireCount);
                //        fireCount++;
                //    }

                //    this.graphics.FillEllipse(Brushes.Black, new Rectangle(controller.TankController.Tank.Bullet.Location, new Size(7, 7)));
                //}
                //else
                //{
                //    fireCount = 0;
                //}

                //if (controller.TankController.Tank.IsHit)
                //{
                //    if (bangCount < 3)
                //    {
                //        DrawBang(controller.TankController.Tank, bangCount);
                //        bangCount++;
                //    }
                //    else
                //    {
                //        bangCount = 0;
                //        controller.TankController.Tank.IsHit = false;
                //    }
                //}



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
                this.graphics.DrawImage(tankImage, tank.TankRectangle,
                    tank.TankImageBounds, GraphicsUnit.Pixel);
            }
            else
            {
                this.graphics.DrawImage(deadTankImage, tank.TankRectangle,
                    tank.TankImageBounds, GraphicsUnit.Pixel);
            }
        }
        private void DrawBang(TankModel tank, int k)
        {
            int width = 15 * (k + 1);
            int height = 15 * (k + 1);

            Point center = new Point(tank.Location.X + tank.TankRectangle.Width / 2 - width / 2,
                tank.Location.Y + tank.TankRectangle.Height / 2 - height / 2);

            this.graphics.DrawImage(bangImage,
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
