using Esale.Share.Authorize;
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

namespace OrderManagement.HttpApi.OrderManagement.Controllers
{
    [DisableAuditing]
    [RemoteService]
    [Route("api/services/app/ProductLevelService/[action]")]
    //[UserAuthorization]
    public class ProductLevelController : Controller, IProductLevelService
    {
        private readonly IProductLevelService _productLevelService;
        public ProductLevelController(IProductLevelService productLevelService)
            => _productLevelService = productLevelService;
        [HttpDelete]
        public Task<bool> Delete(int id)
       => _productLevelService.Delete(id);
        [HttpGet]
        public Task<List<ProductLevelDto>> GetList()
        =>_productLevelService.GetList();
        [HttpPost]
        public Task<int> Save(ProductLevelDto productLevelDto)
       => _productLevelService.Save(productLevelDto);
        [HttpPut]
        public Task<int> Update(ProductLevelDto productLevelDto)
        =>_productLevelService.Update(productLevelDto); 
    }
}
