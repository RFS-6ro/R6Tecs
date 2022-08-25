// ----------------------------------------------------------------------------
// The Proprietary or MIT License
// Copyright (c) 2022-2022 RFS_6ro <rfs6ro@gmail.com>
// ----------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using R6ThreadECS.World;

namespace R6ThreadECS.Systems
{
    public class R6ParallelGroup : IEnumerable
    {
        private List<R6SystemInfo> _systemInfos;
        
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
            
            // _systemInfos = systems.ToList();

            if (!CheckDependencies())
            {
                throw new ArgumentException();
            }
        }

        [PublicAPI]
        public bool TryAdd(R6EcsSystem r6EcsSystem)
        {
            // _systemInfos.Add(r6EcsSystem);

            if (!CheckDependencies())
            {
                // _systemInfos.Remove(r6EcsSystem);
                return false;
            }

            return true;
        }

        private bool CheckDependencies()
        {
            HashSet<int> set = new HashSet<int>();
            
            foreach (var r6SystemInfo in _systemInfos)
            {
                foreach (var id in r6SystemInfo.Writes)
                {
                    if (!set.Add(id))
                    {
                        return false;
                    }
                }
            }
            
            foreach (var r6SystemInfo in _systemInfos)
            {
                foreach (var id in r6SystemInfo.Reads)
                {
                    if (set.Contains(id))
                    {
                        return false;
                    }
                }   
            }

            return true;
        }

        [PublicAPI]
        public int Count => _systemInfos.Count;

        [PublicAPI]
        public void SetOwner(R6World r6World)
        {
            foreach (var r6SystemInfo in _systemInfos)
            {
                // r6SystemInfo.System.SetOwner(r6World);
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
                throw new NotImplementedException();

                // return _systemInfos[index];
            }
        }

        public IEnumerator GetEnumerator()
        {
            return new R6ParallelGroupEnumerator(this);
        }
    }
}
