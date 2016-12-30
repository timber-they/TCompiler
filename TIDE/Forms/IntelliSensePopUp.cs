using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace TIDE.Forms
{
    public partial class IntelliSensePopUp : Form
    {
        public IntelliSensePopUp(IEnumerable<string> items, Point pos)
        {
            InitializeComponent();
            StartPosition = FormStartPosition.Manual;
            Location = pos;
            Items.Items.AddRange(items.Select(s => s as object).ToArray());
        }

        private void Items_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            switch (e.KeyData)
            {
                case Keys.Escape:
                    Visible = false;
                    break;
                case Keys.Enter:
                    ItemEntered?.Invoke(null, (string) Items.SelectedItem);
                    break;
            }
        }

        public event EventHandler<string> ItemEntered;

        public void UpdateList(IEnumerable<string> list)
        {
            Items.Items.Clear();
            Items.Items.AddRange(list.Select(s => s as object).ToArray());
        }
    }
}