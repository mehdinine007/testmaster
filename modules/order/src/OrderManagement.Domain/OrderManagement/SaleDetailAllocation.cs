using OrderManagement.Domain.OrderManagement;
using Volo.Abp.Domain.Entities.Auditing;

namespace OrderManagement.Domain;

public class SaleDetailAllocation : FullAuditedEntity<int>
{
    public int SaleDetailId { get; set; }
    public int Count { get; set; }
    public bool IsComplete { get; set; }
    public int? TotalCount { get; set; }
    public SaleDetail SaleDetail { get; set; }
    public int? SeasonAllocationId { get; set; }
    public SeasonAllocation SeasonAllocation { get; set; }

}
