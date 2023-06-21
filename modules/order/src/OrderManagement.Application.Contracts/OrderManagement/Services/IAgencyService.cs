using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace OrderManagement.Application.Contracts.OrderManagement.Services
{
    public interface IAgencyService: IApplicationService
    {
        Task<PagedResultDto<AgencyDto>> GetAgencies(int pageNo, int sizeNo);
        Task<int> Save(AgencyDto agencyDto);
        Task<int> Update(AgencyDto agencyDto);
        Task Delete(int id);

    }
}
