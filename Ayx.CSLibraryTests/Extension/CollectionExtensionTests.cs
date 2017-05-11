﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ayx.CSLibrary.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ayx.CSLibrary.Extension.Tests
{
    [TestClass()]
    public class CollectionExtensionTests
    {
        [TestMethod()]
        public void MoveLeftDefaultTest()
        {
            var test = new List<int> { 1, 2, 3, 4, 5 };
            var actual = test.MoveLeft();
            for (int i = 0; i < actual.Count-1; i++)
            {
                Assert.AreEqual(i + 2, actual[i]);
            }
            Assert.AreEqual(0, actual.Last());
        }

        [TestMethod()]
        public void MoveLeftTest()
        {
            var test = new List<int> { 1, 2, 3, 4, 5 };
            var actual = test.MoveLeft(6);
            actual = actual.MoveLeft(7);
            for (int i = 0; i < test.Count; i++)
            {
                Assert.AreEqual(i + 3, actual[i]);
            }
        }
    }
}