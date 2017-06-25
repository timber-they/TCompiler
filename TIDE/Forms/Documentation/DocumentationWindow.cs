#region

using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

using TIDE.Properties;

#endregion


namespace TIDE.Forms.Documentation
{
    /// <summary>
    ///     The documentation window containing the documentation
    /// </summary>
    public partial class DocumentationWindow : Form
    {
        /// <summary>
        ///     Initializes a new documentationWindow
        /// </summary>
        public DocumentationWindow ()
        {
            InitializeComponent ();
            var url = new Uri (Environment.CurrentDirectory + "\\Forms\\Documentation\\TDocumentation.html");
            if (!File.Exists (url.LocalPath))
            {
                MessageBox.Show (
                    Resources.helpNotFoundText);
                Content.DocumentText =
                    "<h1 align=\"center\">Help not found!</h1><p>Did you delete any of my files?</p>";
                return;
            }
            Content.Url = url;
        }

        /// <summary>
        ///     Gets called when the ok button got clicked
        /// </summary>
        /// <param name="sender">The control that called this. Actually not important.</param>
        /// <param name="e">Useless.</param>
        private void OkButton_Click (object sender = null, EventArgs e = null) => Close ();

        /// <summary>
        ///     Gets called when the visibility of the window changed
        /// </summary>
        /// <param name="sender">Useless.</param>
        /// <param name="e">Useless</param>
        private void DocumentationWindow_VisibleChanged (object sender = null, EventArgs e = null)
        {
            if (!Visible)
                return;
            FocusView ();
        }

        /// <summary>
        ///     Focuses to the content
        /// </summary>
        private async void FocusView ()
        {
            Content.Focus ();
            await Task.Run (() => Invoke (new Action (() =>
            {
                while (!Content.Focused)
                    Content.Focus ();
            })));
        }
    }
}