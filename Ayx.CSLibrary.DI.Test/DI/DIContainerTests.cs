using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ayx.CSLibrary.DI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ayx.CSLibrary.DI.Test.DI;

namespace Ayx.CSLibrary.DI.Tests
{
    [TestClass()]
    public class DIContainerTests
    {
        [TestMethod()]
        public void WireTest()
        {
            var container = new AyxContainer();
            container.Wire<DIClass>();
            container.Wire<ClassB>();
            container.Wire<ClassC>();

            var test = container.Get<DIClass>();

            Assert.IsNotNull(test.B.C);
        }

        [TestMethod]
        public void SingletonTest()
        {
            var container = new AyxContainer();

            container.WireSingleton<DIClass>();
            container.Wire<ClassB>();
            container.Wire<ClassC>();

            var test1 = container.Get<DIClass>();
            var test2 = container.Get<DIClass>();

            Assert.AreSame(test1, test2);
        }

        [TestMethod]
        public void AutoCreateTest()
        {
            var container = new AyxContainer();

            var test = container.Get<DIClass>();
            Assert.AreEqual(test.B.C.IntPorperty, 3333);
        }

        [TestMethod]
        public void AutoCreateTest2()
        {
            var container = new AyxContainer();

            container.Wire<DIClass>();
            container.Wire<ClassB>();

            var test = container.Get<DIClass>();

            Assert.IsNotNull(test.B.C);
        }

        [TestMethod]
        public void ConstructorTest()
        {
            var container = new AyxContainer();

            var test = container.Get<ParamClass>();

            Assert.AreEqual(test.IntProperty, 0);
            Assert.AreEqual(test.StringProperty, "param");
            Assert.AreEqual(test.A.B.C.IntPorperty, 3333);
        }

        [TestMethod]
        public void MultiParamConstructorTest()
        {
            var container = new AyxContainer();

            var test = container.Get<MultiParamClass>();

            Assert.AreEqual(200, test.IntProperty);
            Assert.AreEqual( "inject", test.StringProperty);
            Assert.IsNull(test.A);
        }
    }
}