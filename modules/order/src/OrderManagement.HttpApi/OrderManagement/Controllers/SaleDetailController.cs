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
    public  async Task<List<SaleDetailDto>> GetActiveList()
    => await _saleDetailService.GetActiveList();
    [HttpGet]
    public async Task<SaleDetailDto> GetById(int id)
    =>await _saleDetailService.GetById(id);

    [HttpGet]
    public async Task<PagedResultDto<SaleDetailDto>> GetSaleDetails(BaseInquery input)
      => await _saleDetailService.GetSaleDetails(input);
    [HttpPost]
    public async Task<SaleDetailDto> Save(CreateSaleDetailDto createSaleDetailDto)
      => await _saleDetailService.Save(createSaleDetailDto);
    [HttpPut]
    public async Task<SaleDetailDto> Update(CreateSaleDetailDto createSaleDetailDto)
    => await _saleDetailService.Update(createSaleDetailDto);

    [HttpGet]
    public async Task<List<SaleDetailForDropDownDto>> GetAll()
          =>await _saleDetailService.GetAll();

    [HttpGet]
    public async Task<List<SaleDetailDto>> GetList(int? saleId)
          =>await _saleDetailService.GetList(saleId);
}
