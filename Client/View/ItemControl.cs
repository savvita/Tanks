using Client.Controller;
using Client.Model;

namespace Client.View
{
    public partial class ItemControl : UserControl
    {
        /// <summary>
        /// Cost of the item
        /// </summary>
        public int Cost { get; private set; }    

        /// <summary>
        /// Type of the item
        /// </summary>
        public ItemTypes Type { get; }

        /// <summary>
        /// Value of the item
        /// </summary>
        public int Value { get; }

        public event Action<ItemControl>? ButtonClicked;

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
            ButtonClicked?.Invoke(this);
        }

        public void EnableButton() => this.buyButton.Enabled = true;

        public void DisableButton() => this.buyButton.Enabled = false;
    }
}
