using ICSharpCode.TextEditor;
using System.Drawing.Configuration;
using x07studio.Forms;

namespace AmstradCpcStudio.Forms
{
    public partial class FormMain : Form
    {
        private static FormMain _Default = new FormMain();

        public static FormMain Default => _Default;

        public FormMain()
        {
            InitializeComponent();

            foreach (var control in Controls)
            {
                if (control is MdiClient c)
                {
                    c.BackColor = Color.FromArgb(255, 93, 107, 153);
                    break;
                }
            }

            UpdateBasicPlusLastFilesMenu();

            UpdateBasicLastFilesMenu();
        }

        public bool PrintDocument(TextEditorControl editor)
        {
            PrintDialog printDialog = new PrintDialog();
            printDialog.Document = editor.PrintDocument;
            printDialog.UseEXDialog = true;

            var r = printDialog.ShowDialog();

            if (r == DialogResult.OK)
            {
                editor.PrintDocument.PrinterSettings = printDialog.PrinterSettings;
                editor.PrintDocument.Print();
                return true;
            }

            return false;
        }

        public void UpdateBasicPlusLastFilesMenu()
        {
            foreach (var item in LastFilesBasicPlusMenu.DropDownItems)
            {
                if (item is ToolStripMenuItem menuItem) menuItem.Click -= LastFileBasicPlusMenuItem_Click;
            }

            LastFilesBasicPlusMenu.DropDownItems.Clear();

            if (Properties.Settings.Default.LastFilesBasicPlus != null)
            {
                foreach (var file in Properties.Settings.Default.LastFilesBasicPlus)
                {
                    var menuItem = new ToolStripMenuItem();

                    menuItem.Text = Path.GetFileName(file);
                    menuItem.Tag = file;
                    menuItem.ToolTipText = file;
                    menuItem.AutoToolTip = true;
                    menuItem.Click += LastFileBasicPlusMenuItem_Click;

                    LastFilesBasicPlusMenu.DropDownItems.Add(menuItem);
                }
            }
        }

        public void UpdateBasicLastFilesMenu()
        {
            foreach (var item in LastFilesBasicMenu.DropDownItems)
            {
                if (item is ToolStripMenuItem menuItem) menuItem.Click -= LastFileBasicMenuItem_Click;
            }

            LastFilesBasicMenu.DropDownItems.Clear();

            if (Properties.Settings.Default.LastFilesBasic != null)
            {
                foreach (var file in Properties.Settings.Default.LastFilesBasic)
                {
                    var menuItem = new ToolStripMenuItem();

                    menuItem.Text = Path.GetFileName(file);
                    menuItem.Tag = file;
                    menuItem.ToolTipText = file;
                    menuItem.AutoToolTip = true;
                    menuItem.Click += LastFileBasicMenuItem_Click;

                    LastFilesBasicMenu.DropDownItems.Add(menuItem);
                }
            }
        }

        private bool OpenBasicPlusFile()
        {
            using var d = new OpenFileDialog()
            {
                AddExtension = true,
                DefaultExt = ".bplus",
                CheckPathExists = true,
                CheckFileExists = true,
                Filter = "Fichiers BASIC+|*.b+"
            };

            var r = d.ShowDialog();
            Application.DoEvents();

            if (r == DialogResult.OK && d.FileName != null)
            {
                return OpenFileInBasicPlusEditor(d.FileName);
            }

            return false;
        }

        private bool OpenBasicFile()
        {
            using var d = new OpenFileDialog()
            {
                AddExtension = true,
                DefaultExt = ".bas",
                CheckPathExists = true,
                CheckFileExists = true,
                Filter = "Fichiers LOCOMOTIVE BASIC|*.bas"
            };

            var r = d.ShowDialog();
            Application.DoEvents();

            if (r == DialogResult.OK && d.FileName != null)
            {
                OpenFileInBasicEditor(d.FileName);
            }

            return false;
        }

