using Esale.Core.Utility.Results;
using Microsoft.Extensions.Configuration;
using Volo.Abp;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net;
using System.Text;
using UserManagement.Application.Contracts.Models;

namespace UserManagement.Application.UserManagement.Implementations;

public class MagfaSendSms
{
    private MagfaConfig _config;
    public MagfaSendSms(MagfaConfig config)
    {
        _config = config;
    }
    public async Task<IDataResult<string>> Send(string text, string mobile)
    {
        var handler = new HttpClientHandler();
        handler.ClientCertificateOptions = ClientCertificateOption.Manual;
        handler.ServerCertificateCustomValidationCallback =
            (httpRequestMessage, cert, cetChain, policyErrors) =>
            {
                return true;
            };
        using (HttpClient client = new HttpClient(handler))
        {
            client.BaseAddress = new Uri(_config.BaseUrl);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var authenticationString = $"{_config.UserName + "/" + _config.Domain}:{_config.Password}";
            var base64EncodedAuthenticationString = Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(authenticationString));
            client.DefaultRequestHeaders.Add("Authorization", $"Basic {base64EncodedAuthenticationString}");
            var content = new StringContent(JsonConvert.SerializeObject(new MagfaBody()
            {
                messages = new List<string>() { text },
                recipients = new List<string>() { mobile },
                senders = new List<string>() { _config.SenderNumber }
            }), Encoding.UTF8, "application/json");

            var response = await client.PostAsync("/api/http/sms/v2/send", content);
            if (response.IsSuccessStatusCode)
            {
                var _content = await response.Content.ReadAsStringAsync();
                var _response = JsonConvert.DeserializeObject<MagfaResponse>(_content);
                if (_response.status == 0)
                {
                    if (_response.messages != null && _response.messages.Count > 0)
                    {
                        if (_response.messages.FirstOrDefault().status == 0)
                        {
                            return new SuccsessDataResult<string>(_content);
                        }
                    }
                }
                return new ErrorDataResult<string>(_content);
            }
            else
            {
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    return new ErrorDataResult<string>("Request :{0}, AccessToken {1}  response Unauthorized", _config.BaseUrl, authenticationString);
                }
                if (response.StatusCode == HttpStatusCode.InternalServerError)
                {
                    return new ErrorDataResult<string>("Request response InternalServerError"); ;
                }
                return new ErrorDataResult<string>("Unknown error");
            }
        }
    }
}