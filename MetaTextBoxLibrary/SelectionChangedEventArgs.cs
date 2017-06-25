using System;
using System.Data.OleDb;
using System.IO;


namespace MetaTextBoxLibrary
{
    public class SelectionChangedEventArgs : EventArgs
    {
        public int OldIndex { get; }
        public int NewIndex { get; }
        
        public SelectionChangedEventArgs (int oldIndex, int newIndex)
        {
            OldIndex = oldIndex;
            NewIndex = newIndex;
        }
    }
}