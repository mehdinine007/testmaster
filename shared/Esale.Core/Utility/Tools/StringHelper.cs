using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Utility.Tools
{
    public static class StringHelper
    {
        public static string Repeat (string _str,int length)
        {
            return string.Concat(Enumerable.Repeat("0", length - _str.Length)) + _str;
        }
    }
}
