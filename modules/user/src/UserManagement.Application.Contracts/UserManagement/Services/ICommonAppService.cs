using Abp.Application.Services;
using UserManagement.Application.Contracts.Models;
using UserManagement.Domain.Shared;

namespace UserManagement.Application.Contracts.Services;

public interface ICommonAppService : IApplicationService
{
    Task<RecaptchaResponse> CheckCaptcha(CaptchaInputDto input);

    Task<bool> ValidateMobileNumber(string nationalCode, string mobileNo);

    Task<bool> ValidateSMS(string Mobile, string NationalCode, string UserSMSCode, SMSType sMSType);

    Task<string> GetAddressByZipCode(string zipCode, string nationalCode);

    //Task<AdvocacyAcountResult> CheckAccount(string nationalCode, string mobileNo);
}