using Volo.Abp.Domain.Entities.Auditing;

namespace OrderManagement.Domain;

public class SaleDetailAllocation : FullAuditedEntity<int>
{
    public int SeasonId { get; set; }

    public int SaleDetailId { get; set; }

    public int Count { get; set; }

    public bool IsComplete { get; set; }

    public int? YearId { get; set; }

    public int? TotalCount { get; set; }

    public Year Year { get; set; }

    public SaleDetail SaleDetail { get; set; }
}
