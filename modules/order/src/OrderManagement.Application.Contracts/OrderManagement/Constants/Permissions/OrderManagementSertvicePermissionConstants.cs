using Esale.Core.Bases;

namespace OrderManagement.Application.Contracts;

public class OrderAppServicePermissionConstants : BasePermissionConstants
{
    public const string CancelOrder = "000100030001";
    public const string CancelOrder_DisplayName = "لغو سفارش";
    public const string CommitOrder = "000100030002";
    public const string CommitOrder_DisplayName = "ثبت سفارش";
    public const string GetCompaniesCustomerOrders = "000100030003";
    public const string GetCompaniesCustomerOrders_DisplayName = "نمایش سفارشات مشتریان خودروساز";
    public const string GetCustomerInfoPriorityUser = "000100030004";
    public const string GetCustomerInfoPriorityUser_DisplayName = "نمایش اطلاعات الویت بندی مشتریان";
    public const string GetDetail = "000100030005";
    public const string GetDetail_DisplayName = "نمایش برنامه فروش";
    public const string InsertUserRejectionAdvocacyPlan = "000100030006";
    public const string InsertUserRejectionAdvocacyPlan_DisplayName = "ثبت انصراف از طرح";
    public const string UserRejectionStatus = "000100030007";
    public const string UserRejectionStatus_DisplayName = "وضعیت انصراف مشتری";

    public override string ModuleIdentifier => "0001";

    public override string ServiceIdentifier => "0003";

    public override string ServiceDisplayName => "سرویس مدیریت سفارشات";
}
