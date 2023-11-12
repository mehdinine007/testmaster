using IFG.Core.Bases;

namespace UserManagement.Application.Contracts.UserManagement.Constant;

public class RolePermissionServicePermissionConstants : BasePermissionConstants
{
    public const string GetById = "000100030001";
    public const string GetById_DisplayName = "نمایش بانک ها توسط شناسه";
    public const string GetList = "000100030002";
    public const string GetList_DisplayName = "نمایش بانک ها";
    public const string Delete = "000100030003";
    public const string Delete_DisplayName = "حذف";
    public const string Add = "000100030004";
    public const string Add_DisplayName = "ثبت";
    public const string Update = "000100030005";
    public const string Update_DisplayName = "بروزرسانی";

    public override string ModuleIdentifier => "0001";

    public override string ServiceIdentifier => "0003";

    public override string ServiceDisplayName => "سرویس مجوز نقش ها";
}
