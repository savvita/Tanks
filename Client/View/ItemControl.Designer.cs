namespace Client.View
{
    partial class ItemControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.titleLabel = new System.Windows.Forms.Label();
            this.propertyLabel = new System.Windows.Forms.Label();
            this.propertyValue = new System.Windows.Forms.Label();
            this.costLabel = new System.Windows.Forms.Label();
            this.costValue = new System.Windows.Forms.Label();
            this.buyButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox
            // 
            this.pictureBox.Location = new System.Drawing.Point(9, 12);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(151, 141);
            this.pictureBox.TabIndex = 0;
            this.pictureBox.TabStop = false;
            // 
            // titleLabel
            // 
            this.titleLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.titleLabel.Location = new System.Drawing.Point(9, 157);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(151, 23);
            this.titleLabel.TabIndex = 1;
            this.titleLabel.Text = "Title";
            // 
            // propertyLabel
            // 
            this.propertyLabel.Location = new System.Drawing.Point(10, 184);
            this.propertyLabel.Name = "propertyLabel";
            this.propertyLabel.Size = new System.Drawing.Size(94, 23);
            this.propertyLabel.TabIndex = 2;
            this.propertyLabel.Text = "Property";
            // 
            // propertyValue
            // 
            this.propertyValue.Location = new System.Drawing.Point(110, 184);
            this.propertyValue.Name = "propertyValue";
            this.propertyValue.Size = new System.Drawing.Size(50, 23);
            this.propertyValue.TabIndex = 3;
            this.propertyValue.Text = "Value";
            // 
            // costLabel
            // 
            this.costLabel.AutoSize = true;
            this.costLabel.Location = new System.Drawing.Point(10, 216);
            this.costLabel.Name = "costLabel";
            this.costLabel.Size = new System.Drawing.Size(31, 15);
            this.costLabel.TabIndex = 4;
            this.costLabel.Text = "Cost";
            // 
            // costValue
            // 
            this.costValue.Location = new System.Drawing.Point(110, 216);
            this.costValue.Name = "costValue";
            this.costValue.Size = new System.Drawing.Size(50, 23);
            this.costValue.TabIndex = 5;
            this.costValue.Text = "Value";
            // 
            // buyButton
            // 
            this.buyButton.Location = new System.Drawing.Point(85, 251);
            this.buyButton.Name = "buyButton";
            this.buyButton.Size = new System.Drawing.Size(75, 23);
            this.buyButton.TabIndex = 6;
            this.buyButton.Text = "Buy";
            this.buyButton.UseVisualStyleBackColor = true;
            this.buyButton.Click += new System.EventHandler(this.Button_Clicked);
            // 
            // ItemControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.buyButton);
            this.Controls.Add(this.costValue);
            this.Controls.Add(this.costLabel);
            this.Controls.Add(this.propertyValue);
            this.Controls.Add(this.propertyLabel);
            this.Controls.Add(this.titleLabel);
            this.Controls.Add(this.pictureBox);
            this.Name = "ItemControl";
            this.Size = new System.Drawing.Size(172, 288);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private PictureBox pictureBox;
        private Label titleLabel;
        private Label propertyLabel;
        private Label propertyValue;
        private Label costLabel;
        private Label costValue;
        private Button buyButton;
    }
}
