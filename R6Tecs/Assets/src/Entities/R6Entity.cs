// ----------------------------------------------------------------------------
// The Proprietary or MIT License
// Copyright (c) 2022-2022 RFS_6ro <rfs6ro@gmail.com>
// ----------------------------------------------------------------------------

using System.Collections.Generic;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using R6ThreadECS.World;

namespace R6ThreadECS.Entity
{
    /// <summary>
    /// components container
    /// </summary>
    public struct R6Entity
    {
        private readonly R6World _world;
        private readonly int _id;
        
        public List<int> Components;

        public R6Entity(R6World world, int id)
        {
            if (world == null)
            {
                throw new System.NotSupportedException();
            }
            
            _world = world;
            _id = id;
            Components = new List<int>();
        }

        [PublicAPI]
        public T GetComponent<T>()
            where T : struct, IR6EcsComponent<T>
        {
            int index = SearchComponent<T>(out T? component);
            return component ?? new T(); //TODO: replace new T() with other construction
        }
        
        [PublicAPI]
        public bool HasComponent<T>()
            where T : struct, IR6EcsComponent<T>
        {
            return SearchComponent<T>(out _) == -1;
        }

        public int SearchComponent<T>(out T? component)
            where T : struct, IR6EcsComponent<T>
        {
            for (int i = 0; i < Components.Count; i++)
            {
                int componentId = Components[i];
                if (_world.TryGetComponent(componentId, out component))
                {
                    return componentId;
                }
            }

            component = null;
            return -1;
        }

        [PublicAPI]
        public void SetComponent<T>(T component)
            where T : struct, IR6EcsComponent<T>
        {
            throw new System.NotImplementedException();
            // T componentInMemory = default;
            //
            // componentInMemory.Write(component);
        }

        [PublicAPI]
        public void DeleteComponent<T>(T component)
            where T : struct, IR6EcsComponent<T>
        {
            int id = SearchComponent<T>(out _);
            if (id == -1)
            {
                return;
            }
            
            
            throw new System.NotImplementedException();
        }
        
        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public static bool operator == (in R6Entity l, in R6Entity r) =>
            l._id == r._id && l.Components.Count == r.Components.Count;

        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public static bool operator != (in R6Entity l, in R6Entity r) =>
            l._id != r._id || l.Components.Count != r.Components.Count;
        
        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode() =>
            _id.GetHashCode() ^ _world?.GetHashCode() ?? 0;

        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object other) =>
            other is R6Entity otherEntity && Equals(otherEntity);
    }
}
