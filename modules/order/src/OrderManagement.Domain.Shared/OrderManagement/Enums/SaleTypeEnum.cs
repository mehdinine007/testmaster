using IFG.Core.Attributes;

namespace OrderManagement.Domain.Shared.OrderManagement.Enums
{
    public enum SaleTypeEnum
    {
        [EnumProperty(Description = "خودروهای وارداتی")]
        saleauto = 1,
        [EnumProperty(Description = "خودروهای داخلی")]
        esalecar = 2,
        [EnumProperty(Description = "خودروهای برقی")]
        evauto = 3
    }
}
