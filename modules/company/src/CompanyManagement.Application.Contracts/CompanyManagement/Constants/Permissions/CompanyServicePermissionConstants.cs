using IFG.Core.Bases;
using Licence;

namespace CompanyManagement.Application.Contracts;

public class CompanyServicePermissionConstants
{
    public const string GetCustomersAndCars = ConstantInfo.ModuleOrder + ServiceIdentifier + "0001";
    public const string GetCustomersAndCars_DisplayName = "نمایش مشتری ها و خودروها";
    public const string InsertCompanyProduction = ConstantInfo.ModuleOrder + ServiceIdentifier + "0002";
    public const string InsertCompanyProduction_DisplayName = "ایجاد برنامه تولید خودرو ساز";
    public const string SubmitOrderInformations = ConstantInfo.ModuleOrder + ServiceIdentifier + "0003";
    public const string SubmitOrderInformations_DisplayName = "ثبت اطلاعات سفارش";
    public const string GetRecentCustomerAndOrder = ConstantInfo.ModuleOrder + ServiceIdentifier + "0004";
    public const string GetRecentCustomerAndOrder_DisplayName = "گرفتن اطلاعات مشتریان برای شرکت های خودرو ساز";


    public const string ServiceIdentifier = "0001";
    public const string ServiceDisplayName = "سرویس شرکت ها";
}
