using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Ayx.CSLibrary.Security
{
    public sealed class RsaHelper
    {
        private readonly RSACryptoServiceProvider rsa;
        public int BlockSize { get; private set; }
        public int EncryptBlockSize { get; private set; }

        public RsaHelper()
        {
            rsa = new RSACryptoServiceProvider();
            BlockSize = rsa.KeySize / 8 - 11;
            EncryptBlockSize = rsa.KeySize / 8;
        }

        /// <summary>
        /// 获取公钥
        /// </summary>
        /// <returns></returns>
        public string GetPkXmlString()
        {
            return rsa.ToXmlString(false);
        }

        /// <summary>
        /// 初始化公钥以便进行加密
        /// </summary>
        /// <param name="xmlPkString">要初始化的公钥</param>
        public void InitPkFromXmlString(string xmlPkString)
        {
            rsa.FromXmlString(xmlPkString);
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="data">要加密的数据</param>
        /// <param name="encoding">编码方式</param>
        /// <returns>加密后的数据</returns>
        public string Encrypt(string data, Encoding encoding = null)
        {
            var enc = encoding ?? Encoding.Default;
            byte[] encryptBuf = Encrypt(enc.GetBytes(data));
            return Convert.ToBase64String(encryptBuf);
        }

        /// <summary>
        /// 加密数据
        /// </summary>
        /// <param name="data">要加密的数据</param>
        /// <returns>加密后的数据</returns>
        public byte[] Encrypt(byte[] data)
        {
            var buf = new byte[BlockSize];
            using (var dataStream = new MemoryStream(data))
            using (var encryptStream = new MemoryStream())
            {
                dataStream.Position = 0;
                int len = dataStream.Read(buf, 0, BlockSize);
                while (len > 0)
                {
                    byte[] encryptTemp;
                    if (len == BlockSize)
                    {
                        encryptTemp = rsa.Encrypt(buf, false);
                    }
                    else
                    {
                        var shortBuf = new byte[len];
                        Array.Copy(buf, 0, shortBuf, 0, len);
                        encryptTemp = rsa.Encrypt(shortBuf, false);
                    }
                    encryptStream.Write(encryptTemp, 0, encryptTemp.Length);
                    len = dataStream.Read(buf, 0, BlockSize);
                }
                return encryptStream.ToArray();
            }
        }

        /// <summary>
        /// 解密数据
        /// </summary>
        /// <param name="encryptString">要解密的数据</param>
        /// <param name="encoding">编码方式</param>
        /// <returns>解密后的数据</returns>
        public string Decrypt(string encryptString, Encoding encoding = null)
        {
            var enc = encoding ?? Encoding.Default;
            var buf = Convert.FromBase64String(encryptString);
            var decryptBytes = Decrypt(buf);
            return enc.GetString(decryptBytes);
        }

        /// <summary>
        /// 解密数据
        /// </summary>
        /// <param name="encryptBytes">要解密的数据</param>
        /// <returns>解密后的数据</returns>
        public byte[] Decrypt(byte[] encryptBytes)
        {
            int max = encryptBytes.Length / EncryptBlockSize;

            using (var encryptStream = new MemoryStream(encryptBytes))
            using (var decryptStream = new MemoryStream())
            {
                encryptStream.Position = 0;
                var tempBuf = new byte[EncryptBlockSize];
                int len = encryptStream.Read(tempBuf, 0, EncryptBlockSize);
                while (len > 0)
                {
                    var decryptTemp = rsa.Decrypt(tempBuf, false);
                    decryptStream.Write(decryptTemp, 0, decryptTemp.Length);
                    len = encryptStream.Read(tempBuf, 0, EncryptBlockSize);
                }
                return decryptStream.ToArray();
            }
        }

        /// <summary>
        /// 加密文件
        /// </summary>
        /// <param name="sourceFilePath">原始文件路径</param>
        /// <param name="encryptFilePath">加密文件路径</param>
        public void EncryptFile(string sourceFilePath, string encryptFilePath)
        {
            using (var fsSource = new FileStream(sourceFilePath, FileMode.Open))
            using (var fsTarget = new FileStream(encryptFilePath, FileMode.Create))
            {
                var fileBytes = new byte[fsSource.Length];
                fsSource.Read(fileBytes, 0, (int)fsSource.Length);
                var encryptBuf = Encrypt(fileBytes);
                fsTarget.Write(encryptBuf, 0, encryptBuf.Length);
                fsTarget.Flush();
            }
        }

        /// <summary>
        /// 解密文件
        /// </summary>
        /// <param name="encryptFilePath">加密路径</param>
        /// <param name="decryptFilePath">解密后路径</param>
        public void DecryptFile(string encryptFilePath, string decryptFilePath)
        {
            using (var fsEncrypt = new FileStream(encryptFilePath, FileMode.Open))
            using (var fsDecrypt = new FileStream(decryptFilePath, FileMode.Create))
            {
                var encryptBytes = new byte[fsEncrypt.Length];
                fsEncrypt.Read(encryptBytes, 0, encryptBytes.Length);
                var decryptBuf = Decrypt(encryptBytes);
                fsDecrypt.Write(decryptBuf, 0, decryptBuf.Length);
                fsDecrypt.Flush();
            }
        }
    }
}
