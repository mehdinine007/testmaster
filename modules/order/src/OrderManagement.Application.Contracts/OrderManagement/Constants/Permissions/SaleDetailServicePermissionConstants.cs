using Esale.Core.Bases;

namespace OrderManagement.Application.Contracts;

public class SaleDetailServicePermissionConstants : BasePermissionConstants
{
    public const string Delete = "000100110001";
    public const string GetActiveList = "000100110002";
    public const string GetById = "000100110003";
    public const string GetSaleDetails = "000100110004";
    public const string Save = "000100110005";
    public const string Update = "000100110006";

    public override string ModuleIdentifier => "0001";

    public override string ServiceIdentifier => "0011";
}