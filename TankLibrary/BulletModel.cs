using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TankLibrary
{
    public class BulletModel
    {
        /// <summary>
        /// Current location of the bullet
        /// </summary>
        public Point Location { get; set; }

        /// <summary>
        /// Speed of the bullet
        /// </summary>
        public int Speed { get; set; } = 20;

        /// <summary>
        /// True if the bullet is flying now otherwise false
        /// </summary>
        public bool IsFlying { get; set; } = false;

        /// <summary>
        /// Move the bullet to the left
        /// </summary>
        public void MoveLeft()
        {
            Location = new Point(Location.X - Speed, Location.Y);
        }

        /// <summary>
        /// Move the bullet to the right
        /// </summary>
        public void MoveRight()
        {
            Location = new Point(Location.X + Speed, Location.Y);
        }

        /// <summary>
        /// Move the bullet to the up
        /// </summary>
        public void MoveUp()
        {
            Location = new Point(Location.X, Location.Y - Speed);
        }

        /// <summary>
        /// Move the bullet to the down
        /// </summary>
        public void MoveDown()
        {
            Location = new Point(Location.X, Location.Y + Speed);
        }
    }
}
