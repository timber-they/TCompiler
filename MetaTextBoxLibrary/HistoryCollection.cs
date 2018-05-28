using System.Collections.Generic;
using System.Linq;


namespace MetaTextBoxLibrary
{
    public class HistoryCollection <T>
    {
        public readonly int Count;

        /*
        Current text -> _index - 1 (__________)
        
        4 |0|
        3 |0|
        2 |0|
        1 |0|
        0 |0| <- _index, _currentHeight
        
        After push (0):
        4 |0|
        3 |0|
        2 |0|
        1 |0| <- _index, _currentHeight
        0 |0| __________
        
        After push (1):
        4 |0|
        3 |0|
        2 |0| <- _index, _currentHeight
        1 |1| __________
        0 |0|
        
        After push (2):
        4 |0|
        3 |0| <- _index, _currentHeight
        2 |2| __________
        1 |1|
        0 |0|
        
        After push (3):
        4 |0| <- _index, _currentHeight
        3 |3|  __________
        2 |2|
        1 |1|
        0 |0|
        
        After Undo () {2}:
        4 |0| <- _currentHeight
        3 |3| <- _index
        2 |2| __________
        1 |1|
        0 |0|
        
        After Undo () {1}:
        4 |0| <- _currentHeight
        3 |3|
        2 |2| <- _index
        1 |1| __________
        0 |0|
        
        After Undo () {0}
        4 |0| <- _currentHeight
        3 |3|
        2 |2|
        1 |1| <- _index
        0 |0| __________
        
        After Redo () {1}:
        4 |0| <- _currentHeight
        3 |3|
        2 |2| <- _index
        1 |1| __________
        0 |0|
        
        After push (4):
        4 |0|
        3 |3| <- _index, _currentHeight
        2 |4| __________
        1 |1|
        0 |0|
        */
        public readonly List <T> Items;

        public int CurrentHeight;

        public int Index;

        public HistoryCollection (int count)
        {
            Items = Enumerable.Repeat (default (T), count).ToList ();
            Count = count;
        }

        public T Undo ()
        {
            if (Index <= 1)
                return default (T);
            Index--;
            return Items [Index - 1];
        }

        public T Redo ()
        {
            T toReturn;
            if (Index < CurrentHeight)
            {
                toReturn = Items [Index];
                Index++;
            }
            else
                toReturn = default (T);

            return toReturn;
        }

        public void Push (T item)
        {
            Items [Index] = item;
            if (Index == Count - 1)
            {
                Items.RemoveAt (0);
                Items.Add (default (T));
            }
            else
            {
                Index++;
                CurrentHeight = Index;
            }
        }
    }
}