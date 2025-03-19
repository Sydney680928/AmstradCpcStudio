using ICSharpCode.TextEditor.Document;
using AmstradCpcStudio.Classes;
using ICSharpCode.TextEditor;

namespace AmstradCpcStudio.Forms
{
    public partial class FormEditorBasic : Form
    {
        private bool _CodeIsModified;

        public string? CurrentFilename { get; private set; }

        public FormEditorBasic(string? filename, string? code)
        {
            InitializeComponent();

            // Chargement de la syntaxe locomotive basic

            string? dir = Path.GetDirectoryName(Application.ExecutablePath);

            if (Directory.Exists(dir))
            {
                var fsmProvider = new FileSyntaxModeProvider(dir);
                HighlightingManager.Manager.AddSyntaxModeFileProvider(fsmProvider);
                CodeEditor.SetHighlighting("LocomotiveBasic");
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
                Text = $"LOCOMOTIVE BASIC / SANS NOM {star}";
            }
            else
            {
                var file = Path.GetFileName(CurrentFilename).ToUpper();
                Text = $"LOCOMOTIVE BASIC / {file} {star}";
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
                MessageBox.Show(MdiParent, "Impossible d'enregistrer le fichier !", "AMSTRAD CPC STUDIO", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                Filter = "Fichiers BASIC|*.bas"
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

        private void FormEditorBasic_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_CodeIsModified)
            {
                var r = MessageBox.Show(this.MdiParent, "Le code a été modifié, si vous continuez vous allez perdre vos modifications !", "AMSTRAD CPC STUDIO", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                e.Cancel = r == DialogResult.Cancel;
            }
        }

        private void FormEditorBasic_Load(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.FormEditorBasicTop > 0 && Properties.Settings.Default.FormEditorBasicWidth > 0)
            {
                Top = Properties.Settings.Default.FormEditorBasicTop;
                Left = Properties.Settings.Default.FormEditorBasicLeft;
                Width = Properties.Settings.Default.FormEditorBasicWidth;
                Height = Properties.Settings.Default.FormEditorBasicHeight;
            }
        }

        private void FormEditorBasic_ResizeEnd(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Normal)
            {
                Properties.Settings.Default.FormEditorBasicTop = Top;
                Properties.Settings.Default.FormEditorBasicLeft = Left;
                Properties.Settings.Default.FormEditorBasicWidth = Width;
                Properties.Settings.Default.FormEditorBasicHeight = Height;
                Properties.Settings.Default.Save();
            }
        }

        #endregion
    }
}
