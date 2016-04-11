using System;
using System.Collections.Generic;
using System.Linq;
/*
 * Description:make it easy to get the attributes of class
*/

using System.Reflection;

namespace Ayx.CSLibrary.Service
{
    public class AttributeHelper
    {

        public static IEnumerable<T> GetAttributes<T>(PropertyInfo property, bool inherit=false) where T :Attribute
        {
            foreach (var attr in property.GetCustomAttributes(typeof(T), inherit))
            {
                yield return (T)attr;
            }
        }

        public static IEnumerable<TAttribute> GetAttributes<TAttribute,TClass>(bool inherit=false) where TAttribute:Attribute
        {
            foreach (var attr in typeof(TClass).GetCustomAttributes(typeof(TAttribute), inherit))
            {
                yield return (TAttribute)attr;
            }
        }

        public static T GetAttribute<T>(PropertyInfo property, bool inherit=false) where T :Attribute
        {
            return property.GetCustomAttributes(typeof(T), inherit).FirstOrDefault() as T;
        }

        public static TAttribute GetAttribute<TAttribute,TClass>(bool inherit=false) where TAttribute:Attribute
        {
            return typeof(TClass).GetCustomAttributes(typeof(TAttribute), inherit).FirstOrDefault() as TAttribute;
        }

        public static bool CheckAttribute<T>(PropertyInfo property, bool inherit = false) where T :Attribute
        {
            return property.GetCustomAttributes(typeof(T), false).Any();
        }
    }
}
