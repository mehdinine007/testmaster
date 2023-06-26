using AutoMapper.Internal.Mappers;
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
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace OrderManagement.Application.OrderManagement.Implementations
{
    public class CarTipService:ApplicationService, ICarTipService
    {
        private readonly IRepository<CarTip> _carTipRepository;
        public CarTipService(IRepository<CarTip> carTipRepository)
        {
            _carTipRepository = carTipRepository;

        }

        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<CarTipDto>> GetAllCarTips()
        {
            var carTips = await _carTipRepository.GetListAsync();
            var carTipDto = ObjectMapper.Map<List<CarTip>, List<CarTipDto>>(carTips);
            return carTipDto;
        }

        public async Task<PagedResultDto<CarTipDto>> GetCarTips(int pageNo, int sizeNo)
        {
            var count = await _carTipRepository.CountAsync();
            var carTips = await _carTipRepository.WithDetailsAsync();
            var queryResult = await carTips.Skip(pageNo * sizeNo).Take(sizeNo).ToListAsync();
            return new PagedResultDto<CarTipDto>
            {
                TotalCount = count,
                Items = ObjectMapper.Map<List<CarTip>, List<CarTipDto>>(queryResult)
            };
        }

        public Task<int> Save(CarTipDto carTipDto)
        {
            throw new NotImplementedException();
        }

        public Task<int> Update(CarTipDto carTipDto)
        {
            throw new NotImplementedException();
        }
    }
}
