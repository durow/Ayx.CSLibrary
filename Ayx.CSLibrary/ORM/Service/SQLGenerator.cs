/*
 * Description:Used to generat SQL
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Ayx.CSLibrary.ORM.Service
{
    public class SQLGenerator
    {
        public static string GetInsertSQL<T>(FieldMapping mapping)
        {
            var fieldSB = new StringBuilder();
            var valueSB = new StringBuilder();
            foreach (var map in mapping)
            {
                fieldSB.Append(map.Value).Append(",");
                valueSB.Append("@").Append(map.Key.Name).Append(",");
            }
            return GetInsertSQL(DbAttributes.GetDbTableName<T>(), fieldSB, valueSB);
        }

        public static string GetInsertSQL(string tableName, NameMapping mapping, params string[] passKey)
        {
            var fieldSB = new StringBuilder();
            var valueSB = new StringBuilder();
            foreach (var map in mapping)
            {
                if (passKey.Contains(map.Key)) continue;
                fieldSB.Append(map.Value).Append(",");
                valueSB.Append("@").Append(map.Key).Append(",");
            }
            return GetInsertSQL(tableName, fieldSB, valueSB);
        }

        public static string GetIdentitySQL()
        {
            return "SELECT @@IDENTITY";
        }

        public static string GetSelectSQL<T>(string fields = null , string where = null)
        {
            if (!string.IsNullOrEmpty(where))
                where = " WHERE " + where;
            return @"SELECT * FROM " + DbAttributes.GetDbTableName<T>() + where;
        }

        public static string GetDeleteSQL<T>()
        {
            var keyProperty = DbAttributes.GetPrimaryKeyProperty<T>();
            return GetDeleteMainSQL<T>() + GetKeyWhereSQL(keyProperty);
        }

        public static string GetDeleteSQL<T>(string where)
        {
            if (!string.IsNullOrEmpty(where))
                where = " WHERE " + where;
            return GetDeleteMainSQL<T>() + where;
        }

        public static string GetDeleteMainSQL<T>()
        {
            return @"DELETE FROM " + DbAttributes.GetDbTableName<T>();
        }

        public static string GetUpdateSQL<T>(Dictionary<PropertyInfo,string> mapping)
        {
            var set = new StringBuilder();
            foreach (var map in mapping)
            {
                set.Append(map.Value).Append("=@").Append(map.Key.Name).Append(",");
            }
            var keyProperty = DbAttributes.GetPrimaryKeyProperty<T>();
            return GetUpdateMainSQL<T>() + 
                set.ToString(0,set.Length-1) + 
                GetKeyWhereSQL(keyProperty);
        }

        public static string GetUpdateSQL<T>(IList<string> fields)
        {
            var type = typeof(T);

            var set = new StringBuilder();
            foreach (var property in type.GetProperties())
            {
                if(fields.Contains(property.Name))
                {
                    var fieldName = DbAttributes.GetDbFieldName(property);
                    set.Append(fieldName).Append("=@").Append(property.Name).Append(",");
                }
            }
            if (set.Length == 0)
                throw new AyxORMException("no field to update!");
            try
            {
                var keyProperty = DbAttributes.GetPrimaryKeyProperty<T>();
                return GetUpdateMainSQL<T>() +
                    set.ToString(0, set.Length - 1) +
                    GetKeyWhereSQL(keyProperty);
            }
            catch
            {
                return "";
            }
        }

        public static string GetUpdateMainSQL<T>()
        {
            return "UPDATE " + DbAttributes.GetDbTableName<T>() + " SET ";
        }

        public static string GetKeyWhereSQL(PropertyInfo keyProperty)
        {
            if (keyProperty == null)
                throw new AyxORMException("did't find primary key,please use PrimaryKey attribute!");
            var fieldName = DbAttributes.GetDbFieldName(keyProperty);
            return " WHERE " + fieldName + "=@" + keyProperty.Name;
        }

        public static string GetClearSQL<T>()
        {
            var tableName = DbAttributes.GetDbTableName<T>();
            return "DELETE FROM " + tableName;
        }

        private static string GetInsertSQL(string tableName,StringBuilder fieldSB,StringBuilder valueSB)
        {
            var result = "INSERT INTO " + tableName +
                              "({fields}) VALUES({values})";
            return result
                .Replace("{fields}", fieldSB.ToString(0, fieldSB.Length - 1))
                .Replace("{values}", valueSB.ToString(0, valueSB.Length - 1));
        }
    }
}
