using Esale.Core.Utility.Results;
using GatewayManagement.Application.Contracts.GatewayManagement.Dtos.Esale;
using GatewayManagement.Application.Contracts.GatewayManagement.IServices;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using GatewayManagement.Application.Contracts.GatewayManagement.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace GatewayManagement.Application.GatewayManagement.Services
{
    public class CaptchaService : ApplicationService, ICaptchaService
    {
        public CaptchaService()
        {
        }
        public async Task<HttpResponseMessageDto> GetCaptcha(ContentInputDto Content)
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
