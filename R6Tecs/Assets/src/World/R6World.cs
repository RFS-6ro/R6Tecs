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
using R6ThreadECS.Components;
using R6ThreadECS.Entity;
using R6ThreadECS.ExecutionStrategy;
using R6ThreadECS.Filters;
using R6ThreadECS.Systems;
using R6ThreadECS.Utils;
using UnityEngine;

namespace R6ThreadECS.World
{
    public class tmp : IDisposable
    {
        private GrowingArray<R6SystemInfo> _systemInfos = 
            new GrowingArray<R6SystemInfo>();
        
        private GrowingArray<IR6ComponentPool> _componentPools = 
            new GrowingArray<IR6ComponentPool>();

        private Dictionary<Type, int> _componentsTypes = 
            new Dictionary<Type, int>();

        private GrowingArray<R6EntityFilter> _filters = 
            new GrowingArray<R6EntityFilter>();
        
        // private Dictionary<int, GrowingArray<R6EntityFilter>> _include =
        //     new Dictionary<int, GrowingArray<R6EntityFilter>>();
        // private Dictionary<int, GrowingArray<R6EntityFilter>> _exclude =
        //     new Dictionary<int, GrowingArray<R6EntityFilter>>();

        private R6ExecutionStrategy _strategy;

        private int _priority = 0;
        
        private bool _isDisposed = false;
        private bool _isLocked = false;
        private bool _isStrategyLocked = false;
        
        public tmp()
        {
        }

        #region Properties

        [PublicAPI]
        public bool IsDisposed => _isDisposed;

        [PublicAPI]
        public bool IsLocked => _isLocked;

        [PublicAPI]
        public IReadOnlyCollection<R6EntityFilter> Filters => _filters;

        [PublicAPI]
        public int Priority 
        {
            get
            {
                return _priority;
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

                _priority = value;
            } 
        }

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
                
                if (_isStrategyLocked)
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

        [PublicAPI, MethodImpl (MethodImplOptions.AggressiveInlining)]
        public R6Entity Instantiate()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Components
        
        [PublicAPI, MethodImpl (MethodImplOptions.AggressiveInlining)]
        public R6ComponentPool<T> GetOrAddPool<T>()
            where T : struct, IR6EcsComponent<T>
        {
            int typeId = ComponentsMapper<T>.Id;

            if (typeId == -1)
            {
                return null;
            }

            _componentPools.AssertIndex(typeId);
            R6ComponentPool<T> pool = (R6ComponentPool<T>)_componentPools[typeId];
            if (pool == null)
            {
                pool = new R6ComponentPool<T>();
                _componentPools[typeId] = pool;
            }

            return pool;
        }

        [PublicAPI, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IR6ComponentPool GetPool(int typeId) => _componentPools[typeId];

        #endregion

        #region Filters

        public void UpdateFilters(int typeId, R6Entity entity)
        {
            bool isNewEntity = entity.ComponentTypes.Has(typeId);
            
            
        }

        #endregion

        #region Systems and ParallelGroups

        [PublicAPI]
        public void Add(R6EcsSystem system)
        {
            EnsureStrategy();
            system.SetOwner(this);
            system.SetFilterId(_filters.Add(new R6EntityFilter()));
            
            
            
            throw new System.NotImplementedException();
        }

        [PublicAPI]
        public void Add(R6ParallelGroup parallelGroup)
        {
            EnsureStrategy();
            parallelGroup.SetOwner(this);
            foreach (R6EcsSystem r6EcsSystem in parallelGroup)
            {
                Add(r6EcsSystem);
            }
            
            
            throw new System.NotImplementedException();
        }

        private void EnsureStrategy()
        {
            _ = Strategy;
            _isStrategyLocked = true;
        }
        
        #endregion

        [PublicAPI]
        public void Build()
        {
            if (_isLocked)
            {
                return;
            }
            
            ComponentsMapper.PerformComponentsMapping(_systemInfos);
            
            Strategy.Initialize();

            _isLocked = true;
        }
        
        [PublicAPI]
        public void Dispose()
        {
            if (_isDisposed)
            {
                return;
            }
            
            Strategy.Dispose();

            int systemInfosLength = _systemInfos.Count;
            for (int i = 0; i < systemInfosLength; ++i)
            {
                var systemInfo = _systemInfos[i];
                systemInfo.Dispose();
            }
            _systemInfos = null;

            int componentPoolsLength = _componentPools.Count;
            for (int i = 0; i < componentPoolsLength; ++i)
            {
                var componentPool = _componentPools[i];
                componentPool.Dispose();
            }
            _componentPools = null;

            _componentsTypes.Clear();
            _componentsTypes = null;

            int filtersLength = _filters.Count;
            for (int i = 0; i < filtersLength; ++i)
            {
                var filter = _filters[i];
                filter.Dispose();
            }
            _filters.Dispose();
            _filters = null;

            // foreach (var includeFilters in _include)
            // {
            //     var filters = includeFilters.Value;
            //     filtersLength = filters.Count;
            //     for (int i = 0; i < filtersLength; ++i)
            //     {
            //         var filter = filters[i];
            //         filter.Dispose();
            //     }
            //     filters.Dispose();
            // }
            // _include.Clear();
            // _include = null;
            //
            // foreach (var excludeFilters in _exclude)
            // {
            //     var filters = excludeFilters.Value;
            //     filtersLength = filters.Count;
            //     for (int i = 0; i < filtersLength; ++i)
            //     {
            //         var filter = filters[i];
            //         filter.Dispose();
            //     }
            //     filters.Dispose();
            // }
            // _exclude.Clear();
            // _exclude = null;
            
            _isDisposed = true;
        }

        public void CheckLeak(R6Entity r6Entity)
        {
            throw new NotImplementedException();
        }
    }
}
