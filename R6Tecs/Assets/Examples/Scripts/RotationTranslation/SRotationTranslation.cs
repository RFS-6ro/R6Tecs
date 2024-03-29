﻿// ----------------------------------------------------------------------------
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
    [Filter(typeof(CRotation), FilterAccessType.ReadWrite)]
    public class SRotationTranslation : R6EcsSystem, IR6FixedUpdateSystem
    {
        public int Frame { get; }
        
        public void FixedUpdate()
        {
            foreach (R6Entity entity in Filter)
            {
                CRotation rotation = entity.Get<CRotation>();
                entity.Set(new CRotation(rotation.Quaternion * Quaternion.Euler(Vector3.left)));
            }
        }
    }
}
