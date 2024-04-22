using Esale.Share.Authorize;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using OrderManagement.Application.Contracts;
using OrderManagement.Application.Contracts.OrderManagement;
using OrderManagement.Application.Contracts.OrderManagement.Services;
using OrderManagement.Application.Contracts.Services;
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


    //[HttpPost]
    //public async Task<bool> CreateSign()
    //      => await _signService.CreateSign();


   

}
