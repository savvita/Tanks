using Client.Controller;
using Client.Model;
using TankLibrary;

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

        private FieldController? controller;

        private SynchronizationContext? context;

        private int[] fireCount = new int[5];
        private int[] bangCount = new int[5];

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
            controller.Win += Controller_Win;
            controller.Lost += Controller_Lost;
            controller.Connect();

            Thread thread = new Thread(Drawing)
            {
                IsBackground = true
            };

            thread.Start();
        }

        private void Controller_Lost()
        {
            context?.Send(SetLost, null);
        }

        private void Controller_Win()
        {
            context?.Send(SetWin, null);
        }

        private void SetWin(object? obj)
        {
            Label result = new Label();
            result.Text = "You win!";
            result.BackColor = Color.Transparent;
            result.Font = new Font("Segoe UI", 32, FontStyle.Bold);
            result.Size = new Size(400, 200);
            result.Location = new Point((this.ClientRectangle.Width - result.Width) / 2, (this.ClientRectangle.Height - result.Height) / 2);

            this.Controls.Add(result);
        }

        private void SetLost(object? obj)
        {
            Label result = new Label();
            result.Text = "You lost (((";
            result.BackColor = Color.Transparent;
            result.Font = new Font("Segoe UI", 32, FontStyle.Bold);
            result.Size = new Size(400, 200);
            result.Location = new Point((this.ClientRectangle.Width - result.Width) / 2, (this.ClientRectangle.Height - result.Height) / 2);

            this.Controls.Add(result);
        }

        private void Drawing()
        {
            while (true)
            {
                if(controller == null || controller.TankMen == null)
                {
                    continue;
                }

                this.graphics.Clear(this.BackColor);

                for (int i = 0; i < controller.TankMen.Count; i++)
                {
                    DrawTank(controller.TankMen[i].Tank);
                    graphics.DrawString(controller.TankMen[i].Name, new Font("Segoe UI", 15), Brushes.Green, new Point(
                        controller.TankMen[i].Tank.Location.X, controller.TankMen[i].Tank.Location.Y + controller.TankMen[i].Tank.Size.Height + 5));

                    graphics.DrawString(controller.TankMen[i].Tank.Health.ToString(), new Font("Segoe UI", 15), Brushes.Green, new Point(
                        controller.TankMen[i].Tank.Location.X, controller.TankMen[i].Tank.Location.Y - 50));

                    if (controller.TankMen[i].Tank == null)
                    {
                        continue;
                    }

                    if (controller.TankMen[i].Tank!.IsFire)
                    {
                        if(controller.TankMen[i].Tank!.Bullet == null)
                        {
                            continue;
                        }
                        if (fireCount[i] < 3)
                        {
                            DrawFire(controller.TankMen[i].Tank!, fireCount[i]);
                            fireCount[i]++;
                        }

                        this.graphics.FillEllipse(Brushes.Black, new Rectangle(controller.TankMen[i].Tank!.Bullet!.Location, new Size(7, 7)));
                    }
                    else
                    {
                        fireCount[i] = 0;
                    }

                    if (controller.TankMen[i].Tank!.IsHit)
                    {
                        if (bangCount[i] < 3)
                        {
                            DrawBang(controller.TankMen[i].Tank, bangCount[i]);

                            bangCount[i]++;
                        }
                        else
                        {
                            bangCount[i] = 0;
                            controller.TankMen[i].Tank!.IsHit = false;
                        }
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

        private void DrawTank(TankModel? tank)
        {
            if (tank == null)
            {
                return;
            }

            if (tank.IsAlive)
            {
                this.graphics.DrawImage(tankImage, tank.Rectangle,
                    tank.ImageBounds, GraphicsUnit.Pixel);
            }
            else
            {
                this.graphics.DrawImage(deadTankImage, tank.Rectangle,
                    tank.ImageBounds, GraphicsUnit.Pixel);
            }
        }
        private void DrawBang(TankModel? tank, int k)
        {
            if(tank == null)
            {
                return;
            }

            int width = 15 * (k + 1);
            int height = 15 * (k + 1);

            Point center = new Point(tank.Location.X + tank.Rectangle.Width / 2 - width / 2,
                tank.Location.Y + tank.Rectangle.Height / 2 - height / 2);

            this.graphics.DrawImage(bangImage,
                                new Rectangle(center, new Size(width, height)),
                                new Rectangle(0, 0, 200, 200), GraphicsUnit.Pixel);
        }

        private void GameForm_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Left)
            {
                controller?.Move(Directions.Left);
            }
            else if(e.KeyCode == Keys.Right)
            {
                controller?.Move(Directions.Right);
            }
            else if (e.KeyCode == Keys.Up)
            {
                controller?.Move(Directions.Up);
            }
            else if (e.KeyCode == Keys.Down)
            {
                controller?.Move(Directions.Down);
            }
            else if (e.KeyCode == Keys.Space)
            {
                controller?.Fire();
            }
        }

        private void GameForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            controller?.Close();
        }
    }
}
