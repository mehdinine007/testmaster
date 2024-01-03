using IFG.Core.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Domain.Shared.OrderManagement.Enums
{
    public enum AnnouncementStatusEnum
    {
        [EnumProperty(Description = "درانتظار انتشار")]
        Awaiting =1,
        [EnumProperty(Description = "درحال انتشار")]
        Publishing = 2,
        [EnumProperty(Description = "منقضی شده")]
        Expired = 3,
       

    }
}
