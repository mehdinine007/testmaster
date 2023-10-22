using Esale.Core.Bases;

namespace OrderManagement.Application.Contracts;

public class SaleSchemaServicePermissionConstants : BasePermissionConstants
{
    public const string GetById = "000100070001";
    public const string GetList = "000100070002";
    public const string Delete = "000100070003";
    public const string Add = "000100070004";
    public const string Update = "000100070005";
    public const string UploadFile = "000100070006";

    public override string ModuleIdentifier => "0001";

    public override string ServiceIdentifier => "0007";

    public override string ServiceDisplayName => "سرویس مدیریت بخش نامه های فروش";
}
