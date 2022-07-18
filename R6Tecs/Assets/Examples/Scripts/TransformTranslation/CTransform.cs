// ----------------------------------------------------------------------------
// The Proprietary or MIT License
// Copyright (c) 2022-2022 RFS_6ro <rfs6ro@gmail.com>
// ----------------------------------------------------------------------------

using UnityEngine;

namespace R6ThreadECS.Examples.TransformTranslation
{
    public struct CTransform : IR6EcsComponent<CTransform>
    {
        public Vector3 Position;
        
        public CTransform(Vector3 position)
        {
            Position = position;
        }

        public void Write(CTransform component)
        {
            Position = component.Position;
        }

        public CTransform Read()
        {
            return new CTransform(Position);
        }
    }
}