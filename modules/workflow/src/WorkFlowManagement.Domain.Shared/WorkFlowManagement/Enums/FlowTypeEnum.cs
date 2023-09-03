using Esale.Core.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkFlowManagement.Domain.Shared.WorkFlowManagement.Enums
{
    public enum FlowTypeEnum
    {
     
        [EnumProperty(Description = "شروع")]
        Start = 1,
        [EnumProperty(Description = "پایان")]
        End = 2,
        [EnumProperty(Description = "مراحل")]
        State = 3,
    }
}
