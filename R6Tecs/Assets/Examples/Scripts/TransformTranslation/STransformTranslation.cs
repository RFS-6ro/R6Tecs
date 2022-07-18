// ----------------------------------------------------------------------------
// The Proprietary or MIT License
// Copyright (c) 2022-2022 RFS_6ro <rfs6ro@gmail.com>
// ----------------------------------------------------------------------------

using R6ThreadECS.Entity;
using R6ThreadECS.Systems;
using UnityEngine;

namespace R6ThreadECS.Examples.TransformTranslation
{
    public class STransformTranslation : R6EcsSystem, IR6FixedUpdateSystem
    {
        public int Frame { get; }
        
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