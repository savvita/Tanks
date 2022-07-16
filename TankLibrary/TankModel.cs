using System.Drawing;

namespace TankLibrary
{
    public class TankModel
    {
        /// <summary>
        /// Current location of the tank
        /// </summary>
        public Point Location { get; set; }

        /// <summary>
        /// Speed of the tank
        /// </summary>
        public int Speed { get; set; } = 5;

        /// <summary>
        /// Current Size of the tank
        /// </summary>
        public Size Size { get; set; }

        /// <summary>
        /// Location of the muzzle of the tank
        /// </summary>
        public Point Muzzle { get; set; }

        /// <summary>
        /// Bullet
        /// </summary>
        public BulletModel? Bullet { get; set; }

        /// <summary>
        /// True if the tank is firing otherwise false
        /// </summary>
        public bool IsFire { get; set; }

        /// <summary>
        /// True if the tank is still alive otherwise false
        /// </summary>
        public bool IsAlive { get; set; }

        /// <summary>
        /// Current health of the tank
        /// </summary>
        public int Health { get; set; }

        /// <summary>
        /// Damage from the tank
        /// </summary>
        public int Damage { get; set; }

        /// <summary>
        /// True if the tank is hitted otherwise false
        /// </summary>
        public bool IsHit { get; set; }

        /// <summary>
        /// Bounds of the tank
        /// </summary>
        public Rectangle Rectangle
        {
            get => new Rectangle(Location, Size);
        }

        /// <summary>
        /// Bounds of the tank at the image sprite
        /// </summary>
        public Rectangle ImageBounds { get; set; }
    }
}