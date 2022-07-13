using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Model
{
    public class ImageBounds
    {
        public int StartX { get; }
        public int StartY { get; }

        public int Width { get; }
        public int Height { get; }

        public ImageBounds(int startX, int startY, int width, int height)
        {
            StartX = startX;
            StartY = startY;
            Width = width;
            Height = height;
        }

        public Rectangle Rectangle
        {
            get => new Rectangle(StartX, StartY, Width, Height);
        }
    }

}
