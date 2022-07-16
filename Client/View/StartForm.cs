using Client.Controller;
using Client.Model;

namespace Client.View
{
    public partial class StartForm : Form
    {
        private StartController? controller;

        public StartForm()
        {
            InitializeComponent();
        }

        public StartForm(ClientModel? client) : this()
        {
            if (client != null)
            {
                controller = new StartController(client);
                RefreshValues();
            }
            else
            {
                this.Close();
            }
        }

        private void RefreshValues()
        {
            if (controller != null)
            {
                this.coinsValue.Text = controller.Coins.ToString();
                this.healthValue.Text = controller.Health.ToString();
                this.damageValue.Text = controller.Damage.ToString();
            }
        }

        private void shopButton_Click(object sender, EventArgs e)
        {

            if (controller != null)
            {
                ShopView shop = new ShopView(controller.Coins);
                shop.ShowDialog();
                controller.AcceptNewValues(shop.Health, shop.Damage, shop.TotalCost);
                RefreshValues();
            }
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            (new GameForm(controller.Client)).Show();
        }
    }
}
