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


        }

        public GameForm(FieldController controller) : this()
        {
            this.controller = controller;
            this.controller.TankController.FieldBounds = new Rectangle(0, 0, this.Width - 10, this.Height - 40);

            this.tankImage = new Bitmap(Properties.Resources.Tank);
            this.tankImage.MakeTransparent(Color.White);
            this.tankImage.MakeTransparent();

            this.fireImage = new Bitmap(Properties.Resources.Fire);
            this.fireImage.MakeTransparent(Color.White);
            this.fireImage.MakeTransparent(Color.FromArgb(238, 238, 238));

            Thread thread = new Thread(Drawing)
            {
                IsBackground = true
            };

            thread.Start();
        }

        private void Drawing()
        {
            while (true)
            {
                this.graphics.Clear(this.BackColor);
                this.graphics.DrawImage(controller.TankController.Tank.Image, controller.TankController.TankRectangle, 
                    controller.TankController.TankImageBounds, GraphicsUnit.Pixel);

                if(controller.Enemy != null)
                {
                    this.graphics.DrawImage(controller.TankController.Tank.Image, new Rectangle(0,0,100,100),
                    new Rectangle(0,0,200,300), GraphicsUnit.Pixel);
                }

                if (controller.TankController.Tank.IsFire)
                {
                    this.graphics.FillEllipse(Brushes.Green, new Rectangle(controller.TankController.Tank.Bullet.Location, new Size(5, 5)));
                }

                this.BackgroundImage = bufferedImage;
                this.Invalidate();

                Thread.Sleep(200);
            }
        }

        private void GameForm_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Left)
            {
                controller.TankController.MoveLeft();
            }
            else if(e.KeyCode == Keys.Right)
            {
                controller.TankController.MoveRight();
            }
            else if (e.KeyCode == Keys.Up)
            {
                controller.TankController.MoveUp();
            }
            else if (e.KeyCode == Keys.Down)
            {
                controller.TankController.MoveDown();
            }
            else if(e.KeyCode == Keys.Space)
            {
                controller.TankController.Fire();
            }
        }

        private void GameForm_MouseClick(object sender, MouseEventArgs e)
        {
            controller.TankController.Fire();
        }
    }
}
