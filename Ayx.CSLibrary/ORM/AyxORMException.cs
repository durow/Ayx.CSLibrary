using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ayx.CSLibrary.ORM
{
    public class AyxORMException:Exception
    {
        public AyxORMException()
        { }

        public AyxORMException(string message):base(message)
        { }
    }
}
