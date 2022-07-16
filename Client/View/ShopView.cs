using Client.Model;

namespace Client.View
{
    public partial class ShopView : Form
    {
        private List<ItemControl> controls = new List<ItemControl>();

        private int coinsToUse;

        public int Health { get; set; } = 0;

        public int Damage { get; set; } = 0;

        public int TotalCost { get; set; }



        public ShopView()
        {
            InitializeComponent();
        }

        public ShopView(int coins) : this()
        {
            this.coinsToUse = coins;
            InitializeControls();
            RefreshControls();
        }

        private void InitializeControls()
        {
            controls.Add(new ItemControl(new ItemModel() { Cost = 10, Title = "Armor", Type = ItemTypes.Armor, Value = 5 }));
            controls.Add(new ItemControl(new ItemModel() { Cost = 20, Title = "Weapon", Type = ItemTypes.Weapon, Value = 15 }));

            SetLocations();

            controls.ForEach((control) => control.ButtonClicked += Control_AddButtonClicked);

            this.Controls.AddRange(controls.ToArray());
        }

        private void SetLocations()
        {
            int margin = 20;

            int width = margin;
            int height = margin;

            foreach (ItemControl control in controls)
            {
                control.Location = new Point(width, height);

                height += control.Height + margin;

                if (height + control.Height >= this.ClientRectangle.Height)
                {
                    height = margin;
                    width += control.Width + margin;
                }
            }
        }

        private void Control_AddButtonClicked(ItemControl control)
        {
            switch (control.Type)
            {
                case ItemTypes.Armor:
                    Health += control.Value;
                    break;
                case ItemTypes.Weapon:
                    Damage += control.Value;
                    break;
            }

            TotalCost += control.Cost;

            coinsToUse -= control.Cost;
            RefreshControls();
        }

        public void RefreshControls()
        {
            controls.ForEach((item) =>
            {
                if (item.Cost <= coinsToUse)
                    item.EnableButton();
                else
                    item.DisableButton();
            });
        }

        public event Action<ItemTypes>? ButtonClicked;


        protected void OnButtonClicked(ItemTypes type)
        {
            if (ButtonClicked != null)
                ButtonClicked(type);
        }
    }
}
