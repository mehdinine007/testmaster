using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace OrderManagement.Application.Contracts.Services
{
    public interface ICommonAppService : IApplicationService
    {
        Task<RecaptchaResponse> CheckCaptcha(CaptchaInputDto input);
        //Task<AdvocacyAcountResult> CheckAccount(string nationalCode, string mobileNo);
        Task<bool> ValidateSMS(string Mobile, string NationalCode, string UserSMSCode, SMSType sMSType);
        Task ValidateVisualizeCaptcha(VisualCaptchaInput input);
        Task IsUserRejected();
        bool IsInRole(string Role);
        HttpClient GetHttpClientWithRquiredHeaderForFaraBoom();
        Task ValidateCustomerBirthDate(List<UserOrderDto> users, CancellationToken cancelationToke);
        Task ValidateCustomerPhoneNumber(List<UserOrderMobileDto> userOrders, CancellationToken cancellationToken);
        string GetNationalCode();
        //Task<string> GetRole();
        long GetUserId();
    }
}
