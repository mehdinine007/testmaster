using Esale.Share.Authorize;
using Microsoft.AspNetCore.Mvc;
using OrderManagement.Application.Contracts;
using OrderManagement.Application.Contracts.Services;
using OrderManagement.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Auditing;

namespace OrderManagement.HttpApi;

[DisableAuditing]
[RemoteService]
[Route("api/services/app/BaseInformationService/[action]")]
public class BaseController : IBaseInformationService
{
    private readonly IBaseInformationService _baseInformationService;

    public BaseController(IBaseInformationService baseInformationService)
        => _baseInformationService = baseInformationService;

    [HttpGet]
    [RemoteService(IsEnabled = false)]
    public async Task CheckAdvocacyPrice(decimal MinimumAmountOfProxyDeposit)
        => await _baseInformationService.CheckAdvocacyPrice(MinimumAmountOfProxyDeposit);

    [HttpGet]
    [RemoteService(IsEnabled = false)]
    public void CheckBlackList(int esaleTypeId)
        => _baseInformationService.CheckBlackList(esaleTypeId);

    [HttpGet]
    [RemoteService(IsEnabled = false)]
    public void CheckWhiteList(WhiteListEnumType whiteListEnumType, string Nationalcode = "")
        => _baseInformationService.CheckWhiteList(whiteListEnumType, Nationalcode);

    //[HttpGet]
    //[UserAuthorization]
    //public async Task<List<CarTipDto>> GetCarTipsByCompanyId(int companyId)
    //    => await _baseInformationService.GetCarTipsByCompanyId(companyId);

    [HttpGet]
    public List<PublicDto> GetCities(int ProvienceId)
        => _baseInformationService.GetCities(ProvienceId);

    [HttpGet]
    public List<CompanyDto> GetCompanies()
        => _baseInformationService.GetCompanies();

    [HttpGet]
    public List<PublicDto> GetProvince()
        => _baseInformationService.GetProvince();

    [HttpPost]
    
    public async Task RegistrationValidation()
        => await _baseInformationService.RegistrationValidation();

    [HttpGet]
    [RemoteService(false)]
    public void RegistrationValidationWithoutCaptcha(RegistrationValidationDto input)
        => _baseInformationService.RegistrationValidationWithoutCaptcha(input);

    [HttpGet]
    public async Task<UserDto> GrpcTest()
        => await _baseInformationService.GrpcTest();

    [HttpGet]
    public Task<List<ESaleTypeDto>> GetSaleTypes()
        => _baseInformationService.GetSaleTypes();

    [HttpGet]
    public async Task<List<AgencyDto>> GetAgencies(Guid saleDetailUid)
        => await _baseInformationService.GetAgencies(saleDetailUid);


    [HttpGet]
    public async Task ClearCache(string prefix)
        => await _baseInformationService.ClearCache(prefix);
}
