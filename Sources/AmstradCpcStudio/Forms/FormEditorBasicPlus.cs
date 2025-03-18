using ICSharpCode.TextEditor.Document;

namespace AmstradCpcStudio.Forms
{
    public partial class FormEditorBasicPlus : Form
    {
        private bool _CodeIsModified;

        public string? CurrentFilename { get; private set; }

        public FormEditorBasicPlus(string? filename,string? code)
        {
            InitializeComponent();

            // Chargement de la syntaxe locomotive basic

            string? dir = Path.GetDirectoryName(Application.ExecutablePath);

            if (Directory.Exists(dir))
            {
                var fsmProvider = new FileSyntaxModeProvider(dir);
                HighlightingManager.Manager.AddSyntaxModeFileProvider(fsmProvider);
                CodeEditor.SetHighlighting("LocomotiveBasicPlus");
            }

            // Intégration du fichier de code

            CurrentFilename = filename;
            CodeEditor.Text = code;

            UpdateFormTitle();
        }

        private void UpdateFormTitle()
        {
            if (CurrentFilename == null)
            {
                Text = "BASIC+ / SANS NOM";
            }
            else
            {
                var file = Path.GetFileNameWithoutExtension(CurrentFilename);
                Text = $"BASIC+ / {file}";
            }
        }

        private bool Save()
        {
            if (CurrentFilename == null) return SaveAs();

            try
            {
                using var writer = new StreamWriter(CurrentFilename);
                writer.Write(CodeEditor.Text);
                writer.Flush();
                return true;
            }
            catch
            {
                MessageBox.Show("Impossible d'enregistrer le fichier !", "AMSTRAD CPC STUDIO", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return false;
        }

        private bool SaveAs()
        {
            using var d = new SaveFileDialog()
            {
                FileName = CurrentFilename,
                AddExtension = true,
                DefaultExt = ".bplus",
                CheckPathExists = true,
                CheckWriteAccess = true,
                Filter = "Fichiers BASIC+|*.bplus"
            };

            var r = d.ShowDialog();
            Application.DoEvents();

            if (r == DialogResult.OK)
            {
                CurrentFilename = d.FileName;
                return Save();
            }

            return false;
        }


        #region UI EVENTS

        private void FileSaveMenu_Click(object sender, EventArgs e)
        {
            if (Save())
            {
                UpdateFormTitle();
            }
        }

        private void FileSaveAsMenu_Click(object sender, EventArgs e)
        {
            if (SaveAs())
            {
                UpdateFormTitle();
            }
        }

        #endregion
    }
}
