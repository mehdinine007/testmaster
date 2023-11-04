using IFG.Core.Bases;

namespace OrderManagement.Application.Contracts;

public class ColorServicePermissionConstants : BasePermissionConstants
{
    public const string Delete = "000200100001";
    public const string Delet_DisplayName = "حذف";
    public const string GetAllColors = "000200100002";
    public const string GetAllColors_DisplayName = "نمایش همه رنگ ها";
    public const string GetColors = "000200100003";
    public const string GetColors_DisplayName = "نمایش رنگ";
    public const string Save = "000200100004";
    public const string Save_DisplayName = "ثبت";
    public const string Update = "000200100005";
    public const string Update_DisplayName = "بروزرسانی";

    public override string ModuleIdentifier => "0002";

    public override string ServiceIdentifier => "0010";

    public override string ServiceDisplayName => "سرویس مدیریت رنگ";
}
