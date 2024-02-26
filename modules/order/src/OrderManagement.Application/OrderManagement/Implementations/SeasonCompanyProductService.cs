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

public class SeasonCompanyProductService : ApplicationService, ISeasonCompanyProductService
{
    private readonly IRepository<SeasonCompanyProduct, int> _seasonCompanyProductRepository;

    public SeasonCompanyProductService(IRepository<SeasonCompanyProduct, int> seasonCompanyProductRepository)
    {
        _seasonCompanyProductRepository = seasonCompanyProductRepository;
    }

    [SecuredOperation(SeasonCompanyProductServicePermissionConstants.Create)]
    public async Task<SeasonCompanyProductDto> Create(SeasonCompanyProductDto seasonCompanyProductDto)
    {
        var entity = await _seasonCompanyProductRepository.InsertAsync(
            ObjectMapper.Map<SeasonCompanyProductDto, SeasonCompanyProduct>(seasonCompanyProductDto));
        return ObjectMapper.Map<SeasonCompanyProduct, SeasonCompanyProductDto>(entity);
    }

    [SecuredOperation(SeasonCompanyProductServicePermissionConstants.Delete)]
    public async Task Delete(int seasonCompanyProductId)
    {
        var seasonCompanyProduct = await GetById(seasonCompanyProductId);
        await _seasonCompanyProductRepository.DeleteAsync(seasonCompanyProductId);
    }

    [SecuredOperation(SeasonCompanyProductServicePermissionConstants.GetById)]
    public async Task<SeasonCompanyProductDto> GetById(int seasonCompanyProductId)
    {
        var seasonCompanyProduct = await _seasonCompanyProductRepository.FindAsync(seasonCompanyProductId)
            ?? throw new UserFriendlyException("آیتم مورد نظر یافت نشد");

        return ObjectMapper.Map<SeasonCompanyProduct, SeasonCompanyProductDto>(seasonCompanyProduct);
    }

    [SecuredOperation(SeasonCompanyProductServicePermissionConstants.Update)]
    public async Task<SeasonCompanyProductDto> Update(SeasonCompanyProductDto seasonCompanyProductDto)
    {
        var seasonCompanyProduct = (await _seasonCompanyProductRepository.GetQueryableAsync())
            .FirstOrDefault(x => x.Id == seasonCompanyProductDto.Id)
            ?? throw new UserFriendlyException("آیتم مورد نظر یافت نشد");

        var result = await _seasonCompanyProductRepository.UpdateAsync(
            ObjectMapper.Map<SeasonCompanyProductDto, SeasonCompanyProduct>(seasonCompanyProductDto, seasonCompanyProduct));

        return ObjectMapper.Map<SeasonCompanyProduct, SeasonCompanyProductDto>(result);
    }
}
