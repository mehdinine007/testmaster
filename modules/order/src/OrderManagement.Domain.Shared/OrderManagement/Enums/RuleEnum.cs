using IFG.Core.Attributes;

namespace OrderManagement.Domain.Shared.OrderManagement.Enums;

public enum RuleEnum
{
    [EnumProperty(Description = "افزودن")]
    Add = 1,
    [EnumProperty(Description = "حذف")]
    Delete = 2,
    [EnumProperty(Description = "ویرایش")]
    Update = 3,

}
