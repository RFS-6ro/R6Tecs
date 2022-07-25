// ----------------------------------------------------------------------------
// The Proprietary or MIT License
// Copyright (c) 2022-2022 RFS_6ro <rfs6ro@gmail.com>
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using R6ThreadECS.Entity;

namespace R6ThreadECS.Filters
{
    public class R6EntityFilter
    {
        private bool _isLocked = false;
        
        private R6Entity[] _entities;
        
        public R6EntityFilter(int capacity)
        {
            _entities = new R6Entity[capacity];
            Count = 0;
        }

        [PublicAPI]
        public void Lock()
        {
            _isLocked = true;
        }
        
        [PublicAPI]
        public int Count { get; private set; }

        [PublicAPI]
        public void Add(R6Entity newEntity)
        {
            if (_isLocked)
            {
                throw new System.NotSupportedException();
            }
            
            if (_entities.Length == Count) 
            {
                System.Array.Resize (ref _entities, _entities.Length << 1);
            }

            Count++;
            _entities[Count] = newEntity;
        }

        [PublicAPI]
        public bool Contains(R6Entity item) => _entities.Contains(item);
        
        public IEnumerator<R6Entity> GetEnumerator()
        {
            return new R6EntityFilterEnumerator(this);
        }

        public R6Entity this[int index]
        {
            get
            {
                if (index < 0 || index >= Count)
                {
                    throw new System.ArgumentOutOfRangeException();
                }

                return _entities[index];
            }
            set
            {
                if (_isLocked)
                {
#if DEBUG
                    throw new System.NotSupportedException();
#else
                    return;
#endif
                }
                
                if (index < 0 || index >= Count)
                {
                    throw new System.ArgumentOutOfRangeException();
                }

                _entities[index] = value;
            }
        }
    }
}
