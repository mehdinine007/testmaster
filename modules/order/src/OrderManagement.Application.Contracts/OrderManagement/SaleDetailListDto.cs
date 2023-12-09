using OrderManagement.Domain.Shared;

namespace OrderManagement.Application.Contracts.OrderManagement;

public class SaleDetailListDto
{
    public int EsaleTypeId { get; set; }
    public string EsaleTypeName { get; set; }
    public decimal CarFee { get; set; }
    public decimal MinimumAmountOfProxyDeposit { get; set; }
    public Guid UID { get; set; }
    public SaleProcessType SaleProcess { get; set; }
}
