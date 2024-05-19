using OrderManagement.Application.Contracts.OrderManagement;

namespace OrderManagement.Application.Contracts;

public sealed record SaleDetailAllocationDto
(
    int? SeasonAllocationId,
    SeasonAllocationDto SeasonAllocation,
    int SaleDetailId,
    SaleDetailDto SaleDetail,
    int Count,
    bool IsComplete,
    int? TotalCount,
    int Id
);