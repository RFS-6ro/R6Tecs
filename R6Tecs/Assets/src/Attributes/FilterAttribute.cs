// ----------------------------------------------------------------------------
// The Proprietary or MIT License
// Copyright (c) 2022-2022 RFS_6ro <rfs6ro@gmail.com>
// ----------------------------------------------------------------------------

using System;
using System.Linq;

namespace R6ThreadECS.Attributes
{
    public enum FilterAccessType
    {
        ReadOnly,
        WriteOnly,
        ReadWrite
    }

    public enum FilterFunctionalityType
    {
        Include,
        Exclude
    }
    
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class FilterAttribute : Attribute
    {
        public readonly Type ComponentType;
        public readonly FilterAccessType FilterAccessType;
        public readonly FilterFunctionalityType FilterFunctionalityType;
        
        public FilterAttribute(Type componentType, FilterAccessType filterAccessType, FilterFunctionalityType filterFunctionalityType = FilterFunctionalityType.Include)
        {
            bool isValid = componentType.IsValueType && 
                           componentType.GetInterfaces()
                               .Any(@interface =>
                               {
                                   return @interface.IsGenericType &&
                                          @interface.GetGenericTypeDefinition() == typeof(IR6EcsComponent<>);
                               });
            
            if (!isValid)
            {
                throw new ArgumentException("Type should be inherited from IR6EcsComponent");
            }
            
            ComponentType = componentType;
            FilterAccessType = filterAccessType;
            FilterFunctionalityType = filterFunctionalityType;
        }
    }
}