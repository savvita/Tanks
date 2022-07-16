namespace Client.View
{
    partial class ShopView
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
            this.itemsPanel = new System.Windows.Forms.Panel();
            this.valuesPanel = new System.Windows.Forms.Panel();
            this.damageLabel = new System.Windows.Forms.Label();
            this.healthLabel = new System.Windows.Forms.Label();
            this.coinsLabel = new System.Windows.Forms.Label();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.closeButton = new System.Windows.Forms.Button();
            this.valuesPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // itemsPanel
            // 
            this.itemsPanel.BackColor = System.Drawing.Color.Transparent;
            this.itemsPanel.Location = new System.Drawing.Point(12, 1);
            this.itemsPanel.Name = "itemsPanel";
            this.itemsPanel.Size = new System.Drawing.Size(841, 502);
            this.itemsPanel.TabIndex = 0;
            // 
            // valuesPanel
            // 
            this.valuesPanel.BackColor = System.Drawing.Color.DarkSlateGray;
            this.valuesPanel.Controls.Add(this.damageLabel);
            this.valuesPanel.Controls.Add(this.healthLabel);
            this.valuesPanel.Controls.Add(this.coinsLabel);
            this.valuesPanel.Controls.Add(this.pictureBox);
            this.valuesPanel.Location = new System.Drawing.Point(12, 509);
            this.valuesPanel.Name = "valuesPanel";
            this.valuesPanel.Size = new System.Drawing.Size(408, 134);
            this.valuesPanel.TabIndex = 1;
            // 
            // damageLabel
            // 
            this.damageLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.damageLabel.ForeColor = System.Drawing.Color.White;
            this.damageLabel.Location = new System.Drawing.Point(187, 74);
            this.damageLabel.Name = "damageLabel";
            this.damageLabel.Size = new System.Drawing.Size(207, 23);
            this.damageLabel.TabIndex = 9;
            this.damageLabel.Text = "Damage: ";
            // 
            // healthLabel
            // 
            this.healthLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.healthLabel.ForeColor = System.Drawing.Color.White;
            this.healthLabel.Location = new System.Drawing.Point(187, 39);
            this.healthLabel.Name = "healthLabel";
            this.healthLabel.Size = new System.Drawing.Size(207, 23);
            this.healthLabel.TabIndex = 8;
            this.healthLabel.Text = "Health: ";
            // 
            // coinsLabel
            // 
            this.coinsLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.coinsLabel.ForeColor = System.Drawing.Color.White;
            this.coinsLabel.Location = new System.Drawing.Point(187, 4);
            this.coinsLabel.Name = "coinsLabel";
            this.coinsLabel.Size = new System.Drawing.Size(207, 23);
            this.coinsLabel.TabIndex = 7;
            this.coinsLabel.Text = "Coins: ";
            // 
            // pictureBox
            // 
            this.pictureBox.Location = new System.Drawing.Point(5, 4);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(176, 127);
            this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox.TabIndex = 0;
            this.pictureBox.TabStop = false;
            // 
            // closeButton
            // 
            this.closeButton.BackColor = System.Drawing.Color.DarkSlateGray;
            this.closeButton.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.closeButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Black;
            this.closeButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Black;
            this.closeButton.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.closeButton.ForeColor = System.Drawing.Color.White;
            this.closeButton.Location = new System.Drawing.Point(662, 592);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(191, 51);
            this.closeButton.TabIndex = 2;
            this.closeButton.Text = "Close";
            this.closeButton.UseVisualStyleBackColor = false;
            this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
            // 
            // ShopView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::Client.Properties.Resources.Logo;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(865, 655);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.valuesPanel);
            this.Controls.Add(this.itemsPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "ShopView";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Shop";
            this.valuesPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Panel itemsPanel;
        private Panel valuesPanel;
        private PictureBox pictureBox;
        private Button closeButton;
        private Label damageLabel;
        private Label healthLabel;
        private Label coinsLabel;
    }
}