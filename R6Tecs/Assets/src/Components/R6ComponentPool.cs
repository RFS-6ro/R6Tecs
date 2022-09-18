// ----------------------------------------------------------------------------
// The Proprietary or MIT License
// Copyright (c) 2022-2022 RFS_6ro <rfs6ro@gmail.com>
// ----------------------------------------------------------------------------

using System;
using R6ThreadECS.Utils;

namespace R6ThreadECS
{
    public interface IR6ComponentPool : IDisposable
    {
        Type TargetType { get; }
        
        void Store(int component);
    }

    public class R6ComponentPool<T> : PoolArray<T>, IR6ComponentPool where T : struct, IR6EcsComponent<T>
    {
        private Type _targetType;

        public R6ComponentPool(int capacity = 256, Func<int, T> getNewItem = null) : base(capacity, getNewItem)
        {
            _targetType = typeof(T);
        }

        public Type TargetType => _targetType;

        public ComponentData<T> Access(int id) => new ComponentData<T>(id, this);
    }
}
