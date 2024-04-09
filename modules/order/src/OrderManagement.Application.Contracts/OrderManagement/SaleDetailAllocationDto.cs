namespace OrderManagement.Application.Contracts;

public sealed record SaleDetailAllocationDto
(
    int SeasonId,
    int SaleDetailId,
    int Count,
    bool IsComplete,
    int? YearId,
    int? TotalCount,
    int Id
);