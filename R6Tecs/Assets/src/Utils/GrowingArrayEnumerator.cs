// ----------------------------------------------------------------------------
// The Proprietary or MIT License
// Copyright (c) 2022-2022 RFS_6ro <rfs6ro@gmail.com>
// ----------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;

namespace R6ThreadECS.Utils
{
    public class GrowingArrayEnumerator<T> : IEnumerator<T>
    {
        private GrowingArray<T> _collection;
        private int _currentIndex;

        public GrowingArrayEnumerator(GrowingArray<T> collection)
        {
            _collection = collection;
            _currentIndex = -1;
            Current = default;
        }

        public bool MoveNext()
        {
            //Avoids going beyond the end of the collection.
            if (++_currentIndex >= _collection.Count)
            {
                return false;
            }
            
            // Set current box to next item in collection.
            Current = _collection[_currentIndex];
            return true;
        }

        public void Reset() { _currentIndex = -1; }

        public T Current { get; private set; }

        object IEnumerator.Current => Current;

        public void Dispose() { }
    }
}
