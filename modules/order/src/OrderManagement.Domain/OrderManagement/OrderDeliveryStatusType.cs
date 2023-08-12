using System.ComponentModel.DataAnnotations;

namespace OrderManagement.Domain
{
    public enum OrderDeliveryStatusType
    {
        [Display(Name = "ثبت سفارش")] OrderRegistered = 1,
        [Display(Name = "الویت بندی")] Prioritization = 2,
        [Display(Name = "تعیین خودرو")] ProductDetermination = 3,
        [Display(Name = "ارسال به خودرو ساز")] SendingToManufaturer = 4,
        [Display(Name = "دریافت ردیف قرارداد")] ReceivingContractRowId = 5,
        [Display(Name = "تکمیل وجه")] ReceivingAmountCompleted = 6,
    }
}
