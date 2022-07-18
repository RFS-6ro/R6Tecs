// ----------------------------------------------------------------------------
// The Proprietary or MIT License
// Copyright (c) 2022-2022 RFS_6ro <rfs6ro@gmail.com>
// ----------------------------------------------------------------------------

namespace R6ThreadECS.Entity
{
    /// <summary>
    /// components container
    /// </summary>
    public struct R6Entity
    {
        public int[] Components;

        public T GetComponent<T>()
            where T : struct, IR6EcsComponent<T>
        {
            return default;
        }

        public void SetComponent<T>(T component)
            where T : struct, IR6EcsComponent<T>
        {
            T componentInMemory = default;

            componentInMemory.Write(component);
        }
    }
}
