using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace OrderManagement.Application.Contracts.OrderManagement.Services
{
    public interface IAgencyService: IApplicationService
    {
        Task<List<AgencyDto>> GetAgencies();
        Task<int> Save(AgencyDto agencyDto);
        Task<int> Update(AgencyDto agencyDto);
        Task<int> Delete(int id);

    }
}
