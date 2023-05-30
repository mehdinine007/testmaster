using Hangfire;
using OrderManagement.Application.OrderManagement;
using OrderService.Host.Infrastructures.Hangfire.Abstract;
using System;

namespace OrderService.Host.Infrastructures.Hangfire.Concrete
{
    public class CapacityControlJob : ICapacityControlJob
    {
        private readonly ICapacityControlAppService _capacityControlAppService;
        public CapacityControlJob(ICapacityControlAppService capacityControlAppService)
        {
            _capacityControlAppService = capacityControlAppService;
        }

        public void Payment()
        {
            _capacityControlAppService.Payment();
            BackgroundJob.Schedule(() => Payment(), TimeSpan.FromSeconds(120));
        }

        public void SaleDetail()
        {
            _capacityControlAppService.SaleDetail();
            BackgroundJob.Schedule(() => SaleDetail(), TimeSpan.FromSeconds(120));
        }
    }
}
