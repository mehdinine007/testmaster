using Esale.Share.Authorize;
using Microsoft.EntityFrameworkCore;
using OrderManagement.Application.Contracts;
using OrderManagement.Application.Contracts.OrderManagement;
using OrderManagement.Application.Contracts.OrderManagement.Services;
using OrderManagement.Domain.OrderManagement;
using OrderManagement.Domain.Shared.OrderManagement.Enums;
using Permission.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;

namespace OrderManagement.Application.OrderManagement.Implementations
{
    public class SeasonAllocationService : ApplicationService, ISeasonAllocationService
    {
        private readonly IRepository<SeasonAllocation> _seasonAllocationRepository;
        public SeasonAllocationService(IRepository<SeasonAllocation> seasonAllocationRepository)
        {
            _seasonAllocationRepository = seasonAllocationRepository;
        }
        [SecuredOperation(SeasonAllocationPermissionConstants.Add)]
        public async Task<SeasonAllocationDto> Add(SeasonAllocationCreateDto seasonAllocationCreateDto)
        {
            var seasonAllocationQuery = (await _seasonAllocationRepository.GetQueryableAsync()).AsNoTracking().ToList();
            int maxCode = 1;
            if (seasonAllocationQuery.Count > 0)
            {
                maxCode = seasonAllocationQuery.Max(x => x.Code);
                maxCode++;
            }
            var seasonAllocation = ObjectMapper.Map<SeasonAllocationCreateDto, SeasonAllocation>(seasonAllocationCreateDto);
            seasonAllocation.Code = maxCode;
            var entity = await _seasonAllocationRepository.InsertAsync(seasonAllocation);
            await CurrentUnitOfWork.SaveChangesAsync();
            return ObjectMapper.Map<SeasonAllocation, SeasonAllocationDto>(entity);
        }
        [SecuredOperation(SeasonAllocationPermissionConstants.Delete)]
        public async Task<bool> Delete(int id)
        {
            await Validation(id);
            await _seasonAllocationRepository.DeleteAsync(x => x.Id == id);
            return true;
        }
        [SecuredOperation(SeasonAllocationPermissionConstants.GetById)]
        public async Task<SeasonAllocationDto> GetById(int id)
        {
            var seasonAllocation = await Validation(id);
            var seasonAllocationDto = ObjectMapper.Map<SeasonAllocation, SeasonAllocationDto>(seasonAllocation);
            return seasonAllocationDto;
        }
        [SecuredOperation(SeasonAllocationPermissionConstants.GetList)]
        public async Task<List<SeasonAllocationDto>> GetList()
        {
            var seasonAllocation = (await _seasonAllocationRepository.GetQueryableAsync()).AsNoTracking().ToList();
            var seasonAllocationDto = ObjectMapper.Map<List<SeasonAllocation>, List<SeasonAllocationDto>>(seasonAllocation);
            return seasonAllocationDto;
        }
        [SecuredOperation(SeasonAllocationPermissionConstants.Update)]
        public async Task<SeasonAllocationDto> Update(SeasonAllocationUpdateDto seasonAllocationUpdateDto)
        {
           var seasonAllocation=await Validation(seasonAllocationUpdateDto.Id);
            var seasonAllocationMap = ObjectMapper.Map<SeasonAllocationUpdateDto, SeasonAllocation>(seasonAllocationUpdateDto, seasonAllocation);
            var entity = await _seasonAllocationRepository.UpdateAsync(seasonAllocation);
            return ObjectMapper.Map<SeasonAllocation, SeasonAllocationDto>(entity);
        }

        private async Task<SeasonAllocation> Validation(int id)
        {
            var seasonAllocationQuery = (await _seasonAllocationRepository.GetQueryableAsync()).AsNoTracking();
            var seasonAllocation = seasonAllocationQuery.FirstOrDefault(x => x.Id == id);
            if (seasonAllocation is null)
                throw new UserFriendlyException(OrderConstant.SeasonAllocationNotFound, OrderConstant.SeasonAllocationNotFoundId);
            return seasonAllocation;
        }



    }
}
