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
    [Route("api/services/app/DashboardWidgetService/[action]")]
    //[UserAuthorization]
    public class DashboardWidgetController: Controller
    {
        private readonly IDashboardWidgetService _dashboardWidgetService;
        public DashboardWidgetController(IDashboardWidgetService dashboardWidgetService)
        => _dashboardWidgetService = dashboardWidgetService;
        [HttpPost]
        public Task<DashboardWidgetDto> Add(DashboardWidgetCreateOrUpdateDto dashboardWidgetCreateOrUpdateDto)
       =>_dashboardWidgetService.Add(dashboardWidgetCreateOrUpdateDto);
        [HttpDelete]
        public Task<bool> Delete(int id)
        =>_dashboardWidgetService.Delete(id);
        [HttpGet]
        public Task<DashboardWidgetDto> GetById(int id)
       =>_dashboardWidgetService.GetById(id);
        [HttpGet]
        public Task<List<DashboardWidgetDto>> GetList()
        =>_dashboardWidgetService.GetList();
        [HttpPut]
        public Task<DashboardWidgetDto> Update(DashboardWidgetCreateOrUpdateDto dashboardWidgetCreateOrUpdateDto)
        =>_dashboardWidgetService.Update(dashboardWidgetCreateOrUpdateDto); 
    }
}
