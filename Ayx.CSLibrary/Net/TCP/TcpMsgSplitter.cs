/**************************************************************************************
 * 文件名：TcpMsgSplitter.cs
 * 主要功能：TCP通信中用于解决粘包问题的消息拆分/包装器
 * 作者：durow
 * 时间：2015.08.21
**************************************************************************************/

using Ayx.CSLibrary.Extension;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ayx.CSLibrary.Net.TCP
{
    public class TcpMsgSplitter
    {

        /// <summary>
        /// 消息缓存
        /// </summary>
        private readonly StringBuilder _tempString;

        /// <summary>
        /// 构造函数
        /// </summary>
        public TcpMsgSplitter()
        {
            _tempString = new StringBuilder();
        }

        /// <summary>
        /// 发送前打包消息方便收到后分离
        /// </summary>
        /// <param name="msg">需要打包的消息</param>
        /// <returns>打包后的消息</returns>
        public static string PackMessage(string msg)
        {
            return $"<{msg.Length.ToString()}>" + msg;
        }

        /// <summary>
        /// 拆分消息
        /// </summary>
        /// <param name="msg">要拆分的消息</param>
        /// <returns>拆分后的消息列表</returns>
        public IEnumerable<string> Split(string msg)
        {
            if (string.IsNullOrEmpty(msg))
                yield break;

            _tempString.Append(msg);
            while (true)
            {
                if (_tempString.Length == 0)
                    break;
                string temp;
                try
                {
                    temp = TryToReadMsg();
                }
                catch (Exception)
                {
                    _tempString.Clear();
                    break;
                }
                if (temp == null)
                    break;
                else
                    yield return temp;
            }
        }

        /// <summary>
        /// 读取消息内容
        /// </summary>
        /// <returns></returns>
        private string TryToReadMsg()
        {
            //var temp = _tempString.ToString();
            var start = _tempString.IndexOf('<');
            //var start = temp.IndexOf("<");
            if (start < 0)
                throw new Exception("Failed to find the start of the message length field!");
            var end = _tempString.IndexOf('>');
            if (end < 0)
                throw new Exception("Failed to find the end of the message length field!");
            if (start > end)
                throw new Exception("Failed to find the message length field!");
            var msgLen = int.Parse(_tempString.ToString(start + 1, end - start - 1));
            if (_tempString.Length < msgLen + end + 1)
                return null;
            var result = _tempString.ToString(end + 1, msgLen);
            _tempString.Remove(0, end + msgLen + 1);
            return result;
        }
    }
}