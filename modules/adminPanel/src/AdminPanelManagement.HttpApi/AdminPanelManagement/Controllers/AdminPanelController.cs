using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Auditing;
using Volo.Abp;
using Microsoft.AspNetCore.Mvc;
using AdminPanelManagement.Application.Contracts.AdminPanelManagement.IServices;
using AdminPanelManagement.Application.Contracts.AdminPanelManagement.Dtos.report;
using AdminPanelManagement.Application.Grpc.ReportGrpcClient;

namespace AdminPanelManagement.HttpApi.AdminPanelManagement.Controllers
{
    [DisableAuditing]
    [RemoteService]
    [Route("api/services/app/ReportGrpcClientService/[action]")]
    //[UserAuthorization]
    public class AdminPanelController: Controller
    {

       private readonly IReportGrpcClientService _reportGrpcClientService;
        public AdminPanelController(IReportGrpcClientService reportGrpcClientService)
        {

            _reportGrpcClientService = reportGrpcClientService;


        }
        [HttpPost]
        public async Task<List<DashboardDto>> GetAllDashboard()
        =>await _reportGrpcClientService.GetAllDashboard();

        [HttpPost]
        public async Task<List<WidgetDto>> GetWidgetByDashboardId(int dashboardId)
       =>await _reportGrpcClientService.GetWidgetByDashboardId(dashboardId);
        [HttpPost]
        public async Task<ChartDto> GetChart(int widgetId, List<ConditionValue> conditionValue)
            => await _reportGrpcClientService.GetChart(widgetId, conditionValue);
        [HttpPost]
        public async Task<GridDto> GetGrid(int widgetId, List<ConditionValue> conditionValue)
            => await _reportGrpcClientService.GetGrid(widgetId, conditionValue);
        [HttpPost]
        public async Task<TestDto> TestNullable()
        => await  _reportGrpcClientService.TestNullable();

    }
}
