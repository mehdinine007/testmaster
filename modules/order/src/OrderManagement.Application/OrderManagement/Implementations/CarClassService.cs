using Esale.Core.DataAccess;
using Microsoft.EntityFrameworkCore;
using OrderManagement.Application.Contracts;
using OrderManagement.Application.Contracts.OrderManagement;
using OrderManagement.Application.Contracts.OrderManagement.Services;
using OrderManagement.Domain;
using OrderManagement.Domain.OrderManagement;
using OrderManagement.Domain.Shared;
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
        private readonly IAttachmentService _attachmentService;
        public CarClassService(IRepository<CarClass> carClassRepository, IAttachmentService attachmentService)
        {
            _carClassRepository = carClassRepository;
            _attachmentService = attachmentService;
        }
        public async Task<bool> Delete(int id)
        {
            await _carClassRepository.DeleteAsync(x => x.Id == id, autoSave: true);
            return true;
        }

        public async Task<List<CarClassDto>> GetList(List<AttachmentEntityTypeEnum> attachmentType)
        {
            var carClassesQuery = await _carClassRepository.GetQueryableAsync();
            var carClasses = carClassesQuery.ToList();
            var carClassDto = ObjectMapper.Map<List<CarClass>, List<CarClassDto>>(carClasses);
            var attachments = await _attachmentService.GetList(AttachmentEntityEnum.CarClass, carClasses.Select(x => x.Id).ToList(), attachmentType);
            carClassDto.ForEach(x =>
            {
                var attachment = attachments.Where(y => y.EntityId == x.Id).ToList();
                x.Attachments = ObjectMapper.Map<List<AttachmentDto>, List<AttachmentViewModel>>(attachment);
            });
            return carClassDto;
        }

        public async Task<CarClassDto> Add(CarClassCreateDto carClassDto)
        {
            var carClass = ObjectMapper.Map<CarClassCreateDto, CarClass>(carClassDto);
            var entity= await _carClassRepository.InsertAsync(carClass, autoSave: true);
            return ObjectMapper.Map<CarClass, CarClassDto>(entity);
        }

        public async Task<CarClassDto> Update(CarClassCreateDto carClassDto)
        {
            var result = await _carClassRepository.WithDetails().AsNoTracking().FirstOrDefaultAsync(x => x.Id == carClassDto.Id);
            if (result == null)
            {
                throw new UserFriendlyException(OrderConstant.CarClassNotFound, OrderConstant.CarClassNotFoundId);
            }
            var carClass = ObjectMapper.Map<CarClassCreateDto, CarClass>(carClassDto);

            var entity= await _carClassRepository.AttachAsync(carClass, c => c.Title);

            return ObjectMapper.Map<CarClass, CarClassDto>(entity);
        }

        public async Task<Guid> UploadFile(UploadFileDto uploadFile)
        {
            return await _attachmentService.UploadFile(AttachmentEntityEnum.CarClass, uploadFile);
        }

        public async Task<CarClassDto> GetById(CarClassQueryDto carClassQueryDto)
        {
            var carClass = (await _carClassRepository.GetQueryableAsync())
               .FirstOrDefault(x => x.Id == carClassQueryDto.Id);
            if (carClass == null)
            {
                throw new UserFriendlyException(OrderConstant.CarClassNotFound, OrderConstant.CarClassNotFoundId);
            }
            var attachments = await _attachmentService.GetList(AttachmentEntityEnum.CarClass, new List<int> { carClass.Id }, carClassQueryDto.AttachmentType);

           var carClassDto= ObjectMapper.Map<CarClass, CarClassDto>(carClass);
            carClassDto.Attachments = ObjectMapper.Map<List<AttachmentDto>, List<AttachmentViewModel>>(attachments);
            return carClassDto;
        }
    }
}
