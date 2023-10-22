using Esale.Core.Bases;

namespace OrderManagement.Application.Contracts;

public class CapacityControlServicePermissionConstants : BasePermissionConstants
{
    public const string ValidationBySaleDetailUId = "000100020001";

    public override string ModuleIdentifier => "0001";

    public override string ServiceIdentifier => "0002";

    public override string ServiceDisplayName => "سرویس کنترل ظرفیت";
}
