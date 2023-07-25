using Volo.Abp.Auditing;
using Volo.Abp;
using Microsoft.AspNetCore.Mvc;
using Esale.Share.Authorize;
using OrderManagement.Application.Contracts.OrderManagement.Services;
using System.Threading.Tasks;
using OrderManagement.Application.Contracts;
using OrderManagement.Application.Contracts.OrderManagement;
using Volo.Abp.AspNetCore.Mvc;

namespace OrderManagement.HttpApi.OrderManagement.Controllers;

[DisableAuditing]
[RemoteService]
[Route("api/services/app/ProductAndCategory/[action]")]
[UserAuthorization]
public class ProductAndCategoryController : AbpController //, IProductAndCategoryService
{
    private readonly IProductAndCategoryService _productAndCategoryService;

    public ProductAndCategoryController(IProductAndCategoryService productAndCategoryService)
        => _productAndCategoryService = productAndCategoryService;

    [HttpDelete]
    public async Task<bool> Delete(int id)
    {
        await _productAndCategoryService.Delete(id);
        return true;
    }

    [HttpGet]
    public async Task<ProductAndCategoryDto> Get(int id)
        => await _productAndCategoryService.Get(id);

    [HttpPost]
    public async Task<ProductAndCategoryDto> Insert([FromBody] ProductAndCategoryDto productAndCategoryDto)
        => await _productAndCategoryService.Insert(productAndCategoryDto);

    [HttpPost]
    public async Task<bool> UploadFile([FromForm] UploadFileDto uploadFileDto)
        => await _productAndCategoryService.UploadFile(uploadFileDto);
}
