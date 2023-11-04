using IFG.Core.Bases;

namespace CompanyManagement.Application.Contracts;

public class CompanyServicePermissionConstants : BasePermissionConstants
{
    public const string GetCustomersAndCars = "000300010001";
    public const string GetCustomersAndCars_DisplayName = "نمایش مشتری ها و خودروها";
    public const string InsertCompanyProduction = "000300010002";
    public const string InsertCompanyProduction_DisplayName = "ایجاد برنامه تولید خودرو ساز";
    public const string SubmitOrderInformations = "000300010003";
    public const string SubmitOrderInformations_DisplayName = "ثبت اطلاعات سفارش";
    public const string GetCustomer = "000300010004";
    public const string GetCustomer_DisplayName = "گرفتن اطلاعات مشتریان برای شرکت های خودرو ساز";


    public override string ModuleIdentifier => "0003";

    public override string ServiceIdentifier => "0001";

    public override string ServiceDisplayName => "سرویس شرکت ها";
}
