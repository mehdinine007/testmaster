using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace OrderManagement.Application.Contracts.OrderManagement.Services
{
    public interface IAgencySaleDetailService: IApplicationService
    {
        Task<List<AgencySaleDetailListDto>> GetAgencySaleDetail(int saleDetailId);
        Task<int> Save(AgencySaleDetailDto agencySaleDetailDto);
        Task<int> Delete(int id);

    }
}
