using Esale.Core.Bases;

namespace OrderManagement.Application.Contracts;

public class OrderAppServicePermissionConstants : BasePermissionConstants
{
    public const string CancelOrder = "000100030001";
    public const string CommitOrder = "000100030002";
    public const string GetCompaniesCustomerOrders = "000100030003";
    public const string GetCustomerInfoPriorityUser = "000100030004";
    public const string GetDetail = "000100030005";
    public const string InsertUserRejectionAdvocacyPlan = "000100030006";
    public const string UserRejectionStatus = "000100030007";

    public override string ModuleIdentifier => "0001";

    public override string ServiceIdentifier => "0003";

    public override string ServiceDisplayName => "سرویس مدیریت سفارشات";
}
