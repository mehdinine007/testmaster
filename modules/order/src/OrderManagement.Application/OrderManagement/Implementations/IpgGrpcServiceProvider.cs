using OrderManagement.Application.Contracts.Services;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using OrderManagement.Application.Contracts;
using System.Collections.Generic;
using Volo.Abp.Auditing;
using Microsoft.Extensions.Configuration;
using System;
using Volo.Abp;

namespace OrderManagement.Application.OrderManagement.Implementations;

public class IpgGrpcServiceProvider : ApplicationService, IIpgServiceProvider
{

    private readonly IEsaleGrpcClient _esaleGrpcClient;
    private readonly IAuditingManager _auditingManager;
    private readonly IConfiguration _configuration;

    public IpgGrpcServiceProvider(IEsaleGrpcClient esaleGrpcClient,
                                  IAuditingManager auditingManager,
                                  IConfiguration configuration
        )
    {
        _esaleGrpcClient = esaleGrpcClient;
        _auditingManager = auditingManager;
        _configuration = configuration;
    }

    public Task<List<PspDto>> GetPsps()
        => throw new NotImplementedException();

    public async Task<ApiResult<IpgApiResult>> HandShakeWithPsp(PspHandShakeRequest handShakeRequest)
    {
        try
        {
            var handshakeResult = await _esaleGrpcClient.HandShake(ObjectMapper.Map<PspHandShakeRequest, PaymentHandShakeDto>(handShakeRequest));
            if (handshakeResult != null && handshakeResult.StatusCode == 0)
                return new ApiResult<IpgApiResult>
                {
                    Result = ObjectMapper.Map<PaymentHandShakeViewModel, IpgApiResult>(handshakeResult),
                    Success = true
                };

            _auditingManager.Current.Log.Exceptions.Add(
                new UserFriendlyException(handshakeResult?.Message ?? "Handshake proccess with psp has been failed"));
        }
        catch(Exception ex)
        {
            _auditingManager.Current.Log.Exceptions.Add(
              new UserFriendlyException(ex.Message));
        }
      
        return null;
    }

    public async Task ReverseTransaction(int paymentId)
        => await _esaleGrpcClient.Reverse(paymentId);

    public async Task<ApiResult<PspInteractionResult>> VerifyTransaction(int paymentId)
    {
        var serviceResponse = await _esaleGrpcClient.Verify(paymentId);
        if (serviceResponse != null && serviceResponse.StatusCode == 0)
            return new ApiResult<PspInteractionResult>
            {
                Success = true,
                Result = ObjectMapper.Map<PaymentResultViewModel, PspInteractionResult>(serviceResponse)
            };

        _auditingManager.Current.Log.Exceptions.Add(
            new UserFriendlyException(serviceResponse?.Message ?? "Transaction verification has been failed"));
        throw new UserFriendlyException("متاسفانه تایید تراکنش با خطا مواجه شد");
    }
}
