using OrderManagement.Application.Contracts;
using OrderManagement.Application.Contracts.Services;
using Volo.Abp.Application.Services;
using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;
using Abp.Domain.Entities;
using OrderManagement.Application.Esale.UserServiceGrpc;

namespace OrderManagement.Application.OrderManagement.Implementations;

public class EsaleGrpcClient : ApplicationService, IEsaleGrpcClient
{
    private readonly IConfiguration _configuration;

    public EsaleGrpcClient(IConfiguration configuration)
        => _configuration = configuration;

    public UserDto GetUserById(long userId)
    {
        var channel = GrpcChannel.ForAddress(_configuration.GetValue<string>("Esale:GrpcAddress"));
        var client = new UserServiceGrpc.UserServiceGrpcClient(channel);
        var user = client.GetUserById(new GetUserModel() { UserId = userId });
        if (user == null)
            throw new EntityNotFoundException(typeof(UserDto), userId);
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
            Shaba = user.Shaba
        };
    }
}
