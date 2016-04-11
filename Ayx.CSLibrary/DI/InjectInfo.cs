/*
 * Author:durow
 * Description:Record the inject information
*/
using System;

namespace Ayx.CSLibrary.DI
{
    public class InjectInfo
    {
        internal object instance;

        public Type From { get; internal set; }
        public Type To { get; internal set; }
        public string Token { get; internal set; }
        public Func<object> CreateFunction { get; internal set; }
        public InjectType InjectType { get; internal set; }

        public static InjectInfo Create<Tfrom, Tto>(string token = "", InjectType injectType = InjectType.Normal, Func<object> createFunc = null)
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
