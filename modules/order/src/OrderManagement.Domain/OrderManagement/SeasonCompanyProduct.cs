using OrderManagement.Domain;
using OrderManagement.Domain.OrderManagement;
using Volo.Abp.Domain.Entities.Auditing;

namespace OrderManagement.Domain;

public class SeasonCompanyProduct : FullAuditedEntity<int>
{
    public int CompanyId { get; set; }

    public int SeasonId { get; set; }

    public int? CarTipId { get; set; }

    public int count { get; set; }

    public long? CreatorUserId { get; set; }

    public long? DeleterUserId { get; set; }

    public int? EsaleTypeId { get; set; }

    public int IsComplete { get; set; }

    public int? YearId { get; set; }

    public int? TotalCount { get; set; }

    public int? CategoryId { get; set; }

    public int? ProductId { get; set; }

    public ProductAndCategory Company { get; set; }

    public ProductAndCategory Product { get; set; }

    public ESaleType EsaleType { get; set; }

    public Year Year { get; set; }
}
