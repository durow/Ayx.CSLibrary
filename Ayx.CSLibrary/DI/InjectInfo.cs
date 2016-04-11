using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ayx.CSLibrary.DI
{
    public class InjectInfo
    {
        public Type From { get; internal set; }
        public Type To { get; internal set; }
        public string Token { get; internal set; }
        public Func<object> CreateFunction { get; internal set; }
        internal object instance;
        public InjectType InjectType { get; internal set; }

        public static InjectInfo Create<Tfrom,Tto>(string token = "", InjectType injectType = InjectType.Normal, Func<object> createFunc = null)
            where Tto:Tfrom
        {
            return new InjectInfo
            {
                From = typeof(Tfrom),
                To = typeof(Tto),
                Token = token,
                InjectType = injectType,
                CreateFunction = createFunc,
            };
        }
        public object GetObject()
        {
            if (InjectType == InjectType.Singleton)
            {
                if (instance == null)
                    instance = CreateInstance();
                return instance;
            }
            return CreateInstance();
        }

        private object CreateInstance()
        {
            if (CreateFunction != null)
            {
                return CreateFunction();
            }
            return Activator.CreateInstance(To);
        }
    }
}
