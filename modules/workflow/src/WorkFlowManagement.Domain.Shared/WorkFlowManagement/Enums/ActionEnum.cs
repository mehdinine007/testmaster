using IFG.Core.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkFlowManagement.Domain.Shared.WorkFlowManagement.Enums
{
    public enum ActionEnum
    {

        [EnumProperty(Description = "پیش نویس")]
        Draft = 1,
        [EnumProperty(Description = "ارجاع")]
        Refrence = 2,
        [EnumProperty(Description = "تایید")]
        Confirm = 3,

        [EnumProperty(Description = "ابطال")]
        Reject = 4,
        [EnumProperty(Description = "برگشت به ارجاع دهنده")]
        ReturnToReturn = 5,
        [EnumProperty(Description = "برگشت به  درخواست کننده")]
        ReturnRequester = 6,
        [EnumProperty(Description = "درجریان")]
        During = 7,
        [EnumProperty(Description = "شروع")]
        Start = 8,

    }
}
