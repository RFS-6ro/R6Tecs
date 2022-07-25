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
    [Filter(typeof(CCollisionEvent), FilterAccessType.ReadOnly, FilterFunctionalityType.Exclude)]
    [Filter(typeof(CTransform), FilterAccessType.ReadWrite)]
    public class STransformTranslation : R6EcsSystem, IR6PreInitSystem, IR6FixedUpdateSystem
    {
        public int Frame { get; }

        public void PreInit()
        {
            throw new System.NotImplementedException();
        }
        
        public void FixedUpdate()
        {
            foreach (R6Entity entity in Filter)
            {
                CTransform transform = entity.GetComponent<CTransform>();
                entity.SetComponent(new CTransform(transform.Position + Vector3.forward));
            }
        }
    }
}
