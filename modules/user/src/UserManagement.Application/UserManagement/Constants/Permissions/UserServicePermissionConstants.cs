using Esale.Core.Bases;

namespace UserManagement.Application.Constants;

public class UserServicePermissionConstants : BasePermissionConstants
{
    public const string GetUserProfile = "000400010001";
    public const string GetUserProfile_DisplayName = "نمایش پروفایل کاربر";
    public const string UpdateSecuritPolicy = "000400010002";
    public const string UpdateSecuritPolicy_DisplayName = "بروزرسانی قراردادهای امنیتی";

    public override string ModuleIdentifier => "0004";

    public override string ServiceIdentifier => "0001";

    public override string ServiceDisplayName => "سرویس مدیریت کاربران";
}
