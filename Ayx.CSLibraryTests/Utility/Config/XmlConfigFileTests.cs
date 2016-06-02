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
    }
}