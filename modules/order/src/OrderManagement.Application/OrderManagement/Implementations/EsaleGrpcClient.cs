using OrderManagement.Application.Contracts;
using OrderManagement.Application.Contracts.Services;
using Volo.Abp.Application.Services;
using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;
using OrderManagement.Application.Esale.UserServiceGrpc;
using OrderManagement.Domain;
using Volo.Abp.Domain.Repositories;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using System.Collections.Generic;
using System.Linq;
using OrderManagement.Application.PaymentServiceGrpc;
using System.Net.Http;
using IFG.Core.Caching;
using Newtonsoft.Json;
using IFG.Core.Infrastructures.TokenAuth;
using CompanyManagement.Application.Contracts;

namespace OrderManagement.Application.OrderManagement.Implementations;

public class EsaleGrpcClient : ApplicationService, IEsaleGrpcClient
{
    private readonly IConfiguration _configuration;
    private readonly IRepository<Logs, long> _logsRepository;
    private readonly ICacheManager _cacheManager;


    public EsaleGrpcClient(IConfiguration configuration, IRepository<Logs, long> logsRepository, ICacheManager cacheManager)
    {
        _configuration = configuration;
        _logsRepository = logsRepository;
        _cacheManager = cacheManager;
    }

    public async Task<UserDto> GetUserId(string userId)
    {
        AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2Support", true);
        AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
        var httpHandler = new HttpClientHandler();
        httpHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
        var channel = GrpcChannel.ForAddress(_configuration.GetValue<string>("Esale:GrpcAddress"), new GrpcChannelOptions { HttpHandler = httpHandler });

        string cacheKey = $"{userId}";
        string prefix = $"{RedisConstants.GrpcGetUserById}";
        var cachedData = await _cacheManager.GetStringAsync(cacheKey, prefix, new CacheOptions
        { Provider = CacheProviderEnum.Hybrid });
        if (!string.IsNullOrEmpty(cachedData))
        {
            return JsonConvert.DeserializeObject<UserDto>(cachedData);
        }

        var client = new UserServiceGrpc.UserServiceGrpcClient(channel);

        var user = client.GetUserById(new GetUserModel() { UserId = userId });

        if (string.IsNullOrEmpty(user.NationalCode))
            return null;
        var userDto = new UserDto
        {
            AccountNumber = user.AccountNumber,
            BankId = user.BankId,
            BirthCityId = user.BirthCityId,
            BirthProvinceId = user.BirthProvinceId,
            HabitationCityId = user.HabitationCityId,
            HabitationProvinceId = user.HabitationProvinceId,
            IssuingCityId = user.IssuingCityId,
            IssuingProvinceId = user.IssuingProvinceId,
            NationalCode = user.NationalCode,
            Shaba = user.Shaba,
            MobileNumber = user.MobileNumber,
            GenderCode = user.GenderCode,
            CompanyId = user.CompanyId,
            Name = user.Name,
            SurName = user.SurName,
            Priority = user.Priority
        };

        await _cacheManager.SetStringAsync(cacheKey, prefix, JsonConvert.SerializeObject(userDto), new CacheOptions
        { Provider = CacheProviderEnum.Hybrid }, TimeSpan.FromMinutes(1).TotalSeconds);
        return userDto;
    }
    public async Task<AdvocacyUserDto> GetUserAdvocacyByNationalCode(string nationlCode)
    {
        var httpHandler = new HttpClientHandler();
        httpHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
        var channel = GrpcChannel.ForAddress(_configuration.GetValue<string>("Esale:GrpcAddress"), new GrpcChannelOptions { HttpHandler = httpHandler });
        var client = new UserServiceGrpc.UserServiceGrpcClient(channel);

        var userAdvocacy = await client.GetUserAdvocacyAsync(new UserAdvocacyRequest()
        {
            NationalCode = nationlCode
        });
        if (userAdvocacy.BankId == 0)
            throw new UserFriendlyException("اطلاعات حساب وکالتی یافت نشد");

        return new AdvocacyUserDto
        {
            AccountNumber = userAdvocacy.AccountNumber,
            BankId = userAdvocacy.BankId,
            ShebaNumber = userAdvocacy.ShebaNumber,
            GenderCode = userAdvocacy.GenderCode
        };
    }

