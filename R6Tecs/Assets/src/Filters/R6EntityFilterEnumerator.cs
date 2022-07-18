// ----------------------------------------------------------------------------
// The Proprietary or MIT License
// Copyright (c) 2022-2022 RFS_6ro <rfs6ro@gmail.com>
// ----------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using R6ThreadECS.Entity;

namespace R6ThreadECS.Filters
{
    /// <summary>
    /// referenced from https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerator-1?view=net-6.0
    /// </summary>
    public class R6EntityFilterEnumerator : IEnumerator<R6Entity>
    {
        private R6EntityFilter _collection;
        private int curIndex;
        private R6Entity curBox;

        public R6EntityFilterEnumerator(R6EntityFilter collection)
        {
            _collection = collection;
            curIndex = -1;
            curBox = default;
        }

        public bool MoveNext()
        {
            //Avoids going beyond the end of the collection.
            if (++curIndex >= _collection.Count)
            {
                return false;
            }
            
            // Set current box to next item in collection.
            curBox = _collection[curIndex];
            return true;
        }

        public void Reset() { curIndex = -1; }

        void IDisposable.Dispose() { }

        public R6Entity Current => curBox;

        object IEnumerator.Current => Current;
    }
}
