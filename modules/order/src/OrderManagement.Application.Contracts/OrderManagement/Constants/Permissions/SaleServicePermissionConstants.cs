using IFG.Core.Bases;
using Licence;

namespace OrderManagement.Application.Contracts;

public class SaleServicePermissionConstants 
{
    public const string GetPreSales = ConstantInfo.ModuleOrder + ServiceIdentifier + "0001";
    public const string GetPreSales_DisplayName ="";
    public const string UserValidationByBirthDate = ConstantInfo.ModuleOrder + ServiceIdentifier + "0002";
    public const string UserValidationByBirthDate_DisplayName = "اعتبارسنجی تاریخ تولد کاربر";
    public const string UserValidationByMobile = ConstantInfo.ModuleOrder + ServiceIdentifier + "0003";
    public const string UserValidationByMobile_DisplayName = "اعتبارسنجی موبایل کاربر";

    public const string ServiceIdentifier = "0006";
    public const string ServiceDisplayName = "سرویس مدیریت نوع فروش";
}
