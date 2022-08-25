// ----------------------------------------------------------------------------
// The Proprietary or MIT License
// Copyright (c) 2022-2022 RFS_6ro <rfs6ro@gmail.com>
// ----------------------------------------------------------------------------

using System.Collections.Generic;

namespace R6ThreadECS.Systems
{
    public class R6SystemInfo
    {
        public IR6EcsSystem System;
        public R6EcsSystem SystemImpl;
        
        public System.Action Action;
        
        public List<int> Reads;
        public List<int> Writes;
    }
}
