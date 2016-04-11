using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ayx.CSLibrary.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ayx.CSLibrary.Service;
using Ayx.CSLibrary.ORMTests.TestData;

namespace Ayx.CSLibrary.ORM.Tests
{
    [TestClass()]
    public class DbAttributesTests
    {
        [DbTable("test_table")]
        class TestClass
        {
            [DbField(FieldName = "test_field")]
            public string TestProperty { get; set; }

            [PrimaryKey]
            [AutoIncrement]
            [DbField(FieldName = "id")]
            public int ID { get; set; }

            [NotDbField]
            public string Remark { get; set; }
        }

        class EmptyClass { }

        [TestMethod()]
        public void GetAttributeTest()
        {
            var test = new TestClass();
            var prop = test.GetType().GetProperty("TestProperty");
            var attr = AttributeHelper.GetAttribute<DbFieldAttribute>(prop);
            var expected = "test_field";
            var actual = attr.FieldName;
            Assert.AreEqual(expected, actual, actual);
        }

        [TestMethod()]
        public void CheckAttributeTest()
        {
            var test = new TestClass();
            var prop = test.GetType().GetProperty("TestProperty");
            var test1 = DbAttributes.IsPrimaryKey(prop);
            var test2 = DbAttributes.IsDbField(prop);
            Assert.IsTrue(test1 == false &&
                                 test2 == true);
        }

        [TestMethod()]
        public void GetDbTableNameTest()
        {
            var expected1 = "test_table";
            var expected2 = "EmptyClass";
            var actual1 = DbAttributes.GetDbTableName<TestClass>();
            var actual2 = DbAttributes.GetDbTableName<EmptyClass>();
            Assert.AreEqual(expected1, actual1, actual1);
            Assert.AreEqual(expected2, actual2, actual2);
        }

        [TestMethod()]
        public void GetDbFieldAttributeTest()
        {
            var test = new TestClass();
            var property = test.GetType().GetProperty("ID");
            var test1 = DbAttributes.GetDbFieldAttribute(property);
            var property2 = test.GetType().GetProperty("Remark");
            var test2 = DbAttributes.GetDbFieldAttribute(property2);
            Assert.IsTrue(test1 != null &&
                                 test2 == null);
        }

        [TestMethod()]
        public void GetAttributesTest()
        {
            var test = new TestClass();
            var property = test.GetType().GetProperty("ID");
            var attCount = AttributeHelper.GetAttributes<DbFieldAttribute>(property).Count();
            Assert.AreEqual(1, attCount, attCount);
        }

        [TestMethod()]
        public void GetDbFieldNameTest()
        {
            var property = typeof(TestData).GetProperties().FirstOrDefault();
            var expected = "ID";
            var actual = DbAttributes.GetDbFieldName(property);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void IsAutoIncrementTest()
        {
            var property = typeof(TestData).GetProperties().FirstOrDefault();
            var test = DbAttributes.IsAutoIncrement(property);
            Assert.IsTrue(test);
        }
    }
}