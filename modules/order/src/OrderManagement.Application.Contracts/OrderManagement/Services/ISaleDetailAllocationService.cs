using Volo.Abp.Application.Services;

namespace OrderManagement.Application.Contracts.Services;

public interface ISaleDetailAllocationService : IApplicationService
{
    Task<SaleDetailAllocationDto> Create(SaleDetailAllocationDto seasonCompanyProductDto);

    Task<SaleDetailAllocationDto> Update(SaleDetailAllocationDto seasonCompanyProductDto);

    Task<SaleDetailAllocationDto> GetById(int seasonCompanyProductId);

    Task Delete(int seasonCompanyProductId);
    Task<List<SaleDetailAllocationDto>> GetList();
}
