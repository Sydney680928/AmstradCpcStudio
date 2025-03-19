using ICSharpCode.TextEditor.Document;
using AmstradCpcStudio.Classes;
using ICSharpCode.TextEditor;

namespace AmstradCpcStudio.Forms
{
    public partial class FormEditorBasic : Form
    {
        private bool _CodeIsModified;      

        public string? CurrentFilename { get; private set; }

        public FormEditorBasicPlus(string? filename, string? code)
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
            _CodeIsModified = false;

            UpdateFormTitle();
        }

        private void UpdateFormTitle()
        {
            var star = _CodeIsModified ? "*" : "";

            if (CurrentFilename == null)
            {
                Text = $"BASIC+ / SANS NOM {star}";
            }
            else
            {
                var file = Path.GetFileNameWithoutExtension(CurrentFilename);
                Text = $"BASIC+ / {file} {star}";
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
                MessageBox.Show(MdiParent,"Impossible d'enregistrer le fichier !", "AMSTRAD CPC STUDIO", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return false;
        }

        private bool SaveAs()
        {
            using var d = new SaveFileDialog()
            {
                FileName = Path.GetFileName(CurrentFilename),
                InitialDirectory = Path.GetDirectoryName(CurrentFilename),
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

        public void SelectLine(int lineNumber)
        {
            try
            {
                CodeEditor.ActiveTextAreaControl.SelectionManager.ClearSelection();

                var startLocation = new TextLocation(0, lineNumber - 1);
                var endLocation = new TextLocation(80, lineNumber - 1);

                CodeEditor.ActiveTextAreaControl.SelectionManager.SetSelection(startLocation, endLocation);
                CodeEditor.ActiveTextAreaControl.ScrollTo(endLocation.Line, endLocation.Column);
            }
            catch
            {

            }
        }


        #region UI EVENTS

        private void FileSaveMenu_Click(object sender, EventArgs e)
        {
            if (Save())
            {
                _CodeIsModified = false;
                UpdateFormTitle();
            }
        }

        private void FileSaveAsMenu_Click(object sender, EventArgs e)
        {
            if (SaveAs())
            {
                _CodeIsModified = false;
                UpdateFormTitle();
            }
        }

        private void CodeEditor_TextChanged(object sender, EventArgs e)
        {
            if (!_CodeIsModified)
            {
                _CodeIsModified = true;
                UpdateFormTitle();
            }

        }

        private void GenerateMenu_Click(object sender, EventArgs e)
        {
            var r = CodeGenerator.Default.Generate(CodeEditor.Text);

            if (r.Status == CodeGenerator.ResultStatusEnum.Success)
            {
                // Génération OK
            }
            else
            {
                // Erreur pendant la génération !

                SelectLine(r.ErrorLineNumber);
                MessageBox.Show(MdiParent,r.ErrorMessage, "AMSTRAD CPC STUDIO", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion
    }
}
