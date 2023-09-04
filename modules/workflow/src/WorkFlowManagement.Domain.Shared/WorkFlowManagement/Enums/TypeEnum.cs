using Esale.Core.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkFlowManagement.Domain.Shared.WorkFlowManagement.Enums
{
    public enum TypeEnum
    {
        [EnumProperty(Description = "نامشخص")]
        None = 0,
        [EnumProperty(Description = "ایجاد")]
        Create = 1,
        [EnumProperty(Description = "پیش نمایش")]
        PreView = 2,

    }
}
