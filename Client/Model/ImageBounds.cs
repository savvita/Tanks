namespace Client.Model
{
    public class ImageBounds
    {
        /// <summary>
        /// Start position X
        /// </summary>
        public int StartX { get; }

        /// <summary>
        /// Start position Y
        /// </summary>
        public int StartY { get; }

        /// <summary>
        /// Width of the image
        /// </summary>
        public int Width { get; }

        /// <summary>
        /// Height of the image
        /// </summary>
        public int Height { get; }

        /// <summary>
        /// Rectangle of the image
        /// </summary>
        public Rectangle Rectangle
        {
            get => new Rectangle(StartX, StartY, Width, Height);
        }

        public ImageBounds(int startX, int startY, int width, int height)
        {
            StartX = startX;
            StartY = startY;
            Width = width;
            Height = height;
        }
    }

}
