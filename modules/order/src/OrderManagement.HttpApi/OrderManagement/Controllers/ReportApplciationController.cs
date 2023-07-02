using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using OrderManagement.Application.Contracts.Services;
using Volo.Abp.Auditing;
using Volo.Abp;
using OrderManagement.Application.Contracts;
using OrderManagement.Application.Contracts.OrderManagement.Inqueries;

namespace OrderManagement.HttpApi.OrderManagement.Controllers
{
    [DisableAuditing]
    [RemoteService]
    [Route("api/services/app/ReportService/[action]")]
    public class ReportApplciationController : AbpController, IReportApplicationService
    {
        private readonly IReportApplicationService _reportApplicationService;

        public ReportApplciationController(IReportApplicationService reportApplicationService)
        {
            _reportApplicationService = reportApplicationService;
        }

        [HttpGet]
        //[UserAuthorization]
        public async Task<CustomPagedResultDto<SaleDetailResultDto>> SaleDetailReport(SaleDetailReportInquery input)
            => await _reportApplicationService.SaleDetailReport(input);

        //[HttpGet]
        //[UserAuthorization]
    }
}
