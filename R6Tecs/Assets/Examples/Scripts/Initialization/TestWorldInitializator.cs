// ----------------------------------------------------------------------------
// The Proprietary or MIT License
// Copyright (c) 2022-2022 RFS_6ro <rfs6ro@gmail.com>
// ----------------------------------------------------------------------------

using R6ThreadECS.Init;
using R6ThreadECS.World;

namespace R6ThreadECS.Examples
{
    public class TestWorldInitializator : R6WorldInitializator
    {
        public override R6World GetWorld()
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
    
    public class HighPriorityWorldInitializator : R6WorldInitializator
    {
        public override R6World GetWorld()
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
    
    public class LowPriorityWorldInitializator : R6WorldInitializator
    {
        public override R6World GetWorld()
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
