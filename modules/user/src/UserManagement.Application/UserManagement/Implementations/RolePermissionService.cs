using MongoDB.Bson;
using UserManagement.Application.Contracts.UserManagement.Services;
using UserManagement.Domain.UserManagement.Authorization.RolePermissions;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using IFG.Core.Caching;
using Abp.UI;
using UserManagement.Application.Contracts.UserManagement.Constant;
using IFG.Core.Constant;
using Core.Utility.Tools;
using UserManagement.Application.Contracts.Models;

namespace UserManagement.Application.UserManagement.Implementations
{
    public class RolePermissionService : ApplicationService, IRolePermissionService
    {
        private readonly IRepository<RolePermission, ObjectId> _rolePermissionRepository;
        private readonly IRepository<RolePermissionWrite, ObjectId> _rolePermissionWritRepository;
        private readonly ICacheManager _cacheManager;
        private readonly IPermissionDefinitionService _permissionDefinitionService;


        public RolePermissionService(IRepository<RolePermission, ObjectId> rolePermissionRepository,
                                     ICacheManager cacheManager,
                                     IRepository<RolePermissionWrite, ObjectId> rolePermissionWritRepository,
                                     IPermissionDefinitionService permissionDefinitionService)
        {
            _rolePermissionRepository = rolePermissionRepository;
            _cacheManager = cacheManager;
            _rolePermissionWritRepository = rolePermissionWritRepository;
            _permissionDefinitionService = permissionDefinitionService;
        }


        public async Task AddToRedis()
        {
            var roleperm = await GetList();
            foreach (var role in roleperm)
            {
                await _cacheManager.SetAsync("Role"+role.Code, RedisCoreConstant.RolePermissionPrefix,role.Permissions, 90000, new CacheOptions { Provider = CacheProviderEnum.Redis });
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

        public async Task InsertList(RolePermissionDto dto)
        {
            var srviceCodeList =new  List<string>();
            var permissions =await _permissionDefinitionService.GetList();
            foreach (var madule in permissions)
            {
                foreach (var service in madule.Children)
                {
                    srviceCodeList.AddRange(service.Children.Select(x=>x.Code).ToList());
                }  
            }
            dto.Permissions = srviceCodeList;
            await Add(dto);
           // var permis = new List<string>() { "00010001", "000100020001", "000100020002", "000100020003", "000100020004", "000100020005", "000100020006" };
            //var rolePermissionDto = new RolePermissionDto()
            //{

            //    Title = "Customer",
            //    Code = "0001",
            //    Permissions = permis

            //};

            //var b = ObjectMapper.Map<RolePermissionDto, RolePermission>(rolePermissionDto);
            //var a = await _rolePermissionRepository.InsertAsync(b);
            //var rolePermissions = (await _rolePermissionRepository.GetQueryableAsync())
            //    .ToList();
        }

        public async Task<RolePermissionDto> Add(RolePermissionDto dto)
        {
            var role = await _rolePermissionRepository.GetQueryableAsync();
            string maxcode = default;
            if (role.Any())
                maxcode = role.Max(x => x.Code);

            dto.Code = StringHelper.GenerateTreeNodePath(maxcode, null, 4);
            var rolePermission = ObjectMapper.Map<RolePermissionDto, RolePermissionWrite>(dto);

            var entity = await _rolePermissionWritRepository.InsertAsync(rolePermission, autoSave: true);
            return ObjectMapper.Map<RolePermissionWrite, RolePermissionDto>(entity);
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
                rolepermission.Permissions = new List<string>();
                foreach (var prm in dto.Permissions)
                {
                    rolepermission.Permissions.Add(prm);

                }
            }
            await _rolePermissionWritRepository.UpdateAsync(ObjectMapper.Map<RolePermission, RolePermissionWrite>(rolepermission), autoSave: true);
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
            await _rolePermissionWritRepository.DeleteAsync(x => x.Id == id);
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


        public async Task<bool> ValidationByCode(string code)
        {
            var role = (await _rolePermissionRepository.GetQueryableAsync())
                .FirstOrDefault(x => x.Code == code);
            if (role is null)
            {
                throw new UserFriendlyException(PermissionConstant.RoleNotFound, PermissionConstant.RoleNotFoundId);
            }
            return true;
        }
    }
}
