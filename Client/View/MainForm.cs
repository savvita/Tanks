using Client.Model;

namespace Client.View
{   
    public partial class MainForm : Form
    {
        private ClientModel? client;

        public MainForm()
        {
            InitializeComponent();
        }

        private void authorizationButton_Click(object sender, EventArgs e)
        {
            AuthorizationForm form = new AuthorizationForm();

            if(form.ShowDialog() == DialogResult.OK)
            {
                authorizationButton.Enabled = false;
                startButton.Enabled = true;
                shopButton.Enabled = true;
                statPanel.Visible = true;

                client = form.Client;

                SetStats();
            }
        }

        private void SetStats()
        {
            if(client == null)
            {
                return;
            }

            welcomeLabel.Text += client.Name;
            totalLabel.Text += client.TotalGames.ToString();
            wonGames.Text += client.WonGames.ToString();

            double rate = Math.Round((double)client.WonGames / client.TotalGames * 100, 2);
            winRateLabel.Text += $"{rate.ToString()}%";
        }

        private void shopButton_Click(object sender, EventArgs e)
        {
            if (client != null)
            {
                ShopView shop = new ShopView(client);
                shop.ShowDialog();
            }
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            if (client != null)
            {
                GameForm shop = new GameForm(client);
                shop.Show(this);
                this.Hide();
            }
        }
    }
}
