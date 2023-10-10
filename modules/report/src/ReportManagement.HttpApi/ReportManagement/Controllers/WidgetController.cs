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
        [HttpGet]
        public Task<ChartDto> GetChart(int widgetId)
       =>_widgetService.GetChart(widgetId);

        [HttpGet]
        public Task<List<WidgetDto>> GetList(int Id)
       =>_widgetService.GetList(Id);
        [HttpPut]
        public Task<WidgetDto> Update(WidgetCreateOrUpdateDto widgetCreateOrUpdateDto)
        =>_widgetService.Update(widgetCreateOrUpdateDto);
    }
}
