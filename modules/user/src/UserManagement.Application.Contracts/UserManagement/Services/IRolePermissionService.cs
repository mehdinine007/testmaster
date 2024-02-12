using Abp.Application.Services;
using MongoDB.Bson;
using UserManagement.Application.Contracts.Models;
using UserManagement.Domain.UserManagement.Enums;

namespace UserManagement.Application.Contracts.UserManagement.Services
{
    public interface IRolePermissionService : IApplicationService
    {
        Task<List<RolePermissionDto>> GetList();
       
        Task<bool> AddToRedis();
        Task<RolePermissionDto> Add(RolePermissionDto dto);
        Task<bool> AddDefaultRole(RolePermissionEnum? type);
        Task<RolePermissionDto> Update(RolePermissionDto dto);
        Task<RolePermissionDto> GetById(ObjectId id);
        Task<bool> Delete(ObjectId id);
        Task<bool> ValidationByCode(string code);
    }
}
