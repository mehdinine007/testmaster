using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Auditing;
using Volo.Abp;
using WorkFlowManagement.Application.Contracts.WorkFlowManagement.IServices;
using WorkFlowManagement.Domain.WorkFlowManagement;
using WorkFlowManagement.Application.Contracts.WorkFlowManagement.Dtos;

namespace WorkFlowManagement.HttpApi.WorkFlowManagement.Controllers
{
    [DisableAuditing]
    [RemoteService]
    [Route("api/services/app/OrganizationChartService/[action]")]
    //[UserAuthorization]
    public class OrganizationChartController : Controller
    {
        private readonly IOrganizationChartService _organizationChartService;
        public OrganizationChartController(IOrganizationChartService organizationChartService)
        => _organizationChartService = organizationChartService;

        [HttpGet]
        public Task<OrganizationChartDto> GetById(int id)
    => _organizationChartService.GetById(id);


        [HttpGet]
        public Task<List<OrganizationChartDto>> GetList()
        => _organizationChartService.GetList();


        [HttpPost]
        public Task<OrganizationChartDto> Add(OrganizationChartCreateOrUpdateDto organizationChartCreateOrUpdateDto)
        => _organizationChartService.Add(organizationChartCreateOrUpdateDto);

        [HttpPut]
        public Task<OrganizationChartDto> Update(OrganizationChartCreateOrUpdateDto organizationChartCreateOrUpdateDto)
        => _organizationChartService.Update(organizationChartCreateOrUpdateDto);

        [HttpDelete]
        public Task<bool> Delete(int id)
        => _organizationChartService.Delete(id);
    }
}
