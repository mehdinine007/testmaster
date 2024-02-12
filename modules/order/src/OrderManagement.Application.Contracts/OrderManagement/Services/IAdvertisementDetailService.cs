using OrderManagement.Domain.Shared;
using OrderManagement.Domain.Shared.OrderManagement.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace OrderManagement.Application.Contracts.OrderManagement.Services
{
    public interface IAdvertisementDetailService : IApplicationService
    {
        Task<List<AdvertisementDetailDto>> GetList(int advertisementId, List<AttachmentEntityTypeEnum> attachmentType = null, List<AttachmentLocationEnum> attachmentlocation = null);
        Task<AdvertisementDetailDto> GetById(int id, List<AttachmentEntityTypeEnum> attachmentType = null, List<AttachmentLocationEnum> attachmentlocation = null);
        Task<PagedResultDto<AdvertisementDetailDto>> GetPagination(AdvertisementDetailPaginationDto input);
        Task<AdvertisementDetailDto> Add(AdvertisementDetailCreateOrUpdateDto advertisementDetailCreateOrUpdateDto);
        Task<AdvertisementDetailDto> Update(AdvertisementDetailCreateOrUpdateDto advertisementDetailCreateOrUpdateDto);
        Task<bool> Delete(int id);
        Task<Guid> UploadFile(UploadFileDto uploadFile);
        Task<bool> Move(AdvertisementDetailWithIdDto advertisementDetailWithId);
    }
}
