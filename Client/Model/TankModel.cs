namespace Client.Model
{
    public class TankModel
    {
        [NonSerialized]
        public Bitmap Image;

        public Point Location { get; set; }
        public int Speed { get; set; } = 5;

        public Size Size { get; set; }

        public Point Muzzle { get; set; }

        public BulletModel Bullet { get; set; }

        public bool IsFire { get; set; }

        public bool IsAlive { get; set; } = true;

        /// <summary>
        /// Bounds of the tank
        /// </summary>
        public Rectangle TankRectangle
        {
            get => new Rectangle(Location, Size);
        }

        public Rectangle TankImageBounds { get; set; }
    }
}
