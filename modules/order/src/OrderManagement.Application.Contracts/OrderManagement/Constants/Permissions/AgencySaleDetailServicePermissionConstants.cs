using Esale.Core.Bases;

namespace OrderManagement.Application.Contracts;

public class AgencySaleDetailServicePermissionConstants : BasePermissionConstants
{
    public const string Delete = "000100090001";
    public const string GetAgencySaleDetail = "000100090002";
    public const string Save = "000100090003";

    public override string ModuleIdentifier => "0001";

    public override string ServiceIdentifier => "0009";
}
