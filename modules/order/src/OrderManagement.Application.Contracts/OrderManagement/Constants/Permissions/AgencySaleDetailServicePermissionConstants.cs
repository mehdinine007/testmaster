using IFG.Core.Bases;

namespace OrderManagement.Application.Contracts;

public class AgencySaleDetailServicePermissionConstants : BasePermissionConstants
{
    public const string Delete = "000200090001";
    public const string Delete_DisplayName = "حذف";
    public const string GetAgencySaleDetail = "000200090002";
    public const string GetAgencySaleDetail_DisplayName = "نمایش نمایندگی های برنامه فروش";
    public const string Save = "000200090003";
    public const string Save_DisplayName = "ذخیره";

    public override string ModuleIdentifier => "0002";

    public override string ServiceIdentifier => "0009";

    public override string ServiceDisplayName => "سرویس برنامه فروش نمایندگی";
}
