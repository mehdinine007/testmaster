using Esale.Core.Bases;

namespace OrderManagement.Application.Contracts;

public class CapacityControlServicePermissionConstants : BasePermissionConstants
{
    public const string ValidationBySaleDetailUId = "000200020001";
    public const string ValidationBySaleDetailUId_DisplayName = "اعتبارسنجی توسط UID برنامه فروش";

    public override string ModuleIdentifier => "0002";

    public override string ServiceIdentifier => "0002";

    public override string ServiceDisplayName => "سرویس کنترل ظرفیت";
}
