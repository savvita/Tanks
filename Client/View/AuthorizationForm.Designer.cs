namespace Client.View
{
    partial class AuthorizationForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tabControl = new System.Windows.Forms.TabControl();
            this.loginPage = new System.Windows.Forms.TabPage();
            this.errorLabel = new System.Windows.Forms.Label();
            this.cancelButton = new System.Windows.Forms.Button();
            this.loginButton = new System.Windows.Forms.Button();
            this.passwordTextBox = new System.Windows.Forms.TextBox();
            this.passwordLabel = new System.Windows.Forms.Label();
            this.loginTextBox = new System.Windows.Forms.TextBox();
            this.nameLabel = new System.Windows.Forms.Label();
            this.registerPage = new System.Windows.Forms.TabPage();
            this.errorRegLabel = new System.Windows.Forms.Label();
            this.confirmPasswordTextBox = new System.Windows.Forms.TextBox();
            this.confirmPassLabel = new System.Windows.Forms.Label();
            this.cancelRegButton = new System.Windows.Forms.Button();
            this.registerButton = new System.Windows.Forms.Button();
            this.passwordRegTextBox = new System.Windows.Forms.TextBox();
            this.passwordRegLabel = new System.Windows.Forms.Label();
            this.loginRegTextBox = new System.Windows.Forms.TextBox();
            this.loginRegLabel = new System.Windows.Forms.Label();
            this.tabControl.SuspendLayout();
            this.loginPage.SuspendLayout();
            this.registerPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.loginPage);
            this.tabControl.Controls.Add(this.registerPage);
            this.tabControl.Location = new System.Drawing.Point(-1, -3);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(505, 215);
            this.tabControl.TabIndex = 0;
            // 
            // loginPage
            // 
            this.loginPage.BackColor = System.Drawing.Color.White;
            this.loginPage.Controls.Add(this.errorLabel);
            this.loginPage.Controls.Add(this.cancelButton);
            this.loginPage.Controls.Add(this.loginButton);
            this.loginPage.Controls.Add(this.passwordTextBox);
            this.loginPage.Controls.Add(this.passwordLabel);
            this.loginPage.Controls.Add(this.loginTextBox);
            this.loginPage.Controls.Add(this.nameLabel);
            this.loginPage.Location = new System.Drawing.Point(4, 24);
            this.loginPage.Name = "loginPage";
            this.loginPage.Padding = new System.Windows.Forms.Padding(3);
            this.loginPage.Size = new System.Drawing.Size(497, 187);
            this.loginPage.TabIndex = 0;
            this.loginPage.Text = "Login";
            // 
            // errorLabel
            // 
            this.errorLabel.AutoSize = true;
            this.errorLabel.Location = new System.Drawing.Point(16, 149);
            this.errorLabel.Name = "errorLabel";
            this.errorLabel.Size = new System.Drawing.Size(176, 15);
            this.errorLabel.TabIndex = 15;
            this.errorLabel.Text = "* Only letters, digits or _ allowed";
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(405, 145);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 5;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // loginButton
            // 
            this.loginButton.Location = new System.Drawing.Point(314, 145);
            this.loginButton.Name = "loginButton";
            this.loginButton.Size = new System.Drawing.Size(75, 23);
            this.loginButton.TabIndex = 4;
            this.loginButton.Text = "Login";
            this.loginButton.UseVisualStyleBackColor = true;
            this.loginButton.Click += new System.EventHandler(this.loginButton_Click);
            // 
            // passwordTextBox
            // 
            this.passwordTextBox.Location = new System.Drawing.Point(124, 59);
            this.passwordTextBox.Name = "passwordTextBox";
            this.passwordTextBox.PasswordChar = '*';
            this.passwordTextBox.Size = new System.Drawing.Size(355, 23);
            this.passwordTextBox.TabIndex = 3;
            this.passwordTextBox.TextChanged += new System.EventHandler(this.loginPage_TextChanged);
            // 
            // passwordLabel
            // 
            this.passwordLabel.AutoSize = true;
            this.passwordLabel.Location = new System.Drawing.Point(16, 62);
            this.passwordLabel.Name = "passwordLabel";
            this.passwordLabel.Size = new System.Drawing.Size(57, 15);
            this.passwordLabel.TabIndex = 2;
            this.passwordLabel.Text = "Password";
            // 
            // loginTextBox
            // 
            this.loginTextBox.Location = new System.Drawing.Point(124, 19);
            this.loginTextBox.Name = "loginTextBox";
            this.loginTextBox.Size = new System.Drawing.Size(355, 23);
            this.loginTextBox.TabIndex = 1;
            this.loginTextBox.TextChanged += new System.EventHandler(this.loginPage_TextChanged);
            // 
            // nameLabel
            // 
            this.nameLabel.AutoSize = true;
            this.nameLabel.Location = new System.Drawing.Point(16, 22);
            this.nameLabel.Name = "nameLabel";
            this.nameLabel.Size = new System.Drawing.Size(37, 15);
            this.nameLabel.TabIndex = 0;
            this.nameLabel.Text = "Login";
            // 
            // registerPage
            // 
            this.registerPage.Controls.Add(this.errorRegLabel);
            this.registerPage.Controls.Add(this.confirmPasswordTextBox);
            this.registerPage.Controls.Add(this.confirmPassLabel);
            this.registerPage.Controls.Add(this.cancelRegButton);
            this.registerPage.Controls.Add(this.registerButton);
            this.registerPage.Controls.Add(this.passwordRegTextBox);
            this.registerPage.Controls.Add(this.passwordRegLabel);
            this.registerPage.Controls.Add(this.loginRegTextBox);
            this.registerPage.Controls.Add(this.loginRegLabel);
            this.registerPage.Location = new System.Drawing.Point(4, 24);
            this.registerPage.Name = "registerPage";
            this.registerPage.Padding = new System.Windows.Forms.Padding(3);
            this.registerPage.Size = new System.Drawing.Size(497, 187);
            this.registerPage.TabIndex = 1;
            this.registerPage.Text = "Register";
            this.registerPage.UseVisualStyleBackColor = true;
            // 
            // errorRegLabel
            // 
            this.errorRegLabel.AutoSize = true;
            this.errorRegLabel.Location = new System.Drawing.Point(16, 149);
            this.errorRegLabel.Name = "errorRegLabel";
            this.errorRegLabel.Size = new System.Drawing.Size(176, 15);
            this.errorRegLabel.TabIndex = 14;
            this.errorRegLabel.Text = "* Only letters, digits or _ allowed";
            // 
            // confirmPasswordTextBox
            // 
            this.confirmPasswordTextBox.Location = new System.Drawing.Point(124, 99);
            this.confirmPasswordTextBox.Name = "confirmPasswordTextBox";
            this.confirmPasswordTextBox.PasswordChar = '*';
            this.confirmPasswordTextBox.Size = new System.Drawing.Size(355, 23);
            this.confirmPasswordTextBox.TabIndex = 11;
            this.confirmPasswordTextBox.TextChanged += new System.EventHandler(this.registerPage_TextChanged);
            // 
            // confirmPassLabel
            // 
            this.confirmPassLabel.AutoSize = true;
            this.confirmPassLabel.Location = new System.Drawing.Point(16, 102);
            this.confirmPassLabel.Name = "confirmPassLabel";
            this.confirmPassLabel.Size = new System.Drawing.Size(104, 15);
            this.confirmPassLabel.TabIndex = 12;
            this.confirmPassLabel.Text = "Confirm password";
            // 
            // cancelRegButton
            // 
            this.cancelRegButton.Location = new System.Drawing.Point(405, 145);
            this.cancelRegButton.Name = "cancelRegButton";
            this.cancelRegButton.Size = new System.Drawing.Size(75, 23);
            this.cancelRegButton.TabIndex = 13;
            this.cancelRegButton.Text = "Cancel";
            this.cancelRegButton.UseVisualStyleBackColor = true;
            this.cancelRegButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // registerButton
            // 
            this.registerButton.Location = new System.Drawing.Point(314, 145);
            this.registerButton.Name = "registerButton";
            this.registerButton.Size = new System.Drawing.Size(75, 23);
            this.registerButton.TabIndex = 12;
            this.registerButton.Text = "Register";
            this.registerButton.UseVisualStyleBackColor = true;
            this.registerButton.Click += new System.EventHandler(this.registerButton_Click);
            // 
            // passwordRegTextBox
            // 
            this.passwordRegTextBox.Location = new System.Drawing.Point(124, 59);
            this.passwordRegTextBox.Name = "passwordRegTextBox";
            this.passwordRegTextBox.PasswordChar = '*';
            this.passwordRegTextBox.Size = new System.Drawing.Size(355, 23);
            this.passwordRegTextBox.TabIndex = 9;
            this.passwordRegTextBox.TextChanged += new System.EventHandler(this.registerPage_TextChanged);
            // 
            // passwordRegLabel
            // 
            this.passwordRegLabel.AutoSize = true;
            this.passwordRegLabel.Location = new System.Drawing.Point(16, 62);
            this.passwordRegLabel.Name = "passwordRegLabel";
            this.passwordRegLabel.Size = new System.Drawing.Size(57, 15);
            this.passwordRegLabel.TabIndex = 8;
            this.passwordRegLabel.Text = "Password";
            // 
            // loginRegTextBox
            // 
            this.loginRegTextBox.Location = new System.Drawing.Point(124, 19);
            this.loginRegTextBox.Name = "loginRegTextBox";
            this.loginRegTextBox.Size = new System.Drawing.Size(355, 23);
            this.loginRegTextBox.TabIndex = 7;
            this.loginRegTextBox.TextChanged += new System.EventHandler(this.registerPage_TextChanged);
            // 
            // loginRegLabel
            // 
            this.loginRegLabel.AutoSize = true;
            this.loginRegLabel.Location = new System.Drawing.Point(16, 22);
            this.loginRegLabel.Name = "loginRegLabel";
            this.loginRegLabel.Size = new System.Drawing.Size(37, 15);
            this.loginRegLabel.TabIndex = 6;
            this.loginRegLabel.Text = "Login";
            // 
            // AuthorizationForm
            // 
            this.AcceptButton = this.loginButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(504, 213);
            this.Controls.Add(this.tabControl);
            this.Name = "AuthorizationForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Authorization";
            this.tabControl.ResumeLayout(false);
            this.loginPage.ResumeLayout(false);
            this.loginPage.PerformLayout();
            this.registerPage.ResumeLayout(false);
            this.registerPage.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private TabControl tabControl;
        private TabPage loginPage;
        private Button cancelButton;
        private Button loginButton;
        private TextBox passwordTextBox;
        private Label passwordLabel;
        private TextBox loginTextBox;
        private Label nameLabel;
        private TabPage registerPage;
        private TextBox confirmPasswordTextBox;
        private Label confirmPassLabel;
        private Button cancelRegButton;
        private Button registerButton;
        private TextBox passwordRegTextBox;
        private Label passwordRegLabel;
        private TextBox loginRegTextBox;
        private Label loginRegLabel;
        private Label errorLabel;
        private Label errorRegLabel;
    }
}