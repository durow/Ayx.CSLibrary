using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ayx.CSLibrary.DI
{
    public class DIContainer
    {
        private static DIContainer _default;
        public static DIContainer Default
        {
            get
            {
                if (_default == null)
                    _default = new DIContainer();
                return _default;
            }
        }

        public int Count { get { return injectInfoList.Count; } }

        private  List<InjectInfo> injectInfoList = new List<InjectInfo>();

        public  void Wire<Tfrom,Tto>(string token = "", Func<object> createFunc=null) where Tto:Tfrom
        {
            var info = InjectInfo.Create<Tfrom, Tto>(token, InjectType.Normal, createFunc);
            injectInfoList.Add(info);
        }

        public  void Wire<Tfrom>(Tfrom instance, string token = "")
        {
            var info = new InjectInfo
            {
                From = typeof(Tfrom),
                To = instance.GetType(),
                InjectType = InjectType.Singleton,
                instance = instance,
                Token = token,
            };
            injectInfoList.Add(info);
        }

        public  void WireSingleton<Tfrom,Tto>(string token = "", Func<object> createFunc = null) where Tto:Tfrom
        {
            var info = InjectInfo.Create<Tfrom, Tto>(token, InjectType.Singleton, createFunc);
            injectInfoList.Add(info);
        }

        public T GetInstance<T>(string token = "")
        {
            var temp = injectInfoList.Where(i => i.From == typeof(T));
            if (!string.IsNullOrEmpty(token))
                temp = temp.Where(i => i.Token == token);
            return (T)temp.FirstOrDefault().GetObject();
        }

        public T CheckInstance<T>(T item, string token = "")
        {
            if (item != null) return item;
            return GetInstance<T>(token);
        }

        public void Remove<T>(string token = "")
        {
            var find = false;
            var type = typeof(T);
            for (int i = 0; i < injectInfoList.Count; i++)
            {
                var item = injectInfoList[i];
                if(CheckEqual(item,type,token))
                {
                    find = true;
                    injectInfoList.Remove(item);
                    break;
                }
            }
            if (find)
                Remove<T>(token);
            else
                return;
        }

        public bool CheckEqual(InjectInfo info, Type t, string token)
        {
            if(string.IsNullOrEmpty(token))
            {
                if (info.From == t)
                    return true;
                else
                    return false;
            }
            if (info.From == t && info.Token == token)
                return true;
            else
                return false;
        }

        public bool CheckExist<Tfrom>(string token)
        {
            return false;
        }
    }
}
