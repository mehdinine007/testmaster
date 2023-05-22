using Hangfire;
using Microsoft.AspNetCore.Mvc;
using OrderService.Host.Infrastructures.Hangfire;
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
        private readonly IHangfireService _jobSaleService;
        public HangfireController(IHangfireService jobSaleService)
        {
            _jobSaleService = jobSaleService;
        }

        //[HttpPost("addHangfireTest")]
        //public async Task<IActionResult> AddHangfireTest()
        //{
        //    BackgroundJob.Schedule(() => _jobSaleService.HangfireTest(), TimeSpan.FromSeconds(10));
        //    return Ok($"BackgroundJob Job Scheduled Inserted");
        //}

    }
}
