using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OrderManagement.Application.Helpers
{
    public static class ApplicationHelper
    {
        public static string GetDisplayName(this Enum enumVal)
        {
            if (enumVal == null)
                return string.Empty;
            var type = enumVal.GetType();
            var memInfo = type.GetMember(enumVal.ToString());
            var attributes = memInfo[0].GetCustomAttributes(typeof(DisplayAttribute), false);
            var attr = attributes.Length > 0 ? (DisplayAttribute)attributes[0] : null;
            if (attr == null)
                return enumVal.ToString();
            return attr.Name;
        }

        public static string ConcatErrorMessages(this List<Exception> exceptions)
        {
            var msg = new StringBuilder();
            exceptions.ForEach(x => msg.AppendLine(x.Message));
            return msg.ToString();
        }
    }
}
