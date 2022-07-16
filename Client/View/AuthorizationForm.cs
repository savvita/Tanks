using Client.Controller;
using Client.Model;

namespace Client.View
{
    public partial class AuthorizationForm : Form
    {
        private AuthorizationController controller = new AuthorizationController();

        public ClientModel? Client { get; private set; }
        
        public AuthorizationForm()
        {
            InitializeComponent();
            loginTextBox.Select();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void registerButton_Click(object sender, EventArgs e)
        {
            if (passwordRegTextBox.Text.Equals(confirmPasswordTextBox.Text))
            {
                bool success = controller.Register(loginRegTextBox.Text, passwordRegTextBox.Text);

                if (!success)
                {
                    MessageBox.Show("Register failed", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    DialogResult = DialogResult.OK;
                    Client = controller.Client;
                }
            }
            else
            {
                MessageBox.Show("Passwords do not match", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void loginButton_Click(object sender, EventArgs e)
        {
            bool success = controller.Login(loginTextBox.Text, passwordTextBox.Text);

            if (!success)
            {
                MessageBox.Show("Authorization failed", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                DialogResult = DialogResult.OK;
                Client = controller.Client;
            }
        }

        private void loginPage_TextChanged(object sender, EventArgs e)
        {
            if (sender is TextBox textbox)
            {
                if (textbox.Text.Any(x => !Char.IsLetterOrDigit(x) && x != '_'))
                {
                    textbox.ForeColor = Color.Red;
                    errorLabel.ForeColor = Color.Red;
                    loginButton.Enabled = false;
                }
                else
                {
                    textbox.ForeColor = Color.Black;
                    errorLabel.ForeColor = Color.Black;
                    loginButton.Enabled = true;
                }
            }
        }

        private void registerPage_TextChanged(object sender, EventArgs e)
        {
            if (sender is TextBox textbox)
            {
                if (textbox.Text.Any(x => !Char.IsLetterOrDigit(x) && x != '_'))
                {
                    textbox.ForeColor = Color.Red;
                    errorRegLabel.ForeColor = Color.Red;
                    registerButton.Enabled = false;
                }
                else
                {
                    textbox.ForeColor = Color.Black;
                    errorRegLabel.ForeColor = Color.Black;
                    registerButton.Enabled = true;
                }
            }
        }
    }
}
