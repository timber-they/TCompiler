using System;


namespace MetaTextBoxLibrary
{
    public class SelectionChangedEventArgs : EventArgs
    {
        public SelectionChangedEventArgs (int oldIndex, int newIndex)
        {
            OldIndex = oldIndex;
            NewIndex = newIndex;
        }

        public int OldIndex { get; }
        public int NewIndex { get; }
    }
}