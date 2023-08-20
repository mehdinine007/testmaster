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
//        Task<AnnouncementDto> GetById(int id);
        Task<List<AnnouncementDto>> GetAllAnnouncement();
        Task<PagedResultDto<AnnouncementDto>> GetPagination(AnnouncementGetListDto input);
        Task<int> Insert(CreateAnnouncementDto announcementDto);
        Task<int> Update(CreateAnnouncementDto announcementDto);
        Task<bool> Delete(int id);
        Task<bool> UploadFile(UploadFileDto uploadFile);
    }
}
