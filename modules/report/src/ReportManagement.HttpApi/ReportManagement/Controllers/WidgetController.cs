using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Auditing;
using Volo.Abp;
using Microsoft.AspNetCore.Mvc;
using ReportManagement.Application.Contracts.ReportManagement.IServices;
using ReportManagement.Application.ReportManagement.Implementations;
using ReportManagement.Application.Contracts.ReportManagement.Dtos;
using ReportManagement.Domain.Shared.ReportManagement.Dtos;

namespace ReportManagement.HttpApi.ReportManagement.Controllers
{
    [DisableAuditing]
    [RemoteService]
    [Route("api/services/app/WidgetService/[action]")]
    //[UserAuthorization]
    public class WidgetController: Controller
    {
        private readonly IWidgetService _widgetService;
        public WidgetController(IWidgetService widgetService)
        => _widgetService = widgetService;
        [HttpPost]
        public Task<WidgetDto> Add(WidgetCreateOrUpdateDto widgetCreateOrUpdateDto)
       =>_widgetService.Add(widgetCreateOrUpdateDto);
        [HttpDelete]
        public Task<bool> Delete(int id)
        =>_widgetService.Delete(id);
        [HttpGet]
        public Task<WidgetDto> GetById(int id)
       =>_widgetService.GetById(id);
        
        [HttpPost]
        public Task<ChartDto> GetChart(ChartInputDto chartInputDto)
       =>_widgetService.GetChart(chartInputDto.WidgetId, chartInputDto.ConditionValue);

        [HttpGet]
        public Task<List<WidgetDto>> GetList(int dashboardId)
       =>_widgetService.GetList(dashboardId);
        [HttpPut]
        public Task<WidgetDto> Update(WidgetCreateOrUpdateDto widgetCreateOrUpdateDto)
        =>_widgetService.Update(widgetCreateOrUpdateDto);
        [HttpPost]
        public Task<GridDto> GetGrid(ChartInputDto chartInputDto)
      => _widgetService.GetGrid(chartInputDto.WidgetId, chartInputDto.ConditionValue);
    }
}
