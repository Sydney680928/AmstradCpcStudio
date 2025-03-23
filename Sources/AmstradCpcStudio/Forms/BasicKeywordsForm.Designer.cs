namespace AmstradCpcStudio.Forms
{
    partial class BasicKeywordsForm
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
            TypesComboBox = new ComboBox();
            KeywordsListBox = new ListBox();
            DescriptionTextBox = new TextBox();
            SamplesListBox = new ListBox();
            SuspendLayout();
            // 
            // TypesComboBox
            // 
            TypesComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            TypesComboBox.Font = new Font("Consolas", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            TypesComboBox.FormattingEnabled = true;
            TypesComboBox.Items.AddRange(new object[] { "Commandes", "Fonctions" });
            TypesComboBox.Location = new Point(12, 12);
            TypesComboBox.Name = "TypesComboBox";
            TypesComboBox.Size = new Size(185, 27);
            TypesComboBox.TabIndex = 0;
            TypesComboBox.SelectedIndexChanged += TypesComboBox_SelectedIndexChanged;
            // 
            // KeywordsListBox
            // 
            KeywordsListBox.Font = new Font("Consolas", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            KeywordsListBox.FormattingEnabled = true;
            KeywordsListBox.IntegralHeight = false;
            KeywordsListBox.ItemHeight = 19;
            KeywordsListBox.Location = new Point(12, 41);
            KeywordsListBox.Name = "KeywordsListBox";
            KeywordsListBox.Size = new Size(185, 357);
            KeywordsListBox.TabIndex = 1;
            KeywordsListBox.SelectedIndexChanged += KeywordsListBox_SelectedIndexChanged;
            // 
            // DescriptionTextBox
            // 
            DescriptionTextBox.BackColor = Color.White;
            DescriptionTextBox.BorderStyle = BorderStyle.FixedSingle;
            DescriptionTextBox.Font = new Font("Consolas", 12F, FontStyle.Italic, GraphicsUnit.Point, 0);
            DescriptionTextBox.Location = new Point(203, 12);
            DescriptionTextBox.Multiline = true;
            DescriptionTextBox.Name = "DescriptionTextBox";
            DescriptionTextBox.ReadOnly = true;
            DescriptionTextBox.Size = new Size(490, 136);
            DescriptionTextBox.TabIndex = 2;
            // 
            // SamplesListBox
            // 
            SamplesListBox.Font = new Font("Consolas", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            SamplesListBox.FormattingEnabled = true;
            SamplesListBox.IntegralHeight = false;
            SamplesListBox.ItemHeight = 19;
            SamplesListBox.Location = new Point(203, 154);
            SamplesListBox.Name = "SamplesListBox";
            SamplesListBox.Size = new Size(490, 244);
            SamplesListBox.TabIndex = 3;
            SamplesListBox.DoubleClick += SamplesListBox_DoubleClick;
            // 
            // BasicKeywordsForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(705, 410);
            Controls.Add(SamplesListBox);
            Controls.Add(DescriptionTextBox);
            Controls.Add(KeywordsListBox);
            Controls.Add(TypesComboBox);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "BasicKeywordsForm";
            ShowIcon = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "LOCOMOTIVE BASIC";
            Load += BasicKeywordsForm_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ComboBox TypesComboBox;
        private ListBox KeywordsListBox;
        private TextBox DescriptionTextBox;
        private ListBox SamplesListBox;
    }
}