using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ayx.CSLibrary.Utility.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Ayx.CSLibrary.Utility.Config.Tests
{
    [TestClass()]
    public class XmlConfigFileTests
    {
        string testFile = "test.xml";
        [TestMethod()]
        public void AddOrSetTest()
        {

            IConfigFile testFile = new XmlConfigFile("test.xml");

            testFile.AddOrSet("Config/Test/Property", "myValue");
            Assert.IsTrue(File.Exists("test.xml"));
            Assert.AreEqual(testFile["Config/Test/Property"], "myValue");
        }

        [TestMethod()]
        public void CreateEmptyTest()
        {
            var testFile = "test.xml";
            if (File.Exists(testFile))
                File.Delete(testFile);
            var test = new XmlConfigFile(testFile);
            Assert.IsTrue(File.Exists(testFile));
        }

        [TestMethod]
        public void FastReadWriteTest()
        {
            var test = new XmlConfigFile(testFile);
            test["Fuck"] = "fuck";
            test["Config.Test1"] = "test1";
            test["Config:Test2"] = "test2";
            test["Config.Section.Font"] = "consolas";

            var test2 = new XmlConfigFile(testFile);
            Assert.AreEqual(test["Fuck"], "fuck");
            Assert.AreEqual(test["Config.Test1"], "test1");
            Assert.AreEqual(test["Config:Test2"], "test2");
            Assert.AreEqual(test["Config.Section.Font"], "consolas");
        }

        [TestMethod()]
        public void GetTest()
        {
            Assert.Fail();
        }
    }
}