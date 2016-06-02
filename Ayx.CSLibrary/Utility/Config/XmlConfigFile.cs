/*
 * Author:durow
 * implement IConfigFile by xml file
 * Date:2016.06.02
 */

using System;
using System.IO;
using System.Xml;

namespace Ayx.CSLibrary.Utility.Config
{
    public sealed class XmlConfigFile : IConfigFile
    {
        /// <summary>
        /// filename of the config file
        /// </summary>
        public string FileName { get; private set; }

        /// <summary>
        /// xml object
        /// </summary>
        public XmlDocument Doc { get; private set; }

        /// <summary>
        /// name of the config file.
        /// if the specific file dosn't exists,it can creates an empty one automatically
        /// </summary>
        /// <param name="filename"></param>
        public XmlConfigFile(string filename)
        {
            if (!File.Exists(filename))
            {
                CreateEmpty(filename);
            }

            FileName = filename;
            Doc = new XmlDocument();
            Doc.Load(filename);
        }

        #region Interface Methods

        /// <summary>
        /// get or set the config value by string
        /// </summary>
        /// <param name="path">config path</param>
        /// <returns>config value</returns>
        public string this[string path]
        {
            get
            {
                return Get(path);
            }

            set
            {
                AddOrSet(path, value);
            }
        }

        /// <summary>
        /// add new config with value
        /// </summary>
        /// <param name="path">config path</param>
        /// <param name="value">config value</param>
        /// <returns>if add is successful</returns>
        public bool Add(string path, string value)
        {
            if (string.IsNullOrEmpty(path))
                return false;

            var node = GetNode(path);
            if (node != null) return false;

            var nodeStringList = StandardPath(path).Split('/');
            var parent = Doc.SelectSingleNode("Root");
            if (parent == null)
                throw new Exception("can't find Root elment!");
            for (int i = 1; i < nodeStringList.Length; i++)
            {
                parent = AddChild(parent, nodeStringList[i]);
            }

            parent.InnerText = value;
            Doc.Save(FileName);
            return true;
        }

        /// <summary>
        /// set config value,if not exists it can create a new path and set it with the value
        /// </summary>
        /// <param name="path">config path</param>
        /// <param name="value">config value</param>
        public void AddOrSet(string path, string value)
        {
            var node = GetNode(path);
            if (node == null)
                Add(path, value);
            else
                node.InnerText = value;
        }

        /// <summary>
        /// create an empty config file
        /// </summary>
        /// <param name="filename">config filename</param>
        public void CreateEmpty(string filename)
        {
            Doc = new XmlDocument();
            var declare = Doc.CreateXmlDeclaration("1.0", "utf-8", null);
            var root = Doc.CreateElement("Root");
            Doc.AppendChild(declare);
            Doc.AppendChild(root);
            Doc.Save(filename);
        }

        /// <summary>
        /// delete config path
        /// </summary>
        /// <param name="path">config path</param>
        public void Delete(string path)
        {
            var node = GetNode(path);
            if (node == null) return;

            Doc.RemoveChild(node);
        }

        /// <summary>
        /// get config value with type T
        /// </summary>
        /// <typeparam name="T">value type</typeparam>
        /// <param name="path">config path</param>
        /// <returns>config value</returns>
        public T Get<T>(string path)
        {
            var value = Get(path);
            return (T)Convert.ChangeType(value, typeof(T));
        }

        /// <summary>
        /// get config value with string,you can use config["path"] instead
        /// </summary>
        /// <param name="path">config path</param>
        /// <returns>config value</returns>
        public string Get(string path)
        {
            return Doc
                .SelectSingleNode(StandardPath(path))?
                .InnerText ?? null;
        }

        /// <summary>
        /// set config value,if config path doesn't exist ,do nothing and return false
        /// you can use config["path"] = value instead.
        /// </summary>
        /// <param name="path">config path</param>
        /// <param name="value">config value</param>
        /// <returns>if set is successful</returns>
        public bool Set(string path, string value)
        {
            var node = GetNode(path);
            if (node == null) return false;

            node.InnerText = value;
            Doc.Save(FileName);
            return true;
        }

        #endregion

        #region PrivateMethods

        private XmlNode GetNode(string path)
        {
            return Doc?.SelectSingleNode(StandardPath(path))?? null;
        }

        private static string StandardPath(string path)
        {
            path = path.Replace(":", "/");
            path = path.Replace(".", "/");
            return "Root/" + path;
        }

        private XmlNode AddChild(XmlNode parent, string childName)
        {
            var result = parent.SelectSingleNode(childName);
            if (result != null) return result;

            result = Doc.CreateElement(childName);
            parent.AppendChild(result);
            return result;
        }

        #endregion
    }
}
