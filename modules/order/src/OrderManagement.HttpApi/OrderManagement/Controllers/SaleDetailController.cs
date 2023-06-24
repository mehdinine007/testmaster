using Microsoft.AspNetCore.Mvc;
using OrderManagement.Application.Contracts.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Auditing;
using Volo.Abp;
using OrderManagement.Application.Contracts.OrderManagement.Services;
using OrderManagement.Application.Contracts;
using OrderManagement.Application.OrderManagement.Implementations;
using Nest;
using OrderManagement.Application.Contracts.OrderManagement;
using Volo.Abp.Application.Dtos;

namespace OrderManagement.HttpApi.OrderManagement.Controllers
{
    [DisableAuditing]
    [RemoteService]
    [Route("api/services/app/SaleDetailService/[action]")]
    public class SaleDetailController:Controller
    {
        private readonly ISaleDetailService _saleDetailService;
        public SaleDetailController(ISaleDetailService saleDetailService)
            => _saleDetailService = saleDetailService;

        [HttpDelete]
        public async Task Delete(int id)
          => await _saleDetailService.Delete(id);
        [HttpGet]
        public async Task<PagedResultDto<SaleDetailDto>> GetSaleDetails(int pageNo, int sizeNo)
          => await _saleDetailService.GetSaleDetails(pageNo,sizeNo);
        [HttpPost]
        public async Task<int> Save(CreateSaleDetailDto createSaleDetailDto)
          => await _saleDetailService.Save(createSaleDetailDto);
        [HttpPut]
        public async Task<int> Update(CreateSaleDetailDto createSaleDetailDto)
        => await _saleDetailService.Update(createSaleDetailDto);
    }
}
