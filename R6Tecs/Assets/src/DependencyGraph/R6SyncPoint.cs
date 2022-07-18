// ----------------------------------------------------------------------------
// The Proprietary or MIT License
// Copyright (c) 2022-2022 RFS_6ro <rfs6ro@gmail.com>
// ----------------------------------------------------------------------------

using R6ThreadECS.Systems;

namespace R6ThreadECS.DependencyGraph
{
    public class R6SyncPoint
    {
        private R6EcsSystem[] _systems;

        public R6SyncPoint(R6EcsSystem[] systems)
        {
            _systems = systems;
        }
    }
}
