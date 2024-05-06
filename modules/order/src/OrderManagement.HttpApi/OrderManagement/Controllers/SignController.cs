using Esale.Share.Authorize;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using OrderManagement.Application.Contracts;
using OrderManagement.Application.Contracts.OrderManagement;
using OrderManagement.Application.Contracts.OrderManagement.Dtos.Sign;
using OrderManagement.Application.Contracts.OrderManagement.Services;
using OrderManagement.Application.Contracts.Services;
using OrderManagement.Application.OrderManagement.Implementations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Auditing;

namespace OrderManagement.HttpApi;
[DisableAuditing]
[RemoteService]
[Route("api/services/app/SignService/[action]")]
public class SignController : Controller
{
    private readonly ISignService _signService;

    public SignController(ISignService signService)
        => _signService = signService;


    [HttpPost]
    public async Task<Guid> ContractSign(ContractSignDto contractSignDto)
          => await _signService.ContractSign(contractSignDto);

    [HttpPost]
    public async Task<bool> CheckSignStatus()
        => await _signService.CheckSignStatus();
    
    [HttpGet]
    public async Task<InquirySignDto> Inquiry(Guid ticketId)
    {
        var _ret = await _signService.Inquiry(ticketId);
        if (_ret.Success)
        {
            return _ret.Data;
        }
        return null;
    }


}
