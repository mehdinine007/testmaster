using Esale.Core.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanelManagement.Domain.Shared.AdminPanelManagement.Enum
{
    public enum ConditionTypeEnum
    {

        [EnumProperty(Description = "حرفی")]
        String = 1,
        [EnumProperty(Description = "عددی")]
        Numerical = 2,
        [EnumProperty(Description = "لیستی")]
        DropDown = 3,
        [EnumProperty(Description = "کدینگ")]
        CodingApi = 4,
        [EnumProperty(Description = "تاریخ")]
        Date = 5,
    }
}
