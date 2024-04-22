using Hangfire;
using Microsoft.Extensions.Configuration;
using OrderManagement.Application.Contracts;
using OrderManagement.Application.Contracts.OrderManagement.Services;
using OrderManagement.Application.Contracts.Services;
using OrderManagement.Application.OrderManagement;
using OrderManagement.Application.OrderManagement.Implementations;
using OrderService.Host.Infrastructures.Hangfire.Abstract;
using System;
using System.Threading.Tasks;

namespace OrderService.Host.Infrastructures.Hangfire.Concrete
{
    public class SignJob : ISignJob
    {
        private readonly ISignService _signService;
        private readonly IConfiguration _configuration;
        public SignJob(ISignService signService, IConfiguration configuration)
        {
            _signService = signService;
            _configuration= configuration;
        }

        public async Task CheckDigitalSign()
        {
            var time = int.Parse(_configuration.GetSection("SignConfig:CheckDigitalSignIntervalMinutes").Value ?? "2");
            await _signService.CheckSignStatus();
            BackgroundJob.Schedule(() => CheckDigitalSign(), TimeSpan.FromMinutes(time));
        }
    }
}
