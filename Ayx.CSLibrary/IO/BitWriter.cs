using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ayx.CSLibrary.IO
{
    public class BitWriter
    {
        public static byte[] WriteHexString(string hexString)
        {
            var len = hexString.Length / 2;
            var result = new byte[len];

            for (int i = 0; i < len; i++)
            {
                var pos = i * 2;
                var hex = hexString.Substring(pos, 2);
                result[i] = Convert.ToByte(hex, 16);
            }

            return result;
        }
    }
}
