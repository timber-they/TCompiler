#region

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

#endregion

namespace TIDE.Forms
{
    /// <summary>
    ///     The popup for the intelliSense stuff
    /// </summary>
    public partial class IntelliSensePopUp : Form
    {
        /// <summary>
        ///     Initializes a new IntelliSensePopUp
        /// </summary>
        /// <param name="location">The location of the popUp in the window</param>
        public IntelliSensePopUp(Point location)
        {
            InitializeComponent();
            StartPosition = FormStartPosition.Manual;
            Location = location;
        }

        /// <summary>
        ///     Gets fired before a key went down
        /// </summary>
        /// <param name="sender">useless</param>
        /// <param name="e">Provides stuff to evaluate the key that was pressed</param>
        private void Items_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            switch (e.KeyData)
            {
                case Keys.Escape:
                    Visible = false;
                    break;
                case Keys.Enter:
                    ItemEntered?.Invoke((string) Items.SelectedItem, null);
                    break;
            }
        }

        /// <summary>
        ///     Gets fired when the user selected and entered an item
        /// </summary>
        public event EventHandler ItemEntered;

        /// <summary>
        ///     Updates the list with the new items while trying to keep the currently selected item selected
        /// </summary>
        /// <param name="list">The new list</param>
        public void UpdateList(List<string> list)
        {
            Items.Invoke(new Action(() =>
            {
                var selected = Items.SelectedItem;
                Items.Items.Clear();
                Items.Items.AddRange(list.Select(s => s as object).ToArray());
                if ((selected != null) && Items.Items.Contains(selected))
                    Items.SelectedItem = selected;
                else if (Items.Items.Count > 0)
                    Items.SelectedIndex = 0;
            }));
        }

        /// <summary>
        ///     Tries to select the specified index
        /// </summary>
        /// <param name="index">The index to select</param>
        public void SelectIndex(int index)
        {
            if ((Items.Items.Count > index) && (index >= 0))
                Items.SelectedIndex = index;
        }

        /// <summary>
        ///     Disselects the selected item
        /// </summary>
        public void Disselect() => Items.SelectedItem = null;

        /// <summary>
        ///     Evaluates the selected item or an empty string
        /// </summary>
        /// <returns>Te string</returns>
        public string GetSelected()
            => Items.SelectedItem as string ?? (Items.Items.Count > 0 ? Items.Items[0] as string : "");

        /// <summary>
        ///     Gets fired when the user double clicks on an item
        /// </summary>
        /// <param name="sender">Useless</param>
        /// <param name="e">Useless</param>
        private void Items_MouseDoubleClick(object sender, MouseEventArgs e)
            => ItemEntered?.Invoke((string) Items.SelectedItem, null);

        /// <summary>
        ///     Tries to increase the selected index
        /// </summary>
        public void ScrollDown()
        {
            if (Items.SelectedIndex < Items.Items.Count - 1)
                Items.SelectedIndex++;
        }

        /// <summary>
        ///     Tries to decrease the selectedIndex
        /// </summary>
        public void ScrollUp()
        {
            if (Items.SelectedIndex > 0)
                Items.SelectedIndex--;
        }
    }
}