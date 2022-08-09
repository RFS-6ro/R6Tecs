// ----------------------------------------------------------------------------
// The Proprietary or MIT License
// Copyright (c) 2022-2022 RFS_6ro <rfs6ro@gmail.com>
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using R6ThreadECS.Utils;

namespace R6ThreadECS
{
    public class R6ComponentPool<T>
        where T : struct, IR6EcsComponent<T>
    {
        private ResizeableArray<T> _components;
        private ResizeableArray<int> _cache;

        private Type _targetType;
        
        public R6ComponentPool(int capacity = 256)
        {
            capacity = capacity <= 0 ? 256 : capacity;

            _components = new ResizeableArray<T>(capacity);
            _cache = new ResizeableArray<int>(capacity);

            _targetType = typeof(T);
        }

        public int Length => _components.Length;
        public int CacheLength => _cache.Capacity - _cache.Length;

        public Type TargetType => _targetType;

        public int Instantiate()
        {
            if (CacheLength > 0)
            {
            }

            throw new NotImplementedException();
        }
    }
}