    private PaymentServiceGrpc.PaymentServiceGrpc.PaymentServiceGrpcClient PaymentServiceGrpcClient()
    {
        var httpHandler = new HttpClientHandler();
        httpHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
        var channel = GrpcChannel.ForAddress(_configuration.GetValue<string>("Payment:GrpcAddress"), new GrpcChannelOptions { HttpHandler = httpHandler });
        return new PaymentServiceGrpc.PaymentServiceGrpc.PaymentServiceGrpcClient(channel);
    }
    public async Task<PaymentInformationResponseDto> GetPaymentInformation(int paymentId)
    {
        var paymentInformation = await PaymentServiceGrpcClient().GetPaymentInformationAsync(new()
        {
            PaymentId = paymentId
        });

        return await Task.FromResult(new PaymentInformationResponseDto
        {
            PaymentId = paymentInformation.PaymentId,
            TransactionCode = paymentInformation.TransactionCode,
            TransactionDate = paymentInformation.TransactionDate.ToDateTime(),
            TransactionPersianDate = paymentInformation.TransactionPersianDate,
            PaymentStatusId = paymentInformation.PaymentStatusId
        });
    }

    public async Task<List<PaymentStatusModel>> GetPaymentStatusList(PaymentStatusDto paymentStatusDto)
    {
        var paymentStatus = await PaymentServiceGrpcClient().GetPaymentStatusListAsync(new()
        {
            RelationId = paymentStatusDto.RelationId,
            RelationIdB = paymentStatusDto.RelationIdB,
            RelationIdC = paymentStatusDto.RelationIdC,
            RelationIdD = paymentStatusDto.RelationIdD,
            IsRelationIdGroup = paymentStatusDto.IsRelationIdGroup,
            IsRelationIdBGroup = paymentStatusDto.IsRelationIdBGroup,
            IsRelationIdCGroup = paymentStatusDto.IsRelationIdCGroup,
            IsRelationIdDGroup = paymentStatusDto.IsRelationIdDGroup

        });
        if (paymentStatus == null || paymentStatus.PaymentStatusData == null || paymentStatus.PaymentStatusData.Count == 0)
        {
            return new List<PaymentStatusModel>();
        }
        return paymentStatus.PaymentStatusData.Select(x => new PaymentStatusModel()
        {
            Count = x.Count,
            Message = x.Message,
            Status = x.Status,
            F1 = x.F1,
            F2 = x.F2,
            F3 = x.F3,
            F4 = x.F4
        }).ToList();
    }
    public async Task<List<PaymentStatusModel>> GetPaymentStatusByGroupList(PaymentStatusDto paymentStatusDto)
    {
        var paymentStatus = await PaymentServiceGrpcClient().GetPaymentStatusByGroupListAsync(new()
        {
            RelationId = paymentStatusDto.RelationId,
            RelationIdB = paymentStatusDto.RelationIdB,
            RelationIdC = paymentStatusDto.RelationIdC,
            RelationIdD = paymentStatusDto.RelationIdD,
        });
        if (paymentStatus == null || paymentStatus.PaymentStatusData == null || paymentStatus.PaymentStatusData.Count == 0)
        {
            return new List<PaymentStatusModel>();
        }
        return paymentStatus.PaymentStatusData.Select(x => new PaymentStatusModel()
        {
            Count = x.Count,
            Message = x.Message,
            Status = x.Status,
            F1 = x.F1,
            F2 = x.F2,
            F3 = x.F3,
            F4 = x.F4
        }).ToList();
    }

