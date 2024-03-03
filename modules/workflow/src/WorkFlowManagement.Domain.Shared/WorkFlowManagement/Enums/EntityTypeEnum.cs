using IFG.Core.Attributes;

namespace WorkFlowManagement.Domain.Shared.WorkFlowManagement.Enums;


public enum EntityTypeEnum
{
    [EnumProperty(Description = "نامشخص")]
    None = 0,
    [EnumProperty(Description = "برنامه فروش")]
    SaleDetail = 1,
   
}
