using Esale.Core.DataAccess;
using Microsoft.EntityFrameworkCore;
using OrderManagement.Application.Contracts;
using OrderManagement.Application.Contracts.OrderManagement;
using OrderManagement.Application.Contracts.OrderManagement.Services;
using OrderManagement.Domain.OrderManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace OrderManagement.Application.OrderManagement.Implementations
{
    public class CarClassService : ApplicationService, ICarClassService
    {
        private readonly IRepository<CarClass> _carClassRepository;
        public CarClassService(IRepository<CarClass> carClassRepository)
        {
            _carClassRepository = carClassRepository;
        }
        public async Task<bool> Delete(int id)
        {
            await _carClassRepository.DeleteAsync(x => x.Id == id, autoSave: true);
            return true;
        }

        public async Task<List<CarClassDto>> GetCarClass()
        {
            var carClasses = await _carClassRepository.GetListAsync();
            var carClassDto = ObjectMapper.Map<List<CarClass>, List<CarClassDto>>(carClasses);
            return carClassDto;
        }

        public async Task<CarClassDto> Save(CarClassDto carClassDto)
        {
            var carClass = ObjectMapper.Map<CarClassDto, CarClass>(carClassDto);
            var entity= await _carClassRepository.InsertAsync(carClass, autoSave: true);
            return ObjectMapper.Map<CarClass, CarClassDto>(entity);
        }

        public async Task<CarClassDto> Update(CarClassDto carClassDto)
        {
            var result = await _carClassRepository.WithDetails().AsNoTracking().FirstOrDefaultAsync(x => x.Id == carClassDto.Id);
            if (result == null)
            {
                throw new UserFriendlyException(OrderConstant.CarClassNotFound, OrderConstant.CarClassNotFoundId);
            }
            var carClass = ObjectMapper.Map<CarClassDto, CarClass>(carClassDto);

            var entity= await _carClassRepository.AttachAsync(carClass, c => c.Title);

            return ObjectMapper.Map<CarClass, CarClassDto>(entity);
        }
    }
}
