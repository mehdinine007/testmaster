using Esale.Core.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkFlowManagement.Domain.Shared.WorkFlowManagement.Enums
{
    public enum SecurityTypeEnum
    {   
        
        [EnumProperty(Description = "ویرایش")]
        Edit = 1,
        [EnumProperty(Description = "ارجاع")]
        Referral = 2,
        [EnumProperty(Description = "برگشت به درخواست کننده")]
        ReturnRequester= 3,
        [EnumProperty(Description = "برگشت به ارجاع دهنده")]
        ReturnToReturn = 4,
        [EnumProperty(Description = "ابطال")]
        Cancellation = 5,
        [EnumProperty(Description = "تایید")]
        Confirm = 6,
    }
}
