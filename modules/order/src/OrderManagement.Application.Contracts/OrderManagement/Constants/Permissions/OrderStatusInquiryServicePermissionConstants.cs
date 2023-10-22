using Esale.Core.Bases;

namespace OrderManagement.Application.Contracts;

public class OrderStatusInquiryServicePermissionConstants: BasePermissionConstants
{
    public const string GetOrderDeilvery = "000100040001";
    public const string GetOrderDeilvery_DisplayName = "نمایش روند سفارش";

    public override string ModuleIdentifier => "0001";

    public override string ServiceIdentifier => "0004";

    public override string ServiceDisplayName => "سرویس استعلام وضعیت سفارشات";
}
