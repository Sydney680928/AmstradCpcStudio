using AmstradCpcStudio.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AmstradCpcStudio.Forms
{
    public partial class BasicKeywordsForm : Form
    {
        public string? SampleSelected { get; private set; }


        public BasicKeywordsForm()
        {
            InitializeComponent();
        }

        #region UI EVENTS

        private void BasicKeywordsForm_Load(object sender, EventArgs e)
        {
            // On se place sur les commandes par défaut

            TypesComboBox.SelectedIndex = 0;
        }

        private void DisplayKeywordInformations(Keyword keyword)
        {
            DescriptionTextBox.Text = keyword.Description;
            SamplesListBox.DataSource = keyword.Samples;
        }

        private void TypesComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (TypesComboBox.SelectedIndex)
            {
                case 1:
                    KeywordsListBox.DataSource = AppGlobal.BasicFunctions;
                    break;

                default:
                    KeywordsListBox.DataSource = AppGlobal.BasicCommands;
                    break;
            }
        }

        private void KeywordsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (KeywordsListBox.SelectedItem is Keyword keyword)
            {
                DisplayKeywordInformations(keyword);
            }
        }

        private void SamplesListBox_DoubleClick(object sender, EventArgs e)
        {
            SampleSelected = SamplesListBox.SelectedItem as string;
            DialogResult = DialogResult.OK;
        }

        #endregion
    }
}
