using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ayx.CSLibrary.DI.Test.DI
{
    public class DIClass
    {
        public int IntProperty { get; set; }
        public string StringProperty { get; set; }
        
        [AutoInject]
        public ClassB B { get; set; }

        public DIClass()
        {
            IntProperty = 1111;
            StringProperty = "ClassA";
        }
    }

    public class ClassB
    {
        public int IntProperty { get; set; }
        public string StringProperty { get; set; }

        [AutoInject]
        public ClassC C { get; set; }

        public ClassB()
        {
            IntProperty = 2222;
            StringProperty = "ClassB";
        }
    }

    public class ClassC
    {
        public int IntPorperty { get; set; }
        public string StringProperty { get; set; }

        public ClassC()
        {
            IntPorperty = 3333;
            StringProperty = "ClassC";
        }
    }

    public class ParamClass
    {
        public int IntProperty { get; set; }
        public string StringProperty { get; set; }

        public ParamClass(int i, string s)
        {
            IntProperty = i;
            StringProperty = s;
        }
    }
}
