using OrderManagement.Application.Contracts.OrderManagement.Inqueries;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace OrderManagement.Application.Contracts.Services
{
    public interface IReportApplicationService : IApplicationService
    {
        Task<CustomPagedResultDto<SaleDetailResultDto>> SaleDetailReport(SaleDetailReportInquery input);
    }
}
