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

        public static string GenerateTreeNodePath(string maxCode,string parentCode, int codeLength)
        {
            if (!string.IsNullOrWhiteSpace(maxCode))
                maxCode = (Convert.ToInt32(maxCode.Substring(maxCode.Length - codeLength)) + 1).ToString();
            else maxCode = "1";
            maxCode = parentCode + StringHelper.Repeat(maxCode, codeLength);
            return maxCode;
        }
    }
}
