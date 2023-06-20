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
        public async  Task<int> Delete(int id)
            => await _agencySaleDetailService.Delete(id);
        [HttpGet]
        public async Task<List<AgencySaleDetailListDto>> GetAgencySaleDetail(int saleDetailId)
         => await _agencySaleDetailService.GetAgencySaleDetail(saleDetailId);
        [HttpPost]
        public async Task<int> Save(AgencySaleDetailDto agencySaleDetailDto)
         => await _agencySaleDetailService.Save(agencySaleDetailDto);
    }
}
