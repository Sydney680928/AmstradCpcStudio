﻿using ICSharpCode.TextEditor.Document;
using AmstradCpcStudio.Classes;
using ICSharpCode.TextEditor;

namespace AmstradCpcStudio.Forms
{
    public partial class FormEditorBasicPlus : Form
    {
        private bool _CodeIsModified;

        public string? CurrentFilename { get; private set; }

        public FormEditorBasicPlus(string? filename, string? code)
        {
            InitializeComponent();

            // Taille de la police

            CodeEditor.Font = new Font(CodeEditor.Font.FontFamily, Properties.Settings.Default.BasicEditorFontSize);

            // Chargement de la syntaxe locomotive basic

            string? dir = Path.GetDirectoryName(Application.ExecutablePath);

            if (Directory.Exists(dir))
            {
                var fsmProvider = new FileSyntaxModeProvider(dir);
                HighlightingManager.Manager.AddSyntaxModeFileProvider(fsmProvider);
                CodeEditor.SetHighlighting("LocomotiveBasicPlus");
            }

            // Folding

            CodeEditor.Document.FoldingManager.FoldingStrategy = new BasicPlusFolding();

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
                var file = Path.GetFileName(CurrentFilename).ToUpper();
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
                Filter = "Fichiers BASIC+|*.b+"
            };

            var r = d.ShowDialog();
            Application.DoEvents();

            var fn = CurrentFilename;

            if (r == DialogResult.OK)
            {
                CurrentFilename = d.FileName;

                var r1 = Save();

                if (r1 && fn != CurrentFilename)
                {
                    AddFileBasicPlusInLastFiles(CurrentFilename);
                    FormMain.Default.UpdateBasicPlusLastFilesMenu();
                }

                return r1;
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
            CodeEditor.Document.FoldingManager.UpdateFoldings(null, null);

            if (!_CodeIsModified)
            {
                _CodeIsModified = true;
                UpdateFormTitle();
            }
        }

        private void GenerateMenu_Click(object sender, EventArgs e)
        {
            var r = CodeGenerator.Default.Generate(CurrentFilename, CodeEditor.Text);

            if (r.Status == CodeGenerator.ResultStatusEnum.Success)
            {
                // Génération OK
                // On ouvre un éditeur locomotive basic et on y insère le code généré

                var f = new FormEditorBasic(null, r.Code);
                f.MdiParent = MdiParent;
                f.Show();
            }
            else
            {
                // Erreur pendant la génération !

                SelectLine(r.ErrorLineNumber);
                MessageBox.Show(MdiParent, r.ErrorMessage, "AMSTRAD CPC STUDIO", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FormEditorBasicPlus_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_CodeIsModified)
            {
                var r = MessageBox.Show(this.MdiParent, "Le code a été modifié, voulez-vous l'enregistrer ?", "AMSTRAD CPC STUDIO", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                if (r == DialogResult.Yes)
                {
                    e.Cancel = !Save();
                }
                else if (r == DialogResult.Cancel)
                {
                    e.Cancel = true;
                }
            }
        }

        private void FormEditorBasicPlus_Load(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.FormEditorBasicPlusTop > 0 && Properties.Settings.Default.FormEditorBasicPlusWidth > 0)
            {
                Top = Properties.Settings.Default.FormEditorBasicPlusTop;
                Left = Properties.Settings.Default.FormEditorBasicPlusLeft;
                Width = Properties.Settings.Default.FormEditorBasicPlusWidth;
                Height = Properties.Settings.Default.FormEditorBasicPlusHeight;
            }
        }

        private void FormEditorBasicPlus_ResizeEnd(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Normal)
            {
                Properties.Settings.Default.FormEditorBasicPlusTop = Top;
                Properties.Settings.Default.FormEditorBasicPlusLeft = Left;
                Properties.Settings.Default.FormEditorBasicPlusWidth = Width;
                Properties.Settings.Default.FormEditorBasicPlusHeight = Height;
                Properties.Settings.Default.Save();
            }
        }

        private void FilePrintMenu_Click(object sender, EventArgs e)
        {
            FormMain.Default.PrintDocument(CodeEditor);
        }

        private void KeywordsMenu_Click(object sender, EventArgs e)
        {
            using var f = new BasicKeywordsForm();
            var r = f.ShowDialog(this);
            Application.DoEvents();

            if (r == DialogResult.OK)
            {
                CodeEditor.ActiveTextAreaControl.TextArea.InsertString(f.SampleSelected);
                CodeEditor.Focus();
            }
        }

        #endregion

        #region CLASSES

        public class BasicPlusFolding : IFoldingStrategy
        {
            public List<FoldMarker> GenerateFoldMarkers(IDocument document, string fileName, object parseInformation)
            {
                List<FoldMarker> list = new List<FoldMarker>();

                int start = 0;

                // Create foldmarkers for the whole document, enumerate through every line.

                for (int i = 0; i < document.TotalNumberOfLines; i++)
                {
                    // Get the text of current line.

                    string text = document.GetText(document.GetLineSegment(i));

                    if (text.StartsWith("// region"))
                    {
                        start = i;
                    }
                    else if (text.StartsWith("// end region"))
                    {
                        list.Add(new FoldMarker(document, start, document.GetLineSegment(start).Length, i, 8));
                    }
                }

                return list;
            }
        }

        #endregion
    }
}
