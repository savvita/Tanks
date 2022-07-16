namespace Client.View
{
    partial class MainForm
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
            this.authorizationButton = new System.Windows.Forms.Button();
            this.startButton = new System.Windows.Forms.Button();
            this.shopButton = new System.Windows.Forms.Button();
            this.welcomeLabel = new System.Windows.Forms.Label();
            this.statPanel = new System.Windows.Forms.Panel();
            this.winRateLabel = new System.Windows.Forms.Label();
            this.wonGames = new System.Windows.Forms.Label();
            this.totalLabel = new System.Windows.Forms.Label();
            this.statPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // authorizationButton
            // 
            this.authorizationButton.BackColor = System.Drawing.Color.DarkSlateGray;
            this.authorizationButton.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.authorizationButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Black;
            this.authorizationButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Black;
            this.authorizationButton.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.authorizationButton.ForeColor = System.Drawing.Color.White;
            this.authorizationButton.Location = new System.Drawing.Point(186, 355);
            this.authorizationButton.Name = "authorizationButton";
            this.authorizationButton.Size = new System.Drawing.Size(191, 51);
            this.authorizationButton.TabIndex = 0;
            this.authorizationButton.Text = "Authorization";
            this.authorizationButton.UseVisualStyleBackColor = false;
            this.authorizationButton.Click += new System.EventHandler(this.authorizationButton_Click);
            // 
            // startButton
            // 
            this.startButton.BackColor = System.Drawing.Color.DarkSlateGray;
            this.startButton.Enabled = false;
            this.startButton.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.startButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Black;
            this.startButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Black;
            this.startButton.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.startButton.ForeColor = System.Drawing.Color.White;
            this.startButton.Location = new System.Drawing.Point(186, 412);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(191, 51);
            this.startButton.TabIndex = 1;
            this.startButton.Text = "Start";
            this.startButton.UseVisualStyleBackColor = false;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // shopButton
            // 
            this.shopButton.BackColor = System.Drawing.Color.DarkSlateGray;
            this.shopButton.Enabled = false;
            this.shopButton.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.shopButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Black;
            this.shopButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Black;
            this.shopButton.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.shopButton.ForeColor = System.Drawing.Color.White;
            this.shopButton.Location = new System.Drawing.Point(186, 469);
            this.shopButton.Name = "shopButton";
            this.shopButton.Size = new System.Drawing.Size(191, 51);
            this.shopButton.TabIndex = 2;
            this.shopButton.Text = "Shop";
            this.shopButton.UseVisualStyleBackColor = false;
            this.shopButton.Click += new System.EventHandler(this.shopButton_Click);
            // 
            // welcomeLabel
            // 
            this.welcomeLabel.BackColor = System.Drawing.Color.Transparent;
            this.welcomeLabel.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.welcomeLabel.ForeColor = System.Drawing.Color.White;
            this.welcomeLabel.Location = new System.Drawing.Point(0, 3);
            this.welcomeLabel.Name = "welcomeLabel";
            this.welcomeLabel.Size = new System.Drawing.Size(366, 23);
            this.welcomeLabel.TabIndex = 3;
            this.welcomeLabel.Text = "Welcome, ";
            // 
            // statPanel
            // 
            this.statPanel.BackColor = System.Drawing.Color.Transparent;
            this.statPanel.Controls.Add(this.winRateLabel);
            this.statPanel.Controls.Add(this.wonGames);
            this.statPanel.Controls.Add(this.totalLabel);
            this.statPanel.Controls.Add(this.welcomeLabel);
            this.statPanel.Location = new System.Drawing.Point(12, 12);
            this.statPanel.Name = "statPanel";
            this.statPanel.Size = new System.Drawing.Size(386, 105);
            this.statPanel.TabIndex = 4;
            this.statPanel.Visible = false;
            // 
            // winRateLabel
            // 
            this.winRateLabel.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.winRateLabel.ForeColor = System.Drawing.Color.White;
            this.winRateLabel.Location = new System.Drawing.Point(8, 78);
            this.winRateLabel.Name = "winRateLabel";
            this.winRateLabel.Size = new System.Drawing.Size(358, 23);
            this.winRateLabel.TabIndex = 6;
            this.winRateLabel.Text = "WinRate: ";
            // 
            // wonGames
            // 
            this.wonGames.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.wonGames.ForeColor = System.Drawing.Color.White;
            this.wonGames.Location = new System.Drawing.Point(8, 55);
            this.wonGames.Name = "wonGames";
            this.wonGames.Size = new System.Drawing.Size(358, 23);
            this.wonGames.TabIndex = 5;
            this.wonGames.Text = "Games won: ";
            // 
            // totalLabel
            // 
            this.totalLabel.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.totalLabel.ForeColor = System.Drawing.Color.White;
            this.totalLabel.Location = new System.Drawing.Point(8, 32);
            this.totalLabel.Name = "totalLabel";
            this.totalLabel.Size = new System.Drawing.Size(358, 23);
            this.totalLabel.TabIndex = 4;
            this.totalLabel.Text = "Total games: ";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::Client.Properties.Resources.Logo;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(570, 531);
            this.Controls.Add(this.statPanel);
            this.Controls.Add(this.shopButton);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.authorizationButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Tank game";
            this.statPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Button authorizationButton;
        private Button startButton;
        private Button shopButton;
        private Label welcomeLabel;
        private Panel statPanel;
        private Label winRateLabel;
        private Label wonGames;
        private Label totalLabel;
    }
}