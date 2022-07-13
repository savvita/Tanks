using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Model
{
    public class BulletModel
    {
        public Point Location;

        public int Speed { get; set; } = 50;

        public bool IsFlying { get; set; } = false;

        public void MoveLeft()
        {
            Location = new Point(Location.X - Speed, Location.Y);
        }

        public void MoveRight()
        {
            Location = new Point(Location.X + Speed, Location.Y);
        }

        public void MoveUp()
        {
            Location = new Point(Location.X, Location.Y - Speed);
        }

        public void MoveDown()
        {
            Location = new Point(Location.X, Location.Y + Speed);
        }
    }
}
