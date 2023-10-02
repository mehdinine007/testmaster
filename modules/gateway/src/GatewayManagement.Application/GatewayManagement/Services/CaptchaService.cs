#region NS
using GatewayManagement.Application.Contracts.GatewayManagement.IServices;
using GatewayManagement.Application.Contracts.GatewayManagement.Dtos;
using Volo.Abp.Application.Services;
#endregion

namespace GatewayManagement.Application.GatewayManagement.Services
{
    public class CaptchaService : ApplicationService, ICaptchaService
    {
        public async Task<HttpResponseMessageDto> ReCaptcha(ContentInputDto Content)
        {
            var dictionary = new Dictionary<string, string>()
         {
             { "secret", Content.ContentValue },
             { "response" , Content.KeyValue }
         };
            var postContent = new FormUrlEncodedContent(dictionary);
            HttpResponseMessage recaptcha = null;
            HttpResponseMessageDto httpResponse = null;
            var http = new HttpClient();

            recaptcha = await http.PostAsync("https://www.google.com/recaptcha/api/siteverify", postContent);

            httpResponse = new HttpResponseMessageDto()
            {
                IsSuccessStatusCode = recaptcha.IsSuccessStatusCode,
                StringContent = await recaptcha.Content.ReadAsStringAsync()
            };

            return httpResponse;
        }
    }
}
