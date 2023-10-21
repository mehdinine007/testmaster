using Esale.Core.Bases;

namespace OrderManagement.Application.Contracts;

public class ColorServicePermissionConstants : BasePermissionConstants
{
    public const string Delete = "000100100001";
    public const string GetAllColors = "000100100002";
    public const string GetColors = "000100100003";
    public const string Save = "000100100004";
    public const string Update = "000100100005";

    public override string ModuleIdentifier => "0001";

    public override string ServiceIdentifier => "0010";
}
