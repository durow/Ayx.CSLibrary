using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ayx.CSLibrary.Utility.Config
{
    public interface IConfigFile
    {
        string this[string path] { get; set; }

        T Get<T>(string path);

        void Set(string path, string value);

        void AddOrSet(string path, string value);

        void Delete(string path);

        void CreateEmpty(string filename);
    }
}
