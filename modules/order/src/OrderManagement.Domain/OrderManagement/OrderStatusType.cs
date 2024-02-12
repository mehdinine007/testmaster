using System.ComponentModel.DataAnnotations;

namespace OrderManagement.Domain
{
    public enum OrderStatusType
    {
        // Important *********
        // in case u add new status just add a migration then the base table will be automatically update

        [Display(Name = "ثبت سفارش اولیه با موفقیت انجام شد")]
        RecentlyAdded = 10,

        [Display(Name = "انصراف داده شده")]
        Canceled = 20,
        [Display(Name = "انتخاب نشده اید")]
        loser = 30,
        [Display(Name = "برنده شده اید")]
        Winner = 40,
        [Display(Name = "انصراف کلی از اولیت بندی")]
        FullCancel = 50,
        [Display(Name = "عدم تخصیصی")]
        CancelledBySystem = 60,
        [Display(Name = "پرداخت با موفقیت انجام شد")]
        PaymentSucceeded = 70,
        [Display(Name = "پرداخت ناموفق")]
        PaymentNotVerified = 80,
        [Display(Name = "Reservation1")]
        Reservation1 = 90,
        [Display(Name = "Reservation2")]
        Reservation2 = 91,
        [Display(Name = "Reservation3")]
        Reservation3 = 92,
    }
}
