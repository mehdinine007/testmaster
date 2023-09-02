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

namespace WorkFlowManagement.HttpApi.WorkFlowManagement.Controllers
{
    [DisableAuditing]
    [RemoteService]
    [Route("api/services/app/SchemeService/[action]")]
    //[UserAuthorization]
    public class SchemeController: Controller
    {

        private readonly ISchemeService _schemeService;
        public SchemeController(ISchemeService schemeService)
        => _schemeService = schemeService;
        [HttpPost]
        public Task<SchemeDto> Add(SchemeCreateOrUpdateDto schemeCreateOrUpdateDto)
        =>_schemeService.Add(schemeCreateOrUpdateDto);
        [HttpDelete]
        public Task<bool> Delete(int id)
        =>_schemeService.Delete(id);
        [HttpGet]
        public Task<SchemeDto> GetById(int id)
        =>_schemeService.GetById(id);
        [HttpGet]
        public Task<List<SchemeDto>> GetList()
        => _schemeService.GetList();
        [HttpPut]
        public Task<SchemeDto> Update(SchemeCreateOrUpdateDto schemeCreateOrUpdateDto)
        =>_schemeService.Update(schemeCreateOrUpdateDto);
    }
}
