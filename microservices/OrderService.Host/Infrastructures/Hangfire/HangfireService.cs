using Hangfire;
using OrderManagement.Application.Contracts.Services;
using System;

namespace OrderService.Host.Infrastructures.Hangfire
{
    public class HangfireService : IHangfireService
    {
        private readonly ISaleService _saleService;
        public HangfireService(ISaleService saleService)
        {
            _saleService = saleService;
        }

        //public void HangfireTest()
        //{
        //    _saleService.HangfireTest();
        //    BackgroundJob.Schedule(() => HangfireTest(), TimeSpan.FromSeconds(10));
        //}

    }
}
