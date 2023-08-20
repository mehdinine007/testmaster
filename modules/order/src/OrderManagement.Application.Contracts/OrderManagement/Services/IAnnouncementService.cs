using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace OrderManagement.Application.Contracts.OrderManagement.Services
{
    public interface IAnnouncementService: IApplicationService
    {
        Task<List<AnnouncementDto>> GetAllAnnouncement();
        Task<PagedResultDto<AnnouncementDto>> GetList(AnnouncementGetListDto input);
        Task<int> Add(CreateAnnouncementDto announcementDto);
        Task<int> Update(CreateAnnouncementDto announcementDto);
        Task<bool> Delete(int id);
        Task<bool> UploadFile(UploadFileDto uploadFile);
    }
}
