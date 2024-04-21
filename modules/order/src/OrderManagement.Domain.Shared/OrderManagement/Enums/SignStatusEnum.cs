using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Domain.Shared.OrderManagement.Enums
{
    public enum SignStatusEnum
    {
        [Display(Name = "درحال آماده سازی قرارداد")]
        PreparingContract = 1,
        [Display(Name = "آماده امضا")]
        ReadyForSignature = 2,
        [Display(Name = "منتظرامضا")]
        AwaitingSignature = 3,
        [Display(Name = "امضاشده")]
        Signed = 4,
        [Display(Name = "منقضی شده")]
        Expired = 5,
    }
}
