using Esale.Share.Authorize;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using OrderManagement.Application.Contracts;
using OrderManagement.Application.Contracts.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Auditing;

namespace OrderManagement.HttpApi;
[DisableAuditing]
[RemoteService]
[Route("api/services/app/SaleService/[action]")]
public class SaleController : Controller,ISaleService
{
    private readonly ISaleService _saleService;

    public SaleController(ISaleService saleService)
        => _saleService = saleService;


    [HttpGet]
    [UserAuthorization]
    public async Task<List<PreSaleDto>> GetPreSales()
          => await _saleService.GetPreSales();


    [HttpGet]
    public async Task<SaleDetailDto> GetSaleDetail(Guid uid)
        => await _saleService.GetSaleDetail(uid);

    [HttpGet]
    public async Task<List<SaleDetailDto>> GetSaleDetails(int tipId, int typeId, int familyId, int companyId, int esaleTypeId)
        => await _saleService.GetSaleDetails(tipId,typeId,familyId,companyId,esaleTypeId);

    [HttpGet]
    [RemoteService(IsEnabled =false)]
    public async Task<List<SalePlanDto>> GetSalePlans(int companyId)
        => await _saleService.GetSalePlans(companyId);

    [HttpGet]
    public async Task<List<ESaleTypeDto>> GetSaleTypes()
        => await _saleService.GetSaleTypes();

    [HttpGet]
    [RemoteService(IsEnabled = false)]
    [UserAuthorization]
    public async Task UserValidationByBirthDate(int saleId)
        => await _saleService.UserValidationByBirthDate(saleId);

    [HttpGet]
    [RemoteService(IsEnabled = false)]
    [UserAuthorization]
    public async Task UserValidationByMobile(int saleId)
        => await _saleService.UserValidationByMobile(saleId);

    [HttpGet]
    public async Task<object> TestIPG()
    {
        return await _saleService.TestIPG();
    }
}
