// ----------------------------------------------------------------------------
// The Proprietary or MIT License
// Copyright (c) 2022-2022 RFS_6ro <rfs6ro@gmail.com>
// ----------------------------------------------------------------------------

using UnityEngine;

namespace R6ThreadECS.Examples
{
    public struct CRotation : IR6EcsComponent<CRotation>
    {
        public Quaternion Quaternion;
        
        public CRotation(Quaternion quaternion)
        {
            Quaternion = quaternion;
        }

        public void Write(CRotation other)
        {
            Quaternion = other.Quaternion;
        }

        public CRotation Read()
        {
            return new CRotation(Quaternion);
        }
    }
}