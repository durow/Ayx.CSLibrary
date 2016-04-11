using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ayx.CSLibrary.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ayx.CSLibrary.Extension.Tests
{
    [TestClass()]
    public class StringBuilderExtensionTests
    {
        [TestMethod()]
        public void IndexOfTest()
        {
            const string data = "abcdefg";
            var expected = data.IndexOf('d');
            var actual = new StringBuilder(data).IndexOf('d');
            Assert.AreEqual(expected, actual);
        }
    }
}