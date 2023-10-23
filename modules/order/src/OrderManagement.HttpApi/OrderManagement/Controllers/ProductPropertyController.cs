using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Auditing;
using Volo.Abp;
using Microsoft.AspNetCore.Mvc;
using OrderManagement.Application.Contracts.OrderManagement.Services;
using OrderManagement.Application.Contracts.OrderManagement;
using OrderManagement.Application.Contracts;
using MongoDB.Bson;

namespace OrderManagement.HttpApi.OrderManagement.Controllers
{
    [DisableAuditing]
    [RemoteService]
    [Route("api/services/app/ProductPropertyService/[action]")]
    //[UserAuthorization]
    public class ProductPropertyController : Controller , IProductPropertyService
    {
        private readonly IProductPropertyService _productPropertyService;
        public ProductPropertyController(IProductPropertyService productPropertyService)
            => _productPropertyService = productPropertyService;

        [HttpGet]
        public async Task<List<PropertyCategoryDto>> GetByProductId(int productId)
        => await _productPropertyService.GetByProductId(productId);

        [HttpGet]
        public async Task<List<ProductPropertyDto>> GetList()
        => await _productPropertyService.GetList();

        [HttpGet]
        public async Task<ProductPropertyDto> GetById(ObjectId id)
        => await _productPropertyService.GetById(id);

        [HttpPost]
        public async Task<ProductPropertyDto> Add(ProductPropertyDto productPropertylDto)
        => await _productPropertyService.Add(productPropertylDto);

        [HttpPut]
        public async Task<ProductPropertyDto> Update(ProductPropertyDto productPropertylDto)
        => await _productPropertyService.Update(productPropertylDto);

        [HttpDelete]
        public async Task<bool> Delete(ObjectId id)
        => await _productPropertyService.Delete(id);
    }
}
