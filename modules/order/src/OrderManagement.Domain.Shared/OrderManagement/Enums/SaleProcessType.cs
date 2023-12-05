using System.ComponentModel.DataAnnotations;

namespace OrderManagement.Domain.Shared;

public enum SaleProcessType
{
    [Display(Name = "فروش عادی")]
    RegularSale = 0,

    [Display(Name = "فروش مستقیم")]
    DirectSale = 1,

    [Display(Name = "فروش با کد پیگیری")]
    SaleWithTrackingCode = 2
}