/*
 * Author:durow
 * Date:2016.09.23
 * Description:Read by bit from byte[]
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ayx.CSLibrary.IO
{
    public class BitReader
    {
        public static string ReadToBinaryString(byte[] buff, int bitStart, int bitLength)
        {
            var startByte = bitStart / 8;
            var startOff = bitStart % 8;
            var allBytes = (bitStart + bitLength - 1) / 8 - startByte + 1;

            var result = new StringBuilder(bitLength);
            var pointer = bitStart;
            var max = bitStart + bitLength;

            for (int i = 0; i < allBytes; i++)
            {
                var b = buff[startByte + i];
                while (true)
                {
                    var position = 7 - pointer % 8;
                    result.Append(GetBitByPosition(b, position));
                    pointer++;

                    if (pointer % 8 == 0 || pointer >= max)
                        break;
                }
            }

            return result.ToString();
        }

        public static int ReadToInt32(byte[] buff, int bitStart, int bitLength)
        {
            var v = ReadToBinaryString(buff, bitStart, bitLength);
            return Convert.ToInt32(v, 2);
        }

        public static int ReadBitToInt32(byte[] buff, int bitOffset)
        {
            var v = ReadToBinaryString(buff, bitOffset, 1);
            return int.Parse(v);
        }

        public bool ReadBitToBool(byte[] buff, int bitOffset)
        {
            var v = ReadToBinaryString(buff, bitOffset, 1);
            return v == "1";
        }

        private static char GetBitByPosition(byte b, int position)
        {
            var v = b & (byte)Math.Pow(2, position);
            if (v == 0)
                return '0';
            else
                return '1';
        }
    }
}
