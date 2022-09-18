// ----------------------------------------------------------------------------
// The Proprietary or MIT License
// Copyright (c) 2022-2022 RFS_6ro <rfs6ro@gmail.com>
// ----------------------------------------------------------------------------

using System;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace R6ThreadECS.Utils
{
    public class PoolArray<T>: IDisposable
    {
        private ResizeableArray<T> _items;
        private ResizeableArray<int> _cache;

        private int _itemsLength;
        
        private bool _isDisposed = false;
        
        public PoolArray(int capacity = 256, Func<int, T> getNewItem = null)
        {
            capacity = capacity <= 0 ? 256 : capacity;

            _itemsLength = 0;
            _items = new ResizeableArray<T>(capacity, getNewItem);
            _cache = new ResizeableArray<int>(capacity);
        }

        [PublicAPI]
        public bool IsDisposed => _isDisposed;
        
        [PublicAPI]
        public int Length => _itemsLength;
        
        [PublicAPI]
        public int CacheLength => _cache.Count;
        
        [PublicAPI]
        public int ItemsCapacity => _items.Capacity;
        
        [PublicAPI]
        public int CacheCapacity => _cache.Capacity;

        [PublicAPI, MethodImpl (MethodImplOptions.AggressiveInlining)]
        public T Get(int index)
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException($"ResizeableArray<{typeof(T)}>");
            }
            
            return _items[index];
        }

        [PublicAPI, MethodImpl (MethodImplOptions.AggressiveInlining)]
        public void Set(int index, T component)
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException($"ResizeableArray<{typeof(T)}>");
            }
            
            _items[index] = component;
        }

        [PublicAPI, MethodImpl (MethodImplOptions.AggressiveInlining)]
        public ref T GetRef(int index)
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException($"ResizeableArray<{typeof(T)}>");
            }
            
            return ref _items.GetRef(index);
        }
        
        [PublicAPI, MethodImpl (MethodImplOptions.AggressiveInlining)]
        public void Store(int index)
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException($"ResizeableArray<{typeof(T)}>");
            }
            
            Set(index, default(T));
            _cache.Add(index);
        }
        
        [PublicAPI, MethodImpl (MethodImplOptions.AggressiveInlining)]
        public int Alloc()
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException($"ResizeableArray<{typeof(T)}>");
            }
            
            int index;
            
            if (CacheLength > 0)
            {
                if (_cache.Remove(_cache.Count, out index))
                {
                    return index;
                }
            }

            index = _itemsLength;
            _items.Add(default(T));
            ++_itemsLength;
            
            return index;
        }
        
        [PublicAPI, MethodImpl (MethodImplOptions.AggressiveInlining)]
        public int Alloc(T data)
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException($"ResizeableArray<{typeof(T)}>");
            }
            
            int index = Alloc();

            Set(index, data);
            
            return index;
        }
        
        [PublicAPI, MethodImpl (MethodImplOptions.AggressiveInlining)]
        public void Copy(int from, int to)
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException($"ResizeableArray<{typeof(T)}>");
            }
            
            Set(to, _items[from]);
        }

        [PublicAPI]
        public void Dispose()
        {
            if (_isDisposed)
            {
                return;
            }

            _items?.Dispose();
            _cache?.Dispose();

            _isDisposed = true;
        }
    }
}