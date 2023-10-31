using IFG.Core.Utility.Results;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using UserManagement.Application.Contracts.Models;

namespace UserManagement.Application.InquiryService;

public class FaraBoomInquiry
{
    private readonly string baseUrl;
    private readonly FaraBoomHeader _FaraBoomHeader;
    public FaraBoomInquiry(string baseUrl, FaraBoomHeader FaraBoomHeader)
    {
        this.baseUrl = baseUrl;
        _FaraBoomHeader = FaraBoomHeader;

    }
    public async Task<IDataResult<string>> BirthDate(string birthdate, string nationalCode)
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
            var _data = new { birth_date = birthdate, national_code = nationalCode };
            try
            {
                client.BaseAddress = new Uri(baseUrl);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Accept-Language", "fa");
                client.DefaultRequestHeaders.Add("App-Key", _FaraBoomHeader.App_Key);
                client.DefaultRequestHeaders.Add("Device-Id", _FaraBoomHeader.Device_Id);
                client.DefaultRequestHeaders.Add("Token-Id", _FaraBoomHeader.Token_Id);
                client.DefaultRequestHeaders.Add("CLIENT-DEVICE-ID", _FaraBoomHeader.CLIENT_DEVICE_ID);
                client.DefaultRequestHeaders.Add("CLIENT-IP-ADDRESS", _FaraBoomHeader.CLIENT_IP_ADDRESS);
                client.DefaultRequestHeaders.Add("CLIENT-USER-AGENT", _FaraBoomHeader.CLIENT_USER_AGENT);
                client.DefaultRequestHeaders.Add("CLIENT-USER-ID", _FaraBoomHeader.CLIENT_USER_ID);
                client.DefaultRequestHeaders.Add("App-Secret", _FaraBoomHeader.App_Secret);
                var content = new StringContent(JsonConvert.SerializeObject(_data), Encoding.UTF8, "application/json");
                var response = await client.PostAsync("/v1/identity/inquiry/birthDate", content);
                if (response.IsSuccessStatusCode)
                {
                    var _content = await response.Content.ReadAsStringAsync();
                    return new SuccsessDataResult<string>(_content, message: JsonConvert.SerializeObject(_data), messageId: "200");
                }
                else
                {
                    var _content = await response.Content.ReadAsStringAsync();
                    int _statusCode = (int)response.StatusCode;
                    return new ErrorDataResult<string>(data: _content, message: JsonConvert.SerializeObject(_data), messageId: _statusCode.ToString());
                }
            }
            catch (Exception e)
            {
                return new ErrorDataResult<string>(data: JsonConvert.SerializeObject(_data), message: e.Message.ToString(), messageId: "100");
            }
        }
    }
    public async Task<IDataResult<string>> PhoneNumber(string mobileNumber, string nationalCode)
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
            var _data = new { mobile = mobileNumber, national_code = nationalCode };
            try
            {
                client.BaseAddress = new Uri(baseUrl);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Accept-Language", "fa");
                client.DefaultRequestHeaders.Add("App-Key", _FaraBoomHeader.App_Key);
                client.DefaultRequestHeaders.Add("Device-Id", _FaraBoomHeader.Device_Id);
                client.DefaultRequestHeaders.Add("Token-Id", _FaraBoomHeader.Token_Id);
                client.DefaultRequestHeaders.Add("CLIENT-DEVICE-ID", _FaraBoomHeader.CLIENT_DEVICE_ID);
                client.DefaultRequestHeaders.Add("CLIENT-IP-ADDRESS", _FaraBoomHeader.CLIENT_IP_ADDRESS);
                client.DefaultRequestHeaders.Add("CLIENT-USER-AGENT", _FaraBoomHeader.CLIENT_USER_AGENT);
                client.DefaultRequestHeaders.Add("CLIENT-USER-ID", _FaraBoomHeader.CLIENT_USER_ID);
                client.DefaultRequestHeaders.Add("App-Secret", _FaraBoomHeader.App_Secret);
                var content = new StringContent(JsonConvert.SerializeObject(_data), Encoding.UTF8, "application/json");
                var response = await client.PostAsync("/v1/mobile/national-code", content);
                if (response.IsSuccessStatusCode)
                {
                    var _content = await response.Content.ReadAsStringAsync();
                    return new SuccsessDataResult<string>(_content, message: JsonConvert.SerializeObject(_data), messageId: "200");
                }
                else
                {
                    var _content = await response.Content.ReadAsStringAsync();
                    int _statusCode = (int)response.StatusCode;
                    return new ErrorDataResult<string>(data: _content, message: JsonConvert.SerializeObject(_data), messageId: _statusCode.ToString());
                }
            }
            catch (Exception e)
            {
                return new ErrorDataResult<string>(message: e.Message.ToString(), messageId: "101");
            }
        }
    }

    public async Task<IDataResult<string>> PostalCode(string zipCode)
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
            var _data = new { zip_code = zipCode };
            try
            {
                client.BaseAddress = new Uri(baseUrl);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Accept-Language", "fa");
                client.DefaultRequestHeaders.Add("App-Key", _FaraBoomHeader.App_Key);
                client.DefaultRequestHeaders.Add("Device-Id", _FaraBoomHeader.Device_Id);
                client.DefaultRequestHeaders.Add("Token-Id", _FaraBoomHeader.Token_Id);
                client.DefaultRequestHeaders.Add("CLIENT-DEVICE-ID", _FaraBoomHeader.CLIENT_DEVICE_ID);
                client.DefaultRequestHeaders.Add("CLIENT-IP-ADDRESS", _FaraBoomHeader.CLIENT_IP_ADDRESS);
                client.DefaultRequestHeaders.Add("CLIENT-USER-AGENT", _FaraBoomHeader.CLIENT_USER_AGENT);
                client.DefaultRequestHeaders.Add("CLIENT-USER-ID", _FaraBoomHeader.CLIENT_USER_ID);
                client.DefaultRequestHeaders.Add("App-Secret", _FaraBoomHeader.App_Secret);
                var content = new StringContent(JsonConvert.SerializeObject(_data), Encoding.UTF8, "application/json");
                var response = await client.PostAsync("/v1/gis/zipcodes/addresses", content);
                if (response.IsSuccessStatusCode)
                {
                    var _content = await response.Content.ReadAsStringAsync();
                    return new SuccsessDataResult<string>(_content, message: JsonConvert.SerializeObject(_data), messageId: "200");
                }
                else
                {
                    var _content = await response.Content.ReadAsStringAsync();
                    int _statusCode = (int)response.StatusCode;
                    return new ErrorDataResult<string>(data: _content, message: JsonConvert.SerializeObject(_data), messageId: _statusCode.ToString());
                }
            }
            catch (Exception e)
            {
                return new ErrorDataResult<string>(message: e.Message.ToString(), messageId: "101");
            }
        }
    }
}
