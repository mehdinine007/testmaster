﻿using EasyCaching.Core;
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
using Volo.Abp.Domain.Entities;

namespace OrderManagement.Application.OrderManagement.Implementations;

public class AgencyService : ApplicationService, IAgencyService
{
    private readonly IRepository<Agency> _agencyRepository;
    private readonly IRepository<Province> _provinceRepository;
    private readonly IRepository<City> _cityRepository;
    private readonly ICacheManager _cacheManager;
    private readonly IAttachmentService _attachmentService;
    public AgencyService(IRepository<Agency> agencyRepository, IRepository<Province> provinceRepository,
         ICacheManager cacheManager, IAttachmentService attachmentService, IRepository<City> cityRepository)
    {
        _agencyRepository = agencyRepository;
        _provinceRepository = provinceRepository;
        _cacheManager = cacheManager;
        _attachmentService = attachmentService;
        _cityRepository = cityRepository;
    }


    [SecuredOperation(AgencyServicePermissionConstants.Delete)]
    public async Task<bool> Delete(int id)
    {
        await Validation(id, null);
        await _agencyRepository.DeleteAsync(x => x.Id == id);
        await _cacheManager.RemoveByPrefixAsync(RedisConstants.AgencyPrefix, new CacheOptions() { Provider = CacheProviderEnum.Hybrid });
        await _attachmentService.DeleteByEntityId(AttachmentEntityEnum.Agency, id);
        return true;
    }

    [SecuredOperation(AgencyServicePermissionConstants.GetAgencies)]
    public async Task<PagedResultDto<AgencyDto>> GetAgencies(int pageNo, int sizeNo)
    {
        var count = await _agencyRepository.CountAsync();
        var agencies =( await _agencyRepository.GetQueryableAsync()).Include(x => x.Province).Include(x=>x.City);
        var queryResult = await agencies.Skip(pageNo * sizeNo).Take(sizeNo).ToListAsync();
        var agencyDto = ObjectMapper.Map<List<Agency>, List<AgencyDto>>(queryResult);
        var attachments = await _attachmentService.GetList(AttachmentEntityEnum.Agency, agencyDto.Select(x => x.Id).ToList());
        agencyDto.ForEach(x =>
        {
            var attachment = attachments.Where(y => y.EntityId == x.Id).ToList();
            x.Attachments = ObjectMapper.Map<List<AttachmentDto>, List<AttachmentViewModel>>(attachment);
        });
        return new PagedResultDto<AgencyDto>
        {
            TotalCount = count,
            Items = agencyDto
        };

    }
    [UnitOfWork]
    [SecuredOperation(AgencyServicePermissionConstants.Add)]
    public async Task<AgencyDto> Add(AgencyCreateDto agencyDto)
    {
        var createOrUpdate = ObjectMapper.Map<AgencyCreateDto, AgencyCreateOrUpdateDto>(agencyDto);
        await Validation(null, createOrUpdate);
        var agency = ObjectMapper.Map<AgencyCreateDto, Agency>(agencyDto);
        var entity = await _agencyRepository.InsertAsync(agency);
        await CurrentUnitOfWork.SaveChangesAsync();
        return await GetById(entity.Id);
    }

    [SecuredOperation(AgencyServicePermissionConstants.Update)]
    public async Task<AgencyDto> Update(AgencyCreateOrUpdateDto agencyDto)
    {
        var _agency = await Validation(agencyDto.Id, agencyDto);
        var agency = ObjectMapper.Map<AgencyCreateOrUpdateDto, Agency>(agencyDto, _agency);
        var entity = await _agencyRepository.UpdateAsync(agency);
        await _cacheManager.RemoveByPrefixAsync(RedisConstants.AgencyPrefix, new CacheOptions() { Provider = CacheProviderEnum.Hybrid });
        return await GetById(entity.Id);
    }


    public async Task<AgencyDto> GetById(int id, List<AttachmentEntityTypeEnum> attachmentType = null, List<AttachmentLocationEnum> attachmentlocation = null)
    {
        var agency = await Validation(id, null);
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
        if (agencyQueryDto.CityId is not null)
        {
            agencyQuery = agencyQuery.Where(x => x.CityId == agencyQueryDto.CityId);
        }
        if (!agencyQueryDto.Code.IsNullOrEmpty())
        {
            agencyQuery = agencyQuery.Where(x => EF.Functions.Like(x.Code, agencyQueryDto.Code + "%"));
        }

        if (!agencyQueryDto.Name.IsNullOrEmpty())
        {
            agencyQuery = agencyQuery.Where(x => EF.Functions.Like(x.Name, agencyQueryDto.Name + "%"));
        }

        if (agencyQueryDto.AgencyType is not null)
        {
            agencyQuery = agencyQuery.Where(x => x.AgencyType == agencyQueryDto.AgencyType);
        }
        var resultquery = agencyQuery.Include(x => x.Province).Include(x => x.City).ToList();
        var agencyDto = ObjectMapper.Map<List<Agency>, List<AgencyDto>>(resultquery);
        var attachments = await _attachmentService.GetList(AttachmentEntityEnum.Agency, agencyDto.Select(x => x.Id).ToList(), agencyQueryDto.AttachmentEntityType, agencyQueryDto.AttachmentLocation);
        agencyDto.ForEach(x =>
        {
            var attachment = attachments.Where(y => y.EntityId == x.Id).ToList();
            x.Attachments = ObjectMapper.Map<List<AttachmentDto>, List<AttachmentViewModel>>(attachment);
        });
        return agencyDto;
    }
    private async Task<Agency> Validation(int? id, AgencyCreateOrUpdateDto agencyCreteOrUpdateDto)
    {

        var agency = new Agency();
        if (id is not null)
        {
            var agencyQuery = (await _agencyRepository.GetQueryableAsync()).AsNoTracking().Include(x => x.City).Include(x => x.Province);
            agency = agencyQuery.FirstOrDefault(x => x.Id == id);
            if (agency is null)
                throw new UserFriendlyException(OrderConstant.AgencyNotFound, OrderConstant.AgencyId);
        }

        if (agencyCreteOrUpdateDto is not null)
        {
            var province = (await _provinceRepository.GetQueryableAsync()).AsNoTracking().FirstOrDefault(x => x.Id == agencyCreteOrUpdateDto.ProvinceId);
            if (province is null)
                throw new UserFriendlyException(OrderConstant.ProvinceNotFound, OrderConstant.ProvinceNotFoundId);
            if (agencyCreteOrUpdateDto.CityId is not null)
            {
                var city = (await _cityRepository.GetQueryableAsync()).AsNoTracking().FirstOrDefault(x => x.Id == agencyCreteOrUpdateDto.CityId);
                if (city is null)
                    throw new UserFriendlyException(OrderConstant.CityNotFound, OrderConstant.CityNotFoundId);
            }
        }
        return agency;
    }
    [SecuredOperation(AgencyServicePermissionConstants.UploadFile)]
    public async Task<Guid> UploadFile(UploadFileDto uploadFile)
    {
        var announcement = await Validation(uploadFile.Id, null);
        return await _attachmentService.UploadFile(AttachmentEntityEnum.Agency, uploadFile);

    }
}
