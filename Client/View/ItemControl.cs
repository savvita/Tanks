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
    public partial class ItemControl : UserControl
    {
        public int Cost { get; private set; }

        public event Action<ItemControl> ButtonClicked;

        public ItemTypes Type { get; }

        public int Value { get; }

        public void SetCost(int cost)
        {
            Cost = cost;
            this.costValue.Text = cost.ToString();
        }

        #region Constructors
        public ItemControl()
        {
            InitializeComponent();
        }

        public ItemControl(ItemModel model) : this()
        {
            this.titleLabel.Text = model.Title;
            
            switch(model.Type)
            {
                case ItemTypes.Armor:
                    this.propertyLabel.Text = "Health";
                    break;
                case ItemTypes.Weapon:
                    this.propertyLabel.Text = "Damage";
                    break;
            }

            this.propertyValue.Text = $"+ {model.Value}";

            this.costValue.Text = model.Cost.ToString();

            this.pictureBox.Image = model.Image;

            Cost = model.Cost;
            Type = model.Type;
            Value = model.Value;
        }
        #endregion

        private void Button_Clicked(object sender, EventArgs e)
        {
            if (ButtonClicked != null)
                ButtonClicked(this);
        }

        public void EnableButton() => this.buyButton.Enabled = true;

        public void DisableButton() => this.buyButton.Enabled = false;
    }
}
