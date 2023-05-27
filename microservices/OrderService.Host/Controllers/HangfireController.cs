using Hangfire;
using Microsoft.AspNetCore.Mvc;
using OrderService.Host.Infrastructures.Hangfire;
using OrderService.Host.Infrastructures.Hangfire.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoyanHangFire.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HangfireController : Controller
    {
        private readonly ICapacityControlJob _capacityControlJob;
        public HangfireController(ICapacityControlJob capacityControlJob)
        {
            _capacityControlJob = capacityControlJob;
        }

        [HttpPost("addSalDetailCapacity")]
        public async Task<IActionResult> AddSalDetailCapacity()
        {
            _capacityControlJob.SaleDetail();
            return Ok($"BackgroundJob Job Scheduled Inserted");
        }

        [HttpPost("addPaymentCapacity")]
        public async Task<IActionResult> AddPaymentCapacity()
        {
            _capacityControlJob.Payment();
            return Ok($"BackgroundJob Job Scheduled Inserted");
        }

    }
}
