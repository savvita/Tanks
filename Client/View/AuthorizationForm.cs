using Client.Controller;
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
    public partial class AuthorizationForm : Form
    {
        private AuthorizationController controller = new AuthorizationController();
        
        public AuthorizationForm()
        {
            InitializeComponent();
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
                    (new GameForm(controller.Client)).Show();
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
                (new GameForm(controller.Client)).Show();
            }
        }
    }
}
