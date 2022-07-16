namespace Client.View
{
    partial class StartForm
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
            this.startButton = new System.Windows.Forms.Button();
            this.shopButton = new System.Windows.Forms.Button();
            this.coinsLabel = new System.Windows.Forms.Label();
            this.coinsValue = new System.Windows.Forms.Label();
            this.healthValue = new System.Windows.Forms.Label();
            this.healthLabel = new System.Windows.Forms.Label();
            this.damageValue = new System.Windows.Forms.Label();
            this.damageLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(475, 192);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(75, 23);
            this.startButton.TabIndex = 0;
            this.startButton.Text = "Start";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // shopButton
            // 
            this.shopButton.Location = new System.Drawing.Point(248, 192);
            this.shopButton.Name = "shopButton";
            this.shopButton.Size = new System.Drawing.Size(75, 23);
            this.shopButton.TabIndex = 1;
            this.shopButton.Text = "Shop";
            this.shopButton.UseVisualStyleBackColor = true;
            this.shopButton.Click += new System.EventHandler(this.shopButton_Click);
            // 
            // coinsLabel
            // 
            this.coinsLabel.AutoSize = true;
            this.coinsLabel.Location = new System.Drawing.Point(266, 30);
            this.coinsLabel.Name = "coinsLabel";
            this.coinsLabel.Size = new System.Drawing.Size(37, 15);
            this.coinsLabel.TabIndex = 2;
            this.coinsLabel.Text = "Coins";
            // 
            // coinsValue
            // 
            this.coinsValue.AutoSize = true;
            this.coinsValue.Location = new System.Drawing.Point(327, 32);
            this.coinsValue.Name = "coinsValue";
            this.coinsValue.Size = new System.Drawing.Size(35, 15);
            this.coinsValue.TabIndex = 3;
            this.coinsValue.Text = "Value";
            // 
            // healthValue
            // 
            this.healthValue.AutoSize = true;
            this.healthValue.Location = new System.Drawing.Point(327, 82);
            this.healthValue.Name = "healthValue";
            this.healthValue.Size = new System.Drawing.Size(35, 15);
            this.healthValue.TabIndex = 5;
            this.healthValue.Text = "Value";
            // 
            // healthLabel
            // 
            this.healthLabel.AutoSize = true;
            this.healthLabel.Location = new System.Drawing.Point(266, 80);
            this.healthLabel.Name = "healthLabel";
            this.healthLabel.Size = new System.Drawing.Size(42, 15);
            this.healthLabel.TabIndex = 4;
            this.healthLabel.Text = "Health";
            // 
            // damageValue
            // 
            this.damageValue.AutoSize = true;
            this.damageValue.Location = new System.Drawing.Point(327, 126);
            this.damageValue.Name = "damageValue";
            this.damageValue.Size = new System.Drawing.Size(35, 15);
            this.damageValue.TabIndex = 7;
            this.damageValue.Text = "Value";
            // 
            // damageLabel
            // 
            this.damageLabel.AutoSize = true;
            this.damageLabel.Location = new System.Drawing.Point(266, 124);
            this.damageLabel.Name = "damageLabel";
            this.damageLabel.Size = new System.Drawing.Size(51, 15);
            this.damageLabel.TabIndex = 6;
            this.damageLabel.Text = "Damage";
            // 
            // StartForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.damageValue);
            this.Controls.Add(this.damageLabel);
            this.Controls.Add(this.healthValue);
            this.Controls.Add(this.healthLabel);
            this.Controls.Add(this.coinsValue);
            this.Controls.Add(this.coinsLabel);
            this.Controls.Add(this.shopButton);
            this.Controls.Add(this.startButton);
            this.Name = "StartForm";
            this.Text = "StartForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button startButton;
        private Button shopButton;
        private Label coinsLabel;
        private Label coinsValue;
        private Label healthValue;
        private Label healthLabel;
        private Label damageValue;
        private Label damageLabel;
    }
}