using IFG.Core.Bases;
using Licence;

namespace OrderManagement.Application.Contracts;

public class OrderStatusInquiryServicePermissionConstants
{
    public const string GetOrderDeilvery = ConstantInfo.ModuleOrder + ServiceIdentifier + "0001";
    public const string GetOrderDeilvery_DisplayName = "نمایش روند سفارش";

    public const string ServiceIdentifier = "0004";
    public const string ServiceDisplayName = "سرویس استعلام وضعیت سفارشات";
}
