using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Domain.Shared.OrderManagement.Enums
{
    public enum ESaleTypeEnums
    {
        [Display(Name = "فروش عادی")]
        NormalSale = 1,

        [Display(Name = "فروش جوانی")]
        YouthSale = 2,

        [Display(Name = "فروش فرسوده")]
        WornOutSale = 3
    }
}
