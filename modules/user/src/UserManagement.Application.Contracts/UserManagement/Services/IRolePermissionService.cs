using Abp.Application.Services;
using MongoDB.Bson;
using UserManagement.Application.Contracts.Models;

namespace UserManagement.Application.Contracts.UserManagement.Services
{
    public interface IRolePermissionService : IApplicationService
    {
        Task<List<RolePermissionDto>> GetList();
       
        Task InsertList(RolePermissionDto dto);
        Task AddToRedis();
        Task<RolePermissionDto> Add(RolePermissionDto dto);
        Task<RolePermissionDto> Update(RolePermissionDto dto);
        Task<RolePermissionDto> GetById(ObjectId id);
        Task<bool> Delete(ObjectId id);
        Task<bool> ValidationByCode(string code);
    }
}
