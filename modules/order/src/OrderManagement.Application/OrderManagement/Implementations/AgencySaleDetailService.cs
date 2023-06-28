﻿using AutoMapper.Internal.Mappers;
using Microsoft.EntityFrameworkCore;
using OrderManagement.Application.Contracts;
using OrderManagement.Application.Contracts.OrderManagement;
using OrderManagement.Application.Contracts.OrderManagement.Services;
using OrderManagement.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Uow;

namespace OrderManagement.Application.OrderManagement.Implementations
{
    public class AgencySaleDetailService : ApplicationService, IAgencySaleDetailService
    {

        private readonly IRepository<AgencySaleDetail> _agencySaleDetailRepository;
        private readonly IRepository<Agency> _agencyRepository;
        private readonly IRepository<SaleDetail> _saleDetailRepository;

        public AgencySaleDetailService(IRepository<AgencySaleDetail> agencySaleDetailRepository, IRepository<Agency> agencyRepository, IRepository<SaleDetail> saleDetailRepository)
        {
            _agencySaleDetailRepository = agencySaleDetailRepository;
            _agencyRepository = agencyRepository;
            _saleDetailRepository = saleDetailRepository;
        }

        public async Task<bool> Delete(int id)
        {
            await _agencySaleDetailRepository.DeleteAsync(x => x.Id == id, autoSave: true);
            return true;
        }

        public async Task<PagedResultDto<AgencySaleDetailListDto>> GetAgencySaleDetail(int saleDetailId, int pageNo, int sizeNo)
        {
            var count = await _agencySaleDetailRepository.CountAsync(x => x.SaleDetailId == saleDetailId);
            var agencySaleDetail = await _agencySaleDetailRepository.WithDetailsAsync(x => x.Agency, x => x.SaleDetail);
            var queryResult = await agencySaleDetail.Where(x => x.SaleDetailId == saleDetailId).Skip(pageNo * sizeNo).Take(sizeNo).ToListAsync();
            return new PagedResultDto<AgencySaleDetailListDto>
            {
                TotalCount = count,
                Items = ObjectMapper.Map<List<AgencySaleDetail>, List<AgencySaleDetailListDto>>(queryResult)
            };




        }

        public async Task<AgencySaleDetailListDto> GetBySaleDetailId(int saleDetailId, int? agancyId = null)
        {
            var agancySaleDetail = _agencySaleDetailRepository
                .WithDetails()
                .AsNoTracking()
                .FirstOrDefault(x => x.SaleDetailId == saleDetailId && !x.IsDeleted &&
                 (agancyId == null || x.AgencyId == agancyId)
               );
            return ObjectMapper.Map<AgencySaleDetail,AgencySaleDetailListDto>(agancySaleDetail);
        }
        public async Task<List<AgencySaleDetailForCapacityControlDto>> GetAgeneciesBySaleDetail(int saleDetailId)
        {
            var IQagancySaleDetails = await _agencySaleDetailRepository.GetQueryableAsync();
            var agancySaleDetail = IQagancySaleDetails
               .AsNoTracking()
               .Where(x => x.SaleDetailId == saleDetailId)
               .Select(x => new AgencySaleDetailForCapacityControlDto
               {
                   AgencyId = x.AgencyId,
                   Id = x.Id,
                   DistributionCapacity = x.DistributionCapacity,
                   ReserveCount = x.ReserveCount
               }).ToList();


            return agancySaleDetail;
        }

        public long GetReservCount(int saleDetailId)
        {
            return _agencySaleDetailRepository
                .WithDetails()
                .AsNoTracking()
                .Where(x => x.SaleDetailId == saleDetailId)
                .Sum(x => x.ReserveCount);
        }

        [UnitOfWork]
        public async Task<int> Save(AgencySaleDetailDto agencySaleDetailDto)
        {
         var result= await  _agencySaleDetailRepository.SingleOrDefaultAsync(x=>x.AgencyId == agencySaleDetailDto.AgencyId && x.SaleDetailId== agencySaleDetailDto.SaleDetailId);
            if (result != null)
            {
                throw new UserFriendlyException("برنامه فروش برای نمایندگی انتخاب شده تعریف شده است");
            }

            var agency=await _agencyRepository.FirstOrDefaultAsync(x => x.Id == agencySaleDetailDto.AgencyId);

            if (agency==null ||agencySaleDetailDto.AgencyId <= 0)
            {
                throw new UserFriendlyException("نمایندگی وجود ندارد.");
            }
            var saleDetail =await _saleDetailRepository.FirstOrDefaultAsync(x => x.Id == agencySaleDetailDto.SaleDetailId);
            if (saleDetail==null||agencySaleDetailDto.SaleDetailId <= 0)
            {
                throw new UserFriendlyException("جزییات برنامه فروش وجود ندارد.");
            }
         var  agencySaleDetail = ObjectMapper.Map<AgencySaleDetailDto, AgencySaleDetail>(agencySaleDetailDto);

            await _agencySaleDetailRepository.InsertAsync(agencySaleDetail, autoSave: true);
            return agencySaleDetail.Id;
        }
    }
}
