// ----------------------------------------------------------------------------
// The Proprietary or MIT License
// Copyright (c) 2022-2022 RFS_6ro <rfs6ro@gmail.com>
// ----------------------------------------------------------------------------

using System;

namespace R6ThreadECS.Utils
{
    public class ResizeableArray<T>
    {
        private T[] _items;

        private int _length;
        
        private bool _isIsLocked;

        public ResizeableArray(int capacity = 256, Func<int, T> getNewItem = null)
        {
            capacity = capacity > 0 ? capacity : 256;

            _items = new T[capacity];
            _length = 0;

            if (getNewItem == null)
            {
                return;
            }

            for (int i = 0; i < capacity; i++)
            {
                Add(getNewItem(i));
            }
        }

        public int Capacity => _items.Length;
        
        public int Length => _length;
        
        public bool IsLocked => _isIsLocked;

        public int Add(T item)
        {
            if (_isIsLocked)
            {
#if DEBUG
                throw new NotSupportedException();
#else
                return;
#endif
            }
            
            if (_length == Capacity)
            {
                Array.Resize (ref _items, _items.Length << 1);
            }

            _items[_length] = item;
            
            return _length++;
        }

        public bool Remove(int index)
        {
            if (_isIsLocked)
            {
                return false;
            }
            
            if (index < 0 || index >= _length)
            {
                return false;
            }

            for (int i = index; i < _length; i++)
            {
                _items[i] = _items[i + 1];
            }

            _length--;
            return true;
        }
        
        public void Lock()
        {
            _isIsLocked = true;
        }

        public void Unlock()
        {
            _isIsLocked = false;
        }

        public T this[int index]
        {
            get
            {
                if (index < 0 || index >= _length)
                {
                    throw new ArgumentOutOfRangeException();
                }

                return _items[index];
            }
            set
            {
                if (_isIsLocked)
                {
#if DEBUG
                    throw new NotSupportedException();
#else
                    return;
#endif
                }
                
                if (index < 0 || index >= _length)
                {
                    throw new ArgumentOutOfRangeException();
                }

                _items[index] = value;
            }
        }
    }
}