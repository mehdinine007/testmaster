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
        private readonly IIpgJob _pgJob;
        private readonly ISignJob _signJob;
        public HangfireController(ICapacityControlJob capacityControlJob, IIpgJob pgJob, ISignJob signJob)
        {
            _capacityControlJob = capacityControlJob;
            _pgJob = pgJob;
            _signJob = signJob;
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

        [HttpPost("addRetryForVerify")]
        public async Task<IActionResult> AddRetryForVerify()
        {
            await _pgJob.RetryForVerify();
            return Ok($"BackgroundJob Job Scheduled Inserted");
        }

        [HttpPost("addRetryOrderForVerify")]
        public async Task<IActionResult> AddRetryOrderForVerify()
        {
            await _pgJob.RetryOrderForVerify();
            return Ok($"BackgroundJob Job Scheduled Inserted");
        }

        [HttpPost("addCheckSignStatus")]
        public async Task<IActionResult> AddCheckSignStatus()
        {
            await _signJob.CheckSignStatus();
            return Ok($"BackgroundJob Job Scheduled Inserted");
        }

    }
}
