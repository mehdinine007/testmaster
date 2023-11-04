using Microsoft.AspNetCore.Mvc;
using OrderManagement.Application.Contracts.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Auditing;
using Volo.Abp;
using OrderManagement.Application.Contracts.OrderManagement.Services;
using OrderManagement.Application.Contracts;
using OrderManagement.Application.OrderManagement.Implementations;
using Nest;
using OrderManagement.Application.Contracts.OrderManagement;
using Volo.Abp.Application.Dtos;
using OrderManagement.Application.Contracts.OrderManagement.Inqueries;
using Esale.Share.Authorize;

namespace OrderManagement.HttpApi.OrderManagement.Controllers;

[DisableAuditing]
[RemoteService]
[Route("api/services/app/SaleDetailService/[action]")]
public class SaleDetailController:Controller, ISaleDetailService
{
    private readonly ISaleDetailService _saleDetailService;
    public SaleDetailController(ISaleDetailService saleDetailService)
        => _saleDetailService = saleDetailService;

    [HttpDelete]
    public async Task<bool> Delete(int id)
      => await _saleDetailService.Delete(id);
    [HttpGet]
    public List<SaleDetailDto> GetActiveList()
    => _saleDetailService.GetActiveList();
    [HttpGet]
    public SaleDetailDto GetById(int id)
    =>_saleDetailService.GetById(id);

    [HttpGet]
    public async Task<PagedResultDto<SaleDetailDto>> GetSaleDetails(BaseInquery input)
      => await _saleDetailService.GetSaleDetails(input);
    [HttpPost]
    public async Task<int> Save(CreateSaleDetailDto createSaleDetailDto)
      => await _saleDetailService.Save(createSaleDetailDto);
    [HttpPut]
    public async Task<int> Update(CreateSaleDetailDto createSaleDetailDto)
    => await _saleDetailService.Update(createSaleDetailDto);

    [HttpGet]
    public  List<SaleDetailForDropDownDto> GetAll()
          => _saleDetailService.GetAll();
}
