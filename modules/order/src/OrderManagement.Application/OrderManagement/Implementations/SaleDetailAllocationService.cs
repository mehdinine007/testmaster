using Esale.Share.Authorize;
using Microsoft.EntityFrameworkCore;
using Nest;
using OrderManagement.Application.Contracts;
using OrderManagement.Application.Contracts.OrderManagement;
using OrderManagement.Application.Contracts.Services;
using OrderManagement.Domain;
using OrderManagement.Domain.OrderManagement;
using OrderManagement.Domain.Shared.OrderManagement.Enums;
using Permission.Order;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;

namespace OrderManagement.Application.Implementations;

public class SaleDetailAllocationService : ApplicationService, ISaleDetailAllocationService
{
    private readonly IRepository<SaleDetailAllocation, int> _saleDetailAllocationRepository;
    private readonly IRepository<SaleDetail, int> _saleDetailRepository;
    private readonly IRepository<SeasonAllocation, int> _seasonAllocationRepository;


    public SaleDetailAllocationService(IRepository<SaleDetailAllocation, int> saleDetailAllocationRepository, IRepository<SaleDetail, int> saleDetailRepository,
        IRepository<SeasonAllocation, int> seasonAllocationRepository)
    {
        _saleDetailAllocationRepository = saleDetailAllocationRepository;
        _saleDetailRepository = saleDetailRepository;
        _seasonAllocationRepository = seasonAllocationRepository;
    }

    [SecuredOperation(SaleDetailAllocationServicePermissionConstants.Create)]
    public async Task<SaleDetailAllocationDto> Create(SaleDetailAllocationCreateOrUpdateDto saleDetailAllocationDto)
    {
        if (saleDetailAllocationDto.Id > 0)
        {
            throw new UserFriendlyException(OrderConstant.NotValid, OrderConstant.NotValidId);
        }
        await Validation(null, saleDetailAllocationDto);
        var input = ObjectMapper.Map<SaleDetailAllocationCreateOrUpdateDto, SaleDetailAllocation>(saleDetailAllocationDto);
        input.IsComplete = false;
        var entity = await _saleDetailAllocationRepository.InsertAsync(input, autoSave: true);
        await CurrentUnitOfWork.SaveChangesAsync();
        return await GetById(entity.Id);
    }

    [SecuredOperation(SaleDetailAllocationServicePermissionConstants.Delete)]
    public async Task<bool> Delete(int saleDetailAllocationId)
    {
        await Validation(saleDetailAllocationId, null);
        await _saleDetailAllocationRepository.DeleteAsync(saleDetailAllocationId);
        return true;
    }

    [SecuredOperation(SaleDetailAllocationServicePermissionConstants.GetById)]
    public async Task<SaleDetailAllocationDto> GetById(int saleDetailAllocationId)
    {
        var saleDetailAllocation = (await _saleDetailAllocationRepository.GetQueryableAsync())
            .AsNoTracking()
            .Include(x=>x.SaleDetail)
            .Include(x=>x.SeasonAllocation)
            .FirstOrDefault(x => x.Id == saleDetailAllocationId);
        if (saleDetailAllocation == null)
        {
            throw new UserFriendlyException(OrderConstant.SaleDetailAllocationNotFound, OrderConstant.SaleDetailAllocationNotFoundId);
        }
        return ObjectMapper.Map<SaleDetailAllocation, SaleDetailAllocationDto>(saleDetailAllocation);
    }

    [SecuredOperation(SaleDetailAllocationServicePermissionConstants.Update)]
    public async Task<SaleDetailAllocationDto> Update(SaleDetailAllocationCreateOrUpdateDto saleDetailAllocationDto)
    {
        var saleDetailAllocation = await Validation(saleDetailAllocationDto.Id, saleDetailAllocationDto);
        var mappedEntity = ObjectMapper.Map<SaleDetailAllocationCreateOrUpdateDto, SaleDetailAllocation>(saleDetailAllocationDto, saleDetailAllocation);

        var result = await _saleDetailAllocationRepository.UpdateAsync(mappedEntity);
        return await GetById(result.Id);
    }

    public async Task<List<SaleDetailAllocationDto>> GetList()
    {
        var saleDetailAllocation = (await _saleDetailAllocationRepository.GetQueryableAsync())
            .AsNoTracking()
            .Include(x => x.SaleDetail)
            .Include(x => x.SeasonAllocation)
            .ToList();
        var saleDetailAllocationDto = ObjectMapper.Map<List<SaleDetailAllocation>, List<SaleDetailAllocationDto>>(saleDetailAllocation);
        return saleDetailAllocationDto;
    }

    private async Task<SaleDetailAllocation> Validation(int? id, SaleDetailAllocationCreateOrUpdateDto saleDetailAllocationCreateOrUpdate)
    {
        var saleDetailAllocation = new SaleDetailAllocation();
       
        if(id is not null)
        {
            var saleDetailAllocationQuery = (await _saleDetailAllocationRepository.GetQueryableAsync());
            saleDetailAllocation = saleDetailAllocationQuery.FirstOrDefault(x => x.Id == id);
            if (saleDetailAllocation == null)
            {
                throw new UserFriendlyException(OrderConstant.SaleDetailAllocationNotFound, OrderConstant.SaleDetailAllocationNotFoundId);
            }
        }
        
        if (saleDetailAllocationCreateOrUpdate is not null)
        {
            var saleDetail = (await _saleDetailRepository.GetQueryableAsync()).AsNoTracking()
                .FirstOrDefault(x => x.Id == saleDetailAllocationCreateOrUpdate.SaleDetailId);

            if (saleDetail == null)
            {
                throw new UserFriendlyException(OrderConstant.SaleDetailNotFound, OrderConstant.SaleDetailNotFoundId);
            }
            if (saleDetailAllocationCreateOrUpdate.SeasonAllocationId is not null)
            {
                var seasonAllocation = (await _seasonAllocationRepository.GetQueryableAsync())
                    .AsNoTracking().FirstOrDefault(x => x.Id == saleDetailAllocationCreateOrUpdate.SeasonAllocationId);
                if (seasonAllocation is null)
                {
                    throw new UserFriendlyException(OrderConstant.SeasonAllocationNotFound, OrderConstant.SeasonAllocationNotFoundId);
                }
            }
        }
        
        return saleDetailAllocation;
    }
}
