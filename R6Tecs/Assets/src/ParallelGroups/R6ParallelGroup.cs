// ----------------------------------------------------------------------------
// The Proprietary or MIT License
// Copyright (c) 2022-2022 RFS_6ro <rfs6ro@gmail.com>
// ----------------------------------------------------------------------------

using System;
using System.Collections;
using JetBrains.Annotations;
using R6ThreadECS.World;

namespace R6ThreadECS.Systems
{
    public class R6ParallelGroup : IEnumerable
    {
        private R6EcsSystem[] _systems;
        
        public R6ParallelGroup(params R6EcsSystem[] systems)
        {
            if (systems == null)
            {
                throw new ArgumentNullException(nameof(systems));
            }
            
            if (systems.Length == 0)
            {
                throw new ArgumentException("Group contain no elements", nameof(systems));
            }
            
            _systems = systems;
            
            CheckDependencies();
        }

        private void CheckDependencies()
        {
            
        }

        [PublicAPI]
        public int Count => _systems.Length;

        [PublicAPI]
        public void SetOwner(R6World r6World)
        {
            foreach (var r6EcsSystem in _systems)
            {
                r6EcsSystem.SetOwner(r6World);
            }
        }
        
        public R6EcsSystem this[int index]
        {
            get
            {
                if (index < 0 || index >= Count)
                {
                    throw new ArgumentOutOfRangeException();
                }

                return _systems[index];
            }
        }

        public IEnumerator GetEnumerator()
        {
            return new R6ParallelGroupEnumerator(this);
        }
    }
}
