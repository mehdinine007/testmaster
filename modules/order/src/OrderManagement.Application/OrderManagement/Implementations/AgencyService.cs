using EasyCaching.Core;
using IFG.Core.Caching;
using Esale.Share.Authorize;
using Microsoft.EntityFrameworkCore;
using OrderManagement.Application.Contracts;
using OrderManagement.Application.Contracts.OrderManagement.Services;
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
using Permission.Order;
using OrderManagement.Application.Contracts.OrderManagement;
using OrderManagement.Domain.OrderManagement;
using OrderManagement.Domain.Shared;

namespace OrderManagement.Application.OrderManagement.Implementations;

public class AgencyService : ApplicationService, IAgencyService
{
    private readonly IRepository<Agency> _agencyRepository;
    private readonly IRepository<Province> _provinceRepository;
    private readonly ICacheManager _cacheManager;
    private readonly IAttachmentService _attachmentService;
    public AgencyService(IRepository<Agency> agencyRepository, IRepository<Province> provinceRepository,
         ICacheManager cacheManager, IAttachmentService attachmentService)
    {
        _agencyRepository = agencyRepository;
        _provinceRepository = provinceRepository;
        _cacheManager = cacheManager;
        _attachmentService = attachmentService;
    }


    [SecuredOperation(AgencyServicePermissionConstants.Delete)]
    public async Task<bool> Delete(int id)
    {
        await _agencyRepository.DeleteAsync(x => x.Id == id, autoSave: true);

        await _cacheManager.RemoveByPrefixAsync(RedisConstants.AgencyPrefix, new CacheOptions() { Provider = CacheProviderEnum.Hybrid });
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
    public async Task<int> Add(AgencyDto agencyDto)
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
        var province = await _provinceRepository.FirstOrDefaultAsync(x => x.Id == agencyDto.ProvinceId);

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


    public async Task<AgencyDto> GetById(int id, List<AttachmentEntityTypeEnum> attachmentType = null, List<AttachmentLocationEnum> attachmentlocation = null)
    {
        var agency = await Validation(id);
        var agencyDto = ObjectMapper.Map<Agency, AgencyDto>(agency);
        var attachments = await _attachmentService.GetList(AttachmentEntityEnum.Agency, new List<int>() { id }, attachmentType, attachmentlocation);
        agencyDto.Attachments = ObjectMapper.Map<List<AttachmentDto>, List<AttachmentViewModel>>(attachments);
        return agencyDto;
    }

    public async Task<List<AgencyDto>> GetList(AgencyQueryDto agencyQueryDto)
    {
        var agencyQuery = (await _agencyRepository.GetQueryableAsync()).AsNoTracking();
        if (agencyQueryDto.ProvinceId is not null)
        {
            agencyQuery = agencyQuery.Where(x => x.ProvinceId == agencyQueryDto.ProvinceId);
        }
        if (agencyQueryDto.CityId is null)
        {
            agencyQuery = agencyQuery.Where(x => x.CityId == agencyQueryDto.CityId);
        }
        if (agencyQueryDto.Code is not null)
        {
            agencyQuery = agencyQuery.Where(x => x.Code == agencyQueryDto.Code);
        }

        if (agencyQueryDto.Name.IsNullOrEmpty())
        {
            agencyQuery = agencyQuery.Where(x => x.Name == agencyQueryDto.Name);
        }
        if (agencyQueryDto.Code.IsNullOrEmpty())
        {
            agencyQuery = agencyQuery.Where(x => x.Name == agencyQueryDto.Name);
        }
        if (agencyQueryDto.AgencyType is not null)
        {
            agencyQuery = agencyQuery.Where(x => x.AgencyType == agencyQueryDto.AgencyType);
        }
        var resultquery = agencyQuery.Include(x => x.Province).Include(x => x.City).ToList();
        var agencyDto = ObjectMapper.Map<List<Agency>, List<AgencyDto>>(resultquery);
        var attachments = await _attachmentService.GetList(AttachmentEntityEnum.Organization, agencyDto.Select(x => x.Id).ToList(), agencyQueryDto.AttachmentEntityType, agencyQueryDto.AttachmentLocation);
        agencyDto.ForEach(x =>
        {
            var attachment = attachments.Where(y => y.EntityId == x.Id).ToList();
            x.Attachments = ObjectMapper.Map<List<AttachmentDto>, List<AttachmentViewModel>>(attachment);
        });
        return agencyDto;
    }
    private async Task<Agency> Validation(int id)
    {
        var agency = (await _agencyRepository.GetQueryableAsync()).AsNoTracking()
            .FirstOrDefault(x => x.Id == id);
        if (agency is null)
        {
            throw new UserFriendlyException(OrderConstant.AgencyNotFound, OrderConstant.AgencyId);
        }
        return agency;
    }


}
