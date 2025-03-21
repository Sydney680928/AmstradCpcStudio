namespace x07studio.Forms
{
    partial class FormAbout
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
            pictureBox1 = new PictureBox();
            AboutLabel = new Label();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // pictureBox1
            // 
            pictureBox1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            pictureBox1.Image = AmstradCpcStudio.Properties.Resources.P_Amstrad;
            pictureBox1.Location = new Point(12, 12);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(355, 196);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // AboutLabel
            // 
            AboutLabel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            AboutLabel.Cursor = Cursors.Hand;
            AboutLabel.ForeColor = Color.White;
            AboutLabel.Location = new Point(12, 222);
            AboutLabel.Name = "AboutLabel";
            AboutLabel.Size = new Size(355, 114);
            AboutLabel.TabIndex = 1;
            AboutLabel.Text = "AMSTRAD CPC STUDIO\r\n\r\nDéveloppé par Stéphane SIBUE\r\nhttps://www.coding4phone.com\r\n";
            AboutLabel.TextAlign = ContentAlignment.MiddleLeft;
            AboutLabel.Click += AboutLabel_Click;
            // 
            // FormAbout
            // 
            AutoScaleMode = AutoScaleMode.Inherit;
            BackColor = Color.Black;
            ClientSize = new Size(379, 345);
            Controls.Add(AboutLabel);
            Controls.Add(pictureBox1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormAbout";
            StartPosition = FormStartPosition.CenterParent;
            Text = "A propos de AMSTRAD CPC STUDIO";
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private PictureBox pictureBox1;
        private Label AboutLabel;
    }
}