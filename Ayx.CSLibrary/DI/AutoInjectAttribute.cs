using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ayx.CSLibrary.DI
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class AutoInjectAttribute:Attribute
    {
        public string Token { get; private set; }
        public AutoInjectAttribute(string token="")
        {
            Token = token;
        }
    }
}
