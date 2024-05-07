namespace CompanyManagement.Application.Contracts.Dto;

public record GetOrderByIdResponseDto
(
    int ProductId,
    string ProductCode,
    int OrganizationId,
    int OrderStatus
);