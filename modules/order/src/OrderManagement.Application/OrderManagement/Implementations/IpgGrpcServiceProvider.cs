using OrderManagement.Application.Contracts.Services;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using OrderManagement.Application.Contracts;
using System.Collections.Generic;
using Volo.Abp.Auditing;
using Microsoft.Extensions.Configuration;
using System;
using Volo.Abp;
using Newtonsoft.Json;
using Volo.Abp.Data;
using OrderManagement.Application.Contracts.OrderManagement;

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
        using (var auditingScope = _auditingManager.BeginScope())
        {
            List<CommentLog> comments = new List<CommentLog>();

            try
            {

                comments.Add(new CommentLog
                {
                    Description = "Start HandShakeWithPsp",
                    Data = JsonConvert.DeserializeObject<Dictionary<string, object>>(JsonConvert.SerializeObject(handShakeRequest))
                });

                var handshakeResult = await _esaleGrpcClient.HandShake(ObjectMapper.Map<PspHandShakeRequest, PaymentHandShakeDto>(handShakeRequest));
                comments.Add(new CommentLog
                {
                    Description = "OutPut EsaleGrpcClient",
                    Data = JsonConvert.DeserializeObject<Dictionary<string, object>>(JsonConvert.SerializeObject(handshakeResult))
                });

                if (handshakeResult != null && handshakeResult.StatusCode == 0)
                {
                    comments.Add(new CommentLog
                    {
                        Description = "Success EsaleGrpcClient",
                    });

                    return new ApiResult<IpgApiResult>
                    {
                        Result = ObjectMapper.Map<PaymentHandShakeViewModel, IpgApiResult>(handshakeResult),
                        Success = true
                    };
                }
                else
                {
                    return new ApiResult<IpgApiResult>
                    {
                        Success = false,
                        Message = "در حال حاضر پرداخت از طریق این درگاه امکان پذیر نمی باشد"
                    };
                }

            }
            catch (Exception ex)
            {
                comments.Add(new CommentLog
                {
                    Description = $"عملیات با خطا مواجه شد"
                });
                _auditingManager.Current.Log.Exceptions.Add(ex);
                return new ApiResult<IpgApiResult>
                {
                    Success = false,
                    Message = "اتصال به درگاه با خطا مواجه شد!"
                };
            }
            finally
            {
                _auditingManager.Current.Log.SetProperty("PspHandShakeLog", comments);
                _auditingManager.Current.Log.Url = "/api/services/app/OrderService/HandShake";
                _auditingManager.Current.Log.Comments.Add(JsonConvert.SerializeObject(handShakeRequest));

                await auditingScope.SaveAsync();
            }
        }
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
