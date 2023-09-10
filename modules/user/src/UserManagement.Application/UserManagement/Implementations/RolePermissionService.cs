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
using Esale.Core.Caching;
using Authorization;

namespace UserManagement.Application.UserManagement.Implementations
{
    public class RolePermissionService : ApplicationService, IRolePermissionService
    {
        private readonly IRepository<RolePermission, ObjectId> _rolePermissionRepository;
        private readonly ICacheManager _cacheManager;

        public RolePermissionService(IRepository<RolePermission, ObjectId> rolePermissionRepository, ICacheManager cacheManager)
        {
            _rolePermissionRepository = rolePermissionRepository;
            _cacheManager = cacheManager;
        }

        public async Task AddToRedis()
        {
            var roleperm = await GetList();
            foreach (var role in roleperm)
            {
                await _cacheManager.SetAsync(role.Code, RolePermissionConstants.RolePermissionPrefix +"Role", role.Permissions.Select(x => x.Code), 2000, new CacheOptions { Provider = CacheProviderEnum.Hybrid });
            }
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
                Code = "0001",
                Permissions = new List<PermissionDataDto>()
                {
                      new PermissionDataDto()
                    {

                        Code = "00010001",
                    },
                    new PermissionDataDto()
                    {

                        Code = "000100020001",
                    },
                    new PermissionDataDto()
                    {

                        Code = "000100020002",
                    },
                    new PermissionDataDto()
                    {

                        Code = "000100020003",
                    },
                       new PermissionDataDto()
                    {
                     Code = "000100020004",
                    } ,
                          new PermissionDataDto()
                    {
  Code = "000100020005",
                    }   ,
                                  new PermissionDataDto()
                    {

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
