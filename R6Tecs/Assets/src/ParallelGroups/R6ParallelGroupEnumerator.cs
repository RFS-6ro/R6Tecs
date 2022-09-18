// ----------------------------------------------------------------------------
// The Proprietary or MIT License
// Copyright (c) 2022-2022 RFS_6ro <rfs6ro@gmail.com>
// ----------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;

namespace R6ThreadECS.Systems
{
    /// <summary>
    /// referenced from https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerator-1?view=net-6.0
    /// </summary>
    public class R6ParallelGroupEnumerator : IEnumerator<R6EcsSystem>
    {
        private R6ParallelGroup _collection;
        private int _currentIndex;

        public R6ParallelGroupEnumerator(R6ParallelGroup collection)
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

        public R6EcsSystem Current { get; private set; }

        object IEnumerator.Current => Current;

        void IDisposable.Dispose() { }
    }
}