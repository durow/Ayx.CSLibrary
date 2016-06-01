using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace Ayx.CSLibrary.Utility.Config
{
    public sealed class XmlConfigFile : IConfigFile
    {
        public string FileName { get; private set; }
        public XmlDocument Doc { get; private set; }

        public XmlConfigFile(string filename)
        {
            if (!File.Exists(filename))
                throw new FileNotFoundException("未找到配置文件:" + filename);

            FileName = filename;
            Doc = new XmlDocument();
            Doc.Load(filename);
        }

        public string this[string path]
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public void AddOrSet(string path, string value)
        {
            throw new NotImplementedException();
        }

        public void CreateEmpty(string filename)
        {
            Doc = new XmlDocument();
            var declare = Doc.CreateXmlDeclaration("1.0", "utf-8", null);
            var root = Doc.CreateElement("Root");
            Doc.AppendChild(declare);
            Doc.AppendChild(root);
        }

        public void Delete(string path)
        {
            throw new NotImplementedException();
        }

        public T Get<T>(string path)
        {
            var value = Get(path);
            return (T)Convert.ChangeType(value, typeof(T));
        }

        public string Get(string path)
        {
            var realPath = "Root/" + path;
            return Doc.SelectSingleNode(realPath)?.InnerText ?? null;
        }

        public void Set(string path, string value)
        {
            throw new NotImplementedException();
        }
    }
}
