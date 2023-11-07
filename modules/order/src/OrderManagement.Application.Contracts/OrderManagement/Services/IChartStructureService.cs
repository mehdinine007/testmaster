using OrderManagement.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace OrderManagement.Application.Contracts.OrderManagement.Services
{
    public interface IChartStructureService: IApplicationService
    {
        Task<List<ChartStructureDto>> GetList(List<AttachmentEntityTypeEnum>? attachmentType=null , List<AttachmentLocationEnum>? attachmentlocation = null);
        Task<bool> UploadFile(UploadFileDto uploadFile);
        Task<ChartStructureDto> GetById(int id, List<AttachmentEntityTypeEnum>? attachmentType = null, List<AttachmentLocationEnum>? attachmentlocation = null);
        Task<ChartStructureDto> Add(ChartStructureCreateOrUpdateDto chartStructureCreateOrUpdateDto);
        Task<ChartStructureDto> Update(ChartStructureCreateOrUpdateDto chartStructureCreateOrUpdateDto);
        Task<bool> Delete(int id);
    }
}
