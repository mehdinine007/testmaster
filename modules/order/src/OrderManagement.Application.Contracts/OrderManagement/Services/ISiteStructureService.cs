using OrderManagement.Domain.Shared;
using Volo.Abp.Application.Services;

namespace OrderManagement.Application.Contracts.OrderManagement.Services
{
    public interface ISiteStructureService : IApplicationService
    {
        Task<SiteStructureDto> GetById(int id,List<AttachmentEntityTypeEnum> attachmentType =null, List<AttachmentLocationEnum> attachmentlocation = null);
        Task<List<SiteStructureDto>> GetList(SiteStructureQueryDto siteStructureQuery);
        Task<SiteStructureDto> Add(SiteStructureAddOrUpdateDto siteStructureDto);
        Task<SiteStructureDto> Update(SiteStructureAddOrUpdateDto siteStructureDto);
        Task<bool> Delete(int id);
        Task<bool> UploadFile(UploadFileDto uploadFile);

    }
}
