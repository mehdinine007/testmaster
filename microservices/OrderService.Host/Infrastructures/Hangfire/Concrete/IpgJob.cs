using Hangfire;
using OrderManagement.Application.Contracts.Services;
using OrderManagement.Application.OrderManagement;
using OrderService.Host.Infrastructures.Hangfire.Abstract;
using System;

namespace OrderService.Host.Infrastructures.Hangfire.Concrete
{
    public class IpgJob : IIpgJob
    {
        private readonly IOrderAppService _orderAppService;
        public IpgJob(IOrderAppService orderAppService)
        {
            _orderAppService = orderAppService;
        }

        public void RetryForVerify()
        {
            _orderAppService.RetryPaymentForVerify();
            BackgroundJob.Schedule(() => RetryForVerify(), TimeSpan.FromMinutes(15));
        }

    }
}
