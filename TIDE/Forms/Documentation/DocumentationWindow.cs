using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TIDE.Forms.Documentation
{
    public partial class DocumentationWindow : Form
    {
        public DocumentationWindow()
        {
            InitializeComponent();
            Content.Url = new Uri(Environment.CurrentDirectory + "\\..\\..\\Forms\\Documentation\\TDocumentation.html");
        }

        private void OkButton_Click(object sender, EventArgs e) => Close();

        private void DocumentationWindow_VisibleChanged(object sender, EventArgs e)
        {
            if (!Visible)
                return;
            FocusView();
        }

        private async void FocusView ()
        {
            Content.Focus();
            await Task.Run(() => Invoke(new Action(() =>
            {
                while (!Content.Focused)
                    Content.Focus();
            })));

        }
    }
}
