using Esale.Share.Authorize;
using Microsoft.EntityFrameworkCore;
using OrderManagement.Application.Contracts;
using OrderManagement.Application.Contracts.Services;
using OrderManagement.Domain;
using Permission.Order;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;

namespace OrderManagement.Application.Implementations;

public class SaleDetailAllocationService : ApplicationService, ISaleDetailAllocationService
{
    private readonly IRepository<SaleDetailAllocation, int> _saleDetailAllocationRepository;


    public SaleDetailAllocationService(IRepository<SaleDetailAllocation, int> saleDetailAllocationRepository)
    {
        _saleDetailAllocationRepository = saleDetailAllocationRepository;
    }

    [SecuredOperation(SaleDetailAllocationServicePermissionConstants.Create)]
    public async Task<SaleDetailAllocationDto> Create(SaleDetailAllocationDto saleDetailAllocationDto)
    {
        var input = ObjectMapper.Map<SaleDetailAllocationDto, SaleDetailAllocation>(saleDetailAllocationDto);
        input.IsComplete = false;
        var entity = await _saleDetailAllocationRepository.InsertAsync(input, autoSave: true);
        return ObjectMapper.Map<SaleDetailAllocation, SaleDetailAllocationDto>(entity);
    }

    [SecuredOperation(SaleDetailAllocationServicePermissionConstants.Delete)]
    public async Task Delete(int saleDetailAllocationId)
    {
        var saleDetailAllocation = await GetById(saleDetailAllocationId);
        await _saleDetailAllocationRepository.DeleteAsync(saleDetailAllocationId);
    }

    [SecuredOperation(SaleDetailAllocationServicePermissionConstants.GetById)]
    public async Task<SaleDetailAllocationDto> GetById(int saleDetailAllocationId)
    {
        var saleDetailAllocation = await _saleDetailAllocationRepository
            .FindAsync(saleDetailAllocationId)
            ?? throw new UserFriendlyException("آیتم مورد نظر یافت نشد");

        return ObjectMapper.Map<SaleDetailAllocation, SaleDetailAllocationDto>(saleDetailAllocation);
    }

    [SecuredOperation(SaleDetailAllocationServicePermissionConstants.Update)]
    public async Task<SaleDetailAllocationDto> Update(SaleDetailAllocationDto saleDetailAllocationDto)
    {
        var saleDetailAllocation = (await _saleDetailAllocationRepository.GetQueryableAsync())
            .FirstOrDefault(x => x.Id == saleDetailAllocationDto.Id)
            ?? throw new UserFriendlyException("آیتم مورد نظر یافت نشد");
        var mappedEntity = ObjectMapper.Map<SaleDetailAllocationDto, SaleDetailAllocation>(saleDetailAllocationDto, saleDetailAllocation);
        mappedEntity.IsComplete = saleDetailAllocation.IsComplete;
        var result = await _saleDetailAllocationRepository.UpdateAsync(mappedEntity);

        return ObjectMapper.Map<SaleDetailAllocation, SaleDetailAllocationDto>(result);
    }

    public async Task<List<SaleDetailAllocationDto>> GetList()
    {
        var saleDetailAllocation = (await _saleDetailAllocationRepository.GetQueryableAsync())
            .AsNoTracking()
            .Include(x=> x.SaleDetail)
            .Include(x => x.SeasonAllocation)
            .ToList();
        var saleDetailAllocationDto = ObjectMapper.Map<List<SaleDetailAllocation>, List<SaleDetailAllocationDto>>(saleDetailAllocation);
        return saleDetailAllocationDto;
    }

}
