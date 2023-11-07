using AdminPanelManagement.Application.Contracts.AdminPanelManagement.Dtos.report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace AdminPanelManagement.Application.Contracts.AdminPanelManagement.IServices
{
    public interface IReportGrpcClientService:IApplicationService
    {
        Task<List<DashboardDto>> GetAllDashboard();
        Task<TestDto> TestNullable();
        Task<List<WidgetDto>> GetWidgetByDashboardId(int dashboardId);
        Task<ChartDto> GetChart(int widgetId, List<ConditionValue> conditionValue);
        Task<GridDto> GetGrid(int widgetId, List<ConditionValue> conditionValue);




    }
}
