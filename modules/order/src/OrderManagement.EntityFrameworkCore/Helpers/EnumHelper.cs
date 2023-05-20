using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.EfCore.Helpers
{
    public static class EnumHelper
    {
        public static string GetDisplayName(this Enum? enumVal)
        {
            if (enumVal == null)
                return string.Empty;
            var type = enumVal.GetType();
            var memInfo = type.GetMember(enumVal.ToString());
            var attributes = memInfo[0].GetCustomAttributes(typeof(DisplayAttribute), false);
            var attr = (attributes.Length > 0) ? (DisplayAttribute)attributes[0] : null;
            if (attr == null)
                return enumVal.ToString();
            return attr.Name;
        }
    }
}
