using Esale.Core.Bases;

namespace OrderManagement.Application.Contracts;

public class AgencyServicePermissionConstants : BasePermissionConstants
{
    public const string Delete = "000100080001";
    public const string Delete_DisplayName = "حذف";
    public const string GetAgencies = "000100080002";
    public const string GetAgencies_DisplayName = "نمایش نمایندگی ها";
    public const string Save = "000100080003";
    public const string Save_DisplayName = "ثبت";
    public const string Update = "000100080004";
    public const string Update_DisplayName = "بروزرسانی";

    public override string ModuleIdentifier => "0001";

    public override string ServiceIdentifier => "0008";

    public override string ServiceDisplayName => "سرویس نمایندگان";
}