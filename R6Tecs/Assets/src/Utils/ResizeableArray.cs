// ----------------------------------------------------------------------------
// The Proprietary or MIT License
// Copyright (c) 2022-2022 RFS_6ro <rfs6ro@gmail.com>
// ----------------------------------------------------------------------------

using System;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace R6ThreadECS.Utils
{
    public class ResizeableArray<T> : IDisposable
    {
        private T[] _items;

        private int _count;
        
        private bool _isIsLocked = false;
        
        private bool _isDisposed = false;

        public ResizeableArray(int capacity = 256, Func<int, T> getNewItem = null)
        {
            capacity = capacity > 0 ? capacity : 256;

            _items = new T[capacity];
            _count = 0;

            if (getNewItem == null)
            {
                return;
            }

            for (int i = 0; i < capacity; i++)
            {
                Add(getNewItem(i));
            }
        }

        [PublicAPI]
        public bool IsDisposed => _isDisposed;
        
        [PublicAPI]
        public int Capacity => _items.Length;
        
        [PublicAPI]
        public int Count => _count;
        
        [PublicAPI]
        public bool IsLocked => _isIsLocked;

        [PublicAPI, MethodImpl (MethodImplOptions.AggressiveInlining)]
        public int Add(T item)
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException($"ResizeableArray<{typeof(T)}>");
            }
            
            if (_isIsLocked)
            {
#if DEBUG
                throw new NotSupportedException();
#else
                return -1;
#endif
            }
            
            if (_count == Capacity)
            {
                Resize(_items.Length << 1);
            }

            _items[_count] = item;
            
            return _count++;
        }

        [PublicAPI, MethodImpl (MethodImplOptions.AggressiveInlining)]
        public bool Remove(int index)
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException($"ResizeableArray<{typeof(T)}>");
            }
            
            if (_isIsLocked)
            {
                return false;
            }
            
            if (index < 0 || index >= _count)
            {
                return false;
            }

            for (int i = index; i < _count - 1; i++)
            {
                _items[i] = _items[i + 1];
            }

            _count--;
            return true;
        }

        [PublicAPI, MethodImpl (MethodImplOptions.AggressiveInlining)]
        public bool Remove(int index, out T element)
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException($"ResizeableArray<{typeof(T)}>");
            }
            
            element = default(T);
            
            if (_isIsLocked)
            {
                return false;
            }
            
            if (index < 0 || index >= _count)
            {
                return false;
            }

            element = _items[index];
            
            for (int i = index; i < _count; i++)
            {
                _items[i] = _items[i + 1];
            }

            _count--;
            return true;
        }

        [PublicAPI, MethodImpl (MethodImplOptions.AggressiveInlining)]
        public bool Has(T item)
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException($"ResizeableArray<{typeof(T)}>");
            }
            
            foreach (var item1 in _items)
            {
                if (item1.Equals(item))
                {
                    return true;
                }
            }

            return false;
        }
        
        [PublicAPI, MethodImpl (MethodImplOptions.AggressiveInlining)]
        public void Lock()
        {
            _isIsLocked = true;
        }

        [PublicAPI, MethodImpl (MethodImplOptions.AggressiveInlining)]
        public void Unlock()
        {
            _isIsLocked = false;
        }

        [PublicAPI, MethodImpl (MethodImplOptions.AggressiveInlining)]
        public ref T GetRef(int index)
        {
            return ref _items[index];
        }

        [PublicAPI, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Resize(int newLength)
        {
            Array.Resize(ref _items, newLength);
        }

        [PublicAPI, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AssertIndex(int index)
        {
            if (index < _count)
            {
                return;
            }
            
            int newLength = _count;
            while (newLength <= index)
            {
                newLength <<= 1;
            }
            Resize(newLength);
        }
        
        [PublicAPI]
        public T this[int index]
        {
            get
            {
                if (_isDisposed)
                {
                    throw new ObjectDisposedException($"ResizeableArray<{typeof(T)}>");
                }
                
                if (index < 0 || index >= _count)
                {
                    throw new ArgumentOutOfRangeException();
                }

                return _items[index];
            }
            set
            {
                if (_isDisposed)
                {
#if DEBUG
                    throw new ObjectDisposedException($"ResizeableArray<{typeof(T)}>");
#else
                    return;
#endif
                }
                
                if (_isIsLocked)
                {
#if DEBUG
                    throw new NotSupportedException();
#else
                    return;
#endif
                }
                
                if (index < 0 || index >= _count)
                {
                    throw new ArgumentOutOfRangeException();
                }

                _items[index] = value;
            }
        }

        [PublicAPI]
        public void Dispose()
        {
            if (_isDisposed)
            {
                return;
            }
            
            Array.Clear(_items, 0, Capacity);
            _items = null;
            _isDisposed = true;
        }
    }
}