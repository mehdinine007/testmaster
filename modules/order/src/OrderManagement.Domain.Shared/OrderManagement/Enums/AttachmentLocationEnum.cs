using IFG.Core.Attributes;

namespace OrderManagement.Domain.Shared;

public enum AttachmentLocationEnum
{
    [EnumProperty(Description = "نامشخص")]
    None = 0,
    [EnumProperty(Description = "بالای صفحه")]
    TopPage = 1,    
}
