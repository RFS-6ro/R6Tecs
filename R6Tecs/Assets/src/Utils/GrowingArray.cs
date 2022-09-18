// ----------------------------------------------------------------------------
// The Proprietary or MIT License
// Copyright (c) 2022-2022 RFS_6ro <rfs6ro@gmail.com>
// ----------------------------------------------------------------------------

using System;
using System.Runtime.CompilerServices;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace R6ThreadECS.Utils
{
    public class GrowingArray<T> : IReadOnlyCollection<T>, IDisposable
    {
        private ResizeableArray<T> _array;
        
        private bool _isDisposed = false;

        public GrowingArray(int capacity = 256)
        {
            _array = new ResizeableArray<T>(capacity);
        }

        [PublicAPI]
        public bool IsDisposed => _isDisposed;
        
        [PublicAPI]
        public int Capacity => _array.Capacity;
        
        [PublicAPI]
        public int Count => _array.Count;
        
        [PublicAPI]
        public bool IsLocked => _array.IsLocked;

        [PublicAPI, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int Add(T item) => _array.Add(item);

        [PublicAPI, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Has(T item) => _array.Has(item);

        [PublicAPI, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Lock() => _array.Lock();

        [PublicAPI, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Unlock() => _array.Unlock();

        [PublicAPI, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref T GetRef(int index) => ref _array.GetRef(index);

        [PublicAPI, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Resize(int newLength) => _array.Resize(newLength);

        [PublicAPI, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AssertIndex(int index) => _array.AssertIndex(index);
        
        [PublicAPI]
        public T this[int index]
        {
            get => _array[index];
            set => _array[index] = value;
        }
        
        public void Dispose()
        {
            if (_isDisposed)
            {
                return;
            }
            
            _array.Dispose();
            _array = null;

            _isDisposed = true;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new GrowingArrayEnumerator<T>(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
