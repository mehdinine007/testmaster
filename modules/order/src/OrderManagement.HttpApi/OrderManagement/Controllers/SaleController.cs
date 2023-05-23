﻿using Esale.Share.Authorize;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using OrderManagement.Application.Contracts;
using OrderManagement.Application.Contracts.Services;
using PaymentManagement.Payments;
using ProtoBuf.Grpc.Client;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;

namespace OrderManagement.HttpApi;

[RemoteService]
[Route("api/services/app/SaleService/[action]")]
public class SaleController : ISaleService
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
    public async Task GrpcTest()
    {
        using (var channel = GrpcChannel.ForAddress("https://localhost:10042"))
        {
            var productAppService = channel.CreateGrpcService<IGrpcPaymentAppService>();
            var productDtos = await productAppService.GetListAsync();

            foreach (var productDto in productDtos)
            {
                Console.WriteLine($"[Product] Id = {productDto.Id}, Name = {productDto.Name}");
            }
        }

    }

}
