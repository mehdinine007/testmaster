using IFG.Core.Bases;

namespace OrderManagement.Application.Contracts;

public class OrderAppServicePermissionConstants : BasePermissionConstants
{
    public const string CancelOrder = "000200030001";
    public const string CancelOrder_DisplayName = "لغو سفارش";
    public const string CommitOrder = "000200030002";
    public const string CommitOrder_DisplayName = "ثبت سفارش";
    public const string GetCompaniesCustomerOrders = "000200030003";
    public const string GetCompaniesCustomerOrders_DisplayName = "نمایش سفارشات مشتریان خودروساز";
    public const string GetCustomerInfoPriorityUser = "000200030004";
    public const string GetCustomerInfoPriorityUser_DisplayName = "نمایش اطلاعات الویت بندی مشتریان";
    public const string GetDetail = "000200030005";
    public const string GetDetail_DisplayName = "نمایش برنامه فروش";
    public const string InsertUserRejectionAdvocacyPlan = "000200030006";
    public const string InsertUserRejectionAdvocacyPlan_DisplayName = "ثبت انصراف از طرح";
    public const string UserRejectionStatus = "000200030007";
    public const string UserRejectionStatus_DisplayName = "وضعیت انصراف مشتری";
    public const string GetCustomerOrderList = "000200030008";
    public const string GetCustomerOrderList_DisplayName = "نمایش لیست سفارشات";
    public const string GetSaleDetailByUid = "000200030009";
    public const string GetSaleDetailByUid_DisplayName = "نمایش برنامه فروش توسط UID";
    public const string GetOrderDetailById = "000200030010";
    public const string GetOrderDetailById_DisplayName = "نمایش جزییات سفارش توسط شناسه";

    


    public override string ModuleIdentifier => "0002";

    public override string ServiceIdentifier => "0003";

    public override string ServiceDisplayName => "سرویس مدیریت سفارشات";
}
