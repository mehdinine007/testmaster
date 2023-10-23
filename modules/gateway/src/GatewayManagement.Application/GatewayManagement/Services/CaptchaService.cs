#region NS
using UserManagement.Application.Contracts.Models.Captcha;
using GatewayManagement.Application.Contracts.GatewayManagement.Dtos;
using GatewayManagement.Application.Contracts.GatewayManagement.IServices;
using Newtonsoft.Json;
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
                Success = recaptcha.IsSuccessStatusCode,
                Error = await recaptcha.Content.ReadAsStringAsync(),
                ErrorCode = recaptcha.StatusCode.ToString(),
            };

            string stringContent = "";

            if (!httpResponse.Success)
            {
                httpResponse = new HttpResponseMessageDto() { Success = false, Error = "Unable to verify recaptcha token", ErrorCode = "S03" };
                return httpResponse;
            }
            if (string.IsNullOrEmpty(stringContent))
            {
                httpResponse = new HttpResponseMessageDto() { Success = false, Error = "Invalid reCAPTCHA verification response", ErrorCode = "S04" };
                return httpResponse;
            }
            var googleReCaptchaResponse = JsonConvert.DeserializeObject<ReCaptchaResponse>(stringContent);
            if (!googleReCaptchaResponse.Success)
            {
                var errors = string.Join(",", googleReCaptchaResponse.ErrorCodes);
                httpResponse = new HttpResponseMessageDto() { Success = false, Error = errors, ErrorCode = "S05" };
                return httpResponse;
            }

            if (googleReCaptchaResponse.Score < 0.5)
            {
                httpResponse = new HttpResponseMessageDto() { Success = false, Error = "This is a potential bot. Signup request rejected", ErrorCode = "S07" };
                return httpResponse;
            }
            return new HttpResponseMessageDto() { Success = true};

        }
    }
}
