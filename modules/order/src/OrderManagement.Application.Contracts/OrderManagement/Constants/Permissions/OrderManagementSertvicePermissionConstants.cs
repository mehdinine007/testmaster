using IFG.Core.Bases;
using Licence;

namespace OrderManagement.Application.Contracts;

public class OrderAppServicePermissionConstants
{
    public const string CancelOrder = ConstantInfo.ModuleOrder + ServiceIdentifier + "0001";
    public const string CancelOrder_DisplayName = "لغو سفارش";
    public const string CommitOrder = ConstantInfo.ModuleOrder + ServiceIdentifier + "0002";
    public const string CommitOrder_DisplayName = "ثبت سفارش";
    public const string GetCompaniesCustomerOrders = ConstantInfo.ModuleOrder + ServiceIdentifier + "0003";
    public const string GetCompaniesCustomerOrders_DisplayName = "نمایش سفارشات مشتریان خودروساز";
    public const string GetCustomerInfoPriorityUser = ConstantInfo.ModuleOrder + ServiceIdentifier + "0004";
    public const string GetCustomerInfoPriorityUser_DisplayName = "نمایش اطلاعات الویت بندی مشتریان";
    public const string GetDetail = ConstantInfo.ModuleOrder + ServiceIdentifier + "0005";
    public const string GetDetail_DisplayName = "نمایش برنامه فروش";
    public const string InsertUserRejectionAdvocacyPlan = ConstantInfo.ModuleOrder + ServiceIdentifier + "0006";
    public const string InsertUserRejectionAdvocacyPlan_DisplayName = "ثبت انصراف از طرح";
    public const string UserRejectionStatus = ConstantInfo.ModuleOrder + ServiceIdentifier + "0007";
    public const string UserRejectionStatus_DisplayName = "وضعیت انصراف مشتری";
    public const string GetCustomerOrderList = ConstantInfo.ModuleOrder + ServiceIdentifier + "0008";
    public const string GetCustomerOrderList_DisplayName = "نمایش لیست سفارشات";
    public const string GetSaleDetailByUid = ConstantInfo.ModuleOrder + ServiceIdentifier + "0009";
    public const string GetSaleDetailByUid_DisplayName = "نمایش برنامه فروش توسط UID";
    public const string GetOrderDetailById = ConstantInfo.ModuleOrder + ServiceIdentifier + "0010";
    public const string GetOrderDetailById_DisplayName = "نمایش جزییات سفارش توسط شناسه";
    

    public const string ServiceIdentifier = "0003";
    public const string ServiceDisplayName = "سرویس مدیریت سفارشات";
}
