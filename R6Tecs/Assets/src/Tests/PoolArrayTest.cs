// ----------------------------------------------------------------------------
// The Proprietary or MIT License
// Copyright (c) 2022-2022 RFS_6ro <rfs6ro@gmail.com>
// ----------------------------------------------------------------------------

using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace R6ThreadECS.Utils.Tests
{
    public class PoolArrayTest : MonoBehaviour
    {
        private void Start()
        {
            PoolArray<int> array = new PoolArray<int>(8);
            
            Assert.IsTrue(array.Length == 0);
            Assert.IsTrue(array.CacheLength == 0);
            Assert.IsTrue(array.ItemsCapacity == 8);
            Assert.IsTrue(array.CacheCapacity == 8);

            for (int i = 0; i < 8; i++)
            {
                array.Alloc(i);
                Assert.IsTrue(array.Length == i + 1);
            }

            for (int i = 0; i < 8; i++)
            {
                Assert.IsTrue(array.Get(i) == i);
            }
            
            for (int i = 8; i < 16; i++)
            {
                array.Alloc(i);
                Assert.IsTrue(array.Length == i + 1);
            }
            
            Assert.IsTrue(array.Length == 16);
            Assert.IsTrue(array.CacheLength == 0);
            Assert.IsTrue(array.ItemsCapacity == 16);
            Assert.IsTrue(array.CacheCapacity == 8);

            
            for (int i = 13; i > 1; i--)
            {
                array.Store(i);
            }
            
            Assert.IsTrue(array.Length == 16);
            Assert.IsTrue(array.CacheLength == 12);
            Assert.IsTrue(array.ItemsCapacity == 16);
            Assert.IsTrue(array.CacheCapacity == 16);
            
            array.Copy(14, 15);
            Assert.IsTrue(array.Get(14) == array.Get(15));
        }
    }
}