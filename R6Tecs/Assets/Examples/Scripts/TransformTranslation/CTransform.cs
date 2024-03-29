﻿// ----------------------------------------------------------------------------
// The Proprietary or MIT License
// Copyright (c) 2022-2022 RFS_6ro <rfs6ro@gmail.com>
// ----------------------------------------------------------------------------

using UnityEngine;

namespace R6ThreadECS.Examples
{
    public struct CTransform : IR6EcsComponent<CTransform>
    {
        public Vector3 Position;
        
        public CTransform(Vector3 position)
        {
            Position = position;
        }

        public void Write(CTransform other)
        {
            Position = other.Position;
        }

        public CTransform Read()
        {
            return new CTransform(Position);
        }
    }
}