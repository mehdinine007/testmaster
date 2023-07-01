using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace OrderManagement.Application.Contracts.Services
{
    public interface IReportApplicationService : IApplicationService
    {
        Task<List<SaleDetailReportDto>> SaleDetailReport(int saleDetailId);
    }
}
