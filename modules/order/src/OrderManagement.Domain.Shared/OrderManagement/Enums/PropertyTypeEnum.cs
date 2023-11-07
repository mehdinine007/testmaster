using IFG.Core.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Domain.Shared
{
    public enum PropertyTypeEnum
    {
        [EnumProperty(Description = "حرفی")]
        Text = 1,
        [EnumProperty(Description = "عددی")]
        Number = 2,
        [EnumProperty(Description = "منطقی")]
        Boolean = 3,
        [EnumProperty(Description = "لیستی")]
        DropDown = 4,
        [EnumProperty(Description = "کدینگ")]
        Coding = 5
    }
}
