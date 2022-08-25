// ----------------------------------------------------------------------------
// The Proprietary or MIT License
// Copyright (c) 2022-2022 RFS_6ro <rfs6ro@gmail.com>
// ----------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using R6ThreadECS.Attributes;
using R6ThreadECS.Init;
using R6ThreadECS.World;

namespace R6ThreadECS
{
    /// <summary>
    /// used for initialisation and system graph execution
    /// </summary>
    public class R6EcsExecutor
    {
        private SortedList<int, R6World> _worlds;

        public SortedList<int, R6World> Worlds
        {
            get
            {
                if (_worlds == null)
                {
                    _worlds = new SortedList<int, R6World>();
                }

                return _worlds;
            }
        }

        public R6EcsExecutor AddWorld([NotNull] R6World world)
        {
            Worlds.Add(world.Priority, world);
            
            return this;
        }

        #region Init

        private void PreInit()
        {
            foreach (var r6World in Worlds.Values)
            {
                r6World.Initialize();
            }
        }

        public void Init()
        {
            PreInit();

            foreach (var r6World in Worlds.Values)
            {
                // r6World.
            }
        }

        #endregion

        #region Loop

        public void Update()
        {
            foreach (var r6World in Worlds.Values)
            {
                // r6World.
            }
        }

        public void FixedUpdate()
        {
            foreach (var r6World in Worlds.Values)
            {
                // r6World.
            }
        }

        public void LateUpdate()
        {
            foreach (var r6World in Worlds.Values)
            {
                // r6World.
            }
        }

        public void OnDestroy()
        {
            foreach (var r6World in Worlds.Values)
            {
                r6World.Destroy();
            }
        }

        #endregion
    }
}
