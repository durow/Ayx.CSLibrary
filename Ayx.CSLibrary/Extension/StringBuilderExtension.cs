using System.Collections.Generic;
using System.Text;

namespace Ayx.CSLibrary.Extension
{
    public static class StringBuilderExtension
    {
        /// <summary>
        /// 在StringBuilder中寻找字符
        /// </summary>
        /// <param name="sb">要寻找的StringBuilder</param>
        /// <param name="c">寻找的字符</param>
        /// <param name="startPos">开始位置</param>
        /// <param name="count">长度</param>
        /// <returns></returns>
        public static int IndexOf(this StringBuilder sb, char c, int startPos = 0, int count = -1)
        {
            var end = sb.Length;
            if (count > 0)
            {
                end = startPos + count;
                if (end > sb.Length) end = sb.Length;
            }
            for (int i = startPos; i < end; i++)
            {
                if (sb[i] == c)
                    return i;
            }
            return -1;
        }
        /// <summary>
        /// 在StringBuilder中寻找字符串并返回偏移量
        /// </summary>
        /// <param name="sb">要寻找的字符串</param>
        /// <param name="str">寻找的字符串</param>
        /// <param name="startPos">开始位置</param>
        /// <param name="count">长度</param>
        /// <returns></returns>
        public static int IndexOf(this StringBuilder sb, string str, int startPos = 0, int count = -1)
        {
            var isEqual = true;
            var end = sb.Length;
            if (count >= 0)
            {
                if (count < str.Length)
                    return -1;
                end = startPos + count;
                if (end > sb.Length) end = sb.Length;
            }
            end = end - str.Length + 1;
            for (int i = startPos; i < end; i++)
            {
                isEqual = true;
                for (int j = 0; j < str.Length; j++)
                {
                    if (sb[i + j] != str[j])
                    {
                        isEqual = false;
                        break;
                    }
                }
                if (isEqual)
                    return i;
            }
            return -1;
        }
        /// <summary>
        /// 获取字符在字符串中最后的位置
        /// </summary>
        /// <param name="sb">字符串</param>
        /// <param name="c">查找的字符</param>
        /// <param name="lastPos">起始的位置</param>
        /// <param name="count">往前查找的长度</param>
        /// <returns>字符所在偏移量，未找到返回-1</returns>
        public static int LastIndexOf(this StringBuilder sb, char c, int lastPos = -1, int count = -1)
        {
            if (lastPos < 0) lastPos = sb.Length - 1;
            var start = 0;
            if (count >= 0)
            {
                start = lastPos - count + 1;
                if (start < 0) start = 0;
            }
            for (int i = lastPos; i >= start; i--)
            {
                if (sb[i] == c)
                    return i;
            }
            return -1;
        }
        /// <summary>
        /// 获取字符串在给定字符串中最后的位置
        /// </summary>
        /// <param name="sb">字符串</param>
        /// <param name="c">查找的字符串</param>
        /// <param name="lastPos">起始的位置</param>
        /// <param name="count">往前查找的长度</param>
        /// <returns>字符所在偏移量，未找到返回-1</returns>
        public static int LastIndexOf(this StringBuilder sb, string str, int lastPos = -1, int count = -1)
        {
            if (lastPos < 0) lastPos = sb.Length - 1;
            lastPos = lastPos - str.Length + 1;
            var start = 0;
            if (count >= 0)
            {
                if (count < str.Length)
                    return -1;
                start = lastPos - count + 1;
                if (start < 0) start = 0;
            }
            for (int i = lastPos; i >= start; i--)
            {
                var isEqual = true;
                for (int j = 0; j < str.Length; j++)
                {
                    if (sb[i + j] != str[j])
                    {
                        isEqual = false;
                        break;
                    }
                }
                if (isEqual)
                    return i;
            }
            return -1;
        }
        /// <summary>
        /// 用字符拆分字符串
        /// </summary>
        /// <param name="sb">要被拆分的字符串</param>
        /// <param name="separator">拆分的字符</param>
        /// <param name="count">拆分次数，0为不拆分，默认全部拆分</param>
        /// <returns>拆分结果</returns>
        public static IEnumerable<string> Split(this StringBuilder sb, char separator = ' ', int count = int.MaxValue)
        {
            var counter = 1;
            var start = 0;
            for (int i = 0; i < sb.Length; i++)
            {
                if (counter > count)
                {
                    yield return sb.ToString(start, sb.Length - start);
                    break;
                }
                if (sb[i] == separator)
                {
                    yield return sb.ToString(start, i - start);
                    start = i + 1;
                    counter++;
                }
                else if (i == sb.Length - 1)
                {
                    yield return sb.ToString(start, sb.Length - start);
                }
            }
        }
        /// <summary>
        /// 从末尾往前按字符拆分字符串
        /// </summary>
        /// <param name="sb">要被拆分的字符串</param>
        /// <param name="separator">拆分的字符</param>
        /// <param name="count">最多拆分次数</param>
        /// <returns></returns>
        public static IEnumerable<string> SplitReverse(this StringBuilder sb, char separator = ' ', int count = int.MaxValue)
        {
            var counter = 1;
            var end = sb.Length;
            for (int i = sb.Length - 1; i >= 0; i--)
            {
                if (counter > count)
                {
                    yield return sb.ToString(0, end);
                    break;
                }
                if (sb[i] == separator)
                {
                    yield return sb.ToString(i + 1, end - i - 1);
                    end = i;
                    counter++;
                }
                else if (i == 0)
                {
                    yield return sb.ToString(0, end);
                }
            }
        }
        /// <summary>
        /// 反转字符串
        /// </summary>
        /// <param name="sb">要反转的字符串</param>
        public static void Reverse(this StringBuilder sb)
        {
            var temp = new char[sb.Length];
            for (int i = 0; i < sb.Length; i++)
            {
                temp[sb.Length - i - 1] = sb[i];
            }
            for (int i = 0; i < sb.Length; i++)
            {
                sb[i] = temp[i];
            }
        }
        /// <summary>
        /// 获取反转的字符串，不影响原值
        /// </summary>
        /// <param name="sb">要反转的字符串</param>
        /// <returns>反转后的字符串</returns>
        public static StringBuilder GetReverse(this StringBuilder sb)
        {
            var result = new StringBuilder();
            for (int i = 0; i < sb.Length; i++)
            {
                result.Append(sb[sb.Length - i - 1]);
            }
            return result;
        }

    }
}
