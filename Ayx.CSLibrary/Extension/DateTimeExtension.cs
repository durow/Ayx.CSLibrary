using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ayx.CSLibrary.Extension
{
    public static class DateTimeExtension
    {
        public static string ToStd(this DateTime dt)
        {
            return dt.ToString("yyyy-MM-dd HH:mm:ss");
        }

        public static string ToStdDate(this DateTime dt)
        {
            return dt.ToString("yyyy-MM-dd");
        }

        public static string ToStdTime(this DateTime dt)
        {
            return dt.ToString("HH:mm:ss");
        }
    }
}
