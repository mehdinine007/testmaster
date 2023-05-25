using RestSharp;
using System;
using System.ComponentModel.DataAnnotations;

namespace OrderManagement.Application.OrderManagement.Utitlities
{
    public static class ApplicationHelper
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
