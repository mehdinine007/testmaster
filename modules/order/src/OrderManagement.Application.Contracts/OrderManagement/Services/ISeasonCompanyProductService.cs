using Volo.Abp.Application.Services;

namespace OrderManagement.Application.Contracts.Services;

public interface ISeasonCompanyProductService : IApplicationService
{
    Task<SeasonCompanyProductDto> Create(SeasonCompanyProductDto seasonCompanyProductDto);

    Task<SeasonCompanyProductDto> Update(SeasonCompanyProductDto seasonCompanyProductDto);

    Task<SeasonCompanyProductDto> GetById(int seasonCompanyProductId);

    Task Delete(int seasonCompanyProductId);
}
