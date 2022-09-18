// ----------------------------------------------------------------------------
// The Proprietary or MIT License
// Copyright (c) 2022-2022 RFS_6ro <rfs6ro@gmail.com>
// ----------------------------------------------------------------------------

using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace R6ThreadECS.Utils.Tests
{
    public class ResizeableArrayTest : MonoBehaviour
    {
        private void Start()
        {
            ResizeableArray<int> array = new ResizeableArray<int>(4);
            
            Assert.IsTrue(array.Count == 0);
            Assert.IsTrue(array.Capacity == 4);

            for (int i = 0; i < 4; i++)
            {
                array.Add(i);
                Assert.IsTrue(array.Count == i + 1);
            }

            for (int i = 0; i < 4; i++)
            {
                Assert.IsTrue(array[i] == i);
            }
            
            array.Add(4);
            
            Assert.IsTrue(array.Count == 5);
            Assert.IsTrue(array.Capacity == 8);

            array.Remove(0);
            Assert.IsTrue(array.Count == 4);
            Assert.IsTrue(array.Capacity == 8);

            for (int i = 0; i < 4; i++)
            {
                Assert.IsTrue(array[i] == (i + 1));
            }

            array.Remove(0, out int value);
            Assert.IsTrue(array.Count == 3);
            Assert.IsTrue(array.Capacity == 8);
            Assert.IsTrue(value == 1);

            for (int i = 0; i < 3; i++)
            {
                Assert.IsTrue(array[i] == (i + 2));
            }
            
            array.Lock();

            bool isLockedCheck = false;
            try
            {
                array.Add(4);
            }
            catch (Exception)
            {
                isLockedCheck = true;
            }

            Assert.IsTrue(isLockedCheck);
            Assert.IsTrue(!array.Remove(2));
            
            array.Unlock();

            bool outOfBoundsCheck = false;
            try
            {
                int item = array[5];
            }
            catch (Exception)
            {
                outOfBoundsCheck = true;
            }
            Assert.IsTrue(outOfBoundsCheck);
            outOfBoundsCheck = false;
            
            try
            {
                array[5] = 5;
            }
            catch (Exception)
            {
                outOfBoundsCheck = true;
            }
            Assert.IsTrue(outOfBoundsCheck);
            outOfBoundsCheck = false;
        }
    }
}