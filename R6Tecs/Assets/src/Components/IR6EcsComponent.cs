// ----------------------------------------------------------------------------
// The Proprietary or MIT License
// Copyright (c) 2022-2022 RFS_6ro <rfs6ro@gmail.com>
// ----------------------------------------------------------------------------

using System;
using System.Runtime.CompilerServices;

namespace R6ThreadECS
{
    public interface IR6EcsComponent
    {
    }
    
    /// <summary>
    /// base for all components
    /// </summary>
    public interface IR6EcsComponent<T> : IR6EcsComponent
        where T : struct, IR6EcsComponent<T>
    {
        void Write(T other);
        
        T Read();
    }

    public interface IR6ResetEcsComponent<T> : IR6EcsComponent<T>
        where T : struct, IR6EcsComponent<T>
    {
        void Reset();
    }

    public struct ComponentData<T>
        where T : struct, IR6EcsComponent<T>
    {
        private int _id;
        private R6ComponentPool<T> _pool;

        public ComponentData(int id, R6ComponentPool<T> pool)
        {
            _id = id;
            _pool = pool;
        }

        public ref T GetRef() => ref _pool.GetRef(_id);

        public bool Equals(ComponentData<T> other) => _id == other._id && Equals(_pool, other._pool);
        
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (obj is ComponentData<T> other)
            {
                return Equals(other);
            }

            return false;
        }
    }
}
