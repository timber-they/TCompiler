using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;


namespace MetaTextBoxLibrary
{
    public class HistoryCollection<T>
    {
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
        public List<T> _items; //TODO: make private

        public int _index;
        public int _currentHeight;
        public int _count;

        public HistoryCollection (int count)
        {
            _items = Enumerable.Repeat (default (T), count).ToList ();
            _count = count;
        }

        public T Undo ()
        {
            if (_index <= 1)
                return default (T);
            _index--;
            return _items [_index - 1];
        }

        public T Redo ()
        {
            T toReturn;
            if (_index < _currentHeight)
            {
                toReturn = _items [_index];
                _index++;
            }
            else
                toReturn = default (T);
            return toReturn;
        }

        public void Push (T item)
        {
            _items [_index] = item;
            if (_index == _count - 1)
            {
                _items.RemoveAt (0);
                _items.Add (default (T));
            }
            else
            {
                _index++;
                _currentHeight = _index;
            }
        }
    }
}