    public async Task<List<RetryForVerifyPaymentDto>> RetryForVerify()
    {
        var paymentStatus = await PaymentServiceGrpcClient().RetryForVerifyAsync(new RetryForVerifyRequest());
        if (paymentStatus == null || paymentStatus.RetryForVerifyData == null || paymentStatus.RetryForVerifyData.Count == 0)
        {
            return new List<RetryForVerifyPaymentDto>();
        }
        return paymentStatus.RetryForVerifyData.Select(x => new RetryForVerifyPaymentDto()
        {
            PaymentId = x.PaymentId,
            PaymentStatus = x.PaymentStatus,
            FilterParam1 = x.FilterParam1,
            FilterParam2 = x.FilterParam2,
            FilterParam3 = x.FilterParam3,
            FilterParam4 = x.FilterParam4
        }).ToList();
    }

    public async Task<PaymentHandShakeViewModel> HandShake(PaymentHandShakeDto handShakeDto)
    {
        var handShake = await PaymentServiceGrpcClient().HandShakeAsync(new HandShakeDto()
        {
            AdditionalData = handShakeDto.AdditionalData,
            Amount = handShakeDto.Amount,
            CallBackUrl = handShakeDto.CallBackUrl,
            Mobile = handShakeDto.Mobile,
            NationalCode = handShakeDto.NationalCode,
            PspAccountId = handShakeDto.PspAccountId,
            FilterParam1 = handShakeDto.FilterParam1,
            FilterParam2 = handShakeDto.FilterParam2,
            FilterParam3 = handShakeDto.FilterParam3,
            FilterParam4 = handShakeDto.FilterParam4
        });
        if (handShake == null)
        {
            return new PaymentHandShakeViewModel();
        }
        return new PaymentHandShakeViewModel()
        {
            HtmlContent = handShake.HtmlContent,
            Message = handShake.Message,
            PaymentId = handShake.PaymentId,
            PspJsonResult = handShake.PspJsonResult,
            StatusCode = handShake.StatusCode,
            Token = handShake.Token
        };
    }

    public async Task<PaymentResultViewModel> Verify(int paymentId)
    {
        var verify = await PaymentServiceGrpcClient().VerifyAsync(new PaymentInputDto()
        {
            PaymentId = paymentId
        });
        if (verify == null)
        {
            return new PaymentResultViewModel();
        }
        return new PaymentResultViewModel()
        {
            Message = verify.Message,
            PaymentId = verify.PaymentId,
            PspJsonResult = verify.PspJsonResult,
            StatusCode = verify.StatusCode,
        };
    }

    public async Task<PaymentResultViewModel> Reverse(int paymentId)
    {
        var reverse = await PaymentServiceGrpcClient().ReverseAsync(new PaymentInputDto()
        {
            PaymentId = paymentId
        });
        if (reverse == null)
        {
            return new PaymentResultViewModel();
        }
        return new PaymentResultViewModel()
        {
            Message = reverse.Message,
            PaymentId = reverse.PaymentId,
            PspJsonResult = reverse.PspJsonResult,
            StatusCode = reverse.StatusCode,
        };
    }

    public async Task<AuthenticateResponseDto> Athenticate(AuthenticateReqDto input)
    {
        AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2Support", true);
        AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

        var httpHandler = new HttpClientHandler();
        httpHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
        var channel = GrpcChannel.ForAddress(_configuration.GetValue<string>("Esale:GrpcAddress"), new GrpcChannelOptions { HttpHandler = httpHandler });
        var client = new UserServiceGrpc.UserServiceGrpcClient(channel);
        var auth = await client.AuthenticateAsync(new AuthenticateRequest() { UserNameOrEmailAddress = input.userID, Password = input.userPWD });
        var res = new AuthenticateResponseDto();
        if (!auth.Success)
        {
            res.Success = auth.Success;
            res.Message = auth.Message;
            res.ErrorCode = auth.ErrorCode.Value;
            return res;
        }

        res.Success = auth.Success;
        res.Data = new AuthenticateResultModel();
        res.Data.AccessToken = auth.Data.AccessToken;
        res.Data.EncryptedAccessToken = auth.Data.EncryptedAccessToken;
        res.Data.ExpireInSeconds = auth.Data.ExpireInSeconds.Value;

        return res;
      //  return null;
    }
}
