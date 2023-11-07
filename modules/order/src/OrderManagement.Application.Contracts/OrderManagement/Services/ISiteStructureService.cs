using OrderManagement.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
