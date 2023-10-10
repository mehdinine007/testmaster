using ReportManagement.Application.Contracts.ReportManagement.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace ReportManagement.Application.Contracts.ReportManagement.IServices
{
    public interface IWidgetService:IApplicationService
    {

        Task<WidgetDto> GetById(int id);
        Task<WidgetDto> Add(WidgetCreateOrUpdateDto widgetCreateOrUpdateDto);
        Task<WidgetDto> Update(WidgetCreateOrUpdateDto widgetCreateOrUpdateDto);
        Task<List<WidgetDto>> GetList(int dashboardId);
        Task<ChartDto> GetChart(int widgetId);

        Task<bool> Delete(int id);

    }
}
