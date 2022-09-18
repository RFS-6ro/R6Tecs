// ----------------------------------------------------------------------------
// The Proprietary or MIT License
// Copyright (c) 2022-2022 RFS_6ro <rfs6ro@gmail.com>
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using JetBrains.Annotations;
using R6Tasks.Parallelizing;
using R6Tasks.Utils;
using R6ThreadECS.Attributes;
using R6ThreadECS.Entity;
using R6ThreadECS.ExecutionStrategy;
using R6ThreadECS.Systems;
using R6ThreadECS.Utils;

namespace R6ThreadECS.World
{
    public class R6World
    {
        private readonly List<R6EcsSystem> _allSystems;
        private Dictionary<int, List<FilterAttribute>> _filters;

        private List<int> _componentsTypes;

        private List<R6Entity> _entities;
        private List<IR6EcsComponent> _components;

        private R6ExecutionStrategy _strategy;
        
        private readonly SystemsInfo _preInitSystems;
        private readonly SystemsInfo _initSystems;
        private readonly SystemsInfo _updateSystems;
        private readonly SystemsInfo _fixedUpdateSystems;
        private readonly SystemsInfo _lateUpdateSystems;

        private bool _lock = false;
        
        public R6World()
        {
            _preInitSystems = new SystemsInfo(this);
            _initSystems = new SystemsInfo(this);
            _updateSystems = new SystemsInfo(this);
            _fixedUpdateSystems = new SystemsInfo(this);
            _lateUpdateSystems = new SystemsInfo(this);
            
            _allSystems = new List<R6EcsSystem>();
            _entities = new List<R6Entity>();
            _components = new List<IR6EcsComponent>();
            
            _filters = new Dictionary<int, List<FilterAttribute>>();
        }

        [PublicAPI]
        public int Priority { get; private set; } = 0;

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
            private set
            {
                _strategy = value;
            }
        }

        [PublicAPI]
        public void Initialize()
        {
            _lock = true;
            
            InitializeSystems();
        }

        [PublicAPI]
        public void SetPriority(int value)
        {
            if (_lock)
            {
#if DEBUG
                throw new NotSupportedException();       
#endif
                return;
            }

            Priority = value;
        }

        [PublicAPI]
        public void SetExecutionStrategy(R6ExecutionStrategy strategy)
        {
            if (_lock)
            {
#if DEBUG
                throw new NotSupportedException();       
#endif
                return;
            }

            if (strategy == null)
            {
#if DEBUG
                throw new ArgumentNullException(nameof(strategy));       
#endif
                return;
            }
            
            Strategy = strategy;
        }

        private void InitializeSystems()
        {
            
            //Sorting systems indexes
            _preInitSystems.Initialize();
            _initSystems.Initialize();
            _updateSystems.Initialize();
            _fixedUpdateSystems.Initialize();
            _lateUpdateSystems.Initialize();
        }

        [PublicAPI]
        public R6Entity InstantiateEntity()
        {
            int id = 0;
            // R6Entity entity = new R6Entity(this, id);
            throw new NotImplementedException();
            // return entity;
        }

        [PublicAPI]
        public R6Entity InstantiateEntity(IEnumerable<IR6EcsComponent> components)
        {
            R6Entity entity = InstantiateEntity();
            
            // init components
            int startIndex = _components.Count;
            _components.AddRange(components);
            int endIndex = _components.Count;
            entity.ComponentTypes = new ResizeableArray<int>(endIndex - startIndex);
            for (int i = 0; i < entity.ComponentTypes.Count; i++)
            {
                entity.ComponentTypes.Add(startIndex + i);
            }

            UpdateFilters();
            return entity;
        }

        private void PerformComponentsMapping()
        {
            // foreach (var r6SystemInfo in systemInfos)
            // {
            //     IEnumerable<FilterAttribute> attributes = 
            //         r6SystemInfo.GetType()
            //             .GetCustomAttributes(typeof(FilterAttribute), false)
            //             .Cast<FilterAttribute>();
            //
            //     foreach (var filterAttribute in attributes)
            //     {
            //         int index = types.Count;
            //
            //         var componentType = filterAttribute.ComponentType;
            //         
            //         if (types.Contains(componentType))
            //         {
            //             index = types.IndexOf(componentType);
            //         }
            //         else
            //         {
            //             types.Add(componentType);
            //         }
            //
            //         if (filterAttribute.FilterAccessType == FilterAccessType.ReadWrite)
            //         {
            //             r6SystemInfo.Writes.Add(index);
            //         }
            //         else
            //         {
            //             r6SystemInfo.Reads.Add(index);
            //         }
            //     }
            // }
        }
        private void UpdateFilters()
        {
            throw new NotImplementedException();
        }

