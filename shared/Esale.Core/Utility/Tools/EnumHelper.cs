using Esale.Core.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Esale.Core.Utility.Tools
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

    }
}
