// ----------------------------------------------------------------------------
// The Proprietary or MIT License
// Copyright (c) 2022-2022 RFS_6ro <rfs6ro@gmail.com>
// ----------------------------------------------------------------------------

using R6ThreadECS.Filters;
using R6ThreadECS.World;

namespace R6ThreadECS.Systems
{
    /// <summary>
    /// Base for all systems
    /// </summary>
    public abstract class R6EcsSystem
    {
        protected R6World World;
        protected R6EntityFilter Filter;
        
        protected int CurrentExecutionFrame { get; private set; }

        public void SetFrameData(R6EntityFilter filter, int currentExecutionFrame)
        {
            if (filter == null)
            {
                throw new System.NotSupportedException();
            }

            if (currentExecutionFrame <= CurrentExecutionFrame)
            {
                throw new System.NotSupportedException();
            }
            
            Filter = filter;
            Filter.Lock();
        }

        public void SetOwner(R6World owner)
        {
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
    }
}
