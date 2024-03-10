using IFG.Core.Attributes;

namespace ReportManagement.Domain.Shared.ReportManagement.Enums;

public enum WidgetTypeEnum
{
    [EnumProperty(Description = "میله ای")]
    Bar = 1,
    [EnumProperty(Description = "ستونی")]
    Column = 2,
    [EnumProperty(Description = "دایره ای")]
    Pie = 3,
    [EnumProperty(Description = "خطی")]
    Line = 4,
    [EnumProperty(Description = "گرید")]
    Grid = 5
}
