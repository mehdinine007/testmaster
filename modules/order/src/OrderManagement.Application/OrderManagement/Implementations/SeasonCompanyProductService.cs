using Esale.Share.Authorize;
using OrderManagement.Application.Contracts;
using OrderManagement.Application.Contracts.Constants.Permissions;
using OrderManagement.Application.Contracts.Services;
using OrderManagement.Domain;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace OrderManagement.Application.Implementations;

public class SaleDetailAllocationService : ApplicationService, ISaleDetailAllocationService
{
    private readonly IRepository<SaleDetailAllocation, int> _seasonCompanyProductRepository;

    public SaleDetailAllocationService(IRepository<SaleDetailAllocation, int> seasonCompanyProductRepository)
    {
        _seasonCompanyProductRepository = seasonCompanyProductRepository;
    }

    //[SecuredOperation(SeasonCompanyProductServicePermissionConstants.Create)]
    public async Task<SaleDetailAllocationDto> Create(SaleDetailAllocationDto seasonCompanyProductDto)
    {
        var input = ObjectMapper.Map<SaleDetailAllocationDto, SaleDetailAllocation>(seasonCompanyProductDto);
        input.IsComplete = false;
        try
        {

            var entity = await _seasonCompanyProductRepository.InsertAsync(input);
            return ObjectMapper.Map<SaleDetailAllocation, SaleDetailAllocationDto>(entity);
        }
        catch (System.Exception ex)
        {
            throw;
        }
    }

    [SecuredOperation(SeasonCompanyProductServicePermissionConstants.Delete)]
    public async Task Delete(int seasonCompanyProductId)
    {
        var seasonCompanyProduct = await GetById(seasonCompanyProductId);
        await _seasonCompanyProductRepository.DeleteAsync(seasonCompanyProductId);
    }

    [SecuredOperation(SeasonCompanyProductServicePermissionConstants.GetById)]
    public async Task<SaleDetailAllocationDto> GetById(int seasonCompanyProductId)
    {
        var seasonCompanyProduct = await _seasonCompanyProductRepository.FindAsync(seasonCompanyProductId)
            ?? throw new UserFriendlyException("آیتم مورد نظر یافت نشد");

        return ObjectMapper.Map<SaleDetailAllocation, SaleDetailAllocationDto>(seasonCompanyProduct);
    }

    [SecuredOperation(SeasonCompanyProductServicePermissionConstants.Update)]
    public async Task<SaleDetailAllocationDto> Update(SaleDetailAllocationDto seasonCompanyProductDto)
    {
        var seasonCompanyProduct = (await _seasonCompanyProductRepository.GetQueryableAsync())
            .FirstOrDefault(x => x.Id == seasonCompanyProductDto.Id)
            ?? throw new UserFriendlyException("آیتم مورد نظر یافت نشد");
        var mappedEntity = ObjectMapper.Map<SaleDetailAllocationDto, SaleDetailAllocation>(seasonCompanyProductDto, seasonCompanyProduct);
        mappedEntity.IsComplete = seasonCompanyProduct.IsComplete;
        var result = await _seasonCompanyProductRepository.UpdateAsync(mappedEntity);

        return ObjectMapper.Map<SaleDetailAllocation, SaleDetailAllocationDto>(result);
    }
}
