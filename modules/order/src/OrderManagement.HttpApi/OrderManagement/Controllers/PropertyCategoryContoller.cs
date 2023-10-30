using Microsoft.AspNetCore.Mvc;
using OrderManagement.Application.Contracts;
using OrderManagement.Application.Contracts.OrderManagement.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Auditing;

namespace OrderManagement.HttpApi.OrderManagement.Controllers;

[DisableAuditing]
[RemoteService]
[Route("api/services/app/[controller]/[action]")]
public class PropertyCategoryController : Controller
{
    private readonly IPropertyCategoryService _propertyCategoryService;
    public PropertyCategoryController(IPropertyCategoryService propertyCategoryService)
    {
        _propertyCategoryService = propertyCategoryService;
    }

    [HttpGet]
    public async Task<List<PropertyCategoryDto>> GetList()
    => await _propertyCategoryService.GetList();

    [HttpGet]
    public async Task<PropertyCategoryDto> GetById(string Id)
    => await _propertyCategoryService.GetById(Id);

    [HttpPost]
    public async Task<PropertyCategoryDto> Add(PropertyCategoryDto propertyCategoryDto)
    => await _propertyCategoryService.Add(propertyCategoryDto);

    [HttpPut]
    public async Task<PropertyCategoryDto> Update(PropertyCategoryDto propertyCategoryDto)
    => await _propertyCategoryService.Update(propertyCategoryDto);

    [HttpDelete]
    public async Task<bool> Delete(string Id)
    => await _propertyCategoryService.Delete(Id);
}