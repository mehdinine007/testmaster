using Volo.Abp.Auditing;
using Volo.Abp;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using UserManagement.Application.Contracts.Services;
using UserManagement.Application.Contracts.Models;

namespace UserManagement.HttpApi.UserManagement.Controllers;

[DisableAuditing]
[RemoteService]
[Route("api/services/app/[controller]/[action]")]
public class BankContoller : AbpController //, IBankAppService
{
    private readonly IBankAppService _bankAppService;

    public BankContoller(IBankAppService bankAppService)
        => _bankAppService = bankAppService;

    [HttpDelete]
    public async Task DeleteAdvocayUserFromBank(string nationalCode)
        => await _bankAppService.DeleteAdvocayUserFromBank(nationalCode);

    [HttpGet]
    public async Task<List<AdvocacyUsersFromBankWithCompanyDto>> GetAdvocacyUserByCompanyId()
        => await _bankAppService.GetAdvocacyUserByCompanyId();

    [HttpGet]
    public async Task<List<UserRejectionAdvocacyDto>> GetUserRejecttionAdvocacyList()
        => await _bankAppService.GetUserRejecttionAdvocacyList();

    [HttpGet]
    public async Task<AdvocacyUserFromBankExportDto> InquiryAdvocacyUserReport(string nationalCode)
        => await _bankAppService.InquiryAdvocacyUserReport(nationalCode);

    [HttpGet]
    public async Task<UserRejecgtionFromBankExportDto> InquiryUserRejectionFromBank(string nationalCode)
        =>  await _bankAppService.InquiryUserRejectionFromBank(nationalCode);

    [HttpPost]
    public async Task SaveAdvocacyUsersFromBank(List<AdvocacyUsersFromBankDto> advocacyUsersFromBankDto)
        => await _bankAppService.SaveAdvocacyUsersFromBank(advocacyUsersFromBankDto);

    [HttpPost]
    public async Task SaveUserRejectionFromBank(UserRejectionFromBankDto userRejectionFromBankDto)
        => await _bankAppService.SaveUserRejectionFromBank(userRejectionFromBankDto);
}