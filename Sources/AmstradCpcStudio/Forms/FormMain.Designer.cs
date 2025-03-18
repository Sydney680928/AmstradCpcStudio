namespace AmstradCpcStudio.Forms
{
    partial class FormMain
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            MenuStrip menuStrip1;
            FileMenu = new ToolStripMenuItem();
            FileNewMenu = new ToolStripMenuItem();
            FileNewProgramBasicPlusMenu = new ToolStripMenuItem();
            FileNewProgramBasicMenu = new ToolStripMenuItem();
            SymbolsMenu = new ToolStripMenuItem();
            WindowsMenu = new ToolStripMenuItem();
            AboutMenu = new ToolStripMenuItem();
            OpenMenu = new ToolStripMenuItem();
            OpenProgramBasicPlusMenu = new ToolStripMenuItem();
            OpenProgramBasicMenu = new ToolStripMenuItem();
            menuStrip1 = new MenuStrip();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.AllowMerge = false;
            menuStrip1.AutoSize = false;
            menuStrip1.BackColor = Color.FromArgb(204, 213, 240);
            menuStrip1.ImageScalingSize = new Size(20, 20);
            menuStrip1.Items.AddRange(new ToolStripItem[] { FileMenu, SymbolsMenu, WindowsMenu, AboutMenu });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.MdiWindowListItem = WindowsMenu;
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(1271, 35);
            menuStrip1.TabIndex = 1;
            menuStrip1.Text = "menuStrip1";
            // 
            // FileMenu
            // 
            FileMenu.DropDownItems.AddRange(new ToolStripItem[] { FileNewMenu, OpenMenu });
            FileMenu.Name = "FileMenu";
            FileMenu.Size = new Size(66, 31);
            FileMenu.Text = "Fichier";
            // 
            // FileNewMenu
            // 
            FileNewMenu.DropDownItems.AddRange(new ToolStripItem[] { FileNewProgramBasicPlusMenu, FileNewProgramBasicMenu });
            FileNewMenu.Name = "FileNewMenu";
            FileNewMenu.Size = new Size(224, 26);
            FileNewMenu.Text = "Nouveau";
            // 
            // FileNewProgramBasicPlusMenu
            // 
            FileNewProgramBasicPlusMenu.Name = "FileNewProgramBasicPlusMenu";
            FileNewProgramBasicPlusMenu.Size = new Size(224, 26);
            FileNewProgramBasicPlusMenu.Text = "Programme Basic +";
            FileNewProgramBasicPlusMenu.Click += FileNewProgramBasicPlusMenu_Click;
            // 
            // FileNewProgramBasicMenu
            // 
            FileNewProgramBasicMenu.Name = "FileNewProgramBasicMenu";
            FileNewProgramBasicMenu.Size = new Size(224, 26);
            FileNewProgramBasicMenu.Text = "Programme Basic";
            // 
            // SymbolsMenu
            // 
            SymbolsMenu.Name = "SymbolsMenu";
            SymbolsMenu.Size = new Size(87, 31);
            SymbolsMenu.Text = "Symboles";
            // 
            // WindowsMenu
            // 
            WindowsMenu.Name = "WindowsMenu";
            WindowsMenu.Size = new Size(78, 31);
            WindowsMenu.Text = "Fenêtres";
            // 
            // AboutMenu
            // 
            AboutMenu.Name = "AboutMenu";
            AboutMenu.Size = new Size(114, 31);
            AboutMenu.Text = "A propos de...";
            // 
            // OpenMenu
            // 
            OpenMenu.DropDownItems.AddRange(new ToolStripItem[] { OpenProgramBasicPlusMenu, OpenProgramBasicMenu });
            OpenMenu.Name = "OpenMenu";
            OpenMenu.Size = new Size(224, 26);
            OpenMenu.Text = "Ouvrir";
            // 
            // OpenProgramBasicPlusMenu
            // 
            OpenProgramBasicPlusMenu.Name = "OpenProgramBasicPlusMenu";
            OpenProgramBasicPlusMenu.Size = new Size(224, 26);
            OpenProgramBasicPlusMenu.Text = "Programme BASIC+";
            OpenProgramBasicPlusMenu.Click += OpenProgramBasicPlusMenu_Click;
            // 
            // OpenProgramBasicMenu
            // 
            OpenProgramBasicMenu.Name = "OpenProgramBasicMenu";
            OpenProgramBasicMenu.Size = new Size(224, 26);
            OpenProgramBasicMenu.Text = "Programme BASIC";
            // 
            // FormMain
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1271, 788);
            Controls.Add(menuStrip1);
            IsMdiContainer = true;
            MainMenuStrip = menuStrip1;
            Margin = new Padding(3, 4, 3, 4);
            Name = "FormMain";
            Text = "AMSTRAD CPC STUDIO";
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private MenuStrip menuStrip1;
        private ToolStripMenuItem FileMenu;
        private ToolStripMenuItem FileNewMenu;
        private ToolStripMenuItem FileNewProgramBasicPlusMenu;
        private ToolStripMenuItem FileNewProgramBasicMenu;
        private ToolStripMenuItem SymbolsMenu;
        private ToolStripMenuItem AboutMenu;
        private ToolStripMenuItem WindowsMenu;
        private ToolStripMenuItem OpenMenu;
        private ToolStripMenuItem OpenProgramBasicPlusMenu;
        private ToolStripMenuItem OpenProgramBasicMenu;
    }
}
