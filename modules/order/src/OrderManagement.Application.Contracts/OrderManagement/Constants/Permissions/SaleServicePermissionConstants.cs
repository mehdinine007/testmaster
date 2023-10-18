using Esale.Core.Bases;

namespace OrderManagement.Application.Contracts;

public class SaleServicePermissionConstants : BasePermissionConstants
{
    public const string GetPreSales = "000100060001";
    public const string UserValidationByBirthDate = "000100060002";
    public const string UserValidationByMobile = "000100060003";
}
