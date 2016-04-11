using Ayx.CSLibrary.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Ayx.CSLibrary.ORM
{
    public sealed class DbAttributes
    {
        public static string GetDbFieldName(PropertyInfo property)
        {
            var attr = GetDbFieldAttribute(property);
            if (attr != null && !string.IsNullOrEmpty(attr.FieldName))
                return attr.FieldName;
            return property.Name;
        }

        public static DbFieldAttribute GetDbFieldAttribute(PropertyInfo property)
        {
            return AttributeHelper.GetAttribute<DbFieldAttribute>(property);
        }

        public static string GetDbTableName<T>()
        {
            var attr = AttributeHelper.GetAttribute<DbTableAttribute, T>();
            if (attr != null && !string.IsNullOrEmpty(attr.TableName))
                return attr.TableName;
            return typeof(T).Name;
        }

        public static PropertyInfo GetPrimaryKeyProperty<T>()
        {
            var type = typeof(T);
            foreach (var property in type.GetProperties())
            {
                if (IsPrimaryKey(property))
                    return property;
            }
            return null;
        }

        public static bool IsPrimaryKey(PropertyInfo property)
        {
            return AttributeHelper.CheckAttribute<PrimaryKeyAttribute>(property);
        }

        public static bool IsDbField(PropertyInfo property)
        {
            return !AttributeHelper.CheckAttribute<NotDbFieldAttribute>(property);
        }

        public static bool IsAutoIncrement(PropertyInfo property)
        {
            return AttributeHelper.CheckAttribute<AutoIncrementAttribute>(property);
        }

    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public sealed class DbFieldAttribute : Attribute
    {
        public string FieldName { get; set; }
        public string FieldType { get; set; }
        public int MaxLength { get; set; }
        public bool AllowEmpty { get; set; }

        public DbFieldAttribute()
        {
        }
    }

    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class DbTableAttribute : Attribute
    {
        public string TableName { get; private set; }
        public DbTableAttribute(string TableName)
        {
            this.TableName = TableName;
        }
    }

    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class PrimaryKeyAttribute : Attribute
    {

    }

    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class NotDbFieldAttribute : Attribute
    {

    }

    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class AutoIncrementAttribute : Attribute
    {

    }
}
