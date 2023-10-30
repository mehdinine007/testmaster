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
    [Route("api/services/app/DashboardService/[action]")]
    //[UserAuthorization]
    public class DashboardController: Controller
    {
        private readonly IDashboardService _dashboardService;
        public DashboardController(IDashboardService dashboardService)
        => _dashboardService = dashboardService;
        [HttpPost]
        public Task<DashboardDto> Add(DashboardCreateOrUpdateDto dashboardCreateOrUpdateDto)
        =>_dashboardService.Add(dashboardCreateOrUpdateDto);
        [HttpPost]
        public Task<DashboardWidgetDto> AddDashboardWidget(DashboardWidgetCreateOrUpdateDto dashboardWidgetCreateOrUpdateDto)
       =>_dashboardService.AddDashboardWidget(dashboardWidgetCreateOrUpdateDto);    

        [HttpDelete]
        public Task<bool> Delete(int id)
        =>_dashboardService.Delete(id);
        [HttpDelete]
        public Task<bool> DeleteDashboardWidget(int dashboardWidgetId)
        =>_dashboardService.DeleteDashboardWidget(dashboardWidgetId);   

        [HttpGet]
        public Task<DashboardDto> GetById(int id)
        =>_dashboardService.GetById(id);
        [HttpGet]
        public Task<List<DashboardDto>> GetList()
        =>_dashboardService.GetList();
        [HttpPut]
        public Task<DashboardDto> Update(DashboardCreateOrUpdateDto dashboardCreateOrUpdateDto)
       =>_dashboardService.Update(dashboardCreateOrUpdateDto);
    }
}
