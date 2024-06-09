using OrderManagement.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace OrderManagement.Application.Contracts.OrderManagement.Services
{
    public interface IAgencyService : IApplicationService
    {
        Task<PagedResultDto<AgencyDto>> GetAgencies(int pageNo, int sizeNo);
        Task<AgencyDto> Add(AgencyCreateDto agencyDto);
        Task<AgencyDto> Update(AgencyCreateOrUpdateDto agencyDto);
        Task<bool> Delete(int id);
        Task<AgencyDto> GetById(int id, List<AttachmentEntityTypeEnum> attachmentType = null, List<AttachmentLocationEnum> attachmentlocation = null);
        Task<List<AgencyDto>> GetList(AgencyQueryDto agencyQueryDto);
        Task<Guid> UploadFile(UploadFileDto uploadFile);
    }
}
