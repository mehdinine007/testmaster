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
using Volo.Abp.Domain.Entities;

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
        var channel = GrpcChannel.ForAddress(_configuration.GetValue<string>("Esale:GrpcAddress"));
        var client = new UserServiceGrpc.UserServiceGrpcClient(channel);
        //try
        //{

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
                Shaba = user.Shaba,
                MobileNumber = user.MobileNumber,
                CompanyId = user.CompanyId,
            };
        //}
        //catch (Exception ex)
        //{


        //    //var errorMessage = ex.Message;


        //    await _logsRepository.InsertAsync(new Logs
        //    {
        //        //Message = ex.InnerException.InnerException.ne,
        //        Method = "GetUserById",
        //        Type = 3,
        //    });
        //    throw new UserFriendlyException("در حال حاضر امکان ادامه فرآیند نیست");
        //}
    }
}
