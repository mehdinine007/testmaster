using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Auditing;
using Volo.Abp;
using Microsoft.AspNetCore.Mvc;
using AdminPanelManagement.Application.Contracts.AdminPanelManagement.Dtos;
using AdminPanelManagement.Application.Contracts.AdminPanelManagement.IServices;
using Microsoft.AspNetCore.Mvc.Versioning;
using AdminPanelManagement.Application.Contracts.IServices;
using AdminPanelManagement.Application.Contracts.AdminPanelManagement.Dtos.report;

namespace AdminPanelManagement.HttpApi.AdminPanelManagement.Controllers
{
    [DisableAuditing]
    [RemoteService]
    [Route("api/services/app/ReportService/[action]")]
    public class ReportController: Controller
    {

        private readonly IReportService _reportService;

        public ReportController(IReportService reportService)
        {
            _reportService = reportService;

        }
        [HttpPost]
        public async Task<List<ReportQuestionnaireDto>> ReportQuestionnaire(ReportQueryDto input)
        =>await _reportService.ReportQuestionnaire(input);
        [HttpGet]
        public async Task<List<DashboardDto>> GetAllDashboard()
       => await _reportService.GetAllDashboard();

        [HttpPost]
        public async Task<ChartDto> GetChart(ChartInputDto chartInputDto)
       => await _reportService.GetChart(chartInputDto.WidgetId, chartInputDto.ConditionValue);

        [HttpPost]
        public async Task<GridDto> GetGrid(ChartInputDto chartInputDto)
       => await _reportService.GetGrid(chartInputDto.WidgetId, chartInputDto.ConditionValue);

        [HttpGet]
        public async Task<List<WidgetDto>> GetWidgetByDashboardId(int widgetId)
      => await _reportService.GetWidgetByDashboardId(widgetId);


    }
}
