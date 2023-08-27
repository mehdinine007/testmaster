using OrderManagement.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace OrderManagement.Application.Contracts.OrderManagement.Services
{
    public interface ICarClassService: IApplicationService
    {
        Task<List<CarClassDto>> GetList(List<AttachmentEntityTypeEnum> attachmentType);
        Task<CarClassDto> GetById(CarClassQueryDto carClassQueryDto);
        Task<CarClassDto> Add(CarClassCreateDto carClassDto);
        Task<CarClassDto> Update(CarClassCreateDto carClassDto);
        Task<bool> Delete(int id);
        Task<Guid> UploadFile(UploadFileDto uploadFile);
    }
}
