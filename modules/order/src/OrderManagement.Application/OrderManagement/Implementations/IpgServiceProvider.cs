using Microsoft.Extensions.Configuration;
using OrderManagement.Application.Contracts.Services;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using RestSharp;
using OrderManagement.Application.Contracts;
using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.Auditing;

namespace OrderManagement.Application.OrderManagement.Implementations;

public class IpgServiceProvider : ApplicationService, IIpgServiceProvider
{
    private readonly IConfiguration _configuration;
    private readonly IAuditingManager _auditingManager;
    public IpgServiceProvider(IConfiguration configuration,IAuditingManager auditingManager)
    {
        _configuration = configuration;
        _auditingManager = auditingManager;
    }

    public async Task<List<PspDto>> GetPsps()
    {
        RestClient client = SetDefaultClient();
        const string GetPspPath = "PaymentService/GetPsps";
        RestRequest request = new(GetPspPath);
        var availablePsps = await client.GetAsync<List<PspDto>>(request);
        return availablePsps;
    }

    public async Task<ApiResult<IpgApiResult>> HandShakeWithPsp(PspHandShakeRequest handShakeRequest)
    {
        //if (handShakeRequest.Amount <= 10000)
        //    throw new HandShakeInvalidRequestException(HandShakeInvalidRequestException.InvalidAmount, "مبالغ کم تر از ده هزار ریال قابل تراکنش نمیباشند");
        //if (string.IsNullOrWhiteSpace(handShakeRequest.NationalCode))
        //    throw new HandShakeInvalidRequestException(HandShakeInvalidRequestException.EmptyNationlCode, "شماره ملی اجباری است");
        //if (string.IsNullOrWhiteSpace(handShakeRequest.CallBackUrl) || !Regex.IsMatch(handShakeRequest.CallBackUrl, RegexConstatnts.Url))
        //    throw new HandShakeInvalidRequestException(HandShakeInvalidRequestException.CallBackUrlIsInvalid, "فیلد آدرس برگشتی اشتباه است و یا فرمت آن صحیح نیست");


        RestClient client = SetDefaultClient();
        const string HandShakePath = "PaymentService/HandShake";
        RestRequest request = new(HandShakePath, Method.Post);
        request.AddJsonBody<PspHandShakeRequest>(handShakeRequest);
        var handshakeResult = await client.ExecuteAsync<ApiResult<IpgApiResult>>(request);
        if (handshakeResult.IsSuccessful && handshakeResult.IsSuccessStatusCode && handshakeResult.Data.Result.StatusCode == 0)
            return handshakeResult.Data;

        _auditingManager.Current.Log.Exceptions.Add(
            new UserFriendlyException(handshakeResult.Data?.Result?.Message ?? "Handshake proccess with psp has been failed"));
        return null;
    }

    public async Task ReverseTransaction(int paymentId)
    {
        var client = SetDefaultClient();
        const string ReverseTransactionPath = "PaymentService/Reverse";
        RestRequest request = new(ReverseTransactionPath, Method.Post);
        request.AddQueryParameter("paymentId", paymentId);
        var serviceResponse = await client.ExecuteAsync<ApiResult<PspInteractionResult>>(request);
    }

    public async Task<ApiResult<PspInteractionResult>> VerifyTransaction(int paymentId)
    {
        var client = SetDefaultClient();
        const string VerifyPath = "PaymentService/Verify";
        RestRequest request = new(VerifyPath, Method.Post);
        request.AddQueryParameter("paymentId", paymentId.ToString());
        var serviceResponse = await client.ExecuteAsync<ApiResult<PspInteractionResult>>(request);
        if (serviceResponse.IsSuccessful && serviceResponse.IsSuccessStatusCode && serviceResponse.Data.Result.StatusCode == 0)
            return serviceResponse.Data;

        _auditingManager.Current.Log.Exceptions.Add(
            new UserFriendlyException(serviceResponse.Data?.Result?.Message ?? "Transaction verification has been failed"));
        throw new UserFriendlyException("متاسفانه تایید تراکنش با خطا مواجه شد");
    }

    #region Utility

    private RestClient SetDefaultClient()
    {

        var options = new RestClientOptions(_configuration.GetValue<string>("IPG:Url"))
        {
            RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true
        };        
        return new RestClient(options);
    }

    #endregion
}
