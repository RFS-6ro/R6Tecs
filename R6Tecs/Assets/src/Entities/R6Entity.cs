// ----------------------------------------------------------------------------
// The Proprietary or MIT License
// Copyright (c) 2022-2022 RFS_6ro <rfs6ro@gmail.com>
// ----------------------------------------------------------------------------

using System;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using R6ThreadECS.Components;
using R6ThreadECS.Utils;
using R6ThreadECS.World;

namespace R6ThreadECS.Entity
{
    /// <summary>
    /// components container
    /// </summary>
    public struct R6Entity : IDisposable
    {
        private tmp _world;
        private int _id;
        
        public ResizeableArray<int> ComponentTypes;
        public ResizeableArray<int> Components;
        
        private bool _isDisposed;

        public R6Entity(tmp world, int id)
        {
            if (world == null)
            {
                _isDisposed = true;
                throw new System.NotSupportedException();
            }
            
            _isDisposed = false;
            
            _world = world;
            _id = id;
            ComponentTypes = new ResizeableArray<int>();
            Components = new ResizeableArray<int>();
        }
        
        [PublicAPI]
        public int Id => _id;
        
        [PublicAPI]
        public int ComponentsCount => ComponentTypes.Count;

        [PublicAPI]
        public tmp World => _world;

        [PublicAPI]
        public bool IsDisposed => _isDisposed;

        [PublicAPI, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref T Get<T>()
            where T : struct, IR6EcsComponent<T>
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException("R6Entity");
            }
            
            int typeId = ComponentsMapper<T>.Id;
            int localId = Search(typeId);
            if (localId == -1)
            {
                Add(default(T));
            }

            return ref _world.GetOrAddPool<T>().GetRef(Components[localId]);
        }
        
        [PublicAPI, MethodImpl (MethodImplOptions.AggressiveInlining)]
        public void Set<T>(T component)
            where T : struct, IR6EcsComponent<T>
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException("R6Entity");
            }
            
            int typeId = ComponentsMapper<T>.Id;
            int localId = Search(typeId);
            if (localId == -1)
            {
                Add(component);
                return;
            }

            _world.GetOrAddPool<T>().Set(Components[localId], component);
        }
        
        [PublicAPI, MethodImpl (MethodImplOptions.AggressiveInlining)]
        public void AddSafe<T>(T component)
            where T : struct, IR6EcsComponent<T>
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException("R6Entity");
            }
            
            if (Has<T>()) { return; }
            Add(component);
        }
        
        [PublicAPI, MethodImpl (MethodImplOptions.AggressiveInlining)]
        public void Add<T>(T component)
            where T : struct, IR6EcsComponent<T>
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException("R6Entity");
            }
            
            int typeId = ComponentsMapper<T>.Id;
            R6ComponentPool<T> pool = _world.GetOrAddPool<T>();
            int poolId = pool.Alloc(component);
            int localId = ComponentTypes.Add(typeId);
            Components.AssertIndex(localId);
            Components[localId] = poolId;
            _world.UpdateFilters(typeId, this);
        }

        [PublicAPI, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Remove<T>()
            where T : struct, IR6EcsComponent<T>
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException("R6Entity");
            }
            
            int typeId = ComponentsMapper<T>.Id;
            Remove(typeId);
        }

        [PublicAPI, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Remove(int typeId)
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException("R6Entity");
            }
            
            int localId = Search(typeId);
            if (localId == -1)
            {
                _world.CheckLeak(this);
                return;
            }

            _world.GetPool(typeId).Store(Components[localId]);
            ComponentTypes.Remove(localId);
            Components.Remove(localId);
            
            _world.UpdateFilters(typeId, this);
            _world.CheckLeak(this);
        }
        
        [PublicAPI, MethodImpl (MethodImplOptions.AggressiveInlining)]
        public int Search<T>()
            where T : struct, IR6EcsComponent<T>
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException("R6Entity");
            }
            
            int typeId = ComponentsMapper<T>.Id;
            return Search(typeId);
        }

        [PublicAPI, MethodImpl (MethodImplOptions.AggressiveInlining)]
        public int Search(int typeId)
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException("R6Entity");
            }
            
            for (int i = 0; i < ComponentTypes.Count; i++)
            {
                if (ComponentTypes[i] == typeId)
                {
                    return i;
                }
            }

            return -1;
        }
        
        [PublicAPI, MethodImpl (MethodImplOptions.AggressiveInlining)]
        public bool Has<T>()  where T : struct, IR6EcsComponent<T> => Search<T>() != -1;

        [PublicAPI, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ComponentData<T> Access<T>()
            where T : struct, IR6EcsComponent<T>
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException("R6Entity");
            }
            
            int typeId = ComponentsMapper<T>.Id;
            
            int localId = Search(typeId);
            if (localId == -1)
            {
#if DEBUG
                throw new AccessViolationException();       
#endif
                return default;
            }

            return _world.GetOrAddPool<T>().Access(Components[localId]);
        }

        [PublicAPI, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Dispose()
        {
            if (_isDisposed)
            {
                return;
            }
            
            for (int i = ComponentTypes.Count - 1; i >= 0 ; --i)
            {
                Remove(ComponentTypes[i]);
            }
            
            _world.CheckLeak(this);
            _world = null;
            _id = -1;
            _isDisposed = true;
        }

        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public static bool operator == (in R6Entity l, in R6Entity r) =>
            l._id == r._id && l.ComponentTypes.Count == r.ComponentTypes.Count;

        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public static bool operator != (in R6Entity l, in R6Entity r) =>
            l._id != r._id || l.ComponentTypes.Count != r.ComponentTypes.Count;
        
        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode() =>
            _id.GetHashCode() ^ _world?.GetHashCode() ?? 0;

        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object other) =>
            other is R6Entity otherEntity && Equals(otherEntity);
        
        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public bool Equals(R6Entity other) => 
            _id == other._id && _world == other._world;
    }
}
