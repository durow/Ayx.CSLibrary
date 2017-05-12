using Microsoft.VisualStudio.TestTools.UnitTesting;
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
        private static List<int> test = new List<int> { 1, 2, 3, 4, 5 };

        [TestMethod()]
        public void MoveLeftDefaultTest()
        {
            var actual = test.MoveLeft();
            for (int i = 0; i < actual.Count - 1; i++)
            {
                Assert.AreEqual(i + 2, actual[i]);
            }
            Assert.AreEqual(0, actual.Last());
        }

        [TestMethod()]
        public void MoveLeftTest()
        {
            var actual = test.MoveLeft(6);
            actual = actual.MoveLeft(7);
            for (int i = 0; i < test.Count; i++)
            {
                Assert.AreEqual(i + 3, actual[i]);
            }
        }

        [TestMethod()]
        public void LeftCircleTest()
        {
            var actual = test.LeftCircle();
            for (int i = 1; i < test.Count; i++)
            {
                Assert.AreEqual(test[i], actual[i - 1]);
            }
            Assert.AreEqual(test.First(), actual.Last());
        }

        [TestMethod()]
        public void MoveRightTest()
        {
            var actual = test.MoveRight(0);
            actual = actual.MoveRight(-1);
            for (int i = 0; i < test.Count; i++)
            {
                Assert.AreEqual(i - 1, actual[i]);
            }
        }

        [TestMethod()]
        public void MoveRightDefaultTest()
        {
            var actual = test.MoveRight();
            for (int i = 0; i < actual.Count - 1; i++)
            {
                Assert.AreEqual(i, actual[i]);
            }
        }

        [TestMethod()]
        public void RightCircleTest()
        {
            var actual = test.RightCircle();
            for (int i = 1; i < actual.Count; i++)
            {
                Assert.AreEqual(test[i - 1], actual[i]);
            }
            Assert.AreEqual(test.Last(), actual[0]);
        }
    }
}