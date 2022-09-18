// ----------------------------------------------------------------------------
// The Proprietary or MIT License
// Copyright (c) 2022-2022 RFS_6ro <rfs6ro@gmail.com>
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using R6ThreadECS.Entity;
using R6ThreadECS.Utils;

namespace R6ThreadECS.Filters
{
    public class R6EntityFilter : IDisposable
    {
        private bool _isDisposed = false;
        
        private GrowingArray<R6Entity> _entities;
        
        public R6EntityFilter(int capacity = 256)
        {
            _entities = new GrowingArray<R6Entity>(capacity);
        }

        [PublicAPI] 
        public int Count => _entities.Count;

        [PublicAPI]
        public bool IsDisposed => _isDisposed;
        
        [PublicAPI]
        public bool IsLocked => _entities.IsLocked;

        [PublicAPI]
        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public void Lock() => _entities.Lock();

        [PublicAPI]
        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public void Add(R6Entity newEntity) => _entities.Add(newEntity);

        [PublicAPI]
        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public bool Contains(R6Entity item) => _entities.Has(item);
        
        [PublicAPI]
        public IEnumerator<R6Entity> GetEnumerator()
        {
            return new R6EntityFilterEnumerator(this);
        }

        [PublicAPI]
        public R6Entity this[int index]
        {
            get => _entities[index];
            set => _entities[index] = value;
        }

        [PublicAPI]
        public void Dispose()
        {
            if (_isDisposed)
            {
                return;
            }

            _entities.Dispose();
            _entities = null;

            _isDisposed = true;
        }
    }
}
