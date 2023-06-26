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
using Volo.Abp.Application.Dtos;

namespace OrderManagement.HttpApi.OrderManagement.Controllers
{
    [DisableAuditing]
    [RemoteService]
    [Route("api/services/app/SaleSchemaService/[action]")]
    public class SaleSchemaController : Controller, ISaleSchemaService
    {
        private readonly ISaleSchemaService _saleSchemaService;
        public SaleSchemaController(ISaleSchemaService saleSchemaService)
            => _saleSchemaService = saleSchemaService;


        [HttpDelete]
        public Task<bool> Delete(int id)
        => _saleSchemaService.Delete(id);
        [HttpGet]
        public Task<List<SaleSchemaDto>> GetAllSaleSchema()
        => _saleSchemaService.GetAllSaleSchema();
        [HttpGet]
        public Task<PagedResultDto<SaleSchemaDto>> GetSaleSchema(int pageNo, int sizeNo)
        => _saleSchemaService.GetSaleSchema(pageNo, sizeNo);
        [HttpPost]
        public Task<int> Save(SaleSchemaDto saleSchemaDto)
        => _saleSchemaService.Save(saleSchemaDto);
        [HttpPut    ]
        public Task<int> Update(SaleSchemaDto saleSchemaDto)
        => _saleSchemaService.Update(saleSchemaDto);
    }
}
