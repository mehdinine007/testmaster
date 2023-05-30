using Hangfire;
using OrderManagement.Application.Contracts.Services;
using OrderManagement.Application.OrderManagement;
using OrderService.Host.Infrastructures.Hangfire.Abstract;
using System;

namespace OrderService.Host.Infrastructures.Hangfire.Concrete
{
    public class IpgJob : IIpgJob
    {
        private readonly IIpgServiceProvider _ipgServiceProvider;
        public IpgJob(IIpgServiceProvider ipgServiceProvider)
        {
            _ipgServiceProvider = ipgServiceProvider;
        }

        public void RetryForVerify()
        {
            _ipgServiceProvider.RetryForVerify();
            BackgroundJob.Schedule(() => RetryForVerify(), TimeSpan.FromMinutes(15));
        }

    }
}
