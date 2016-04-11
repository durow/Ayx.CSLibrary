/*
 * Instead of Dictionary<PropertyInfo,string> for short.
*/

using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace Ayx.CSLibrary.ORM
{
    public class FieldMapping:Dictionary<PropertyInfo,string>
    {
        public static FieldMapping GetSelectMapping<T>(DataColumnCollection columns = null)
        {
            var result = new FieldMapping();
            var type = typeof(T);
            foreach (var property in type.GetProperties())
            {
                if (!DbAttributes.IsDbField(property)) continue;
                var fieldName = DbAttributes.GetDbFieldName(property);
                if (columns != null)
                    if (!columns.Contains(fieldName)) continue;
                result.Add(property, fieldName);
            }
            return result;
        }

        public static FieldMapping GetInsertMapping<T>()
        {
            var result = new FieldMapping();
            var type = typeof(T);
            foreach (var property in type.GetProperties())
            {
                if (!DbAttributes.IsDbField(property)) continue;
                if (DbAttributes.IsAutoIncrement(property)) continue;
                var fieldName = DbAttributes.GetDbFieldName(property);
                result.Add(property, fieldName);
            }
            return result;
        }

        public static FieldMapping GetUpdateMapping<T>()
        {
            var result = new FieldMapping();
            var type = typeof(T);
            foreach (var property in type.GetProperties())
            {
                if (!DbAttributes.IsDbField(property)) continue;
                if (DbAttributes.IsAutoIncrement(property)) continue;
                var fieldName = DbAttributes.GetDbFieldName(property);
                result.Add(property, fieldName);
            }
            return result;
        }

        public static FieldMapping GetFromNameMapping<T>(NameMapping mapping)
        {
            var result = new FieldMapping();
            var type = typeof(T);
            foreach (var property in type.GetProperties())
            {
                if (mapping.ContainsKey(property.Name))
                    result.Add(property, mapping[property.Name]);
            }
            return result;
        }
    }
}
