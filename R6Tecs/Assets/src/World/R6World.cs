// ----------------------------------------------------------------------------
// The Proprietary or MIT License
// Copyright (c) 2022-2022 RFS_6ro <rfs6ro@gmail.com>
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using R6ThreadECS.Attributes;
using R6ThreadECS.Entity;
using R6ThreadECS.ExecutionStrategy;
using R6ThreadECS.Systems;
using R6ThreadECS.Utils;
using UnityEngine;

namespace R6ThreadECS.World
{
    public class tmp : IDisposable
    {
        private ResizeableArray<R6SystemInfo> _systemInfos = 
            new ResizeableArray<R6SystemInfo>();
        
        private ResizeableArray<IR6ComponentPool> _componentPools = 
            new ResizeableArray<IR6ComponentPool>();

        private Dictionary<Type, int> _componentsTypes = 
            new Dictionary<Type, int>();

        private ResizeableArray<ResizeableArray<R6Entity>> _filters = 
            new ResizeableArray<ResizeableArray<R6Entity>>();
        private Dictionary<int, ResizeableArray<ResizeableArray<R6Entity>>> _include =
            new Dictionary<int, ResizeableArray<ResizeableArray<R6Entity>>>();
        private Dictionary<int, ResizeableArray<ResizeableArray<R6Entity>>> _exclude =
            new Dictionary<int, ResizeableArray<ResizeableArray<R6Entity>>>();

        private R6ExecutionStrategy _strategy;

        private bool _isDisposed = false;
        private bool _isLocked = false;
        
        public tmp()
        {
        }

        #region Properties

        [PublicAPI]
        public int Priority { get; private set; } = 0;

        [PublicAPI]
        public bool IsDisposed => _isDisposed;

        [PublicAPI]
        public bool IsLocked => _isLocked;

        [PublicAPI]
        public R6ExecutionStrategy Strategy
        {
            get
            {
                if (_strategy == null)
                {
                    _strategy = R6ExecutionStrategy.Auto;
                }

                return _strategy;
            }
            set
            {
                if (_isLocked)
                {
#if DEBUG
                    throw new NotSupportedException();       
#endif
                    return;
                }

                if (value == null)
                {
#if DEBUG
                    throw new ArgumentNullException(nameof(value));       
#endif
                    return;
                }

                _strategy = value;
            }
        }

        #endregion

        #region Entities

        [PublicAPI]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public R6Entity Instantiate()
        {
            throw new NotImplementedException();
        }
        

        #endregion

        #region Components
        
        [PublicAPI]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public R6ComponentPool<T> GetPool<T>()
            where T : struct, IR6EcsComponent<T>
        {
            R6ComponentPool<T> pool;
            
            try
            {
                int typeIndex = _componentsTypes[typeof(T)];
                pool = (R6ComponentPool<T>)_componentPools[typeIndex];
            }
            catch (KeyNotFoundException _)
            {
#if DEBUG
                Debug.LogError($"Trying to get not registered component of type {typeof(T)}");
                throw;   
#endif
                return null;
            }
            catch (ArgumentOutOfRangeException _)
            {
                pool = new R6ComponentPool<T>();
                _componentPools.Add(pool);
            }
            
            return pool;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void PerformComponentsMapping()
        {
            int systemsCount = _systemInfos.Length;
            for (int i = 0; i < systemsCount; i++)
            {
                R6SystemInfo r6SystemInfo = _systemInfos[i];
                
                IEnumerable<FilterAttribute> attributes = 
                    r6SystemInfo.GetType()
                        .GetCustomAttributes(typeof(FilterAttribute), false)
                        .Cast<FilterAttribute>();
            
                foreach (var filterAttribute in attributes)
                {
                    int index = _componentsTypes.Count;
            
                    var componentType = filterAttribute.ComponentType;
                    
                    if (_componentsTypes.ContainsKey(componentType))
                    {
                        index = _componentsTypes[componentType];
                    }
                    else
                    {
                        _componentsTypes.Add(componentType, index);
                    }
            
                    if (filterAttribute.FilterAccessType == FilterAccessType.ReadWrite)
                    {
                        r6SystemInfo.Writes.Add(index);
                    }
                    else
                    {
                        r6SystemInfo.Reads.Add(index);
                    }
                }
            }
        }

        #endregion

        #region Filters

        

        #endregion

        #region Systems and ParallelGroups
        

        #endregion

        [PublicAPI]
        public void Build()
        {
            PerformComponentsMapping();
            
            _isLocked = true;
        }
        
        [PublicAPI]
        public void Dispose()
        {
            
            
            _isDisposed = true;
        }
    }
}
