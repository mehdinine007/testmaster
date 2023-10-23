using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Auditing;
using Volo.Abp;
using Microsoft.AspNetCore.Mvc;
using WorkFlowManagement.Application.Contracts.WorkFlowManagement.IServices;
using WorkFlowManagement.Application.Contracts.WorkFlowManagement.Dtos;
using WorkFlowManagement.Application.WorkFlowManagement.Implementations;

namespace WorkFlowManagement.HttpApi.WorkFlowManagement.Controllers
{
    [DisableAuditing]
    [RemoteService]
    [Route("api/services/app/OrganizationPositionService/[action]")]
    //[UserAuthorization]
    public class OrganizationPositionController: Controller
    {
        private readonly IOrganizationPositionService _organizationPositionService;
        public OrganizationPositionController(IOrganizationPositionService organizationPositionService)
        => _organizationPositionService = organizationPositionService;

        [HttpGet]
        public Task<OrganizationPositionDto> GetById(int id)
    => _organizationPositionService.GetById(id);


        [HttpGet]
        public Task<List<OrganizationPositionDto>> GetList(int organizationChartId)
        => _organizationPositionService.GetList(organizationChartId);


        [HttpPost]
        public Task<OrganizationPositionDto> Add(OrganizationPositionCreateOrUpdateDto organizationPositionCreateOrUpdateDto)
        => _organizationPositionService.Add(organizationPositionCreateOrUpdateDto);

        [HttpPut]
        public Task<OrganizationPositionDto> Update(OrganizationPositionCreateOrUpdateDto organizationPositionCreateOrUpdateDto)
        => _organizationPositionService.Update(organizationPositionCreateOrUpdateDto);

        [HttpDelete]
        public Task<bool> Delete(int id)
        => _organizationPositionService.Delete(id);


        [HttpPost]
        public Task<AuthenticateResponseDto> Authenticate(AuthenticateReqDto input)
            =>_organizationPositionService.Authenticate(input);


    }
}
