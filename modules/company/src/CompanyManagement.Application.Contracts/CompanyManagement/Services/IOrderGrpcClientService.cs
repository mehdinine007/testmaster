using CompanyManagement.Application.Contracts.Dto;
using Volo.Abp.Application.Services;

namespace CompanyManagement.Application.Contracts.Services;

public interface IOrderGrpcClientService : IApplicationService
{
    Task<GetOrderByIdResponseDto> GetOrderById(int orderId);
}