using ReportManagement.Application.Contracts.ReportManagement.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace ReportManagement.Application.Contracts.ReportManagement.IServices
{
    public interface IDashboardService:IApplicationService
    {
        Task<DashboardDto> GetById(int id);
        Task<DashboardDto> Add(DashboardCreateOrUpdateDto dashboardCreateOrUpdateDto);
        Task<DashboardDto> Update(DashboardCreateOrUpdateDto dashboardCreateOrUpdateDto);
        Task<List<DashboardDto>> GetList();
        Task<bool> Delete(int id);
        Task<DashboardWidgetDto> AddDashboardWidget(DashboardWidgetCreateOrUpdateDto dashboardWidgetCreateOrUpdateDto);
        Task<bool> DeleteDashboardWidget(int dashboardWidgetId);

    }
}
