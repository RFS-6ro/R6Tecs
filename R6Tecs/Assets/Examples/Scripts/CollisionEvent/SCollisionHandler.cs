// ----------------------------------------------------------------------------
// The Proprietary or MIT License
// Copyright (c) 2022-2022 RFS_6ro <rfs6ro@gmail.com>
// ----------------------------------------------------------------------------

using R6ThreadECS.Attributes;
using R6ThreadECS.Entity;
using R6ThreadECS.Systems;
using UnityEngine;

namespace R6ThreadECS.Examples
{
    [Filter(typeof(CTransform), FilterAccessType.ReadWrite)]
    [Filter(typeof(CRotation), FilterAccessType.ReadWrite)]
    [Filter(typeof(CCollisionEvent), FilterAccessType.ReadOnly)]
    [R6SystemExecutionOrder(-10)]
    public class SCollisionHandler : R6EcsSystem, IR6FixedUpdateSystem
    {
        public int Frame { get; }
        
        public void FixedUpdate()
        {
            foreach (R6Entity entity in Filter)
            {
                CTransform transform = entity.GetComponent<CTransform>();
                CRotation rotation = entity.GetComponent<CRotation>();
                
                entity.SetComponent(new CTransform(transform.Position + 10f * Vector3.forward));
                entity.SetComponent(new CRotation(rotation.Quaternion * Quaternion.Euler(10f * Vector3.left)));
            }
        }
    }
}
