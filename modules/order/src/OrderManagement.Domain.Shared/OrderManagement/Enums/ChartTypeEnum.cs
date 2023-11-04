using IFG.Core.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Domain.Shared
{
    public enum ChartTypeEnum
    {
        [EnumProperty(Description = "میله ای")]
        Bar = 1,
        [EnumProperty(Description = "ستونی")]
        Column = 2,
        [EnumProperty(Description = "دایره ای")]
        Pie = 3,
        [EnumProperty(Description = "خطی")]
        Line =4
    }
}