        [PublicAPI]
        public R6World AddParallelGroup(R6ParallelGroup parallelGroup)
        {
            if (_lock)
            {
#if DEBUG
                throw new NotSupportedException();       
#endif
                return this;
            }

            // parallelGroup.SetOwner(this);

            throw new NotImplementedException();
            // int index = _allSystems.Count;
            // _allSystems.Add(parallelGroup);
            //
            // HandleType(parallelGroup, index);
            // HandleAttributes<T>(index);

            return this;
        }

        [PublicAPI]
        public R6World AddSystem<T>(T system)
            where T : R6EcsSystem, IR6EcsSystem
        {
            if (_lock)
            {
#if DEBUG
                throw new NotSupportedException();       
#endif
                return this;
            }

            // system.SetOwner(this);
            
            int index = _allSystems.Count;
            _allSystems.Add(system);

            HandleType(system, index);
            HandleAttributes<T>(index);

            return this;
        }

        private void HandleType<T>(T system, int globalIndex)
            where T : R6EcsSystem, IR6EcsSystem
        {
            // ReSharper disable once ConvertIfStatementToSwitchStatement
            if (system is IR6PreInitSystem r6PreInitSystem)
            {
                _preInitSystems.Add(globalIndex, r6PreInitSystem.PreInit);
            }

            if (system is IR6InitSystem r6InitSystem)
            {
                _initSystems.Add(globalIndex, r6InitSystem.Init);
            }
            
            if (system is IR6UpdateSystem r6UpdateSystem)
            {
                _updateSystems.Add(globalIndex, r6UpdateSystem.Update);
            }
            
            if (system is IR6FixedUpdateSystem r6FixedUpdateSystem)
            {
                _fixedUpdateSystems.Add(globalIndex, r6FixedUpdateSystem.FixedUpdate);
            }
            
            if (system is IR6LateUpdateSystem r6LateUpdateSystem)
            {
                _lateUpdateSystems.Add(globalIndex, r6LateUpdateSystem.LateUpdate);
            }
        }
        
        private void HandleAttributes<T>(int index)
            where T : R6EcsSystem, IR6EcsSystem
        {
            MemberInfo memberInfo = typeof(T);
            object[] attributes = memberInfo.GetCustomAttributes(true);
            
            SetFilters(attributes, index);
        }

        private void SetFilters(object[] attributes, int index)
        {
            List<FilterAttribute> filterAttributes = attributes
                .Where((attribute) => attribute is FilterAttribute)
                .Select((attribute) => ((FilterAttribute) attribute))
                .ToList();

            if (filterAttributes.Count == 0)
            {
                return;
            }
            
            _filters.Add(index, filterAttributes);
        }
        
        [PublicAPI]
        public R6World AddOneFrameComponent<T>()
            where T : struct, IR6EcsComponent<T>
        {
            if (_lock)
            {
#if DEBUG
                throw new NotSupportedException();       
#endif
                return this;
            }
            
            throw new NotImplementedException();

            return this;
        }

        [PublicAPI]
        public bool TryGetComponent<T>(int componentId, out T? r6EcsComponent) 
            where T : struct, IR6EcsComponent<T>
        {
            if (_components[componentId] is T)
            {
                r6EcsComponent = (T) _components[componentId];
                return true;
            }

            r6EcsComponent = null;
            return false;
        }

        public void Dispose()
        {
            _preInitSystems.Dispose();
            _initSystems.Dispose();
            _updateSystems.Dispose();
            _fixedUpdateSystems.Dispose();
            _lateUpdateSystems.Dispose();
        }

        public void Destroy()
        {
            throw new NotImplementedException();
        }

        private class SystemsInfo : IDisposable
        {
            private readonly R6World _world;
            private CancellationTokenSource _cts;
            
            public List<int> SystemIds;
            public List<Action> Actions;
            public R6Parallelizer Parallelizer;

            private List<int[]> _edges;
            
            public SystemsInfo(R6World world)
            {
                _world = world;
                SystemIds = new List<int>();
                Actions = new List<Action>();
            }

            [PublicAPI]
            public void Add(int id, Action action)
            {
                SystemIds.Add(id);
                Actions.Add(action);
            }

            [PublicAPI]
            public void Initialize()
            {
                BuildDependencies();
                InitiateParallelizer();
            }

            [PublicAPI]
            public void Iterate()
            {
                Parallelizer.Resolve();
            }

            private void BuildDependencies()
            {
                //TODO: use _world._filters;
                throw new NotImplementedException();
            }
            
            private void InitiateParallelizer()
            {
                Parallelizer = new R6Parallelizer(CancellationUtils.RefreshToken(ref _cts));

                for (int i = 0; i < SystemIds.Count; i++)
                {
                    Parallelizer.AddTask(SystemIds[i], Actions[i], _edges?[i]);
                }
            }

            public void Dispose()
            {
                CancellationUtils.Cancel(_cts);
            }
        }
    }
}
