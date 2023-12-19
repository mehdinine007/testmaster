using IFG.Core.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Domain.Shared.OrderManagement.Enums
{
    public enum DeviceEnum
    {
        [EnumProperty(Description = "Desktop")]
        Desktop = 0,
        [EnumProperty(Description = "Mobile")]
        Mobile = 1,
        [EnumProperty(Description = "Tablet")]
        Tablet = 2,

    }
}
