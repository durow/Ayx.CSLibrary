using Ayx.CSLibrary.ORM;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Ayx.CSLibrary.ORMTests.TestData
{

    [DbTable("TestTable")]
    public class TestData
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        [DbField(FieldName = "ShortTextField")]
        public string ShortTextProperty { get; set; }

        [DbField(FieldName = "IntField")]
        public int IntProperty { get; set; }

        [DbField(FieldName = "DateTimeField")]
        public string DateTimeProperty { get; set; }

        [DbField(FieldName = "BoolField")]
        public bool BoolField { get; set; }

        [DbField(FieldName = "LongTextField")]
        public string LongTextProperty { get; set; }

        public string AddField1 { get; set; }
        public string AddField2 { get; set; }
        public string AddField3 { get; set; }
        public string AddField4 { get; set; }
        public string AddField5 { get; set; }

        [NotDbField]
        public string Remark { get; set; }
        [NotDbField]
        public string Comment { get; set; }

        public static TestData FromDataRow(DataRow dr)
        {
            return new TestData
            {
                ID = (int)dr["ID"],
                ShortTextProperty = dr["ShortTextField"].ToString(),
                AddField1 = dr["AddField1"].ToString(),
                AddField2 = dr["AddField2"].ToString(),
                AddField3 = dr["AddField3"].ToString(),
                AddField4 = dr["AddField4"].ToString(),
                AddField5 = dr["AddField5"].ToString(),
                BoolField = (bool)dr["BoolField"],
                DateTimeProperty = dr["DateTimeField"].ToString(),
                IntProperty = (int)dr["IntField"],
                LongTextProperty = dr["LongTextField"].ToString(),
            };
        }

        public static TestData FromReader(IDataReader reader)
        {
            return new TestData
            {
                ID = (int)reader["ID"],
                ShortTextProperty = reader["ShortTextField"].ToString(),
                AddField1 = reader["AddField1"].ToString(),
                AddField2 = reader["AddField2"].ToString(),
                AddField3 = reader["AddField3"].ToString(),
                AddField4 = reader["AddField4"].ToString(),
                AddField5 = reader["AddField5"].ToString(),
                BoolField = (bool)reader["BoolField"],
                DateTimeProperty = reader["DateTimeField"].ToString(),
                IntProperty = (int)reader["IntField"],
                LongTextProperty = reader["LongTextField"].ToString(),
            };
        }

        public static IEnumerable<TestData> FromDataTable(DataTable dt)
        {
            foreach (DataRow dr in dt.Rows)
            {
                yield return FromDataRow(dr);
            }
        }

        public static IEnumerable<TestData> GetTestData(int count)
        {
            for (int i = 0; i < count; i++)
            {
                yield return new TestData
                {
                    AddField1 = "fffffffff" + i,
                    AddField2 = "ggggggggg" + i,
                    AddField3 = "hhhhhhhhh" + i,
                    AddField4 = "kkkkkkkkk" + i,
                    AddField5 = "ppppppppp" + i,
                    BoolField = i % 2 == 0,
                    IntProperty = i + 1,
                    DateTimeProperty = DateTime.Now.ToString(),
                    LongTextProperty = "LongTextField" + i,
                    ShortTextProperty = "ShortTextField" + i,
                };
            }
        }
    }
}
