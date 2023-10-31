using IFG.Core.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkFlowManagement.Domain.Shared.WorkFlowManagement.Enums
{
    public enum StatusEnum
    {
        [EnumProperty(Description = "مقداردهی اولیه")]
        Initialize = 1,
        [EnumProperty(Description = "نهایی")]
        finalize = 2,
        [EnumProperty(Description = "درحال اجرا")]
        Runing = 3,
        [EnumProperty(Description = "خاتمه دادن")]
        Terminate = 4,
    }
}
