namespace Client.Model
{
    public class TankModel
    {
        [NonSerialized]
        public Bitmap Image;

        public Point Location { get; set; }
        public int Speed { get; set; } = 10;

        public Size Size { get; set; }

        public Point Muzzle { get; set; }

        public BulletModel Bullet { get; set; } = new BulletModel();

        public bool IsFire { get; set; }
    }
}
