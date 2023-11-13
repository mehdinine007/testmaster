using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanelManagement.Domain.Shared.AdminPanelManagement.Enum
{
    public enum GenderTypeEnum
    {
        [Display(Name = "مرد")]
        Male = 0,

        [Display(Name = "زن")]
        Female = 1
    }
}
