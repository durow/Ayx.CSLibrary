using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace Ayx.CSLibrary.Utility
{
    public class ConfigFile
    {

        /// <summary>
        /// 当前配置文件的完整路径
        /// </summary>
        public string FileName { get; private set; }

        /// <summary>
        /// 获取或设置配置
        /// </summary>
        /// <param name="key">键名</param>
        /// <returns>键名对应的值</returns>
        public string this[string key]
        {
            get
            {
                return GetConfigValue(key);
            }
            set
            {
                AddOrSetConfigValue(key, value);
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        protected ConfigFile()
        {
        }

        /// <summary>
        /// 读取配置文件
        /// </summary>
        /// <param name="filename">配置文件名</param>
        /// <returns>ConfigFile对象</returns>
        public static ConfigFile LoadFile(string filename)
        {
            if (!File.Exists(filename)) return null;
            return new ConfigFile { FileName = filename };
        }

        /// <summary>
        /// 读取或创建配置文件
        /// </summary>
        /// <param name="filename">配置文件名</param>
        /// <returns>ConfigFile对象</returns>
        public static ConfigFile LoadOrCreateFile(string filename)
        {
            if (!File.Exists(filename))
                new ConfigFile().CreateFile(filename);
            return LoadFile(filename);
        }

        /// <summary>
        /// 创建配置文件，可在继承类中重写此方法
        /// </summary>
        /// <param name="filename">配置文件名</param>
        protected virtual void CreateFile(string filename)
        {
            XmlDocument doc = new XmlDocument();
            XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            doc.AppendChild(dec);
            XmlElement root = doc.CreateElement("Config");
            doc.AppendChild(root);
            doc.Save(filename);
        }

        /// <summary>
        /// 根据Key从配置文件中获取值
        /// </summary>
        /// <param name="path">键名</param>
        /// <returns>获取的值，找不到返回""</returns>
        public string GetConfigValue(string path)
        {
            var doc = GetXmlDocument();
            var node = doc.SelectSingleNode("Config/" + path);
            return node?.InnerText ?? "";
        }

        /// <summary>
        /// 从配置文件中根据key获取值并转化为相应数据类型
        /// </summary>
        /// <param name="key">键名</param>
        /// <param name="result">转化后的值</param>
        /// <returns>转化是否成功</returns>
        public bool TryParseConfigValue(string key, out int result)
        {
            var v = GetConfigValue(key);
            return int.TryParse(v, out result);
        }

        /// <summary>
        /// 从配置文件中根据key获取值并转化为相应数据类型
        /// </summary>
        /// <param name="key">键名</param>
        /// <param name="result">转化后的值</param>
        /// <returns>转化是否成功</returns>
        public bool TryParseConfigValue(string key, out bool result)
        {
            var v = GetConfigValue(key);
            return bool.TryParse(v, out result);
        }

        /// <summary>
        /// 从配置文件中根据key获取值并转化为相应数据类型
        /// </summary>
        /// <param name="key">键名</param>
        /// <param name="result">转化后的值</param>
        /// <returns>转化是否成功</returns>
        public bool TryParseConfigValue(string key, out DateTime result)
        {
            var v = GetConfigValue(key);
            return DateTime.TryParse(v, out result);
        }

        /// <summary>
        /// 从配置文件中根据key获取值并转化为相应数据类型
        /// </summary>
        /// <param name="key">键名</param>
        /// <param name="result">转化后的值</param>
        /// <returns>转化是否成功</returns>
        public bool TryParseConfigValue(string key, out float result)
        {
            var v = GetConfigValue(key);
            return float.TryParse(v, out result);
        }

        /// <summary>
        /// 从配置文件中根据key获取值并转化为相应数据类型
        /// </summary>
        /// <param name="key">键名</param>
        /// <param name="result">转化后的值</param>
        /// <returns>转化是否成功</returns>
        public bool TryParseConfigValue(string key, out double result)
        {
            var v = GetConfigValue(key);
            return double.TryParse(v, out result);
        }

        /// <summary>
        /// 从配置文件中根据key获取值并转化为相应数据类型
        /// </summary>
        /// <param name="key">键名</param>
        /// <param name="result">转化后的值</param>
        /// <returns>转化是否成功</returns>
        public bool TryParseConfigValue(string key, out decimal result)
        {
            var v = GetConfigValue(key);
            return decimal.TryParse(v, out result);
        }

        /// <summary>
        /// 在配置文件中根据key取出值并转化为int类型
        /// </summary>
        /// <param name="key"></param>
        /// <returns>返回的值</returns>
        public int GetConfigValueInt(string key)
        {
            var v = GetConfigValue(key);
            return int.Parse(v);
        }

        /// <summary>
        /// 在配置文件中根据key取出值并转化为bool类型
        /// </summary>
        /// <param name="key"></param>
        /// <returns>返回的值</returns>
        public bool GetConfigValueBool(string key)
        {
            var v = GetConfigValue(key);
            return bool.Parse(v);
        }

        /// <summary>
        /// 在配置文件中根据key取出值并转化为float类型
        /// </summary>
        /// <param name="key"></param>
        /// <returns>返回的值</returns>
        public float GetConfigValueFloat(string key)
        {
            var v = GetConfigValue(key);
            return float.Parse(v);
        }

        /// <summary>
        /// 在配置文件中根据key取出值并转化为double类型
        /// </summary>
        /// <param name="key"></param>
        /// <returns>返回的值</returns>
        public double GetConfigValueDouble(string key)
        {
            var v = GetConfigValue(key);
            return double.Parse(v);
        }

        /// <summary>
        /// 在配置文件中根据key取出值并转化为DateTime类型
        /// </summary>
        /// <param name="key"></param>
        /// <returns>返回的值</returns>
        public DateTime GetConfigValueDateTime(string key)
        {
            var v = GetConfigValue(key);
            return DateTime.Parse(v);
        }

        /// <summary>
        /// 在配置文件中根据key取出值并转化为decimal类型
        /// </summary>
        /// <param name="key"></param>
        /// <returns>返回的值</returns>
        public decimal GetConfigValueDecimal(string key)
        {
            var v = GetConfigValue(key);
            return decimal.Parse(v);
        }

        /// <summary>
        /// 修改Key对应的值，如果Key不存在则添加后存入值
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="value">要存入的值</param>
        public void AddOrSetConfigValue(string key, string value)
        {
            var doc = GetXmlDocument();
            var node = doc.SelectSingleNode("Root/Config/" + key);
            if (node == null)
            {
                node = doc.CreateElement(key);
                doc.SelectSingleNode("Root/Config")?.AppendChild(node);
            }
            node.InnerText = value;
            doc.Save(FileName);
        }

        public void AddOrSetConfigValue<T>(string key, T value)
        {
            var v = value.ToString();
            AddOrSetConfigValue(key, v);
        }

        /// <summary>
        /// 删除Key以及对应的值
        /// </summary>
        /// <param name="key">Key</param>
        public void DeleteConfigKey(string key)
        {
            var doc = GetXmlDocument();
            var node = doc.SelectSingleNode("Root/Config/" + key);
            if (node == null) return;
            doc.SelectSingleNode("Root/Config")?.RemoveChild(node);
            doc.Save(FileName);
        }

        /// <summary>
        /// 读取XML文档
        /// </summary>
        /// <returns></returns>
        private XmlDocument GetXmlDocument()
        {
            var doc = new XmlDocument();
            doc.Load(FileName);
            return doc;
        }
    }
}
