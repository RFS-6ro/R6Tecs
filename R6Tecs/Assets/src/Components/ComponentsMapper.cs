// ----------------------------------------------------------------------------
// The Proprietary or MIT License
// Copyright (c) 2022-2022 RFS_6ro <rfs6ro@gmail.com>
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using R6ThreadECS.Attributes;
using R6ThreadECS.Systems;
using R6ThreadECS.Utils;
using UnityEngine;

namespace R6ThreadECS.Components
{
    public static class ComponentsMapper
    {
        private static Dictionary<Type, int> _componentsTypes = new Dictionary<Type, int>();
        
        public static void PerformComponentsMapping(GrowingArray<R6SystemInfo> systemInfos)
        {
            int systemsCount = systemInfos.Count;
            for (int i = 0; i < systemsCount; i++)
            {
                R6SystemInfo r6SystemInfo = systemInfos[i];
                
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

        public static int GetTypeId<T>()
            where T : struct, IR6EcsComponent<T>
        {
            int typeIndex = -1;
            try
            {
                typeIndex = _componentsTypes[typeof(T)];
            }
            catch (KeyNotFoundException _)
            {
#if DEBUG
                Debug.LogError($"Trying to get not registered component of type {typeof(T)}");
                throw;   
#endif
            }

            return typeIndex;
        }

        public static int GetTypeId<T>(this T type)
            where T : struct, IR6EcsComponent<T>
        {
            int typeIndex = -1;
            try
            {
                typeIndex = _componentsTypes[typeof(T)];
            }
            catch (KeyNotFoundException _)
            {
#if DEBUG
                Debug.LogError($"Trying to get not registered component of type {typeof(T)}");
                throw;   
#endif
            }

            return typeIndex;
        }
    }

    public static class ComponentsMapper<T>
        where T : struct, IR6EcsComponent<T>
    {
        private static readonly int _id;

        static ComponentsMapper()
        {
            _id = ComponentsMapper.GetTypeId<T>();
        }

        public static int Id => _id;
    }
}