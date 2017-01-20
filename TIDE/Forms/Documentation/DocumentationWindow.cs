#region

using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using TIDE.Properties;

#endregion

namespace TIDE.Forms.Documentation
{
    public partial class DocumentationWindow : Form
    {
        public DocumentationWindow()
        {
            InitializeComponent();
            var url = new Uri(Environment.CurrentDirectory + "\\Forms\\Documentation\\TDocumentation.html");
            if (!File.Exists(url.LocalPath))
            {
                MessageBox.Show(
                    Resources.helpNotFoundText);
                Content.DocumentText = "<h1 align=\"center\">Help not found!</h1><p>Did you delete any of my files?</p>";
                return;
            }
            Content.Url = url;
        }

        private void OkButton_Click(object sender, EventArgs e) => Close();

        private void DocumentationWindow_VisibleChanged(object sender, EventArgs e)
        {
            if (!Visible)
                return;
            FocusView();
        }

        private async void FocusView()
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