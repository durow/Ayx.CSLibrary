using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ayx.CSLibrary.DI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ayx.CSLibrary.DI.Tests
{
    [TestClass()]
    public class DIContainerTests
    {
        [TestMethod()]
        public void WireTest()
        {
            DIContainer.Default.Wire<IWeapon, Sword>();
            Assert.AreEqual(1, DIContainer.Default.Count);
        }

        [TestMethod()]
        public void WireTest1()
        {
            DIContainer.Default.Wire<IWeapon>(new Knife(), "knife");
            Assert.AreEqual(1, DIContainer.Default.Count);
        }

        [TestMethod()]
        public void WireSingletonTest()
        {
            DIContainer.Default.WireSingleton<IWeapon, Knife>();
            Assert.AreEqual(1, DIContainer.Default.Count);
        }

        [TestMethod()]
        public void GetTest()
        {
            DIContainer.Default.WireSingleton<IWeapon, Knife>();
            var expected = typeof(Knife);
            var actual = DIContainer.Default.Get<IWeapon>().GetType();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void WireSingletonTest1()
        {
            DIContainer.Default.WireSingleton<IWeapon, Knife>();
            var test1 = DIContainer.Default.Get<IWeapon>();
            var test2 = DIContainer.Default.Get<IWeapon>();
            Assert.AreSame(test1, test2);
        }

        [TestMethod()]
        public void RemoveTest()
        {
            DIContainer.Default.WireSingleton<IWeapon, Knife>();
            DIContainer.Default.WireSingleton<IWeapon, Sword>();
            Assert.AreEqual(2, DIContainer.Default.Count);
            DIContainer.Default.Remove<IWeapon>();
            Assert.AreEqual(0, DIContainer.Default.Count);
        }

        [TestMethod()]
        public void CheckExistTest()
        {
            DIContainer.Default.WireSingleton<IWeapon, Knife>();
            var actual = DIContainer.Default.CheckExist<IWeapon>();
            Assert.IsTrue(actual);
        }

        [TestMethod()]
        public void WireVMTest()
        {
            DIContainer.Default.WireVM<TestWin, TestViewModel>();
            Assert.AreEqual(1, DIContainer.Default.Count);
        }

        [TestMethod()]
        public void GetVMTest()
        {
            DIContainer.Default.WireVM<TestWin, TestViewModel>();
            var vm = DIContainer.Default.GetVM<TestWin>();
            Assert.AreEqual(typeof(TestViewModel), vm.GetType());
        }
    }
}