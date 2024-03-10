using IFG.Core.Attributes;

namespace OrderManagement.Domain.Shared.OrderManagement.Enums;

public enum AnnouncementStatusEnum
{
    [EnumProperty(Description = "درانتظار انتشار")]
    Awaiting =1,
    [EnumProperty(Description = "درحال انتشار")]
    Publishing = 2,
    [EnumProperty(Description = "منقضی شده")]
    Expired = 3,
   

}
