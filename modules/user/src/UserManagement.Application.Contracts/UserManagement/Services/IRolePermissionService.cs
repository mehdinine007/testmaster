using Abp.Application.Services;
using UserManagement.Application.Contracts.Models;

namespace UserManagement.Application.Contracts.UserManagement.Services
{
    public interface IRolePermissionService : IApplicationService
    {
        Task<List<RolePermissionDto>> GetList();
         Task InsertList();
        Task AddToRedis();
    }
}
