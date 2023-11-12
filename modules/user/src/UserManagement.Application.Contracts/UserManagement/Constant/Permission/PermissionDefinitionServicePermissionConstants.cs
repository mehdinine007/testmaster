using IFG.Core.Bases;

namespace UserManagement.Application.Contracts.UserManagement.Constant;

public class PermissionDefinitionServicePermissionConstants : BasePermissionConstants
{
    public const string GetById = "000100040001";
    public const string GetById_DisplayName = "نمایش تعریف مجوز توسط شناسه";
    public const string GetList = "000100040002";
    public const string GetList_DisplayName = "نمایش تعریف مجوز";
    public const string Delete = "000100040003";
    public const string Delete_DisplayName = "حذف";
    public const string Add = "000100040004";
    public const string Add_DisplayName = "ثبت";
    public const string Update = "000100040005";
    public const string Update_DisplayName = "بروزرسانی";

    public override string ModuleIdentifier => "0001";

    public override string ServiceIdentifier => "0004";

    public override string ServiceDisplayName => "سرویس تعریف مجوز";
}
