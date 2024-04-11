using Volo.Abp.Auditing;
using Volo.Abp;
using Microsoft.AspNetCore.Mvc;
using Esale.Share.Authorize;
using OrderManagement.Application.Contracts.OrderManagement.Services;
using System.Threading.Tasks;
using OrderManagement.Application.Contracts;
using OrderManagement.Application.Contracts.OrderManagement;
using Volo.Abp.AspNetCore.Mvc;
using System.Collections.Generic;
using Nest;
using System;
using OrderManagement.Domain.Shared;
using IFG.Core.Utility.Tools;
using Microsoft.AspNetCore.Http;
using OrderManagement.Domain.Shared.OrderManagement.Enums;

namespace OrderManagement.HttpApi.OrderManagement.Controllers;

[DisableAuditing]
[RemoteService]
[Route("api/services/app/ProductAndCategory/[action]")]
//[UserAuthorization]
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
    public async Task<ProductAndCategoryWithChildDto> GetById(int id, bool hasProperty, string attachmentType, string attachmentlocation)
        => await _productAndCategoryService.GetById(id, hasProperty, EnumHelper.ConvertStringToEnum<AttachmentEntityTypeEnum>(attachmentType), EnumHelper.ConvertStringToEnum<AttachmentLocationEnum>(attachmentlocation));

    [HttpGet]
    public async Task<ProductAndCategoryAddDto> GetByAdd(int? productId)
        => await _productAndCategoryService.GetByAdd(productId);
    [HttpPost]
    public async Task<ProductAndCategoryDto> Insert([FromBody] ProductAndCategoryCreateDto productAndCategoryCreateDto)
        => await _productAndCategoryService.Insert(productAndCategoryCreateDto);

    [HttpPost]
    public async Task<Guid> UploadFile([FromForm] UploadFileDto uploadFileDto)
        => await _productAndCategoryService.UploadFile(uploadFileDto);

    [HttpPost]
    public async Task<CustomPagedResultDto<ProductAndCategoryDto>> GetPagination([FromBody] ProductAndCategoryQueryDto input)
        => await _productAndCategoryService.GetListWithPagination(input);

    [HttpGet]
    public async Task<List<ProductAndCategoryWithChildDto>> GetList(ProductAndCategoryGetListQueryDto input)
        => await _productAndCategoryService.GetList(input);

    [HttpPut]
    public async Task<ProductAndCategoryDto> Update(ProductAndCategoryUpdateDto productAndCategoryUpdateDto)
        => await _productAndCategoryService.Update(productAndCategoryUpdateDto);

    [HttpGet]
    public async Task<List<ProductAndCategoryWithChildDto>> GetProductAndSaleDetailList([FromQuery]ProductAndSaleDetailGetListQueryDto input)
        => await _productAndCategoryService.GetProductAndSaleDetailList(input);
    [HttpPut]
    public async Task<bool> Move(MoveDto moveDto)
       => await _productAndCategoryService.Move(moveDto);
    [HttpPost]
    public async Task<bool> Import([FromForm] ImportExcelDto importExcelDto)
      => await _productAndCategoryService.Import(importExcelDto);

}