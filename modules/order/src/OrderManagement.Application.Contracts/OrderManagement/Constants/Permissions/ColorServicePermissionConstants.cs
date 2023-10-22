using Esale.Core.Bases;

namespace OrderManagement.Application.Contracts;

public class ColorServicePermissionConstants : BasePermissionConstants
{
    public const string Delete = "000100100001";
    public const string Delet_DisplayName = "حذف";
    public const string GetAllColors = "000100100002";
    public const string GetAllColors_DisplayName = "نمایش همه رنگ ها";
    public const string GetColors = "000100100003";
    public const string GetColors_DisplayName = "نمایش رنگ";
    public const string Save = "000100100004";
    public const string Save_DisplayName = "ثبت";
    public const string Update = "000100100005";
    public const string Update_DisplayName = "بروزرسانی";

    public override string ModuleIdentifier => "0001";

    public override string ServiceIdentifier => "0010";

    public override string ServiceDisplayName => "سرویس مدیریت رنگ";
}
