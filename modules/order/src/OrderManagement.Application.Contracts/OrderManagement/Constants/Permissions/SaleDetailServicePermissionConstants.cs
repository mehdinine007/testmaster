using Esale.Core.Bases;

namespace OrderManagement.Application.Contracts;

public class SaleDetailServicePermissionConstants : BasePermissionConstants
{
    public const string Delete = "000100110001";
    public const string Delete_DisplayName = "حذف";
    public const string GetActiveList = "000100110002";
    public const string GetActiveList_DisplayName = "نمایش برنامه فروش های فعال";
    public const string GetById = "000100110003";
    public const string GetById_DisplayName = "نمایش برنامه فروش توسط شناسه";
    public const string GetSaleDetails = "000100110004";
    public const string GetSaleDetails_DisplayName = "نمایش برنامه فروش ها";
    public const string Save = "000100110005";
    public const string Save_DisplayName = "ثبت";
    public const string Update = "000100110006";
    public const string Update_DisplayName = "بروزرسانی";

    public override string ModuleIdentifier => "0001";

    public override string ServiceIdentifier => "0011";

    public override string ServiceDisplayName => "سرویس مدیریت برنامه فروش";
}