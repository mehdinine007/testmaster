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
using StackExchange.Redis;
using Google.Protobuf.WellKnownTypes;
using System.Collections.Generic;
using System.Linq;
using OrderManagement.Application.PaymentServiceGrpc;
using System.Net.Http;

namespace OrderManagement.Application.OrderManagement.Implementations;

public class EsaleGrpcClient : ApplicationService, IEsaleGrpcClient
{
    private readonly IConfiguration _configuration;
    private readonly IRepository<Logs, long> _logsRepository;

    public EsaleGrpcClient(IConfiguration configuration, IRepository<Logs, long> logsRepository)
    {
        _configuration = configuration;
        _logsRepository = logsRepository;
    }

    public async Task<UserDto> GetUserById(long userId)
    {
        AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2Support", true);
        AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
        var httpHandler = new HttpClientHandler();
        httpHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
        var channel = GrpcChannel.ForAddress(_configuration.GetValue<string>("Esale:GrpcAddress"), new GrpcChannelOptions { HttpHandler = httpHandler });
        var client = new UserServiceGrpc.UserServiceGrpcClient(channel);

        var user = client.GetUserById(new GetUserModel() { UserId = userId });
        if (user.BankId == 0)
            return null;
        return new UserDto
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
            CompanyId = user.CompanyId,
            Name = user.Name,
            SurName = user.SurName
        };
    }
    public async Task<UserDto> GetUserByUBPId(string userId)
    {
        AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2Support", true);
        AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
        var httpHandler = new HttpClientHandler();
        httpHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
        var channel = GrpcChannel.ForAddress(_configuration.GetValue<string>("Esale:GrpcAddress"), new GrpcChannelOptions { HttpHandler = httpHandler });
       

        var client = new UserServiceGrpc.UserServiceGrpcClient(channel);

        var user = client.GetUserByUBPId(new GetUserUBPModel() { UserId = userId });
        if (user.BankId == 0)
            return null;
        return new UserDto
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
            CompanyId = user.CompanyId,
            Name = user.Name,
            SurName = user.SurName
        };
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
            TransactionPersianDate = paymentInformation.TransactionPersianDate
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
            F1= x.F1,
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
}
