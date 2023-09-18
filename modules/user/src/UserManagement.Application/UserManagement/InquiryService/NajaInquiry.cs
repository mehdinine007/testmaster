using Esale.Core.Utility.Results;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.ServiceModel;
using System.Text;
using UserManagement.Application.Contracts.Models;

namespace UserManagement.Application.InquiryService;

public class NajaInquiry
{
    private readonly string url;
    private readonly string userName;
    private readonly string password;
    public NajaInquiry(string url, string userName, string password)
    {
        this.url = url;
        this.userName = userName;
        this.password = password;

    }
    public async Task<IDataResult<string>> Plaque(string nationalCode)
    {
        var data = new { NationalCode = nationalCode };
        try
        {
            wsFava.GatewayServiceClient client = new wsFava.GatewayServiceClient(wsFava.GatewayServiceClient.EndpointConfiguration.BasicHttpBinding_IGatewayService,
                new EndpointAddress(url));

            var _result = await client.NajaCheckNUMAsync(nationalCode, userName, password);
            if (_result != null && _result.descField.ToUpper() == "OK")
            {
                return new SuccsessDataResult<string>(JsonConvert.SerializeObject(_result), message: JsonConvert.SerializeObject(data), messageId: "200");
            }
            return new ErrorDataResult<string>(JsonConvert.SerializeObject(_result), message: JsonConvert.SerializeObject(data), messageId: "400");
        }
        catch (Exception ex)
        {
            return new ErrorDataResult<string>(data: JsonConvert.SerializeObject(data), message: ex.Message.ToString(), messageId: "100");

        }
    }
    public async Task<IDataResult<string>> Certificate(string nationalCode)
    {
        var data = new { NationalCode = nationalCode };
        try
        {
            wsFava.GatewayServiceClient client = new wsFava.GatewayServiceClient(wsFava.GatewayServiceClient.EndpointConfiguration.BasicHttpBinding_IGatewayService,
                new EndpointAddress(url));

            var _result = await client.NajaCheckLicAsync(nationalCode, userName, password);
            if (_result != null && _result.descField.ToUpper() == "OK")
            {
                return new SuccsessDataResult<string>(JsonConvert.SerializeObject(_result), message: JsonConvert.SerializeObject(data), messageId: "200");
            }
            return new ErrorDataResult<string>(JsonConvert.SerializeObject(_result), message: JsonConvert.SerializeObject(data), messageId: "400");
        }
        catch (Exception ex)
        {
            return new ErrorDataResult<string>(data: JsonConvert.SerializeObject(data), message: ex.Message.ToString(), messageId: "100");

        }
    }

    public async Task<IDataResult<string>> PlaqueFromGSB(PlaqueFromGSBInput data)
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
            try
            {
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var authenticationString = $"{userName}:{password}";
                var base64EncodedAuthenticationString = Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(authenticationString));
                client.DefaultRequestHeaders.Add("Authorization", $"Basic {base64EncodedAuthenticationString}");
                var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
                var response = await client.PostAsync("/services/NajaAPI/CarCheckStatusLicence", content);
                if (response.IsSuccessStatusCode)
                {
                    var _content = await response.Content.ReadAsStringAsync();
                    return new SuccsessDataResult<string>(_content, message: JsonConvert.SerializeObject(data), messageId: "200");
                }
                else
                {
                    int _statusCode = (int)response.StatusCode;
                    var _content = await response.Content.ReadAsStringAsync();
                    return new ErrorDataResult<string>(data: _content, message: JsonConvert.SerializeObject(data), messageId: _statusCode.ToString());
                }
            }
            catch (Exception e)
            {
                return new ErrorDataResult<string>(data: JsonConvert.SerializeObject(data), message: e.Message.ToString(), messageId: "100");
            }
        }




    }
}
