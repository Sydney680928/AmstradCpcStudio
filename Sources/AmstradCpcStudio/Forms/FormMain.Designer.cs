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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            FileMenu = new ToolStripMenuItem();
            FileNewMenu = new ToolStripMenuItem();
            FileNewProgramBasicPlusMenu = new ToolStripMenuItem();
            FileNewProgramBasicMenu = new ToolStripMenuItem();
            OpenMenu = new ToolStripMenuItem();
            OpenProgramBasicPlusMenu = new ToolStripMenuItem();
            OpenProgramBasicMenu = new ToolStripMenuItem();
            SymbolsMenu = new ToolStripMenuItem();
            WindowsMenu = new ToolStripMenuItem();
            AboutMenu = new ToolStripMenuItem();
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
            menuStrip1.Padding = new Padding(5, 2, 0, 2);
            menuStrip1.Size = new Size(1273, 30);
            menuStrip1.TabIndex = 1;
            menuStrip1.Text = "menuStrip1";
            // 
            // FileMenu
            // 
            FileMenu.DropDownItems.AddRange(new ToolStripItem[] { FileNewMenu, OpenMenu });
            FileMenu.Name = "FileMenu";
            FileMenu.Size = new Size(54, 26);
            FileMenu.Text = "Fichier";
            // 
            // FileNewMenu
            // 
            FileNewMenu.DropDownItems.AddRange(new ToolStripItem[] { FileNewProgramBasicPlusMenu, FileNewProgramBasicMenu });
            FileNewMenu.Name = "FileNewMenu";
            FileNewMenu.Size = new Size(122, 22);
            FileNewMenu.Text = "Nouveau";
            // 
            // FileNewProgramBasicPlusMenu
            // 
            FileNewProgramBasicPlusMenu.Name = "FileNewProgramBasicPlusMenu";
            FileNewProgramBasicPlusMenu.Size = new Size(249, 22);
            FileNewProgramBasicPlusMenu.Text = "Programme BASIC+";
            FileNewProgramBasicPlusMenu.Click += FileNewProgramBasicPlusMenu_Click;
            // 
            // FileNewProgramBasicMenu
            // 
            FileNewProgramBasicMenu.Name = "FileNewProgramBasicMenu";
            FileNewProgramBasicMenu.Size = new Size(249, 22);
            FileNewProgramBasicMenu.Text = "Programme LOCOMOTIVE BASIC";
            FileNewProgramBasicMenu.Click += FileNewProgramBasicMenu_Click;
            // 
            // OpenMenu
            // 
            OpenMenu.DropDownItems.AddRange(new ToolStripItem[] { OpenProgramBasicPlusMenu, OpenProgramBasicMenu });
            OpenMenu.Name = "OpenMenu";
            OpenMenu.Size = new Size(122, 22);
            OpenMenu.Text = "Ouvrir";
            // 
            // OpenProgramBasicPlusMenu
            // 
            OpenProgramBasicPlusMenu.Name = "OpenProgramBasicPlusMenu";
            OpenProgramBasicPlusMenu.Size = new Size(249, 22);
            OpenProgramBasicPlusMenu.Text = "Programme BASIC+";
            OpenProgramBasicPlusMenu.Click += OpenProgramBasicPlusMenu_Click;
            // 
            // OpenProgramBasicMenu
            // 
            OpenProgramBasicMenu.Name = "OpenProgramBasicMenu";
            OpenProgramBasicMenu.Size = new Size(249, 22);
            OpenProgramBasicMenu.Text = "Programme LOCOMOTIVE BASIC";
            OpenProgramBasicMenu.Click += OpenProgramBasicMenu_Click;
            // 
            // SymbolsMenu
            // 
            SymbolsMenu.Name = "SymbolsMenu";
            SymbolsMenu.Size = new Size(70, 26);
            SymbolsMenu.Text = "Symboles";
            // 
            // WindowsMenu
            // 
            WindowsMenu.Name = "WindowsMenu";
            WindowsMenu.Size = new Size(63, 26);
            WindowsMenu.Text = "Fenêtres";
            // 
            // AboutMenu
            // 
            AboutMenu.Name = "AboutMenu";
            AboutMenu.Size = new Size(92, 26);
            AboutMenu.Text = "A propos de...";
            AboutMenu.Click += AboutMenu_Click;
            // 
            // FormMain
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1273, 832);
            Controls.Add(menuStrip1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            IsMdiContainer = true;
            MainMenuStrip = menuStrip1;
            Name = "FormMain";
            ShowIcon = false;
            Text = "AMSTRAD CPC STUDIO";
            Load += FormMain_Load;
            ResizeEnd += FormMain_ResizeEnd;
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
