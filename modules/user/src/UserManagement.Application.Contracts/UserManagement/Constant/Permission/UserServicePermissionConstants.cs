using IFG.Core.Bases;
using Licence;
namespace UserManagement.Application.Contracts.UserManagement.Constant;

public class UserServicePermissionConstants 
{
    public const string GetUserProfile = ConstantInfo.ModuleIUser + ServiceIdentifier + "0001";
    public const string GetUserProfile_DisplayName = "نمایش پروفایل کاربر";
    public const string UpdateSecuritPolicy = ConstantInfo.ModuleIUser + ServiceIdentifier + "0002";
    public const string UpdateSecuritPolicy_DisplayName = "بروزرسانی قراردادهای امنیتی";
    public const string ChangePassword = ConstantInfo.ModuleIUser + ServiceIdentifier + "0003";
    public const string ChangePassword_DisplayName = "تغییر رمز کاربری";
    public const string UpdateUserProfile = ConstantInfo.ModuleIUser + ServiceIdentifier + "0004";
    public const string UpdateUserProfile_DisplayName = "بروز رسانی حساب کاربری";
    public const string ChangeMobile = ConstantInfo.ModuleIUser + ServiceIdentifier + "0005";
    public const string ChangeMobile_DisplayName = "تغییر شماره موبایل";

    public const string ServiceIdentifier = "0001";
    public string ServiceDisplayName = "سرویس مدیریت کاربران";
}
