using Esale.Core.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Domain.Shared.OrderManagement.Enums
{
    public enum SaleTypeEnum
    {
        [EnumProperty(Description = "خودروهای وارداتی")]
        saleauto = 1,
        [EnumProperty(Description = "خودروهای داخلی")]
        esalecar =2
    }
}
