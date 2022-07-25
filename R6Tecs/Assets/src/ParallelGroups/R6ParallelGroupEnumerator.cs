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
        private int curIndex;
        private R6EcsSystem curBox;

        public R6ParallelGroupEnumerator(R6ParallelGroup collection)
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

        public R6EcsSystem Current => curBox;

        object IEnumerator.Current => Current;
    }
}