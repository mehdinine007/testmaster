
using AdminPanelManagement.Application.Contracts.AdminPanelManagement.Dtos;
using AdminPanelManagement.Application.Contracts.AdminPanelManagement.Dtos.report;
using AdminPanelManagement.Domain.Shared.AdminPanelManagement.Db;
using Volo.Abp.Application.Services;

namespace AdminPanelManagement.Application.Contracts.IServices
{
    public interface IReportService : IApplicationService
    {
        public Task<List<ReportQuestionnaireDto>> ReportQuestionnaire(ReportQueryDto input);
        Task<List<DashboardDto>> GetAllDashboard();
        Task<TestDto> TestNullable();
        Task<List<WidgetDto>> GetWidgetByDashboardId(int dashboardId);
        Task<ChartDto> GetChart(int widgetId, List<ConditionValue> conditionValue);
        Task<GridDto> GetGrid(int widgetId, List<ConditionValue> conditionValue);

    }
}
