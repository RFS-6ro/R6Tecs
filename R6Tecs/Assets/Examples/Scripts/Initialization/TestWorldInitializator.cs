// ----------------------------------------------------------------------------
// The Proprietary or MIT License
// Copyright (c) 2022-2022 RFS_6ro <rfs6ro@gmail.com>
// ----------------------------------------------------------------------------

using System;
using R6ThreadECS.Init;
using R6ThreadECS.Systems;
using R6ThreadECS.World;
using UnityEngine;

namespace R6ThreadECS.Examples
{
    public class TestWorldInitializator : IR6WorldInitializator
    {
        public R6World GetWorld()
        {
            R6World world = new R6World()
                .AddSystem(new STransformTranslation())
                .AddSystem(new SRotationTranslation())
                .AddSystem(new SCollisionHandler())
                .AddOneFrameComponent<CCollisionEvent>();

            world.SetPriority(10);

            return world;
        }
    }
    
    public class TestWorldInitializatorWithParallelGroups : IR6WorldInitializator
    {
        public R6World GetWorld()
        {
            R6World world = new R6World()
                .AddParallelGroup(new R6ParallelGroup(new STransformTranslation(), new SRotationTranslation()))
                .AddSystem(new SCollisionHandler())
                .AddOneFrameComponent<CCollisionEvent>();

            world.SetPriority(10);

            return world;
        }
    }
    
    public class HighPriorityWorldInitializator : IR6WorldInitializator
    {
        public R6World GetWorld()
        {
            R6World world = new R6World()
                .AddSystem(new STransformTranslation())
                .AddSystem(new SRotationTranslation())
                .AddSystem(new SCollisionHandler())
                .AddOneFrameComponent<CCollisionEvent>();

            world.SetPriority(-1000);

            return world;
        }
    }
    
    public class LowPriorityWorldInitializator : IR6WorldInitializator
    {
        public R6World GetWorld()
        {
            R6World world = new R6World()
                .AddSystem(new STransformTranslation())
                .AddSystem(new SRotationTranslation())
                .AddSystem(new SCollisionHandler())
                .AddOneFrameComponent<CCollisionEvent>();

            world.SetPriority(1000);

            return world;
        }
    }
}
