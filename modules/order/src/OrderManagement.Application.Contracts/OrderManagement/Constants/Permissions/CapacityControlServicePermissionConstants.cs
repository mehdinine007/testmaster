using IFG.Core.Bases;
using Licence;

namespace OrderManagement.Application.Contracts;

public class CapacityControlServicePermissionConstants
{
    public const string ValidationBySaleDetailUId = ConstantInfo.ModuleOrder + ServiceIdentifier + "0001";
    public const string ValidationBySaleDetailUId_DisplayName = "اعتبارسنجی توسط UID برنامه فروش";

    public const string ServiceIdentifier = "0002";
    public const string ServiceDisplayName = "سرویس کنترل ظرفیت";
}
