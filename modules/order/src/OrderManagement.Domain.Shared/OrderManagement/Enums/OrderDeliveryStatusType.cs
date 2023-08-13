using System.ComponentModel.DataAnnotations;

namespace OrderManagement.Domain.Shared
{
    public enum OrderDeliveryStatusType
    {
        [Display(Name = "ثبت سفارش", Description = "لورم اپیسوم")]
        OrderRegistered = 1,
        [Display(Name = "الویت بندی", Description = "لورم اپیسوم")]
        Prioritization = 2,
        [Display(Name = "تعیین خودرو", Description = "لورم اپیسوم")]
        ProductDetermination = 3,
        [Display(Name = "ارسال به خودرو ساز", Description = "لورم اپیسوم")]
        SendingToManufaturer = 4,
        [Display(Name = "دریافت ردیف قرارداد", Description = "لورم اپیسوم")]
        ReceivingContractRowId = 5,
        [Display(Name = "تکمیل وجه", Description = "لورم اپیسوم")]
        ReceivingAmountCompleted = 6,
    }
}


