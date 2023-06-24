using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Auditing;
using Volo.Abp;
using Microsoft.AspNetCore.Mvc;
using OrderManagement.Application.Contracts.OrderManagement.Services;
using OrderManagement.Application.Contracts.OrderManagement;
using Nest;
using OrderManagement.Domain;
using Volo.Abp.Application.Dtos;

namespace OrderManagement.HttpApi.OrderManagement.Controllers
{
    [DisableAuditing]
    [RemoteService]
    [Route("api/services/app/AgencySaleDetailService/[action]")]
    public class AgencySaleDetailController : Controller, IAgencySaleDetailService
    {
        private readonly IAgencySaleDetailService _agencySaleDetailService;
        public AgencySaleDetailController(IAgencySaleDetailService agencySaleDetailService)
            => _agencySaleDetailService = agencySaleDetailService;
        [HttpDelete]
        public async  Task Delete(int id)
            => await _agencySaleDetailService.Delete(id);
        [HttpGet]
        public async Task<PagedResultDto<AgencySaleDetailListDto>> GetAgencySaleDetail(int saleDetailId, int pageNo, int sizeNo)
         => await _agencySaleDetailService.GetAgencySaleDetail(saleDetailId,pageNo,sizeNo);
        [HttpPost]
        public async Task<int> Save(AgencySaleDetailDto agencySaleDetailDto)
         => await _agencySaleDetailService.Save(agencySaleDetailDto);
    }
}
