using Esale.Core.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkFlowManagement.Domain.Shared.WorkFlowManagement.Enums
{
    public  enum OrganizationTypeEnum
    {
        [EnumProperty(Description = "نامشخص")]
        None = 0,
        [EnumProperty(Description = "تک پستی")]
        SinglePosition = 1,
        [EnumProperty(Description = "چند پستی")]
        MultiplePosition = 2,
    }
}
