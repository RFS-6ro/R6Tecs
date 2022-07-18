// ----------------------------------------------------------------------------
// The Proprietary or MIT License
// Copyright (c) 2022-2022 RFS_6ro <rfs6ro@gmail.com>
// ----------------------------------------------------------------------------

using System.Collections.Generic;
using R6ThreadECS.Init;
using R6ThreadECS.World;

namespace R6ThreadECS
{
    /// <summary>
    /// used for initialisation and system graph execution
    /// </summary>
    public class R6EcsExecutor
    {
        private List<R6World> _worlds;

        public List<R6World> Worlds
        {
            get
            {
                if (_worlds == null)
                {
                    _worlds = new List<R6World>();
                }

                return _worlds;
            }
        }

        public R6EcsExecutor AddWorld(R6WorldInitializator worldInitializator)
        {
            Worlds.Add(new R6World(worldInitializator));
            
            return this;
        }

        #region Init

        private void PreInit()
        {
            
        }

        public void Init()
        {
            PreInit();
            
        }

        #endregion

        #region Loop

        public void Update()
        {
            
        }

        public void FixedUpdate()
        {
            
        }

        public void LateUpdate()
        {
            
        }

        public void OnDestroy()
        {
            
        }

        #endregion
    }
}
