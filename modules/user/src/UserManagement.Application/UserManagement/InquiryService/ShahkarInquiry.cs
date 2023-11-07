using IFG.Core.Utility.Results;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using UserManagement.Application.Contracts.Models;

namespace UserManagement.Application.InquiryService;

public class ShahkarInquiry
{
    private readonly string baseUrl;
    private readonly string userName;
    private readonly string password;
    public ShahkarInquiry(string baseUrl, string userName, string password)
    {
        this.baseUrl = baseUrl;
        this.userName = userName;
        this.password = password;

    }
    public async Task<IDataResult<string>> Inquiry(ShahkarInquiryBodyData data)
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
                client.BaseAddress = new Uri(baseUrl);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var authenticationString = $"{userName}:{password}";
                var base64EncodedAuthenticationString = Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(authenticationString));
                client.DefaultRequestHeaders.Add("Authorization", $"Basic {base64EncodedAuthenticationString}");
                var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
                var response = await client.PostAsync("/services/GetIDMatching", content);
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
    public async Task<IDataResult<string>> InquirySabteAhval(SabteAhvalInput data)
    {
        GSBSabteAhval.getEstelam3Response response = null;
        try
        {
            GSBSabteAhval.MIMTGetPersonInfoOnlineCarPortTypeClient client = new GSBSabteAhval.MIMTGetPersonInfoOnlineCarPortTypeClient(GSBSabteAhval.MIMTGetPersonInfoOnlineCarPortTypeClient.EndpointConfiguration.MIMTGetPersonInfoOnlineCarHttpsSoap11Endpoint);
            GSBSabteAhval.getEstelam3Request request = new GSBSabteAhval.getEstelam3Request();
            client.ClientCredentials.UserName.UserName = this.userName;
            client.ClientCredentials.UserName.Password = this.password;

            GSBSabteAhval.PersonInfo person = new GSBSabteAhval.PersonInfo();
            person.birthDate = data.BirthDate;
            person.nin = data.Nationalcode;
            response = await client.getEstelam3Async("", "", "", "", person);

            foreach (string item in response.@return[0].message)
            {
                if (!string.IsNullOrWhiteSpace(item))
                {
                    return new ErrorDataResult<string>(JsonConvert.SerializeObject(response), message: JsonConvert.SerializeObject(data), messageId: "200");

                }
            }
            return new SuccsessDataResult<string>(JsonConvert.SerializeObject(response), message: JsonConvert.SerializeObject(data), messageId: "200");
        }
        catch (Exception ex)
        {
            return new ErrorDataResult<string>(data: JsonConvert.SerializeObject(response), message: ex.Message.ToString(), messageId: "100");

        }


    }
}
