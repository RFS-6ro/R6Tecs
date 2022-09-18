// ----------------------------------------------------------------------------
// The Proprietary or MIT License
// Copyright (c) 2022-2022 RFS_6ro <rfs6ro@gmail.com>
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using R6ThreadECS.Attributes;
using R6ThreadECS.Filters;
using R6ThreadECS.World;

namespace R6ThreadECS.Systems
{
    /// <summary>
    /// Base for all systems
    /// </summary>
    public abstract class R6EcsSystem : IDisposable
    {
        private int _filterId;
        
        private bool _isDisposed = false;
        
        protected tmp World;
        
        protected R6EntityFilter Filter
        {
            get
            {
                if (_isDisposed)
                {
                    return null;
                }
                
                return World.Filters.ElementAt(_filterId);
            }
        }
        
        [PublicAPI]
        public bool IsDisposed => _isDisposed;
        
        
        protected int CurrentExecutionFrame { get; private set; }

        [PublicAPI]
        public void SetFilterId(int id) => _filterId = id;

        [PublicAPI]
        public void SetOwner(tmp owner)
        {
            if (_isDisposed)
            {
                return;
            }
            
            if (owner == null)
            {
                throw new System.NotSupportedException();
            }

            if (World != null)
            {
                throw new System.NotSupportedException();
            }
            
            World = owner;
        }

        [PublicAPI]
        public virtual void Dispose()
        {
            if (_isDisposed)
            {
                return;
            }
            
            World = null;

            _isDisposed = true;
        }
    }
}
