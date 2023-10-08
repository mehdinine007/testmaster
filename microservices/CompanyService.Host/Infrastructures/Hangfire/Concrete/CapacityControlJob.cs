using Hangfire;
using Microsoft.Extensions.Configuration;
using OrderManagement.Application.OrderManagement;
using OrderService.Host.Infrastructures.Hangfire.Abstract;
using System;
using System.Threading.Tasks;

namespace OrderService.Host.Infrastructures.Hangfire.Concrete
{
    public class CapacityControlJob : ICapacityControlJob
    {
        private readonly ICapacityControlAppService _capacityControlAppService;
        private readonly IConfiguration _configuration;
        public CapacityControlJob(ICapacityControlAppService capacityControlAppService, IConfiguration configuration)
        {
            _capacityControlAppService = capacityControlAppService;
            _configuration = configuration;
        }

        public async Task Payment()
        {
            await _capacityControlAppService.Payment();
            BackgroundJob.Schedule(() => Payment(), TimeSpan.FromSeconds(int.Parse(_configuration.GetSection("Hangfire:CapacityControl:PaymentCount_IntervalInSecond").Value)));
        }

        public async Task SaleDetail()
        {
            await _capacityControlAppService.SaleDetail();
            BackgroundJob.Schedule(() => SaleDetail(), TimeSpan.FromSeconds(int.Parse(_configuration.GetSection("Hangfire:CapacityControl:SaleDetail_IntervalInSecond").Value)));
        }
    }
}
