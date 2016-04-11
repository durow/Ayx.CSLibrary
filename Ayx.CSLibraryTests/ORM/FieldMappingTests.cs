using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ayx.CSLibrary.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ayx.CSLibrary.ORMTests.TestData;

namespace Ayx.CSLibrary.ORM.Tests
{
    [TestClass()]
    public class FieldMappingTests
    {
        [TestMethod()]
        public void GetFromNameMappingTest()
        {
            var nameMapping = new NameMapping
            {
                {"ID","ID" },
                {"ShortTextProperty","ShortTextField" },
                {"IntProperty","IntField" },
                {"LongTextProperty","LongTextField" },
                {"sfsdfs","dsfdsfdfs" },
                {"bvcbcvb","dsfdsfdfs" },
                {"hgjfgh","dsfdsfdfs" },
            };
            var fieldMapping = FieldMapping.GetFromNameMapping<TestData>(nameMapping);
            Assert.AreEqual(fieldMapping.Count, 4);
        }

        [TestMethod()]
        public void GetSelectMappingTest()
        {
            var dt = TestTable.GetTable(1);
            var mapping = FieldMapping.GetSelectMapping<TestData>(dt.Columns);
            var mapping2 = FieldMapping.GetSelectMapping<TestData>();
            Assert.AreEqual(mapping.Count, 11);
            Assert.AreEqual(mapping2.Count, 11);
        }

        [TestMethod()]
        public void GetInsertMappingTest()
        {
            var mapping = FieldMapping.GetInsertMapping<TestData>();
            Assert.AreEqual(mapping.Count, 10);
        }

        [TestMethod()]
        public void GetUpdateMappingTest()
        {
            var mapping = FieldMapping.GetUpdateMapping<TestData>();
            Assert.AreEqual(mapping.Count, 10);
        }
    }
}