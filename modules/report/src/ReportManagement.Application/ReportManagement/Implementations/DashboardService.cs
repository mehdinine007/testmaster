using NPOI.OpenXmlFormats.Dml;
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
    public class DashboardService : ApplicationService, IDashboardService
    {
        private readonly IRepository<Dashboard, int> _dashboardRepository;
        private readonly IRepository<DashboardWidget, int> _dashboardWidgetRepository;
        private readonly  IWidgetService _widgetService;
        public DashboardService(IRepository<Dashboard, int> dashboardRepository, IRepository<DashboardWidget, int> dashboardWidgetRepository
            , IWidgetService widgetService)
        {
            _dashboardRepository = dashboardRepository;
            _dashboardWidgetRepository = dashboardWidgetRepository;
            _widgetService = widgetService;
        }


        public async Task<DashboardDto> Add(DashboardCreateOrUpdateDto dashboardCreateOrUpdateDto)
        {
            var dashboard = ObjectMapper.Map<DashboardCreateOrUpdateDto, Dashboard>(dashboardCreateOrUpdateDto);
            var entity = await _dashboardRepository.InsertAsync(dashboard, autoSave: true);
            return ObjectMapper.Map<Dashboard, DashboardDto>(entity);
        }

        public async Task<bool> Delete(int id)
        {
            var dashboard = await Validation(id, null);
            await _dashboardRepository.DeleteAsync(id);
            return true;
        }

        public async Task<DashboardDto> GetById(int id)
        {
            var dashboard = await Validation(id, null);
            var dashboardDto = ObjectMapper.Map<Dashboard, DashboardDto>(dashboard);
            return dashboardDto;
        }

        public async Task<List<DashboardDto>> GetList()
        {
            var dashboard = (await _dashboardRepository.GetQueryableAsync()).ToList();
            var dashboardDto = ObjectMapper.Map<List<Dashboard>, List<DashboardDto>>(dashboard);
            return dashboardDto;
        }

        public async Task<DashboardDto> Update(DashboardCreateOrUpdateDto dashboardCreateOrUpdateDto)
        {
            var dashboard = await Validation(dashboardCreateOrUpdateDto.Id, dashboardCreateOrUpdateDto);
            dashboard.Title = dashboardCreateOrUpdateDto.Title;
            dashboard.Priority = dashboardCreateOrUpdateDto.Priority;
            var entity = await _dashboardRepository.UpdateAsync(dashboard);
            return ObjectMapper.Map<Dashboard, DashboardDto>(entity);
        }

        private async Task<Dashboard> Validation(int? id, DashboardCreateOrUpdateDto dashboardCreateOrUpdateDto)
        {
            var dashboardQuery = await _dashboardRepository.GetQueryableAsync();
            Dashboard dashboard = new Dashboard();
            if (id is not null)
            {
                dashboard = dashboardQuery.FirstOrDefault(x => x.Id == id);
                if (dashboard is null)
                {
                    throw new UserFriendlyException(ReportConstant.DashboardNotFound, ReportConstant.DashboardNotFoundId);
                }
            }

            return dashboard;
        }


        public async Task<DashboardWidgetDto> AddDashboardWidget(DashboardWidgetCreateOrUpdateDto dashboardWidgetCreateOrUpdateDto)
        {
            var dashboardWidget = ObjectMapper.Map<DashboardWidgetCreateOrUpdateDto, DashboardWidget>(dashboardWidgetCreateOrUpdateDto);
          await  GetById(dashboardWidget.DashboardId);
            await _widgetService.GetById(dashboardWidget.WidgetId);
            var entity = await _dashboardWidgetRepository.InsertAsync(dashboardWidget, autoSave: true);
            return ObjectMapper.Map<DashboardWidget, DashboardWidgetDto>(entity);
        }

        public async Task<bool> DeleteDashboardWidget(int dashboardWidgetId)
        {
          var  dashboardWidgetQuery=await _dashboardWidgetRepository.GetQueryableAsync();
           var dashboardWidget= dashboardWidgetQuery.FirstOrDefault(x => x.Id == dashboardWidgetId);
            if (dashboardWidget is null)
            {
                throw new UserFriendlyException(ReportConstant.DashboardWidgetNotFound, ReportConstant.DashboardWidgetNotFoundId);
            }
            await _dashboardRepository.DeleteAsync(dashboardWidgetId);
            return true;
        }
    }
}
