using Microsoft.Extensions.Configuration;
using OrderManagement.Application.Contracts.Services;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using RestSharp;
using OrderManagement.Application.Contracts;
using System.Collections.Generic;
using OrderManagement.Application.Contracts.OrderManagement.Exceptions;
using System.Text.RegularExpressions;
using OrderManagement.Application.OrderManagement.Utitlities;
using Volo.Abp;
using Grpc.Net.Client;
using OrderManagement.Domain;
using PaymentManagement.Application.Contracts.Dtos;
using PaymentManagement.Application.Contracts.IServices;
using ProtoBuf.Grpc.Client;

namespace OrderManagement.Application.OrderManagement.Implementations;

public class IpgServiceProvider : ApplicationService, IIpgServiceProvider
{
    private readonly IConfiguration _configuration;
    private readonly IOrderAppService _orderAppService;
    public IpgServiceProvider(IConfiguration configuration, IOrderAppService orderAppService)
    {
        _configuration = configuration;
        _orderAppService = orderAppService;
    }

    public async Task<List<PspDto>> GetPsps()
    {
        RestClient client = SetDefaultClient();
        const string GetPspPath = "PaymentService/GetPsps";
        RestRequest request = new(GetPspPath);
        var availablePsps = await client.GetAsync<List<PspDto>>(request);
        return availablePsps;
    }

    public async Task<HandShakeResponseDto> HandShakeWithPsp(PspHandShakeRequest handShakeRequest)
    {
        //if (handShakeRequest.Amount <= 10000)
        //    throw new HandShakeInvalidRequestException(HandShakeInvalidRequestException.InvalidAmount, "مبالغ کم تر از ده هزار ریال قابل تراکنش نمیباشند");
        if (string.IsNullOrWhiteSpace(handShakeRequest.NationalCode))
            throw new HandShakeInvalidRequestException(HandShakeInvalidRequestException.EmptyNationlCode, "شماره ملی اجباری است");
        if (string.IsNullOrWhiteSpace(handShakeRequest.CallBackUrl) || !Regex.IsMatch(handShakeRequest.CallBackUrl, RegexConstatnts.Url))
            throw new HandShakeInvalidRequestException(HandShakeInvalidRequestException.CallBackUrlIsInvalid, "فیلد آدرس برگشتی اشتباه است و یا فرمت آن صحیح نیست");


        RestClient client = SetDefaultClient();
        const string HandShakePath = "PaymentService/HandShake";
        RestRequest request = new(HandShakePath,Method.Post);
        request.AddJsonBody<PspHandShakeRequest>(handShakeRequest);
        var handshakeResult = await client.ExecuteAsync<HandShakeResponseDto>(request);
        if(handshakeResult.IsSuccessful && handshakeResult.IsSuccessStatusCode && handshakeResult.Data.Result.StatusCode == 0)
            return handshakeResult.Data;

        //TODO: Add log for failure reason
        throw new UserFriendlyException("در حال حاضر پرداخت وجه از طریق این درگاه ممکن نیست لطفا درگاه دیگری را انتخاب کنید");
    }

    public async Task RetryForVerify()
    {
        using (var channel = GrpcChannel.ForAddress(_configuration.GetSection("gRPC:PaymentUrl").Value))
        {
            var paymentAppService = channel.CreateGrpcService<IGrpcPaymentAppService>();
            var payments = await paymentAppService.RetryForVerify();
            if (payments != null && payments.Count > 0)
            {
                foreach (var payment in payments)
                {
                    int orderId = payment.FilterParam3 ?? 0;
                    _orderAppService.UpdateStatus(orderId,payment.StatusId == 0 ? (int)OrderStatusType.PaymentSucceeded: (int)OrderStatusType.PaymentNotVerified);
                }
            }
        }
    }

    public async Task<PspInteractionResult> VerifyTransaction(int paymentId)
    {
        var client = SetDefaultClient();
        const string VerifyPath = "PaymentService/Verify";
        RestRequest request = new(VerifyPath,Method.Post);
        request.AddQueryParameter("paymentId", paymentId.ToString());
        var serviceResponse = await client.ExecuteAsync<PspInteractionResult>(request);
        if (serviceResponse.IsSuccessful && serviceResponse.IsSuccessStatusCode && serviceResponse.Data.StatusCode == 0)
            return serviceResponse.Data;

        //TODO: Add log for failure reason

        throw new UserFriendlyException("متاسفانه تایید تراکنش با خطا مواجه شد");
    }

    #region Utility

    private RestClient SetDefaultClient()
        => new RestClient(new RestClientOptions(_configuration.GetValue<string>("IPG:Url")));

    #endregion
}
