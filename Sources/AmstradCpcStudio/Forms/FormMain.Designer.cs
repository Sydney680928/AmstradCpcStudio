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
            pictureBox1 = new PictureBox();
            menuStrip1 = new MenuStrip();
            menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
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
            menuStrip1.Size = new Size(1273, 26);
            menuStrip1.TabIndex = 1;
            menuStrip1.Text = "menuStrip1";
            // 
            // FileMenu
            // 
            FileMenu.DropDownItems.AddRange(new ToolStripItem[] { FileNewMenu, OpenMenu });
            FileMenu.Name = "FileMenu";
            FileMenu.Size = new Size(54, 22);
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
            SymbolsMenu.Size = new Size(70, 22);
            SymbolsMenu.Text = "Symboles";
            // 
            // WindowsMenu
            // 
            WindowsMenu.Name = "WindowsMenu";
            WindowsMenu.Size = new Size(63, 22);
            WindowsMenu.Text = "Fenêtres";
            // 
            // AboutMenu
            // 
            AboutMenu.Name = "AboutMenu";
            AboutMenu.Size = new Size(92, 22);
            AboutMenu.Text = "A propos de...";
            // 
            // pictureBox1
            // 
            pictureBox1.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            pictureBox1.BackColor = Color.FromArgb(93, 107, 153);
            pictureBox1.Image = Properties.Resources.P_Amstrad;
            pictureBox1.Location = new Point(1061, 662);
            pictureBox1.Margin = new Padding(3, 2, 3, 2);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(201, 161);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 5;
            pictureBox1.TabStop = false;
            // 
            // FormMain
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1273, 832);
            Controls.Add(pictureBox1);
            Controls.Add(menuStrip1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            IsMdiContainer = true;
            MainMenuStrip = menuStrip1;
            Name = "FormMain";
            Text = "AMSTRAD CPC STUDIO";
            Load += FormMain_Load;
            ResizeEnd += FormMain_ResizeEnd;
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
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
        private PictureBox pictureBox1;
    }
}
