using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Auditing;
using Volo.Abp;
using Microsoft.AspNetCore.Mvc;
using OrderManagement.Application.Contracts.OrderManagement.Services;
using Volo.Abp.Application.Dtos;
using OrderManagement.Application.Contracts.OrderManagement;

namespace OrderManagement.HttpApi.OrderManagement.Controllers;

[DisableAuditing]
[RemoteService]
[Route("api/services/app/ColorService/[action]")]
public class ColorController : Controller, IColorService
{
    private readonly IColorService _colorService;
    public ColorController(IColorService colorService)
        => _colorService = colorService;

    [HttpDelete]
    public Task<bool> Delete(int id)
    =>_colorService.Delete(id);
    [HttpGet]
    public Task<List<ColorDto>> GetAllColors()
    =>_colorService.GetAllColors();

    [HttpGet]
    public Task<PagedResultDto<ColorDto>> GetColors(int pageNo, int sizeNo)
    =>_colorService.GetColors(pageNo, sizeNo);
    [HttpPost]
    public Task<int> Save(ColorDto colorDto)
    =>_colorService.Save(colorDto);
    [HttpPut]
    public Task<int> Update(ColorDto colorDto)
    =>_colorService.Update(colorDto);
}
