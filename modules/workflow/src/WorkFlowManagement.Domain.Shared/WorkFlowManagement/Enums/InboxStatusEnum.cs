using Esale.Core.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkFlowManagement.Domain.Shared.WorkFlowManagement.Enums
{
    public enum InboxStatusEnum
    {
        [EnumProperty(Description = "فعال")]
        Active = 1,
        [EnumProperty(Description = "ارسال شده")]
        Posted = 2,
        [EnumProperty(Description = "آرشیو")]
        Archive = 3
       ,
    }
}
