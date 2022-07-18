// ----------------------------------------------------------------------------
// The Proprietary or MIT License
// Copyright (c) 2022-2022 RFS_6ro <rfs6ro@gmail.com>
// ----------------------------------------------------------------------------

using R6ThreadECS.Systems;

namespace R6ThreadECS.DependencyGraph
{
    public class R6SystemGraph
    {
        private readonly R6EcsSystem[] _systems;
        
        private R6SyncPoint[] _layers;

        public R6SystemGraph(R6EcsSystem[] systems)
        {
            _systems = systems;
            GenerateDependencyGraph();
        }

        private void GenerateDependencyGraph()
        {
            
        }
    }

    public class R6Vertex
    {
        
    }
}

