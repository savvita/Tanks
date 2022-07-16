using Client.Controller;
using Client.Model;

namespace Client.View
{
    public partial class ShopView : Form
    {
        private List<ItemControl> controls = new List<ItemControl>();
        private ShopController? controller;

        private int coinsToUse;
        private int health = 0;
        private int damage = 0;

        public ShopView()
        {
            InitializeComponent();
        }

        public ShopView(ClientModel client) : this()
        {
            controller = new ShopController(client);
            this.coinsToUse = controller.Coins;
            InitializeControls();
            RefreshControls();
            RefreshValues();

            this.pictureBox.Image = Properties.Resources.TankPreview;
        }

        private void InitializeControls()
        {
            controls.Add(new ItemControl(new ItemModel() 
            { 
                Cost = 50, 
                Title = "Light armor", 
                Type = ItemTypes.Armor, 
                Value = 5,
                Image = Properties.Resources.Helmet
            }));

            controls.Add(new ItemControl(new ItemModel() 
            { 
                Cost = 80, 
                Title = "Hard armor", 
                Type = ItemTypes.Armor, 
                Value = 10,
                Image = Properties.Resources.Plantain
            }));

            controls.Add(new ItemControl(new ItemModel() { 
                Cost = 80, 
                Title = "Weapon", 
                Type = ItemTypes.Weapon, 
                Value = 15,
                Image = Properties.Resources.Slingshot
            }));

            SetLocations();

            controls.ForEach((control) => control.ButtonClicked += Control_AddButtonClicked);

            this.itemsPanel.Controls.AddRange(controls.ToArray());
        }

        private void SetLocations()
        {
            int margin = 20;

            int width = margin;
            int height = margin;

            foreach (ItemControl control in controls)
            {
                control.Location = new Point(width, height);

                width += control.Width + margin;

                if (width + control.Width >= this.ClientRectangle.Width)
                {
                    width = margin;
                    height += control.Height + margin;
                }
            }
        }

        private void Control_AddButtonClicked(ItemControl control)
        {
            if(controller == null)
            {
                return;
            }

            switch (control.Type)
            {
                case ItemTypes.Armor:
                    health = control.Value;
                    break;
                case ItemTypes.Weapon:
                    damage = control.Value;
                    break;
            }

            controller.AcceptNewValues(health, damage, control.Cost);

            coinsToUse -= control.Cost;
            health = 0;
            damage = 0;
            RefreshControls();
            RefreshValues();
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


        private void RefreshValues()
        {
            if (controller != null)
            {
                this.coinsLabel.Text = $"Coins: {controller.Coins.ToString()}";
                this.healthLabel.Text = $"Health: {controller.Health.ToString()}";
                this.damageLabel.Text = $"Damage: {controller.Damage.ToString()}";
            }
        }

        public event Action<ItemTypes>? ButtonClicked;

        protected void OnButtonClicked(ItemTypes type)
        {
            if (ButtonClicked != null)
                ButtonClicked(type);
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
