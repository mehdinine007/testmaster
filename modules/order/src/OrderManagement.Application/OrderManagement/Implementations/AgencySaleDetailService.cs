using AutoMapper.Internal.Mappers;
using OrderManagement.Application.Contracts;
using OrderManagement.Application.Contracts.OrderManagement;
using OrderManagement.Application.Contracts.OrderManagement.Services;
using OrderManagement.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Uow;

namespace OrderManagement.Application.OrderManagement.Implementations
{
    public class AgencySaleDetailService : ApplicationService, IAgencySaleDetailService
    {

        private readonly IRepository<AgencySaleDetail> _agencySaleDetailRepository;

        public AgencySaleDetailService(IRepository<AgencySaleDetail> agencySaleDetailRepository)
        {
            _agencySaleDetailRepository = agencySaleDetailRepository;
        }

        public async Task<int> Delete(int id)
        {
            await _agencySaleDetailRepository.DeleteAsync(x => x.Id == id);
            await CurrentUnitOfWork.SaveChangesAsync();
            return id;
        }

        public async Task<List<AgencySaleDetailListDto>> GetAgencySaleDetail(int saleDetailId)
        {
            var agencySaleDetail = await _agencySaleDetailRepository.WithDetailsAsync(x => x.Agency, x => x.SaleDetail);
            var queryResult = agencySaleDetail.Where(x => x.SaleDetailId == saleDetailId).ToList();
            var agencySaleDetailDto = ObjectMapper.Map<List<AgencySaleDetail>, List<AgencySaleDetailListDto>>(queryResult);
            return agencySaleDetailDto;

        }
        [UnitOfWork]
        public async Task<int> Save(AgencySaleDetailDto agencySaleDetailDto)
        {
            var agencySaleDetail = ObjectMapper.Map<AgencySaleDetailDto, AgencySaleDetail>(agencySaleDetailDto);
           
            await _agencySaleDetailRepository.InsertAsync(agencySaleDetail);
            await CurrentUnitOfWork.SaveChangesAsync();
            return agencySaleDetail.Id;
        }
    }
}
