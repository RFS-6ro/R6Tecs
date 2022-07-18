// ----------------------------------------------------------------------------
// The Proprietary or MIT License
// Copyright (c) 2022-2022 RFS_6ro <rfs6ro@gmail.com>
// ----------------------------------------------------------------------------

using System;

namespace R6ThreadECS.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class R6SystemExecutionOrderAttribute : Attribute
    {
        public readonly int Order;

        public R6SystemExecutionOrderAttribute(int order)
        {
            Order = order;
        }
    }
}