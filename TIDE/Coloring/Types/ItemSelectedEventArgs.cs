using System;

namespace TIDE.Coloring.Types
{
    public class ItemSelectedEventArgs : EventArgs
    {
        public ItemSelectedEventArgs(string selectedItem)
        {
            SelectedItem = selectedItem;
        }

        public string SelectedItem { get; }
    }
}