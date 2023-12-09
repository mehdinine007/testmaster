using IFG.Core.Bases;
using Licence;
namespace UserManagement.Application.Contracts.UserManagement.Constant;

public class PermissionDefinitionServicePermissionConstants 
{
    public const string GetById = ConstantInfo.ModuleIUser + ServiceIdentifier + "0001";
    public const string GetById_DisplayName = "نمایش تعریف مجوز توسط شناسه";
    public const string GetList = ConstantInfo.ModuleIUser + ServiceIdentifier + "0002";
    public const string GetList_DisplayName = "نمایش تعریف مجوز";
    public const string Delete = ConstantInfo.ModuleIUser + ServiceIdentifier + "0003";
    public const string Delete_DisplayName = "حذف";
    public const string Add = ConstantInfo.ModuleIUser + ServiceIdentifier + "0004";
    public const string Add_DisplayName = "ثبت";
    public const string Update = ConstantInfo.ModuleIUser + ServiceIdentifier + "0005";
    public const string Update_DisplayName = "بروزرسانی";

    public const string ServiceIdentifier = "0004";
    public const string ServiceDisplayName = "سرویس تعریف مجوز";
}
