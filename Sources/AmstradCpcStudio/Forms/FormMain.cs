namespace AmstradCpcStudio.Forms
{
    public partial class FormMain : Form
    {
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
        }

        private bool OpenBasicPlus()
        {
            using var d = new OpenFileDialog()
            {
                AddExtension = true,
                DefaultExt = ".bplus",
                CheckPathExists = true,
                CheckFileExists = true,
                Filter = "Fichiers BASIC+|*.bplus"
            };

            var r = d.ShowDialog();
            Application.DoEvents();

            if (r == DialogResult.OK && d.FileName != null)
            {
                // On vérifie que le fichier n'est pas déjà ouvert 

                foreach (var form in MdiChildren)
                {
                    if (form is FormEditorBasicPlus f && f.CurrentFilename != null && f.CurrentFilename == d.FileName)
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
                    using var reader = new StreamReader(d.FileName);
                    var code = reader.ReadToEnd();
                    OpenBasicPlusEditor(d.FileName, code);
                    return true;
                }
                catch
                {
                    MessageBox.Show(this, "Impossible d'ouvrir le fichier !", "AMSTRAD CPC STUDIO", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            return false;
        }

        private void OpenBasicPlusEditor(string? filename = null, string? code = null)
        {
            var f = new FormEditorBasicPlus(filename, code);
            f.MdiParent = this;
            f.Show();
        }

        #region UI EVENTS

        private void FileNewProgramBasicPlusMenu_Click(object sender, EventArgs e)
        {
            OpenBasicPlusEditor();
        }

        private void OpenProgramBasicPlusMenu_Click(object sender, EventArgs e)
        {
            OpenBasicPlus();
        }

        #endregion
    }
}
