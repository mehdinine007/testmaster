using IFG.Core.Bases;

namespace OrderManagement.Application.Contracts;

public class SaleDetailServicePermissionConstants : BasePermissionConstants
{
    public const string Delete = "000200110001";
    public const string Delete_DisplayName = "حذف";
    public const string GetActiveList = "000200110002";
    public const string GetActiveList_DisplayName = "نمایش برنامه فروش های فعال";
    public const string GetById = "000200110003";
    public const string GetById_DisplayName = "نمایش برنامه فروش توسط شناسه";
    public const string GetSaleDetails = "000200110004";
    public const string GetSaleDetails_DisplayName = "نمایش برنامه فروش ها";
    public const string Save = "000200110005";
    public const string Save_DisplayName = "ثبت";
    public const string Update = "000200110006";
    public const string Update_DisplayName = "بروزرسانی";

    public override string ModuleIdentifier => "0002";

    public override string ServiceIdentifier => "0011";

    public override string ServiceDisplayName => "سرویس مدیریت برنامه فروش";
}