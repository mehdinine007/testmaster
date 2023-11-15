using EasyCaching.Core;
using IFG.Core.Caching;
using Esale.Share.Authorize;
using Microsoft.EntityFrameworkCore;
using OrderManagement.Application.Contracts;
using OrderManagement.Application.Contracts.OrderManagement.Services;
using OrderManagement.Application.OrderManagement.Constants;
using OrderManagement.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Uow;

namespace OrderManagement.Application.OrderManagement.Implementations;

public class AgencyService : ApplicationService, IAgencyService
{
    private readonly IRepository<Agency> _agencyRepository;
    private readonly IRepository<Province> _provinceRepository;
    private readonly ICacheManager _cacheManager;

    public AgencyService(IRepository<Agency> agencyRepository, IRepository<Province> provinceRepository,
         ICacheManager cacheManager)
    {
        _agencyRepository = agencyRepository;
        _provinceRepository = provinceRepository;
        _cacheManager = cacheManager;
    }


    [SecuredOperation(AgencyServicePermissionConstants.Delete)]
    public async Task<bool> Delete(int id)
    {
        await _agencyRepository.DeleteAsync(x => x.Id == id, autoSave: true);

        await _cacheManager.RemoveByPrefixAsync(RedisConstants.AgencyPrefix ,new CacheOptions() { Provider = CacheProviderEnum.Hybrid });
        return true;
    }

    [SecuredOperation(AgencyServicePermissionConstants.GetAgencies)]
    public async Task<PagedResultDto<AgencyDto>> GetAgencies(int pageNo, int sizeNo)
    {
        var count = await _agencyRepository.CountAsync();
        var agencies = await _agencyRepository.WithDetailsAsync(x => x.Province);
        var queryResult = await agencies.Skip(pageNo * sizeNo).Take(sizeNo).ToListAsync();
        return new PagedResultDto<AgencyDto>
        {
            TotalCount = count,
            Items = ObjectMapper.Map<List<Agency>, List<AgencyDto>>(queryResult)
        };

    }
    [UnitOfWork]
    [SecuredOperation(AgencyServicePermissionConstants.Save)]
    public async Task<int> Save(AgencyDto agencyDto)
    {
        var province = await _provinceRepository.FirstOrDefaultAsync(x => x.Id == agencyDto.ProvinceId);
        if (province == null)
        {
            throw new UserFriendlyException("استان وجود ندارد.");
        }
        var agency = ObjectMapper.Map<AgencyDto, Agency>(agencyDto);
        await _agencyRepository.InsertAsync(agency, autoSave: true);
        return agency.Id;
    }

    [SecuredOperation(AgencyServicePermissionConstants.Update)]
    public async Task<int> Update(AgencyDto agencyDto)
    {
        var result = await _agencyRepository.FirstOrDefaultAsync(x => x.Id == agencyDto.Id);
        if (result == null)
        {
            throw new UserFriendlyException("نمایندگی انتخاب شده وجود ندارد.");
        }
        var province = await _provinceRepository.FirstOrDefaultAsync(x => x.Id == agencyDto.ProvinceId );

        if (province == null)
        {
            throw new UserFriendlyException("استان وجود ندارد.");
        }
        result.Name = agencyDto.Name;
        result.ProvinceId = agencyDto.ProvinceId;
        await _agencyRepository.UpdateAsync(result, autoSave: true);
        await _cacheManager.RemoveByPrefixAsync(RedisConstants.AgencyPrefix, new CacheOptions() { Provider = CacheProviderEnum.Hybrid });
        return result.Id;
    }
}
