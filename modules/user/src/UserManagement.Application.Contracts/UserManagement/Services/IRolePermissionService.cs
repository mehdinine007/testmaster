using Abp.Application.Services;
using Abp.Authorization;
using MongoDB.Bson;
using System.Reflection;
using UserManagement.Application.Contracts.UserManagement;
using UserManagement.Application.Contracts;//.UserManagement.RolePermissions;
using UserManagement.Application.Contracts.UserManagement.UserDto;

namespace UserManagement.Application.Contracts.UserManagement.Services
{
    public interface IRolePermissionService : IApplicationService
    {
        Task<List<RolePermissionDto>> GetList();
         Task InsertList();
        Task AddToRedis();
    }
}
