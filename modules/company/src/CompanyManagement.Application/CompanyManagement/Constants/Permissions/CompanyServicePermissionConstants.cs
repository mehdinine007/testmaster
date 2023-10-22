using Esale.Core.Bases;

namespace CompanyManagement.Application.Contracts;

public class CompanyServicePermissionConstants : BasePermissionConstants
{
    public const string GetCustomersAndCars = "000200010001";
    public const string InsertCompanyProduction = "000200010002";
    public const string SubmitOrderInformations = "000200010003";

    public override string ModuleIdentifier => "0002";

    public override string ServiceIdentifier => "0001";

    public override string ServiceDisplayName => "سرویس شرکت ها";
}
