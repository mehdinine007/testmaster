using IFG.Core.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Domain.Shared.OrderManagement.Enums
{
    public enum AgencyTypeEnum
    {
        [EnumProperty(Description = "فروش ")]
        Sale = 1,
        [EnumProperty(Description = "خدمات پس از فروش")]
        SaleSupoort = 2,
    
    }
}
