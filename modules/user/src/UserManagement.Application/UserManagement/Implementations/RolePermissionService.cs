using MongoDB.Bson;
using UserManagement.Application.Contracts.UserManagement.Services;
using UserManagement.Domain.UserManagement.Authorization.RolePermissions;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Esale.Core.Caching;
using Abp.UI;
using UserManagement.Application.Contracts.UserManagement.Constant;
using Esale.Core.Constant;
using UserManagement.Application.Contracts.Models;

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
                await _cacheManager.SetAsync(role.Code, RedisCoreConstant.RolePermissionPrefix + "Role", role.Permissions.Select(x => x.Code), 2000, new CacheOptions { Provider = CacheProviderEnum.Hybrid });
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

        public async Task<RolePermissionDto> Add(RolePermissionDto dto)
        {
            var rolePermission = ObjectMapper.Map<RolePermissionDto, RolePermission>(dto);
            var entity = await _rolePermissionRepository.InsertAsync(rolePermission, autoSave: true);
            return ObjectMapper.Map<RolePermission, RolePermissionDto>(entity);
        }

        public async Task<RolePermissionDto> Update(RolePermissionDto dto)
        {

            var rolepermission = await Validation(ObjectId.Parse(dto.Id), dto);
            if (!string.IsNullOrEmpty(dto.Title))
                rolepermission.Title = dto.Title;
            if (!string.IsNullOrEmpty(dto.Code))
                rolepermission.Code = dto.Code;
            if (dto.Permissions.Count > 0)
            {
                rolepermission.Permissions = new List<PermissionDataDto>();
                foreach (var prm in dto.Permissions)
                {
                    rolepermission.Permissions.Add(new PermissionDataDto { Code = prm.Code });

                }
            }
            await _rolePermissionRepository.UpdateAsync(rolepermission, autoSave: true);
            return await GetById(rolepermission.Id);

        }

        public async Task<RolePermissionDto> GetById(ObjectId id)
        {
            var rolepermission = await Validation(id, null);
            var rolepermissionDto = ObjectMapper.Map<RolePermission, RolePermissionDto>(rolepermission);
            return rolepermissionDto;
        }
        public async Task<bool> Delete(ObjectId id)
        {
            await Validation(id, null);
            await _rolePermissionRepository.DeleteAsync(x => x.Id == id);
            return true;
        }

        private async Task<RolePermission> Validation(ObjectId id, RolePermissionDto dto)
        {
            var rolepermission = (await _rolePermissionRepository.GetQueryableAsync())
                .FirstOrDefault(x => x.Id == id);
            if (rolepermission is null)
            {
                throw new UserFriendlyException(PermissionConstant.RoleNotFound, PermissionConstant.RoleNotFoundId);
            }
            return rolepermission;
        }



    }
}
