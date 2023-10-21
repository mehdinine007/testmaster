using Esale.Core.Bases;

namespace OrderManagement.Application.Contracts;

public class SaleSchemaServicePermissionConstants : BasePermissionConstants
{
    public const string GetById = "000100070001";
    public const string GetList = "000100070002";

    public override string ModuleIdentifier => "0001";

    public override string ServiceIdentifier => "0007";
}
