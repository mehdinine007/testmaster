using IFG.Core.Bases;

namespace UserManagement.Application.Constants;

public class UserServicePermissionConstants : BasePermissionConstants
{
    public const string GetUserProfile = "000100010001";
    public const string GetUserProfile_DisplayName = "نمایش پروفایل کاربر";
    public const string UpdateSecuritPolicy = "000100010002";
    public const string UpdateSecuritPolicy_DisplayName = "بروزرسانی قراردادهای امنیتی";
    public const string ChangePassword = "000100010003";
    public const string ChangePassword_DisplayName = "تغییر رمز کاربری";

    public const string UpdateUserProfile = "000100010004";
    public const string UpdateUserProfile_DisplayName = "بروز رسانی حساب کاربری";

    public override string ModuleIdentifier => "0001";

    public override string ServiceIdentifier => "0001";

    public override string ServiceDisplayName => "سرویس مدیریت کاربران";
}
