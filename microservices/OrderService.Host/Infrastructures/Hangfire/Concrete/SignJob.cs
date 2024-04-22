using Hangfire;
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
        public SignJob(ISignService signService)
        {
            _signService = signService;
        }

        public async Task CheckSignStatus()
        {
            await _signService.CheckSignStatus();
            BackgroundJob.Schedule(() => CheckSignStatus(), TimeSpan.FromMinutes(5));
        }
    }
}
