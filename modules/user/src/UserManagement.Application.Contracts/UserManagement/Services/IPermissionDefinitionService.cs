using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Application.Contracts.UserManagement.UserDto;
using UserManagement.Domain.UserManagement.Authorization.RolePermissions;
using Volo.Abp.Application.Services;

namespace UserManagement.Application.Contracts.UserManagement.Services
{
    public interface IPermissionDefinitionService : IApplicationService
    {
        Task InsertList();
        Task<List<PermissionDefinitionDto>> GetList();
        Task<PermissionDefinitionDto> GetById(ObjectId Id);
        Task<List<PermissionDefinitionDto>> Insert(PermissionDefinitionDto permission);
        Task<PermissionDefinitionDto> Add(PermissionDefinitionDto permission);
        Task<PermissionDefinitionDto> Update(PermissionDefinitionDto permission);
        Task<bool> Delete(ObjectId Id);
    }
}
