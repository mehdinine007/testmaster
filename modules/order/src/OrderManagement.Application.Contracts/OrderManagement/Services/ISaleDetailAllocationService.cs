using OrderManagement.Application.Contracts.OrderManagement;
using Volo.Abp.Application.Services;

namespace OrderManagement.Application.Contracts.Services;

public interface ISaleDetailAllocationService : IApplicationService
{
    Task<SaleDetailAllocationDto> Create(SaleDetailAllocationCreateOrUpdateDto seasonCompanyProductDto);

    Task<SaleDetailAllocationDto> Update(SaleDetailAllocationCreateOrUpdateDto seasonCompanyProductDto);

    Task<SaleDetailAllocationDto> GetById(int seasonCompanyProductId);

    Task<bool> Delete(int seasonCompanyProductId);
    Task<List<SaleDetailAllocationDto>> GetList();
}
