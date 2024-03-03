using IFG.Core.Attributes;

namespace WorkFlowManagement.Domain.Shared.WorkFlowManagement.Enums;

public enum FlowTypeEnum
{
 
    [EnumProperty(Description = "شروع")]
    Start = 1,
    [EnumProperty(Description = "پایان")]
    End = 2,
    [EnumProperty(Description = "مراحل")]
    State = 3,
}
