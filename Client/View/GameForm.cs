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

        private TankController controller;
        public GameForm()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.BackgroundImageLayout = ImageLayout.Center;

            this.bufferedImage = new Bitmap(this.Width, this.Height);
            this.graphics = Graphics.FromImage(bufferedImage);


        }

        public GameForm(TankController controller) : this()
        {
            this.controller = controller;
            this.controller.FieldBounds = new Rectangle(0, 0, this.Width - 10, this.Height - 40);

            this.tankImage = new Bitmap(Properties.Resources.Tank);
            this.tankImage.MakeTransparent(Color.White);
            this.tankImage.MakeTransparent();

            this.fireImage = new Bitmap(Properties.Resources.Fire);
            this.fireImage.MakeTransparent(Color.White);
            this.fireImage.MakeTransparent(Color.FromArgb(238, 238, 238));

            f();
        }

        private void f()
        {
            this.graphics.Clear(this.BackColor);
            this.graphics.DrawImage(controller.Tank.Image, controller.TankRectangle, controller.TankImageBounds, GraphicsUnit.Pixel);

            //this.graphics.DrawImage(fireImage, new Rectangle(100, 100, 230, 360), new Rectangle(0, 0, 900, 500), GraphicsUnit.Pixel);

            this.BackgroundImage = bufferedImage; 
            this.Invalidate();
        }

        private void GameForm_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Left)
            {
                controller.MoveLeft();
            }
            else if(e.KeyCode == Keys.Right)
            {
                controller.MoveRight();
            }
            else if (e.KeyCode == Keys.Up)
            {
                controller.MoveUp();
            }
            else if (e.KeyCode == Keys.Down)
            {
                controller.MoveDown();
            }

            f();
        }
    }
}
