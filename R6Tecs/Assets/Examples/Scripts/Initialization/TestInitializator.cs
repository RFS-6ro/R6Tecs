// ----------------------------------------------------------------------------
// The Proprietary or MIT License
// Copyright (c) 2022-2022 RFS_6ro <rfs6ro@gmail.com>
// ----------------------------------------------------------------------------

using UnityEngine;

namespace R6ThreadECS.Examples
{
    public class TestInitializator : MonoBehaviour
    {
        private R6EcsExecutor _executor;

        private void Awake()
        {
            _executor = new R6EcsExecutor();
            _executor.AddWorld(new TestWorldInitializator());
        }

        private void Start()
        {
            _executor.Init();
        }

        private void Update()
        {
            _executor.Update();
        }

        private void FixedUpdate()
        {
            _executor.FixedUpdate();
        }

        private void LateUpdate()
        {
            _executor.LateUpdate();
        }

        private void OnDestroy()
        {
            _executor.OnDestroy();
        }
    }
}