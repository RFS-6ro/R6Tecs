// ----------------------------------------------------------------------------
// The Proprietary or MIT License
// Copyright (c) 2022-2022 RFS_6ro <rfs6ro@gmail.com>
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using R6ThreadECS.Utils;

namespace R6ThreadECS.Systems
{
    public class R6SystemInfo : IDisposable
    {
        public IR6EcsSystem System;
        public R6EcsSystem SystemImpl;
        
        public Action Action;
        
        public GrowingArray<int> Reads = new GrowingArray<int>(4);
        public GrowingArray<int> Writes = new GrowingArray<int>(4);

        private bool _isDisposed = false;

        public R6SystemInfo(IR6EcsSystem system, Action action)
        {
            if (system == null)
            {
                throw new NullReferenceException();
            }
            
            if (action == null)
            {
                throw new NullReferenceException();
            }
            
            System = system;
            Action = action;
            SystemImpl = (R6EcsSystem) System;
        }
        
        [PublicAPI]
        public bool IsDisposed => _isDisposed;

        public void Dispose()
        {
            if (_isDisposed || SystemImpl.IsDisposed)
            {
                _isDisposed = true;
                return;
            }

            SystemImpl.Dispose();
            SystemImpl = null;
            
            System = null;
            
            Action = null;
            
            Reads.Dispose();
            Reads = null;
            
            Writes.Dispose();
            Writes = null;

            _isDisposed = true;
        }
    }
}
