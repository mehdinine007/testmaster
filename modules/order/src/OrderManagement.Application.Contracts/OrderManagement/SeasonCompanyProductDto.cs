namespace OrderManagement.Application.Contracts;

public class SeasonCompanyProductDto
{
    public int CompanyId { get; set; }

    public int SeasonId { get; set; }

    public int? CarTipId { get; set; }

    public int count { get; set; }

    public long? CreatorUserId { get; set; }

    public long? DeleterUserId { get; set; }

    public int? EsaleTypeId { get; set; }

    public bool IsComplete { get; set; }

    public int? YearId { get; set; }

    public int? TotalCount { get; set; }

    public int? CategoryId { get; set; }

    public int? ProductId { get; set; }

    public int Id { get; set; }
}