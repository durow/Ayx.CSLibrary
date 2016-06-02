/*
 * Author:durow
 * Date:2015.12.30
 */

using Ayx.CSLibrary.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;

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

        public void WireVM<TView, TViewModel>(string token = "", Func<object> createFunc = null)
            where TView : UIElement where TViewModel : INotifyPropertyChanged
        {
            var info = InjectInfo.Create<TView, TViewModel>(token,InjectType.ViewModel, createFunc);
            injectInfoList.Add(info);
        }

        public T Get<T>(string token = "")
        {
            return (T)Get(typeof(T), token);
        }

        public object Get(Type fromType, string token = "")
        {
            var resultList = GetInjectionInfo(fromType,token);
            var result = resultList.FirstOrDefault().GetObject();

            foreach (var property in fromType.GetProperties())
            {
                if (property.PropertyType.IsValueType)
                    continue;
                var attr = AttributeHelper.GetAttribute<AutoInjectAttribute>(property);
                if (attr == null)
                    continue;

                var valueType = property.PropertyType;
                var propertyValue = Get(valueType, attr.Token);
                property.SetValue(result, propertyValue,null);
            }

            return result;
        }

        public object GetVM<TView>(string token = "")
        {
            return GetInjectionInfo(typeof(TView), token)
                .Where(p => p.InjectType == InjectType.ViewModel)
                .FirstOrDefault()
                .GetObject();
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

        public bool CheckExist<Tfrom>(string token = "")
        {
            var resultList = GetInjectionInfo<Tfrom>(token);
            return resultList.Any();
        }

        #region Private Methods

        private IEnumerable<InjectInfo> GetInjectionInfo(Type type, string token = "")
        {
            var result = injectInfoList.Where(p => p.From == type);
            if (!string.IsNullOrEmpty(token))
            {
                result = result.Where(p => p.Token == token);
            }
            return result;
        }

        private bool CheckEqual(InjectInfo info, Type t, string token)
        {
            if (string.IsNullOrEmpty(token))
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

        #endregion
    }
}
