using Abp.Application.Services;
using UserManagement.Application.Contracts.Models;
using UserManagement.Domain.Shared;
using UserManagement.Domain.UserManagement.CommonService.Dto;

namespace UserManagement.Application.Contracts.Services;

public interface ICommonAppService : IApplicationService
{
    Task<RecaptchaResponse> CheckCaptcha(CaptchaInputDto input);

    Task<bool> ValidateMobileNumber(string nationalCode, string mobileNo);

    Task<bool> ValidateSMS(string Mobile, string NationalCode, string UserSMSCode, SMSType sMSType);

    Task<string> GetAddressByZipCode(string zipCode, string nationalCode);

    public Guid GetUID();

    bool IsInRole(string Role);

    string GetRole();
    Task ValidateVisualizeCaptcha(VisualCaptchaInput input);
}