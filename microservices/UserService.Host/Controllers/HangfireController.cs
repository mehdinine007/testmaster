using Microsoft.AspNetCore.Mvc;
using UserService.Host.Infrastructures.Hangfire.Abstract;

namespace UserService.Host.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HangfireController:Controller
    {
        private readonly IRolePermissionJob _rolePermissionJob;

        public HangfireController(IRolePermissionJob rolePermissionJob)
        {
            _rolePermissionJob = rolePermissionJob;
        }

        [HttpPost("addToRedis")]
        public async Task<IActionResult> AddToRedis()
        {
            _rolePermissionJob.AddToRedis();
            return Ok($"BackgroundJob Job Scheduled Inserted");
        }
    }
}
