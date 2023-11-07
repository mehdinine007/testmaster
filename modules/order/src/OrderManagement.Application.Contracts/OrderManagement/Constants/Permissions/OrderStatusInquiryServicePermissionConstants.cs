using IFG.Core.Bases;

namespace OrderManagement.Application.Contracts;

public class OrderStatusInquiryServicePermissionConstants: BasePermissionConstants
{
    public const string GetOrderDeilvery = "000200040001";
    public const string GetOrderDeilvery_DisplayName = "نمایش روند سفارش";

    public override string ModuleIdentifier => "0002";

    public override string ServiceIdentifier => "0004";

    public override string ServiceDisplayName => "سرویس استعلام وضعیت سفارشات";
}
