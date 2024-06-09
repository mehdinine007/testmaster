using IFG.Core.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Domain.Shared.OrderManagement.Enums
{
    public enum SeasonTypeEnum
    {

        [EnumProperty(Description = "بهار")]
        Spring = 1,
        [EnumProperty(Description = "تابستان")]
        Summer = 2,
        [EnumProperty(Description = "پاییز")]
        Autumn = 3,
        [EnumProperty(Description = "زمستان")]
        Winter = 4

    }
}
