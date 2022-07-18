// ----------------------------------------------------------------------------
// The Proprietary or MIT License
// Copyright (c) 2022-2022 RFS_6ro <rfs6ro@gmail.com>
// ----------------------------------------------------------------------------

namespace R6ThreadECS.Systems
{
    public interface IR6EcsSystem { }
    
    public interface IR6PreInitSystem : IR6EcsSystem
    {
        void PreInit();
    }
    
    public interface IR6InitSystem : IR6EcsSystem
    {
        void Init();
    }
    
    public interface IR6UpdateSystem : IR6EcsSystem
    {
        void Update();
    }
    
    public interface IR6FixedUpdateSystem : IR6EcsSystem
    {
        void FixedUpdate();
    }
    
    public interface IR6LateUpdateSystem : IR6EcsSystem
    {
        void LateUpdate();
    }
}