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
            UpdateList(items);
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
            var selected = Items.SelectedItem;
            Items.Items.Clear();
            Items.Items.AddRange(list.Select(s => s as object).ToArray());
            if (selected != null && Items.Items.Contains(selected))
                Items.SelectedItem = selected;
            else if (Items.Items.Count > 0)
                Items.SelectedIndex = 0;
                
        }

        public string GetSelected() => Items.SelectedItem as string ?? (Items.Items.Count > 0 ? Items.Items[0] as string : "");

        private void Items_MouseDoubleClick(object sender, MouseEventArgs e) => ItemEntered?.Invoke(null, (string) Items.SelectedItem);

        public void ScrollDown()
        {
            if (Items.SelectedIndex < Items.Items.Count - 1)
                Items.SelectedIndex++;
        }

        public void ScrollUp()
        {
            if (Items.SelectedIndex > 0)
                Items.SelectedIndex--;
        }
    }
}