        private bool OpenFileInBasicPlusEditor(string filename)
        {
            // On vérifie que le fichier n'est pas déjà ouvert 

            foreach (var form in MdiChildren)
            {
                if (form is FormEditorBasicPlus f && f.CurrentFilename != null && f.CurrentFilename == filename)
                {
                    // Le fichier est déjà ouvert, on place en AVP l'éditeur correspondant

                    form.Activate();
                    Application.DoEvents();

                    // Et on affiche un message pour prévenir

                    MessageBox.Show(this, "Le fichier est déjà ouvert.", "AMSTRAD CPC STUDIO", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    return false;
                }
            }

            // On charge le fichier

            try
            {
                using var reader = new StreamReader(filename);
                var code = reader.ReadToEnd();

                OpenBasicPlusEditor(filename, code);

                AddFileBasicPlusInLastFiles(filename);
                UpdateBasicPlusLastFilesMenu();

                return true;
            }
            catch
            {
                MessageBox.Show(this, "Impossible d'ouvrir le fichier !", "AMSTRAD CPC STUDIO", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return false;
        }

        private bool OpenFileInBasicEditor(string filename)
        {
            // On vérifie que le fichier n'est pas déjà ouvert 

            foreach (var form in MdiChildren)
            {
                if (form is FormEditorBasic f && f.CurrentFilename != null && f.CurrentFilename == filename)
                {
                    // Le fichier est déjà ouvert, on place en AVP l'éditeur correspondant

                    form.Activate();
                    Application.DoEvents();

                    // Et on affiche un message pour prévenir

                    MessageBox.Show(this, "Le fichier est déjà ouvert.", "AMSTRAD CPC STUDIO", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    return false;
                }
            }

            // On charge le fichier

            try
            {
                using var reader = new StreamReader(filename);
                var code = reader.ReadToEnd();

                OpenBasicEditor(filename, code);

                AddFileBasicInLastFiles(filename);
                UpdateBasicLastFilesMenu();

                return true;
            }
            catch
            {
                MessageBox.Show(this, "Impossible d'ouvrir le fichier !", "AMSTRAD CPC STUDIO", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return false;
        }

        private void OpenBasicPlusEditor(string? filename = null, string? code = null)
        {
            var f = new FormEditorBasicPlus(filename, code);
            f.MdiParent = this;
            f.Show();
        }

        private void OpenBasicEditor(string? filename = null, string? code = null)
        {
            var f = new FormEditorBasic(filename, code);
            f.MdiParent = this;
            f.Show();
        }

        private void AddFileBasicPlusInLastFiles(string? filename)
        {
            if (filename != null)
            {
                // On ajoute le fichier dans la liste des derniers fichiers ouverts

                if (Properties.Settings.Default.LastFilesBasicPlus == null) Properties.Settings.Default.LastFilesBasicPlus = new System.Collections.Specialized.StringCollection();

                // Si déjà présent en l'enlève de la liste

                if (Properties.Settings.Default.LastFilesBasicPlus.Contains(filename)) Properties.Settings.Default.LastFilesBasicPlus.Remove(filename);

                // On l'ajoute au début

                Properties.Settings.Default.LastFilesBasicPlus.Insert(0, filename);

                // max 10 fichiers dans la liste

                while (Properties.Settings.Default.LastFilesBasicPlus.Count > 10)
                {
                    Properties.Settings.Default.LastFilesBasicPlus.RemoveAt(Properties.Settings.Default.LastFilesBasicPlus.Count - 1);
                }

                // save

                Properties.Settings.Default.Save();
            }
        }

        private void AddFileBasicInLastFiles(string? filename)
        {
            if (filename != null)
            {
                // On ajoute le fichier dans la liste des derniers fichiers ouverts

                if (Properties.Settings.Default.LastFilesBasic == null) Properties.Settings.Default.LastFilesBasic = new System.Collections.Specialized.StringCollection();

                // Si déjà présent en l'enlève de la liste

                if (Properties.Settings.Default.LastFilesBasic.Contains(filename)) Properties.Settings.Default.LastFilesBasic.Remove(filename);

                // On l'ajoute au début

                Properties.Settings.Default.LastFilesBasic.Insert(0, filename);

                // max 10 fichiers dans la liste

                while (Properties.Settings.Default.LastFilesBasic.Count > 10)
                {
                    Properties.Settings.Default.LastFilesBasic.RemoveAt(Properties.Settings.Default.LastFilesBasic.Count - 1);
                }

                // save

                Properties.Settings.Default.Save();
            }
        }


        #region UI EVENTS

        private void FileNewProgramBasicPlusMenu_Click(object sender, EventArgs e)
        {
            OpenBasicPlusEditor();
        }

        private void OpenProgramBasicPlusMenu_Click(object sender, EventArgs e)
        {
            OpenBasicPlusFile();
        }

        private void FileNewProgramBasicMenu_Click(object sender, EventArgs e)
        {
            OpenBasicEditor();
        }

        private void OpenProgramBasicMenu_Click(object sender, EventArgs e)
        {
            OpenBasicFile();
        }

        private void FormMain_ResizeEnd(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Normal)
            {
                Properties.Settings.Default.FormMainTop = Top;
                Properties.Settings.Default.FormMainLeft = Left;
                Properties.Settings.Default.FormMainWidth = Width;
                Properties.Settings.Default.FormMainHeight = Height;
                Properties.Settings.Default.Save();
            }
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.FormMainWidth > 0 && Properties.Settings.Default.FormMainHeight > 0)
            {
                Top = Properties.Settings.Default.FormMainTop;
                Left = Properties.Settings.Default.FormMainLeft;
                Width = Properties.Settings.Default.FormMainWidth;
                Height = Properties.Settings.Default.FormMainHeight;
            }
        }

        private void AboutMenu_Click(object sender, EventArgs e)
        {
            var f = new FormAbout();
            f.ShowDialog(this);
            Application.DoEvents();
        }

        private void LastFileBasicPlusMenuItem_Click(object? sender, EventArgs e)
        {
            if (sender is ToolStripMenuItem menuItem && menuItem.Tag is string filename)
            {
                OpenFileInBasicPlusEditor(filename);    
            }
        }

        private void LastFileBasicMenuItem_Click(object? sender, EventArgs e)
        {
            if (sender is ToolStripMenuItem menuItem && menuItem.Tag is string filename)
            {
                OpenFileInBasicEditor(filename);
            }
        }

        #endregion
    }
}
