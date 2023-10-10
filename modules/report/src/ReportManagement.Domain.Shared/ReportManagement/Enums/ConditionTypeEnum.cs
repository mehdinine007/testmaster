using Esale.Core.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportManagement.Domain.Shared.ReportManagement.Enums
{
    public enum ConditionTypeEnum
    {
        [EnumProperty(Description = "حرفی")]
        Word = 1,
        [EnumProperty(Description = "عددی")]
        Numerical = 2,
        [EnumProperty(Description = "لیستی")]
        List = 3,
        [EnumProperty(Description = "کدینگ")]
        Coding = 4,
    }
}
