using System.ComponentModel.DataAnnotations;

namespace OrderManagement.Domain.Shared
{
    public enum OrderDeliveryStatusType
    {
        [Display(Name = "ثبت سفارش", Description = "شماره سفارش {0}")]
        OrderRegistered = 1,
        [Display(Name = "الویت بندی", Description = "")]
        Prioritization = 2,
        [Display(Name = "تعیین خودرو", Description = "خودرو شما {0} می باشد")]
        ProductDetermination = 3,
        [Display(Name = "ارسال به خودرو ساز", Description = "ارسال به خودرو ساز ({0})")]
        SendingToManufaturer = 4,
        [Display(Name = "دریافت ردیف قرارداد", Description = "شماره قرارداد {0}")]
        ReceivingContractRowId = 5,
        [Display(Name = "تکمیل وجه", Description = "مبلغ پرداختی {0}")]
        ReceivingAmountCompleted = 6,
    }
}


