using Microsoft.EntityFrameworkCore;
using ReportManagement.Application.Contracts.ReportManagement.Dtos;
using ReportManagement.Application.Contracts.ReportManagement.IServices;
using ReportManagement.Application.Contracts.WorkFlowManagement.Constants;
using ReportManagement.Domain.ReportManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;

namespace ReportManagement.Application.ReportManagement.Implementations
{
    public class DashboardWidgetService : ApplicationService, IDashboardWidgetService
    {
        private readonly IRepository<DashboardWidget, int> _dashboardWidgetRepository;
        private readonly IDashboardService _dashboardService;
        private readonly IWidgetService _widgetService;
        public DashboardWidgetService(IRepository<DashboardWidget, int> dashboardWidgetRepository, IDashboardService dashboardService, IWidgetService widgetService)
        {
            _dashboardWidgetRepository = dashboardWidgetRepository;
            _dashboardService = dashboardService;
            _widgetService = widgetService;

        }

        public async Task<DashboardWidgetDto> Add(DashboardWidgetCreateOrUpdateDto dashboardWidgetCreateOrUpdateDto)
        {
            await Validation(null, dashboardWidgetCreateOrUpdateDto);
            var dashboardWidget = ObjectMapper.Map<DashboardWidgetCreateOrUpdateDto, DashboardWidget>(dashboardWidgetCreateOrUpdateDto);
            var entity = await _dashboardWidgetRepository.InsertAsync(dashboardWidget, autoSave: true);
            return await GetById(entity.Id);
        }

        public async Task<bool> Delete(int id)
        {
            var dashboardWidget = await Validation(id, null);
            await _dashboardWidgetRepository.DeleteAsync(id);
            return true;
        }

        public async Task<DashboardWidgetDto> GetById(int id)
        {
            var dashboardWidget = await Validation(id, null);
            var dashboardWidgetDto = ObjectMapper.Map<DashboardWidget, DashboardWidgetDto>(dashboardWidget);
            return dashboardWidgetDto;
        }

        public async Task<List<DashboardWidgetDto>> GetList()
        {
            var dashboardWidget = (await _dashboardWidgetRepository.GetQueryableAsync()).ToList();
            var dashboardWidgetDto = ObjectMapper.Map<List<DashboardWidget>, List<DashboardWidgetDto>>(dashboardWidget);
            return dashboardWidgetDto;
        }

        public async Task<DashboardWidgetDto> Update(DashboardWidgetCreateOrUpdateDto dashboardWidgetCreateOrUpdateDto)
        {
            var dashboardWidget = await Validation(dashboardWidgetCreateOrUpdateDto.Id, dashboardWidgetCreateOrUpdateDto);
            dashboardWidget.DashboardId = dashboardWidgetCreateOrUpdateDto.DashboardId;
            dashboardWidget.WidgetId = dashboardWidgetCreateOrUpdateDto.WidgetId;
            var entity = await _dashboardWidgetRepository.UpdateAsync(dashboardWidget);
            return ObjectMapper.Map<DashboardWidget, DashboardWidgetDto>(entity);
        }

        private async Task<DashboardWidget> Validation(int? id, DashboardWidgetCreateOrUpdateDto dashboardWidgetCreateOrUpdateDto)
        {
            var dashboardQuery = (await _dashboardWidgetRepository.GetQueryableAsync()).Include(x=>x.Widget).Include(x=>x.Dashboard);
            DashboardWidget dashboardWidget = new DashboardWidget();
            if (id is not null)
            {
                dashboardWidget = dashboardQuery.FirstOrDefault(x => x.Id == id);
                if (dashboardWidget is null)
                {
                    throw new UserFriendlyException(ReportConstant.DashboardNotFound, ReportConstant.DashboardNotFoundId);
                }
            }
            if (dashboardWidgetCreateOrUpdateDto is not null)
            {
              var dashboard=await  _dashboardService.GetById(dashboardWidgetCreateOrUpdateDto.DashboardId);
              var widget = await  _widgetService.GetById(dashboardWidgetCreateOrUpdateDto.WidgetId);
            
            }



                return dashboardWidget;
        }



    }
}
