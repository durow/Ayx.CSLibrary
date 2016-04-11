/**************************************************************************************
 * 文件名：AyxTcpMessage.cs
 * 主要功能：TCP通信中的消息对象，可以方便的存取消息头信息并以键值对的方式存取消息内容
 * 作者：durow
 * 时间：2015.08.21
**************************************************************************************/

using System;
using System.Collections.Generic;
using System.Xml;

namespace Ayx.CSLibrary.Net.TCP
{
    public class AyxTcpMessage
    {
        #region 字段

        /// <summary>
        /// Xml文档
        /// </summary>
        public XmlDocument Doc { get; private set; }

        /// <summary>
        /// 命令
        /// </summary>
        public string Command { get; private set; }

        /// <summary>
        /// 目标列表
        /// </summary>
        public IEnumerable<string> TargetList { get; private set; }

        /// <summary>
        /// 所属模块
        /// </summary>
        public string Module { get; private set; }

        /// <summary>
        /// 发送方
        /// </summary>
        public string Sender { get; private set; }

        /// <summary>
        /// 版本信息
        /// </summary>
        public string Version { get; private set; }

        /// <summary>
        /// 附加信息
        /// </summary>
        public string Extra { get; private set; }

        #endregion

        #region 构造函数

        /// <summary>
        /// 创建一个空的消息对象
        /// </summary>
        private AyxTcpMessage()
        {
            Doc = new XmlDocument();
        }

        /// <summary>
        /// 创建一个消息对象
        /// </summary>
        /// <param name="command">命令</param>
        /// <param name="targetList">目标列表</param>
        /// <param name="sender">发送方</param>
        /// <param name="module">所属模块</param>
        /// <param name="extra">附加信息</param>
        /// <param name="version">版本信息</param>
        public AyxTcpMessage(
            string command, //命令
            IEnumerable<string> targetList = null, //目标列表，空为所有
            string sender = "", //发送方ID
            string module = "", //消息所属模块
            string extra = "", //附加信息
            string version = "1.0") //版本号     
        {
            CreateDoc();
            Command = command;
            TargetList = targetList ?? new List<string>();
            Module = module;
            Sender = sender;
            Extra = extra;
            Version = version;
            InitMsgInfo();
        }

        #endregion

        #region 公开方法

        /// <summary>
        /// 从字符串解析消息
        /// </summary>
        /// <param name="msg">要解析的字符串</param>
        /// <returns>解析后的消息对象</returns>
        public static AyxTcpMessage Parse(string msg)
        {
            var result = new AyxTcpMessage();
            result.Doc.LoadXml(msg);
            result.ReadMsgInfo();
            return result;
        }

        /// <summary>
        /// 从字符串解析消息
        /// </summary>
        /// <param name="msg">要解析的字符串</param>
        /// <param name="tcpMsg">解析后的消息对象，解析错误则为null</param>
        /// <returns>解析是否成功</returns>
        public static bool TryParse(string msg, out AyxTcpMessage tcpMsg)
        {
            try
            {
                tcpMsg = Parse(msg);
                return true;
            }
            catch (Exception)
            {
                tcpMsg = null;
                return false;
            }
        }

        /// <summary>
        /// 添加键值对，键名可重复
        /// </summary>
        /// <param name="key">键名</param>
        /// <param name="value">值</param>
        public void AddKeyValue(string key, string value)
        {
            var node = Doc.CreateElement(key);
            node.InnerText = value;
            Doc.SelectSingleNode("R/K")?.AppendChild(node);
        }

        /// <summary>
        /// 批量添加键值对
        /// </summary>
        /// <param name="keyValues">Dictionary对象</param>
        public void AddKeyValues(Dictionary<string, string> keyValues)
        {
            if (keyValues == null) return;
            var keyNode = Doc.SelectSingleNode("R/K");
            if (keyNode == null) throw new Exception("Key node is not exists!");
            foreach (var item in keyValues)
            {
                var node = Doc.CreateElement(item.Key);
                node.InnerText = item.Value;
                keyNode.AppendChild(node);
            }
        }

        /// <summary>
        /// 获取键对应的值列表
        /// </summary>
        /// <param name="key">键名</param>
        /// <returns>值列表</returns>
        public IEnumerable<string> GetKeyValues(string key)
        {
            var nodes = Doc.SelectNodes("R/K/" + key);
            if (nodes == null) yield break;
            foreach (XmlNode node in nodes)
            {
                yield return node.InnerText;
            }
        }

        /// <summary>
        /// 根据键名获取值
        /// </summary>
        /// <param name="key">键名</param>
        /// <returns>对应的值，找不到则为空</returns>
        public string GetKeyValue(string key)
        {
            var node = Doc.SelectSingleNode("R/K/" + key);
            return node?.InnerText ?? "";
        }

        /// <summary>
        /// 输出为可供发送的Tcp字符串
        /// </summary>
        /// <returns>字符串</returns>
        public override string ToString()
        {
            return Doc?.InnerXml ?? "";
        }

        #endregion

        #region 非公开方法

        /// <summary>
        /// 创建内置的XmlDocument对象
        /// </summary>
        protected virtual void CreateDoc()
        {
            Doc = new XmlDocument();
            var root = Doc.CreateElement("R");
            Doc.AppendChild(root);
            var msg = Doc.CreateElement("M");
            var keys = Doc.CreateElement("K");
            root.AppendChild(msg);
            root.AppendChild(keys);
        }


        /// <summary>
        /// 初始化Tcp消息的描述区域
        /// </summary>
        private void InitMsgInfo()
        {
            var msgNode = Doc.SelectSingleNode("R/M");
            if (msgNode == null) return;
            AddMsgNode(msgNode, "C", Command);
            AddMsgNode(msgNode, "M", Module);
            AddMsgNode(msgNode, "S", Sender);
            AddMsgNode(msgNode, "E", Extra);
            AddMsgNode(msgNode, "V", Version);
            foreach (var item in TargetList)
            {
                AddMsgNode(msgNode, "T", item);
            }
        }

        /// <summary>
        /// 从Doc中读取消息描述信息
        /// </summary>
        private void ReadMsgInfo()
        {
            Command = GetMessageInfo("C");
            Module = GetMessageInfo("M");
            Sender = GetMessageInfo("S");
            Extra = GetMessageInfo("E");
            Version = GetMessageInfo("V");
            TargetList = GetMessageInfoList("T");
        }

        /// <summary>
        /// 添加消息描述节点
        /// </summary>
        /// <param name="msgNode"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        private void AddMsgNode(XmlNode msgNode, string key, string value)
        {
            var node = Doc.CreateElement(key);
            node.InnerText = value;
            msgNode.AppendChild(node);
        }

        /// <summary>
        /// 填充消息描述信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private string GetMessageInfo(string key)
        {
            return Doc.SelectSingleNode("R/M/" + key)?.InnerText ?? "";
        }

        /// <summary>
        /// 填充消息描述信息列表
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private IEnumerable<string> GetMessageInfoList(string key)
        {
            var nodes = Doc.SelectNodes("R/M/" + key);
            if (nodes == null) yield break;
            foreach (XmlNode node in nodes)
            {
                yield return node.InnerText;
            }
        }

        #endregion
    }
}