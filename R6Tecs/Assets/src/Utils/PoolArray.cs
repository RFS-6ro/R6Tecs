// ----------------------------------------------------------------------------
// The Proprietary or MIT License
// Copyright (c) 2022-2022 RFS_6ro <rfs6ro@gmail.com>
// ----------------------------------------------------------------------------

using System;
using System.Runtime.CompilerServices;

namespace R6ThreadECS.Utils
{
    public class PoolArray<T>
    {
        private ResizeableArray<T> _items;
        private ResizeableArray<int> _cache;

        private int _itemsLength;
        private Func<int, T> _instantiateFunc;
        
        public PoolArray(int capacity = 256, Func<int, T> getNewItem = null)
        {
            capacity = capacity <= 0 ? 256 : capacity;

            _instantiateFunc = getNewItem;
            _itemsLength = 0;
            _items = new ResizeableArray<T>(capacity, _instantiateFunc);
            _cache = new ResizeableArray<int>(capacity);
        }
        
        public int Length => _itemsLength;
        public int CacheLength => _cache.Length;
        
        public int ItemsCapacity => _items.Capacity;
        
        public int CacheCapacity => _cache.Capacity;

        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public T Get(int index)
        {
            return _items[index];
        }

        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public ref T GetRef(int index)
        {
            return ref _items.GetRef(index);
        }
        
        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public void Store(int index)
        {
            _items[index] = default(T);
            _cache.Add(index);
        }
        
        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public int Alloc()
        {
            int index;
            
            if (CacheLength > 0)
            {
                if (_cache.Remove(_cache.Length, out index))
                {
                    return index;
                }
            }

            index = _itemsLength;
            _items.Add(default(T));
            ++_itemsLength;
            
            return index;
        }
        
        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public int Alloc(T data)
        {
            int index = Alloc();

            _items[index] = data; 
            
            return index;
        }
        
        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public void Copy(int from, int to)
        {
            _items[to] = _items[from]; 
        }
    }
}