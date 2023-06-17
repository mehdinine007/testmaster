﻿using OrderManagement.Application.Contracts;
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
        var channel = GrpcChannel.ForAddress(_configuration.GetValue<string>("Esale:GrpcAddress"));
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

    public async Task<AdvocacyUserDto> GetUserAdvocacyByNationalCode(string nationlCode)
    {
        var channel = GrpcChannel.ForAddress(_configuration.GetValue<string>("Esale:GrpcAddress"));
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

    public async Task<PaymentInformationResponseDto> GetPaymentInformation(int paymentId)
    {
        var channel = GrpcChannel.ForAddress(_configuration.GetValue<string>("Payment:GrpcAddress"));
        var client  = new PaymentServiceGrpc.PaymentServiceGrpc.PaymentServiceGrpcClient(channel);

        var paymentInformation = await client.GetPaymentInformationAsync(new()
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
}
