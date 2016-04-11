using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Ayx.CSLibrary.Security
{
    public class Md5Helper
    {
        /// <summary>
        /// 计算文件的MD5
        /// </summary>
        /// <param name="filename">文件名</param>
        /// <returns>计算结果</returns>
        public static string ComputeFileMd5(string filename)
        {
            try
            {
                using (var fs = new FileStream(filename, FileMode.Open))
                {
                    return ComputeMd5(fs);
                }
            }

            catch (Exception)
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 计算字符串的MD5
        /// </summary>
        /// <param name="inputString">字符串</param>
        /// <param name="encoding">编码方式</param>
        /// <returns>计算结果</returns>
        public static string ComputeStringMd5(string inputString, Encoding encoding = null)
        {
            try
            {
                var enc = encoding ?? Encoding.Default;
                var strBytes = enc.GetBytes(inputString);
                return ComputeMd5(strBytes);
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 计算Bytes的MD5
        /// </summary>
        /// <param name="inputData">要计算的数据</param>
        /// <returns>计算结果</returns>
        public static string ComputeMd5(byte[] inputData)
        {
            try
            {
                var resultBytes = new MD5CryptoServiceProvider().ComputeHash(inputData);
                return ByteToHexString(resultBytes);
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 计算流的MD5
        /// </summary>
        /// <param name="stream">要计算的流</param>
        /// <returns>计算结果</returns>
        public static string ComputeMd5(Stream stream)
        {
            try
            {
                var resultBytes = new MD5CryptoServiceProvider().ComputeHash(stream);
                return ByteToHexString(resultBytes);
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Bytes转换为16进制输出的字符串
        /// </summary>
        /// <param name="bytes">要转换的数据</param>
        /// <returns>转换结果</returns>
        public static string ByteToHexString(byte[] bytes)
        {
            var strB = new StringBuilder();

            try
            {
                foreach (var b in bytes)
                {
                    strB.Append(b.ToString("X2"));
                }

                return strB.ToString();
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
    }
}
