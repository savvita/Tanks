using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Model
{
    public class ItemModel
    {
        public string Title { get; set; }

        public int Cost { get; set; }

        public ItemTypes Type { get; set; }

        public Image Image { get; set; }

        public int Value { get; set; }
    }

    public enum ItemTypes
    {
        Armor,
        Weapon
    }
}
