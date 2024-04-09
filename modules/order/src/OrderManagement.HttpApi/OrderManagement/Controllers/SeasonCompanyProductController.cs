using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Auditing;
using Volo.Abp;
using OrderManagement.Application.Contracts.Services;
using System.Threading.Tasks;
using OrderManagement.Application.Contracts;

namespace OrderManagement.HttpApi.Controllers;

[DisableAuditing]
[RemoteService]
[Route("api/services/app/SaleDetailAllocationService/[action]")]
public class SaleDetailAllocationController : Controller
{
    private readonly ISaleDetailAllocationService _seasonCompanyProductService;

    public SaleDetailAllocationController(ISaleDetailAllocationService seasonCompanyProductService)
        => _seasonCompanyProductService = seasonCompanyProductService;

    [HttpPost]
    public async Task<SaleDetailAllocationDto> Create([FromBody] SaleDetailAllocationDto seasonCompanyProductDto)
        => await _seasonCompanyProductService.Create(seasonCompanyProductDto);

    [HttpDelete]
    public async Task Delete(int seasonCompanyProductId)
        => await _seasonCompanyProductService.Delete(seasonCompanyProductId);

    [HttpGet]
    public async Task<SaleDetailAllocationDto> GetById(int seasonCompanyProductId)
        => await _seasonCompanyProductService.GetById(seasonCompanyProductId);

    [HttpPut]
    public async Task<SaleDetailAllocationDto> Update(SaleDetailAllocationDto seasonCompanyProductDto)
        => await _seasonCompanyProductService.Update(seasonCompanyProductDto);
}
