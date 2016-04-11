using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Ayx.CSLibrary.ORMTests.TestData
{
    class TestTable
    {
        public static DataTable GetTable(int count)
        {
            var dt = new DataTable();
            dt.Columns.Add("ID");
            dt.Columns.Add("ShortTextField");
            dt.Columns.Add("IntField");
            dt.Columns.Add("DateTimeField");
            dt.Columns.Add("BoolField");
            dt.Columns.Add("LongTextField");
            dt.Columns.Add("AddField1");
            dt.Columns.Add("AddField2");
            dt.Columns.Add("AddField3");
            dt.Columns.Add("AddField4");
            dt.Columns.Add("AddField5");

            for (int i = 0; i < count; i++)
            {
                dt.Rows.Add(i,
                    "ShortText" + i,
                    i,
                    DateTime.Now.ToString(),
                    true,
                    "LongText" + i,
                    "AddField1" + i,
                    "AddField2" + i,
                    "AddField3" + i,
                    "AddField4" + i,
                    "AddField5" + i);
            }
            return dt;
        }
    }
}
