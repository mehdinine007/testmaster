using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace OrderManagement.Application.Contracts.OrderManagement.Services
{
    public interface IAgencySaleDetailService: IApplicationService
    {
        Task<PagedResultDto<AgencySaleDetailListDto>> GetAgencySaleDetail(int saleDetailId, int pageNo, int sizeNo);
        AgencySaleDetailListDto GetBySaleDetailId(int saleDetailId, int agancyId);
        long GetReservCount(int saleDetailId);
        Task<int> Save(AgencySaleDetailDto agencySaleDetailDto);
        Task Delete(int id);


    }
}
