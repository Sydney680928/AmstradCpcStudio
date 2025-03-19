namespace AmstradCpcStudio.Forms
{
    partial class FormEditorBasic
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
            MenuStrip menuStrip1;
            FileMenu = new ToolStripMenuItem();
            FileSaveMenu = new ToolStripMenuItem();
            FileSaveAsMenu = new ToolStripMenuItem();
            GenerateMenu = new ToolStripMenuItem();
            CodeEditor = new ICSharpCode.TextEditor.TextEditorControl();
            menuStrip1 = new MenuStrip();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.AutoSize = false;
            menuStrip1.BackColor = Color.FromArgb(204, 213, 240);
            menuStrip1.ImageScalingSize = new Size(20, 20);
            menuStrip1.Items.AddRange(new ToolStripItem[] { FileMenu, GenerateMenu });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Padding = new Padding(5, 2, 0, 2);
            menuStrip1.Size = new Size(627, 28);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // FileMenu
            // 
            FileMenu.DropDownItems.AddRange(new ToolStripItem[] { FileSaveMenu, FileSaveAsMenu });
            FileMenu.Name = "FileMenu";
            FileMenu.Size = new Size(54, 24);
            FileMenu.Text = "Fichier";
            // 
            // FileSaveMenu
            // 
            FileSaveMenu.Name = "FileSaveMenu";
            FileSaveMenu.Size = new Size(166, 22);
            FileSaveMenu.Text = "Enregistrer";
            FileSaveMenu.Click += FileSaveMenu_Click;
            // 
            // FileSaveAsMenu
            // 
            FileSaveAsMenu.Name = "FileSaveAsMenu";
            FileSaveAsMenu.Size = new Size(166, 22);
            FileSaveAsMenu.Text = "Enregistrer sous...";
            FileSaveAsMenu.Click += FileSaveAsMenu_Click;
            // 
            // GenerateMenu
            // 
            GenerateMenu.Name = "GenerateMenu";
            GenerateMenu.Size = new Size(60, 24);
            GenerateMenu.Text = "Générer";
            GenerateMenu.Click += GenerateMenu_Click;
            // 
            // CodeEditor
            // 
            CodeEditor.BorderStyle = BorderStyle.FixedSingle;
            CodeEditor.Dock = DockStyle.Fill;
            CodeEditor.Font = new Font("Consolas", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            CodeEditor.IsReadOnly = false;
            CodeEditor.Location = new Point(0, 28);
            CodeEditor.Margin = new Padding(2);
            CodeEditor.Name = "CodeEditor";
            CodeEditor.Size = new Size(627, 330);
            CodeEditor.TabIndex = 1;
            CodeEditor.VRulerRow = 255;
            CodeEditor.TextChanged += CodeEditor_TextChanged;
            // 
            // FormEditorBasic
            // 
            AutoScaleMode = AutoScaleMode.Inherit;
            ClientSize = new Size(627, 358);
            Controls.Add(CodeEditor);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Margin = new Padding(2);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormEditorBasic";
            Text = "BASIC+";
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private MenuStrip menuStrip1;
        private ToolStripMenuItem FileMenu;
        private ToolStripMenuItem FileSaveMenu;
        private ToolStripMenuItem FileSaveAsMenu;
        private ICSharpCode.TextEditor.TextEditorControl CodeEditor;
        private ToolStripMenuItem GenerateMenu;
    }
}