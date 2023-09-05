using Abp.Authorization;
using MongoDB.Bson;
using System.Reflection;
using UserManagement.Application.Contracts.UserManagement;
using UserManagement.Application.Contracts;//.UserManagement.RolePermissions;
using UserManagement.Application.Contracts.UserManagement.Services;
using UserManagement.Application.Contracts.UserManagement.UserDto;
using UserManagement.Domain.UserManagement.Authorization.RolePermissions;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace UserManagement.Application.UserManagement.Implementations
{
    public class RolePermissionService : ApplicationService, IRolePermissionService
    {
        private readonly IRepository<RolePermission, ObjectId> _rolePermissionRepository;
        public RolePermissionService(IRepository<RolePermission, ObjectId> rolePermissionRepository)
        {
            _rolePermissionRepository = rolePermissionRepository;
        }
        public async Task<List<RolePermissionDto>> GetList()
        {
            List<RolePermission> rolePermissions = new();

            var rolePermissionQuery = await _rolePermissionRepository.GetQueryableAsync();
            rolePermissions = rolePermissionQuery.ToList();
            var getRolePermission = ObjectMapper.Map<List<RolePermission>, List<RolePermissionDto>>(rolePermissions);
            return getRolePermission;
        }

        public async Task InsertList()
        {
            var rolePermissionDto = new RolePermissionDto()
            {

                Title = "Company",
                Permissions = new List<PermissionDataDto>()
                {
                      new PermissionDataDto()
                    {
                        Title = "SaleSchema",
                        Code = "00010001",
                    },
                    new PermissionDataDto()
                    {
                        Title = "SaleSchemaGetList",
                        Code = "000100020001",
                    },
                    new PermissionDataDto()
                    {
                        Title = "SaleSchemaAdd",
                        Code = "000100020002",
                    },
                    new PermissionDataDto()
                    {
                        Title = "SaleSchemaUpdate",
                        Code = "000100020003",
                    },
                       new PermissionDataDto()
                    {
                             Title = "SaleSchemaDelete",
                                Code = "000100020004",
                    } ,
                          new PermissionDataDto()
                    {
                              Title = "SaleSchemaGetById",
                                Code = "000100020005",
                    }   ,
                                  new PermissionDataDto()
                    {
                              Title = "SaleSchemaUploadFile",
                                Code = "000100020006",
                    }

                }
            };

            var b = ObjectMapper.Map<RolePermissionDto, RolePermission>(rolePermissionDto);
            var a = await _rolePermissionRepository.InsertAsync(b);
            var rolePermissions = (await _rolePermissionRepository.GetQueryableAsync())
                .ToList();
        }

   
    }
}
