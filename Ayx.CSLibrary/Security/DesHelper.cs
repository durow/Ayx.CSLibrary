using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Ayx.CSLibrary.Security
{
    public sealed class DesHelper
    {
        /// <summary>
        /// 密钥
        /// </summary>
        public readonly string Key;

        /// <summary>
        /// 加密向量
        /// </summary>
        public readonly string Iv;

        /// <summary>
        /// 构造一个DES加解密助手
        /// </summary>
        /// <param name="key">密钥</param>
        /// <param name="iv">加密向量</param>
        public DesHelper(string key = null, string iv = null)
        {
            var rand = new Random(DateTime.Now.Millisecond);
            if (string.IsNullOrEmpty(key))
            {
                int start = rand.Next(0, 24);
                key = Md5Helper
                    .ComputeStringMd5(DateTime.Now.ToString())
                    .Substring(start, 8);
            }
            if (string.IsNullOrEmpty(iv))
            {
                int start = rand.Next(0, 24);
                iv = Md5Helper
                    .ComputeStringMd5(DateTime.Now.ToString())
                    .Substring(start, 8);
            }
            Key = key;
            Iv = iv;
        }

        /// <summary>
        /// 加密字符串
        /// </summary>
        /// <param name="data">要加密的字符串</param>
        /// <param name="encoding">编码方式</param>
        /// <returns>加密后的字符串</returns>
        public string Encrypt(string data, Encoding encoding = null)
        {
            return Encrypt(data, Key, Iv, encoding);
        }

        /// <summary>
        /// 加密数据
        /// </summary>
        /// <param name="data">要加密的数据</param>
        /// <returns>加密后的数据</returns>
        public byte[] Encrypt(byte[] data)
        {
            return Encrypt(data, Key, Iv);
        }

        /// <summary>
        /// 解密字符串
        /// </summary>
        /// <param name="data">要解密的字符串</param>
        /// <param name="encoding">编码方式</param>
        /// <returns>解密后的字符串</returns>
        public string Decrypt(string data, Encoding encoding = null)
        {
            return Decrypt(data, Key, Iv, encoding);
        }

        /// <summary>
        /// 加密字符串
        /// </summary>
        /// <param name="data">要加密的字符串</param>
        /// <param name="key">密钥</param>
        /// <param name="iv">加密向量</param>
        /// <param name="encoding">编码方式</param>
        /// <returns></returns>
        public static string Encrypt(string data, string key, string iv, Encoding encoding = null)
        {
            if (encoding == null)
                encoding = Encoding.UTF8;

            var dataBytes = encoding.GetBytes(data);
            var encryptByte = Encrypt(dataBytes, key, iv);
            return Convert.ToBase64String(encryptByte);
        }

        /// <summary>
        /// 加密数据
        /// </summary>
        /// <param name="data">要加密的数据</param>
        /// <param name="key">密钥</param>
        /// <param name="iv">加密向量</param>
        /// <returns>加密后的数据</returns>
        public static byte[] Encrypt(byte[] data, string key, string iv)
        {
            var keyBytes = Encoding.UTF8.GetBytes(key);
            var ivBytes = Encoding.UTF8.GetBytes(iv);
            var desTrans = new DESCryptoServiceProvider().CreateEncryptor(keyBytes, ivBytes);
            return CryptoTransform(data, desTrans);
        }

        /// <summary>
        /// 解密字符串
        /// </summary>
        /// <param name="data">要解密的字符串</param>
        /// <param name="key">密钥</param>
        /// <param name="iv">向量</param>
        /// <param name="encoding">编码方式</param>
        /// <returns>解密后的字符串</returns>
        public static string Decrypt(string data, string key, string iv, Encoding encoding = null)
        {
            if (encoding == null)
                encoding = Encoding.UTF8;

            var dataBytes = Convert.FromBase64String(data);
            var decryptBytes = Decrypt(dataBytes, key, iv);
            return encoding.GetString(decryptBytes);
        }

        /// <summary>
        /// 解密数据
        /// </summary>
        /// <param name="data">要解密的数据</param>
        /// <param name="key">密钥</param>
        /// <param name="iv">向量</param>
        /// <returns>解密后的数据</returns>
        public static byte[] Decrypt(byte[] data, string key, string iv)
        {
            var keyBytes = Encoding.UTF8.GetBytes(key);
            var ivBytes = Encoding.UTF8.GetBytes(iv);
            var desTrans = new DESCryptoServiceProvider().CreateDecryptor(keyBytes, ivBytes);
            return CryptoTransform(data, desTrans);
        }

        /// <summary>
        /// 加解密
        /// </summary>
        /// <param name="data">待操作的数据</param>
        /// <param name="trans">加解密方式</param>
        /// <returns></returns>
        public static byte[] CryptoTransform(byte[] data, ICryptoTransform trans)
        {
            using (var ms = new MemoryStream())
            using (var desStream = new CryptoStream(ms, trans, CryptoStreamMode.Write))
            {
                desStream.Write(data, 0, data.Length);
                desStream.FlushFinalBlock();
                return ms.ToArray();
            }
        }
    }
}
