using Client.Controller;
using Client.Model;
using TankLibrary;

namespace Client.View
{
    public partial class GameForm : Form
    {
        #region Graphics
        private readonly Bitmap bufferedImage;
        private readonly Bitmap tankImage;
        private readonly Bitmap deadTankImage;
        private readonly Bitmap bangImage;
        private readonly Bitmap backImage;
        private readonly Graphics graphics;
        #endregion

        private readonly FieldController? controller;

        private readonly SynchronizationContext? context;

        private readonly CancellationToken token;
        private readonly CancellationTokenSource source;

        private int[] fireCount = new int[5];
        private int[] bangCount = new int[5];

        private readonly Font font = new Font("Segoe UI", 14);

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

            this.backImage = new Bitmap(Properties.Resources.Field);

            source = new CancellationTokenSource();
            token = source.Token;
        }

        public GameForm(ClientModel? client) : this()
        {
            if(client == null)
            {
                return;
            }

            this.Text = client.Name;

            context = SynchronizationContext.Current;

            controller = new FieldController(new Rectangle(0, 0, this.Width - 10, this.Height - 40), client);
            controller.Win += Controller_Win;
            controller.Lost += Controller_Lost;
            controller.ConnectToBattle();

            Task task = new Task(Drawing, token);
            task.Start();
        }


        private void Drawing()
        {
            while (true)
            {
                if(token.IsCancellationRequested)
                {
                    break;
                }

                if(controller == null || controller.TankMen == null)
                {
                    continue;
                }

                this.graphics.Clear(this.BackColor);

                graphics.DrawImage(backImage, 0, 0);

                for (int i = 0; i < controller.TankMen.Count; i++)
                {
                    if (controller.TankMen[i].Tank == null)
                    {
                        continue;
                    }

                    DrawTank(controller.TankMen[i].Tank, controller.TankMen[i].Name);

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

                        this.graphics.FillEllipse(Brushes.Red, new Rectangle(controller.TankMen[i].Tank!.Bullet!.Location, new Size(7, 7)));
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

        private void DrawTank(TankModel? tank, string? name)
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

            SizeF size;

            if (name != null)
            {               
                size = graphics.MeasureString(name, font);
                graphics.DrawString(name, font, Brushes.White, 
                    new Point(tank.Location.X + (tank.Size.Width - (int)size.Width) / 2, tank.Location.Y + tank.Size.Height + 5));
            }

            size = graphics.MeasureString(tank.Health.ToString(), font);
            graphics.DrawString(tank.Health.ToString(), font, Brushes.White, 
                new Point(tank.Location.X + (tank.Size.Width - (int)size.Width) / 2, tank.Location.Y - 50));
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
            resultLabel.Text = "You win!";
            resultLabel.Visible = true;
        }

        private void SetLost(object? obj)
        {
            resultLabel.Text = "You lost (((";
            resultLabel.Visible = true;
        }

        private void GameForm_FormClosing(object sender, FormClosedEventArgs e)
        {
            source.Cancel();
            controller?.LeaveBattle();
            Application.Exit();
        }
    }
}
