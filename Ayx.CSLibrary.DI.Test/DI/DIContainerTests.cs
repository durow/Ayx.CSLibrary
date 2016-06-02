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
            var container = new DIContainer();
            container.Wire<DIClass>();
            container.Wire<ClassB>();
            container.Wire<ClassC>();

            var test = container.Get<DIClass>();

            Assert.IsNotNull(test.B.C);
        }

        [TestMethod]
        public void SingletonTest()
        {
            var container = new DIContainer();

            container.WireSingleton<DIClass>();
            container.Wire<ClassB>();
            container.Wire<ClassC>();

            var test1 = container.Get<DIClass>();
            var test2 = container.Get<DIClass>();

            Assert.AreSame(test1, test2);
        }

        [TestMethod]
        public void GetErrorTest()
        {
            var container = new DIContainer();

            var test = container.Get<DIClass>();
            Assert.IsNull(test);
        }

        [TestMethod]
        public void GetErrorTest2()
        {
            var container = new DIContainer();

            container.Wire<DIClass>();
            container.Wire<ClassB>();

            var test = container.Get<DIClass>();

            Assert.IsNotNull(test);
            Assert.IsNotNull(test.B);
            Assert.IsNull(test.B.C);
        }
    }
}