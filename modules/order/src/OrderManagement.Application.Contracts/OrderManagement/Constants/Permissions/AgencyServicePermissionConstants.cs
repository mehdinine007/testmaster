using IFG.Core.Bases;

namespace OrderManagement.Application.Contracts;

public class AgencyServicePermissionConstants : BasePermissionConstants
{
    public const string Delete = "000200080001";
    public const string Delete_DisplayName = "حذف";
    public const string GetAgencies = "000200080002";
    public const string GetAgencies_DisplayName = "نمایش نمایندگی ها";
    public const string Save = "000200080003";
    public const string Save_DisplayName = "ثبت";
    public const string Update = "000200080004";
    public const string Update_DisplayName = "بروزرسانی";

    public override string ModuleIdentifier => "0002";

    public override string ServiceIdentifier => "0008";

    public override string ServiceDisplayName => "سرویس نمایندگان";
}