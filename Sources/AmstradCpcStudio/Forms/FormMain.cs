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
    }
}
