namespace OrderManagement.Application.Contracts;

public sealed record SaleDetailAllocationDto
(
    int? SeasonAllocationId,
    int SaleDetailId,
    int Count,
    bool IsComplete,
    int? TotalCount,
    int Id
);