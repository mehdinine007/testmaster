using Esale.Core.Bases;

namespace OrderManagement.Application.Contracts;

public class SaleSchemaServicePermissionConstants : BasePermissionConstants
{
    public const string GetById = "000100070001";
    public const string GetById_DisplayName = "نمایش بخشنامه توسط شناسه";
    public const string GetList = "000100070002";
    public const string GetList_DisplayName = "نمایش بخشنامه ها";
    public const string Delete = "000100070003";
    public const string Delete_DisplayName = "حذف";
    public const string Add = "000100070004";
    public const string Add_DisplayName = "ثبت";
    public const string Update = "000100070005";
    public const string Update_DisplayName = "بروزرسانی";
    public const string UploadFile = "000100070006";
    public const string UploadFile_DisplayName = "بروزرسانی فایل";

    public override string ModuleIdentifier => "0001";

    public override string ServiceIdentifier => "0007";

    public override string ServiceDisplayName => "سرویس مدیریت بخش نامه های فروش";
}
