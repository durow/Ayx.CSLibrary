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
    public class AyxORMTests
    {
        AyxORM orm = AyxORM.UseAccess2003(@"test.mdb");
        [TestMethod()]
        public void ExecuteNonQueryTest()
        {
            var testData = TestData.GetTestData(1).FirstOrDefault();
            var test1 = orm.ExecuteNonQuery(@"UPDATE TestTable SET ShortTextField=@ShortTextField WHERE ID=@ID",
                new { ShortTextField = "UPDATE TEST", ID = -1 });
            var test2 = orm.ExecuteNonQuery(@"DELETE FROM TestTable WHERE ID=@ID", new { ID = -1 });
            Assert.IsTrue(test1 == 0 && test2 == 0);
        }

        [TestMethod()]
        public void ExecuteDataTableTest()
        {
            var result = orm.ExecuteDataTable("SELECT TOP 10 * FROM TestTable");
            Assert.IsNotNull(result);
        }

        [TestMethod()]
        public void ExecuteDataSetTest()
        {
            var result = orm.ExecuteDataSet("SELECT TOP 10 * FROM TestTable");
            Assert.IsNotNull(result);
        }

        [TestMethod()]
        public void ExecuteListTest()
        {
            var result = orm.ExecuteList<TestData>("SELECT TOP 10 ID FROM TestTable");
            Assert.IsNotNull(result);
        }

        [TestMethod()]
        public void SelectTest()
        {
            var test = orm.Select<TestData>("ID>@ID", new { ID = 10 });
            Assert.IsTrue(test.Count() >= 0);
        }

        [TestMethod()]
        public void DeleteTest()
        {
            var data = TestData.GetTestData(1).FirstOrDefault();
            var test = orm.Delete(data);
            Assert.IsTrue(test == 0);
        }

        [TestMethod()]
        public void DeleteWithWhereTest()
        {
            var test = orm.Delete<TestData>("ID>@ID", new { ID = 30 });
            Assert.IsTrue(test >= 0);
        }

        [TestMethod()]
        public void UpdateTest()
        {
            var data = TestData.GetTestData(1).FirstOrDefault();
            var test = orm.Update(data);
            Assert.IsTrue(test == 0);
        }

        [TestMethod()]
        public void UpdateFieldsTest()
        {
            var data = TestData.GetTestData(1).FirstOrDefault();
            var test = orm.Update(new List<string>{ "ShortTextProperty"}, data);
            Assert.IsTrue(test >= 0);
        }

        [TestMethod()]
        public void InsertTest()
        {
            var data = TestData.GetTestData(1).FirstOrDefault();
            var test = orm.Insert(data);
            Assert.AreEqual(test, 1);
        }

        [TestMethod()]
        public void InsertAndGetIDTest()
        {
            var data = TestData.GetTestData(1).FirstOrDefault();
            var test = orm.InsertAndGetID(data);
            Assert.IsTrue(test > 0);
        }

        [TestMethod()]
        public void InsertListTest()
        {
            var expected = 10;
            var data = TestData.GetTestData(expected);
            var actual = orm.InsertList(data.ToList());
            Assert.AreEqual(actual, expected);
        }

        [TestMethod()]
        public void ClearTableTest()
        {
            var test = orm.ClearTable<TestData>();
            Assert.IsTrue(test >= 0);
        }

        
    }
}