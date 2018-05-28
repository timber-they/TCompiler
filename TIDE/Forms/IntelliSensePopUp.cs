#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using TIDE.Coloring.Types;

#endregion


namespace TIDE.Forms
{
    /// <summary>
    ///     The popup for the intelliSense stuff
    /// </summary>
    public sealed partial class IntelliSensePopUp : ListBox
    {
        public IntelliSensePopUp () => DoubleBuffered = true;

        /// <summary>
        ///     Gets fired before a key went down
        /// </summary>
        /// <param name="sender">useless</param>
        /// <param name="e">Provides stuff to evaluate the key that was pressed</param>
        public void Items_PreviewKeyDown (object sender, PreviewKeyDownEventArgs e)
        {
            switch (e.KeyData)
            {
                case Keys.Escape:
                    Visible = false;
                    break;
                case Keys.Enter:
                    EnterItem ();
                    break;
            }
        }

        /// <summary>
        ///     Gets fired when the user selected and entered an item
        /// </summary>
        public event EventHandler <ItemSelectedEventArgs> ItemEntered;

        public void EnterItem () => ItemEntered?.Invoke (this, new ItemSelectedEventArgs (GetSelected ()));

        /// <summary>
        ///     Updates the list with the new items while trying to keep the currently selected item selected
        /// </summary>
        /// <param name="list">The new list</param>
        public void UpdateList (List <string> list)
        {
            Invoke (new Action (() =>
            {
                var selected = SelectedItem;
                Items.Clear ();
                Items.AddRange (list.Select (s => s as object).ToArray ());
                if (selected != null && Items.Contains (selected))
                    SelectedItem = selected;
                else if (Items.Count > 0)
                    SelectedIndex = 0;
            }));
        }

        /// <summary>
        ///     Tries to select the specified index
        /// </summary>
        /// <param name="index">The index to select</param>
        public void SelectIndex (int index)
        {
            if (Items.Count > index && index >= 0)
                SelectedIndex = index;
        }

        /// <summary>
        ///     Disselects the selected item
        /// </summary>
        public void Disselect () => SelectedItem = null;

        /// <summary>
        ///     Evaluates the selected item or an empty string
        /// </summary>
        /// <returns>Te string</returns>
        private string GetSelected ()
            => SelectedItem as string ?? (Items.Count > 0 ? Items [0] as string : "");

        /// <summary>
        ///     Gets fired when the user double clicks on an item
        /// </summary>
        /// <param name="sender">Useless</param>
        /// <param name="e">Useless</param>
        public void Items_MouseDoubleClick (object sender, MouseEventArgs e) => EnterItem ();

        /// <summary>
        ///     Tries to increase the selected index
        /// </summary>
        public void ScrollDown ()
        {
            if (SelectedIndex < Items.Count - 1)
                SelectedIndex++;
        }

        /// <summary>
        ///     Tries to decrease the selectedIndex
        /// </summary>
        public void ScrollUp ()
        {
            if (SelectedIndex > 0)
                SelectedIndex--;
        }
    }
}