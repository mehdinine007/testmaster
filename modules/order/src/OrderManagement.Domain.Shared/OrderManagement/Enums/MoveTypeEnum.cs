using Microsoft.EntityFrameworkCore.Update.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace OrderManagement.Domain.Shared.OrderManagement.Enums
{
    public enum MoveTypeEnum
    {
        [Display(Name = "بالا")]
        Up = 1,

        [Display(Name = "پایین")]
        Down = 2

    }
}
