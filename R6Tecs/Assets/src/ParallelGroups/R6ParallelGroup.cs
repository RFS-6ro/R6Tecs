// ----------------------------------------------------------------------------
// The Proprietary or MIT License
// Copyright (c) 2022-2022 RFS_6ro <rfs6ro@gmail.com>
// ----------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
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

            _systemInfos = new List<R6SystemInfo>(systems.Length);
            foreach (var r6EcsSystem in systems)
            {
                var systemInfo = CreateSystemInfo(r6EcsSystem);
                if (systemInfo == null)
                {
                    throw new ArgumentException("System types are not supported");
                }
                _systemInfos.Add(systemInfo);
            }

            if (!CheckDependencies())
            {
                throw new ArgumentException("Group filters has collisions");
            }
        }

        [PublicAPI]
        public bool TryAdd(R6EcsSystem r6EcsSystem)
        {
            var systemInfo = CreateSystemInfo(r6EcsSystem);
            if (systemInfo == null)
            {
                throw new ArgumentException("System type is not supported");
            }
            _systemInfos.Add(systemInfo);

            if (!CheckDependencies())
            {
                _systemInfos.Remove(systemInfo);
                return false;
            }

            return true;
        }

        private R6SystemInfo CreateSystemInfo(R6EcsSystem r6EcsSystem)
        {
            Action targetAction;
            IR6EcsSystem targetSystem = (IR6EcsSystem) r6EcsSystem;
            switch (targetSystem)
            {
                case IR6PreInitSystem initSystem:
                    targetAction = initSystem.PreInit;
                    break;
                case IR6InitSystem initSystem:
                    targetAction = initSystem.Init;
                    break;
                case IR6UpdateSystem initSystem:
                    targetAction = initSystem.Update;
                    break;
                case IR6FixedUpdateSystem initSystem:
                    targetAction = initSystem.FixedUpdate;
                    break;
                case IR6LateUpdateSystem initSystem:
                    targetAction = initSystem.LateUpdate;
                    break;
                default:
                    return null;
            }

            return new R6SystemInfo(targetSystem, targetAction);
        }

        private bool CheckDependencies()
        {
            HashSet<int> set = new HashSet<int>();
            
            int length;
            foreach (var r6SystemInfo in _systemInfos)
            {
                length = r6SystemInfo.Writes.Count;
                for (int i = 0; i < length; i++)
                {
                    int id = r6SystemInfo.Writes[i];
                    if (!set.Add(id))
                    {
                        return false;
                    }
                }
            }
            
            foreach (var r6SystemInfo in _systemInfos)
            {
                length = r6SystemInfo.Reads.Count;
                for (int i = 0; i < length; i++)
                {
                    int id = r6SystemInfo.Reads[i];
                    if (set.Contains(id))
                    {
                        return false;
                    }
                }
            }
            
            set.Clear();

            return true;
        }

        [PublicAPI]
        public int Count => _systemInfos.Count;

        [PublicAPI]
        public void SetOwner(tmp r6World)
        {
            foreach (var systemInfo in _systemInfos)
            {
                systemInfo.SystemImpl.SetOwner(r6World);
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

                return _systemInfos[index].SystemImpl;
            }
        }

        public IEnumerator GetEnumerator()
        {
            return new R6ParallelGroupEnumerator(this);
        }
    }
}
