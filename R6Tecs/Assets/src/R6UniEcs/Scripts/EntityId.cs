// ----------------------------------------------------------------------------
// The Proprietary or MIT License
// Copyright (c) 2022-2022 RFS_6ro <rfs6ro@gmail.com>
// ----------------------------------------------------------------------------

using System;
using R6ThreadECS.Entity;
using UnityEngine;

namespace R6ThreadECS.UniEcs
{
    public abstract class EntityId : MonoBehaviour
    {
        private bool _inited = false;

        private Guid _guid;
        public Guid Guid => _guid;

        public void ManualInit()
        {
            if (_inited)
            {
                return;
            }

            _inited = true;
            ManualInitImpl();
            СheckDependencies();
            _guid = Guid.NewGuid();
            //_entityManager = World.GetExistingManager<EntityManager>();
            //_entityManager.RegisterEntity(this);
        }
        
        protected abstract void ManualInitImpl();

        protected abstract void СheckDependencies();

        public R6Entity GetEntity()
        {
            return default(R6Entity);
        }

        private void OnDisable()
        {
            //_entityManager.UnregisterEntity(this);
        }
    }
}
