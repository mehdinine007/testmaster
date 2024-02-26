using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Auditing;
using Volo.Abp;
using OrderManagement.Application.Contracts.Services;
using System.Threading.Tasks;
using OrderManagement.Application.Contracts;

namespace OrderManagement.HttpApi.Controllers;

[DisableAuditing]
[RemoteService]
[Route("api/services/app/SeasonCompanyProductService/[action]")]
public class SeasonCompanyProductController : Controller
{
    private readonly ISeasonCompanyProductService _seasonCompanyProductService;

    public SeasonCompanyProductController(ISeasonCompanyProductService seasonCompanyProductService)
        => _seasonCompanyProductService = seasonCompanyProductService;

    [HttpPost]
    public async Task<SeasonCompanyProductDto> Create([FromBody] SeasonCompanyProductDto seasonCompanyProductDto)
        => await _seasonCompanyProductService.Create(seasonCompanyProductDto);

    [HttpDelete]
    public async Task Delete(int seasonCompanyProductId)
        => await _seasonCompanyProductService.Delete(seasonCompanyProductId);

    [HttpGet]
    public async Task<SeasonCompanyProductDto> GetById(int seasonCompanyProductId)
        => await _seasonCompanyProductService.GetById(seasonCompanyProductId);

    [HttpPut]
    public async Task<SeasonCompanyProductDto> Update(SeasonCompanyProductDto seasonCompanyProductDto)
        => await _seasonCompanyProductService.Update(seasonCompanyProductDto);
}
