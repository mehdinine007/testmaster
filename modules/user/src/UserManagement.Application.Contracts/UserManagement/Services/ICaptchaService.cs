#region NS
using UserManagement.Application.Contracts.Models;
using Volo.Abp.Application.Services;
#endregion

namespace UserManagement.Application.Contracts.UserManagement.Services
{
    public interface ICaptchaService : IApplicationService
    {
        Task<RecaptchaResponse> ReCaptcha(CaptchaInputDto Content);
    }
}
