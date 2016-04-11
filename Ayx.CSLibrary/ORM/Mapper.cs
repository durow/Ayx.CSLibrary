using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Ayx.CSLibrary.ORM
{
    public class Mapper<T>
    {
        public FieldMapping FieldMapping { get; private set; }

        public Mapper() { }

        public Mapper(FieldMapping mapping)
        {
            FieldMapping = mapping;
        }

        public Mapper(NameMapping mapping)
        {
            FieldMapping = FieldMapping.GetFromNameMapping<T>(mapping);
        }

        public T From(DataRow row)
        {
            CheckFieldMapping(row.Table.Columns);
            var result = Activator.CreateInstance<T>();
            foreach (var property in FieldMapping)
            {
                SetPropertyValue(result, property.Key, row[property.Value]);
            }
            return result;
        }

        public IEnumerable<T> From(DataTable table)
        {
            if (table == null) yield break;
            CheckFieldMapping(table.Columns);
            foreach (DataRow dr in table.Rows)
            {
                var temp = Activator.CreateInstance<T>();
                foreach (var map in FieldMapping)
                {
                    SetPropertyValue(temp, map.Key, dr[map.Value]);
                }
                yield return temp;
            }
        }

        public IEnumerable<T> From(IDataReader reader)
        {
            CheckFieldMapping(null);

            while (reader.Read())
            {
                var result = Activator.CreateInstance<T>();
                foreach (var property in FieldMapping)
                {
                    SetPropertyValue(result, property.Key, reader[property.Value]);
                }
                yield return result;
            }
        }

        private void CheckFieldMapping(DataColumnCollection columns)
        {
            if (FieldMapping != null) return;
            FieldMapping = FieldMapping.GetSelectMapping<T>(columns);
        }

        public static void SetPropertyValue(object item, PropertyInfo property, object value)
        {
            try
            {
                var newValue = Convert.ChangeType(value, property.PropertyType);
                property.SetValue(item, newValue, null);
            }
            catch
            {
                throw;
            }
        }
    }
}
