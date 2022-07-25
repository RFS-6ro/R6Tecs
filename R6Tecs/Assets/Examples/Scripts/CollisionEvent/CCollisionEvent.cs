// ----------------------------------------------------------------------------
// The Proprietary or MIT License
// Copyright (c) 2022-2022 RFS_6ro <rfs6ro@gmail.com>
// ----------------------------------------------------------------------------

using UnityEngine;

namespace R6ThreadECS.Examples
{
    public struct CCollisionEvent : IR6ResetEcsComponent<CCollisionEvent>
    {
        public GameObject First;
        public GameObject Second;
        public Collision Collision;
        
        public CCollisionEvent(GameObject first, GameObject second, Collision collision)
        {
            First = first;
            Second = second;
            Collision = collision;
        }

        public void Write(CCollisionEvent other)
        {
            First = other.First;
            Second = other.Second;
            Collision = other.Collision;
        }

        public CCollisionEvent Read()
        {
            return new CCollisionEvent(First, Second, Collision);
        }

        public void Reset()
        {
            First = null;
            Second = null;
            Collision = null;
        }
    }
}