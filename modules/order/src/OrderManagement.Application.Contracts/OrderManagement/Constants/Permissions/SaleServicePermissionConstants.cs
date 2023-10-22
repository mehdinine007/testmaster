using Esale.Core.Bases;

namespace OrderManagement.Application.Contracts;

public class SaleServicePermissionConstants : BasePermissionConstants
{
    public const string GetPreSales = "000100060001";
    public const string GetPreSales_DisplayName ="";
    public const string UserValidationByBirthDate = "000100060002";
    public const string UserValidationByBirthDate_DisplayName = "اعتبارسنجی تاریخ تولد کاربر";
    public const string UserValidationByMobile = "000100060003";
    public const string UserValidationByMobile_DisplayName = "اعتبارسنجی موبایل کاربر";

    public override string ModuleIdentifier => "0001";

    public override string ServiceIdentifier => "0006";

    public override string ServiceDisplayName => "سرویس مدیریت نوع فروش";
}
