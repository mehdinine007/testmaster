using OrderManagement.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace OrderManagement.Application.Contracts.OrderManagement.Services
{
    public interface IAdvertisementService: IApplicationService
    {

        Task<List<AdvertisementDto>> GetList(List<AttachmentEntityTypeEnum> attachmentType = null, List<AttachmentLocationEnum> attachmentlocation = null);
        Task<AdvertisementDto> GetById(AdvertisementQueryDto advertisementQueryDto);
        Task<AdvertisementDto> Add(AdvertisementCreateOrUpdateDto advertisementCreateOrUpdateDto);
        Task<AdvertisementDto> Update(AdvertisementCreateOrUpdateDto advertisementCreateOrUpdateDto);
        Task<bool> Delete(int id);

    }
}
