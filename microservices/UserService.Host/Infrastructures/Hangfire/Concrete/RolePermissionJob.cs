using Abp.BackgroundJobs;
using Hangfire;
using MongoDB.Bson;
using UserManagement.Application.Contracts.UserManagement.Services;
using UserManagement.Domain.UserManagement.Authorization.RolePermissions;
using UserService.Host.Infrastructures.Hangfire.Abstract;

namespace UserService.Host.Infrastructures.Hangfire.Concrete
{
    public class RolePermissionJob : IRolePermissionJob
    {
        private readonly IRolePermissionService _rolePermissionAppService;
        private readonly IConfiguration _configuration;

        public RolePermissionJob(IRolePermissionService rolePermissionAppService, IConfiguration configuration)
        {
            _rolePermissionAppService = rolePermissionAppService;
            _configuration = configuration;
        }

        public async Task AddToRedis()
        {
            await _rolePermissionAppService.AddToRedis();
            BackgroundJob.Schedule(() => AddToRedis(), TimeSpan.FromMinutes(int.Parse(_configuration.GetSection("Hangfire:RolePermission:AddToRedis_IntervalInMin").Value)));

        }
    }
}
