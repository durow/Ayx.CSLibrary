using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ayx.CSLibrary.Extension
{
    public static class CollectionExtension
    {
        public static List<T> LeftCircle<T>(this List<T> list)
        {
            if (list.Count < 2) return list;
            return MoveLeft(list, list.First());
        }

        public static List<T> MoveLeft<T>(this List<T> list)
        {
            return MoveLeft(list, default(T));
        }

        public static List<T> MoveLeft<T>(this List<T> list, T newValue)
        {
            if (list.Count == 0) return list;

            var count = list.Count;
            var newList = new List<T>(new T[count]);
            for (int i = 1; i < count; i++)
            {
                newList[i - 1] = list[i];
            }
            newList[count - 1] = newValue;
            return newList;
        }

        public static List<T> RightCircle<T>(this List<T> list)
        {
            if (list.Count < 2) return list;
            return MoveRight(list, list.Last());
        }

        public static List<T> MoveRight<T>(this List<T> list)
        {
            return MoveRight(list, default(T));
        }

        public static List<T> MoveRight<T>(this List<T> list, T newValue)
        {
            if (list.Count == 0) return list;

            var count = list.Count;
            var newList = new List<T>(new T[count]);
            for (int i = 1; i < count; i++)
            {
                newList[i] = list[i - 1];
            }
            newList[0] = newValue;
            return newList;
        }
    }
}
