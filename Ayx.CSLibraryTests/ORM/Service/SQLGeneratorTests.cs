using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ayx.CSLibrary.ORM.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ayx.CSLibrary.ORMTests.TestData;

namespace Ayx.CSLibrary.ORM.Service.Tests
{
    [TestClass()]
    public class SQLGeneratorTests
    {
        [TestMethod()]
        public void GetInsertSQLTest()
        {
            var expected = "INSERT INTO TestTable(ShortTextField,IntField,DateTimeField,BoolField,LongTextField,AddField1,AddField2,AddField3,AddField4,AddField5) VALUES(@ShortTextProperty,@IntProperty,@DateTimeProperty,@BoolField,@LongTextProperty,@AddField1,@AddField2,@AddField3,@AddField4,@AddField5)";
            var data = TestData.GetTestData(1).First();
            var mapping = FieldMapping.GetInsertMapping<TestData>();
            var actual = SQLGenerator.GetInsertSQL<TestData>(mapping);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void GetInsertSQLTest1()
        {
            var nameMapping = new NameMapping
            {
                {"ID","ID" },
                {"ShortTextProperty","ShortTextField" },
                {"IntProperty","IntField" },
                {"LongTextProperty","LongTextField" },
            };
            var expected = "INSERT INTO TestTable(ShortTextField,IntField,LongTextField) "+
                "VALUES(@ShortTextProperty,@IntProperty,@LongTextProperty)";
            var actual = SQLGenerator.GetInsertSQL("TestTable", nameMapping, "ID");
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void GetIdentitySQLTest()
        {
            var expected = "SELECT @@IDENTITY";
            var actual = SQLGenerator.GetIdentitySQL();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void GetSelectSQLTest()
        {
            var expected = "SELECT * FROM TestTable WHERE ID>@ID";
            var actual = SQLGenerator.GetSelectSQL<TestData>(null,"ID>@ID");
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void GetDeleteSQLTest()
        {
            var expected = "DELETE FROM TestTable WHERE ID=@ID";
            var actual = SQLGenerator.GetDeleteSQL<TestData>();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void GetDeleteSQLWithWhereTest()
        {
            var expected = "DELETE FROM TestTable WHERE ID<@ID";
            var actual = SQLGenerator.GetDeleteSQL<TestData>("ID<@ID");
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void GetDeleteMainSQLTest()
        {
            var expected = "DELETE FROM TestTable";
            var actual = SQLGenerator.GetDeleteMainSQL<TestData>();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void GetUpdateSQLTest()
        {
            var expected = "UPDATE TestTable SET ShortTextField=@ShortTextProperty,IntField=@IntProperty,DateTimeField=@DateTimeProperty,BoolField=@BoolField,LongTextField=@LongTextProperty,AddField1=@AddField1,AddField2=@AddField2,AddField3=@AddField3,AddField4=@AddField4,AddField5=@AddField5 WHERE ID=@ID";
            var mapping = FieldMapping.GetUpdateMapping<TestData>();
            var actual = SQLGenerator.GetUpdateSQL<TestData>(mapping);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void GetUpdateSQLWithFieldTest()
        {
            var fields = new List<string> { "ShortTextProperty", "IntProperty" };
            var expected = "UPDATE TestTable SET ShortTextField=@ShortTextProperty,IntField=@IntProperty WHERE ID=@ID";
            var actual = SQLGenerator.GetUpdateSQL<TestData>(fields);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void GetUpdateMainSQLTest()
        {
            var expected = "UPDATE TestTable SET ";
            var actual = SQLGenerator.GetUpdateMainSQL<TestData>();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void GetKeyWhereSQLTest()
        {
            var expected = " WHERE ID=@ID";
            var key = DbAttributes.GetPrimaryKeyProperty<TestData>();
            var actual = SQLGenerator.GetKeyWhereSQL(key);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void GetClearSQLTest()
        {
            var expected = "DELETE FROM TestTable";
            var actual = SQLGenerator.GetClearSQL<TestData>();
            Assert.AreEqual(expected, actual);
        }

        
    }
}