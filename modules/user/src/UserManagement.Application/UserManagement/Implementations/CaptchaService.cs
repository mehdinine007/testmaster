#region NS
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using UserManagement.Application.Contracts.Models;
using UserManagement.Application.Contracts.Models.SendBox;
using UserManagement.Application.Contracts.UserManagement.Services;
using Volo.Abp.Application.Services;
#endregion

namespace UserManagement.Application.UserManagement.Implementations
{
    public class CaptchaService : ApplicationService, ICaptchaService
    {
        private readonly IGetwayGrpcClient _grpcClient;
        private readonly IConfiguration _configuration;

        public CaptchaService(IGetwayGrpcClient grpcClient, IConfiguration configuration)
        {
            _grpcClient = grpcClient;
            _configuration = configuration;
        }

        public async Task<RecaptchaResponse> ReCaptcha(CaptchaInputDto Content)
        {
            var recaptchaResponse = await _grpcClient.ReCaptcha(new ContentInputDto
            {
                ContentValue = _configuration.GetValue<string>("RecaptchaSiteKey"),
                KeyValue = Content.Token
            });

            RecaptchaResponse signupResponse = null;
            string stringContent = "";

            if (!recaptchaResponse.IsSuccessStatusCode)
            {
                signupResponse = new RecaptchaResponse() { Success = false, Error = "Unable to verify recaptcha token", ErrorCode = "S03" };
                return signupResponse;
            }
            if (string.IsNullOrEmpty(stringContent))
            {
                signupResponse = new RecaptchaResponse() { Success = false, Error = "Invalid reCAPTCHA verification response", ErrorCode = "S04" };
                return signupResponse;
            }
            var googleReCaptchaResponse = JsonConvert.DeserializeObject<ReCaptchaResponse>(stringContent);
            if (!googleReCaptchaResponse.Success)
            {
                var errors = string.Join(",", googleReCaptchaResponse.ErrorCodes);
                signupResponse = new RecaptchaResponse() { Success = false, Error = errors, ErrorCode = "S05" };
                return signupResponse;
            }

            if (googleReCaptchaResponse.Score < 0.5)
            {
                signupResponse = new RecaptchaResponse() { Success = false, Error = "This is a potential bot. Signup request rejected", ErrorCode = "S07" };
                return signupResponse;
            }
            return new RecaptchaResponse() { Success = true };
        }
    }
}
