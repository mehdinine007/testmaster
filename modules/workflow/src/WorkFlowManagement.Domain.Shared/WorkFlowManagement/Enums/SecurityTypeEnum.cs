﻿using Esale.Core.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkFlowManagement.Domain.Shared.WorkFlowManagement.Enums
{
    public enum SecurityTypeEnum
    {   
        [EnumProperty(Description = "نامشخص")]
        None = 0,
        [EnumProperty(Description = "ویرایش")]
        Edit = 1,
        [EnumProperty(Description = "ارجاع")]
        Referral = 2,
        [EnumProperty(Description = "برگشت درخواست کننده")]
        ReturnRequester= 3,
        [EnumProperty(Description = "برگشت ارجاع دهنده")]
        ReturnReferrer = 4,
        [EnumProperty(Description = "ابطال")]
        Cancellation = 5,
    }
}
