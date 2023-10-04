#region NS
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using UserManagement.Application.Contracts.Models;
using UserManagement.Application.Contracts.Models.SendBox;
using UserManagement.Application.Contracts.UserManagement.Services;
using Volo.Abp.Application.Services;
using wsFava;
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
                KeyValue = Content.Token,

            });

            var mappedResponse = new RecaptchaResponse
            {
                Success = recaptchaResponse.Success,
                Error = recaptchaResponse.Error,
                ErrorCode = recaptchaResponse.ErrorCode
            };

            return mappedResponse;
        }
    }
}
