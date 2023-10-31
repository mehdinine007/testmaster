using IFG.Core.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace IFG.Core.Utility.Tools
{
    public static class EnumHelper
    {
        public static EnumValueList GetField(Enum _enum, int enumValue)
        {
            EnumPropertyAttribute attribute = _enum.GetType()
                .GetField(_enum.ToString())
                .GetCustomAttributes(typeof(EnumPropertyAttribute), false)
                .SingleOrDefault() as EnumPropertyAttribute;
            return attribute == null
                ? new EnumValueList()
                : new EnumValueList()
                {
                    Key = _enum.ToString(),
                    Value = enumValue,
                    DisplayTitle = attribute.Description
                };
        }

        public static string GetDescription(Enum _enum)
        {
            EnumPropertyAttribute attribute = _enum.GetType()
                .GetField(_enum.ToString())
                .GetCustomAttributes(typeof(EnumPropertyAttribute), false)
                .SingleOrDefault() as EnumPropertyAttribute;
            return attribute == null ? "" : attribute.Description;
        }

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

        public static string GetDisplayDescription(this Enum enumVal)
        {
            if (enumVal == null)
                return string.Empty;
            var type = enumVal.GetType();
            var memInfo = type.GetMember(enumVal.ToString());
            var attributes = memInfo[0].GetCustomAttributes(typeof(DisplayAttribute), false);
            var attr = attributes.Length > 0 ? (DisplayAttribute)attributes[0] : null;
            if (attr == null)
                return enumVal.ToString();
            return attr.Description;
        }

        public static List<T> ConvertStringToEnum<T>(string source)
            where T : Enum
        {
            if (!string.IsNullOrWhiteSpace(source))
            {
                var list = source.Split(',').ToList();
                return list.Select(x => (T)Enum.Parse(typeof(T),x,true)).ToList();
            }
            return null;
        }

    }
}
