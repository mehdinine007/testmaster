using ReportManagement.Application.Contracts.ReportManagement.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace ReportManagement.Application.Contracts.ReportManagement.IServices
{
    public interface IDashboardWidgetService: IApplicationService
    {
        Task<DashboardWidgetDto> GetById(int id);
        Task<DashboardWidgetDto> Add(DashboardWidgetCreateOrUpdateDto dashboardWidgetCreateOrUpdateDto);
        Task<DashboardWidgetDto> Update(DashboardWidgetCreateOrUpdateDto dashboardWidgetCreateOrUpdateDto);
        Task<List<DashboardWidgetDto>> GetList();
        Task<bool> Delete(int id);
    }
}
