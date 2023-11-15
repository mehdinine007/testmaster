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
using UserManagement.Domain.UserManagement.Enums;
using NuGet.Packaging;
using MongoDB.Driver;
using Esale.Share.Authorize;

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
                await _cacheManager.SetAsync("Role" + role.Code, RedisCoreConstant.RolePermissionPrefix, role.Permissions, 90000, new CacheOptions { Provider = CacheProviderEnum.Hybrid });
            }
        }
        [SecuredOperation(RolePermissionServicePermissionConstants.GetList)]
        public async Task<List<RolePermissionDto>> GetList()
        {
            List<RolePermission> rolePermissions = new();

            var rolePermissionQuery = await _rolePermissionRepository.GetQueryableAsync();
            rolePermissions = rolePermissionQuery.ToList();
            var getRolePermission = ObjectMapper.Map<List<RolePermission>, List<RolePermissionDto>>(rolePermissions);
            return getRolePermission;
        }

        public async Task<bool> AddDefaultRole(RolePermissionEnum? type)
        {
            RolePermissionDto rolePermission = new RolePermissionDto();
            var permissions = await _permissionDefinitionService.GetList();
            var rolePermissions = await _rolePermissionRepository.GetListAsync();

            var existRole = rolePermissions.Where(x => x.Type == type).FirstOrDefault();
            if (existRole != null)
            {
                await _rolePermissionWritRepository.HardDeleteAsync(ObjectMapper.Map<RolePermission, RolePermissionWrite>(existRole), autoSave: true);
            }
    
            if (type == RolePermissionEnum.Admin || type == null)
            {
            var serviceList = new List<string>();
                foreach (var per in permissions)
                {
                    foreach (var child in per.Children)
                    {
                        serviceList.AddRange(child.Children.Select(c => c.Code).ToList());
                    }
                }
                rolePermission.Title = RolePermissionEnum.Admin.ToString();
                rolePermission.Type = RolePermissionEnum.Admin;
                rolePermission.Permissions = serviceList;
                await Add(rolePermission);
            }

            if (type == RolePermissionEnum.Customer || type == null)
            {
                var serviceList = new List<string>();
                var permission = permissions.Where(x => x.Code == "0001" || x.Code == "0002").ToList();
                foreach (var per in permission)
                {
                    var code = per.Children.Select(x => x.Code).ToList();

                    foreach (var child in per.Children)
                    {
                        serviceList.AddRange(child.Children.Select(c => c.Code).ToList());
                    }
                }
                rolePermission.Title = RolePermissionEnum.Customer.ToString();
                rolePermission.Type = RolePermissionEnum.Customer;
                rolePermission.Permissions = serviceList;
                await Add(rolePermission);
            }
            if (type == RolePermissionEnum.Company || type == null)
            {
                var serviceList = new List<string>();
                var permission = permissions.Where(x => x.Code == "0003").ToList();
                foreach (var per in permission)
                {
                    var code = per.Children.Select(x => x.Code).ToList();
                    foreach (var child in per.Children)
                    {
                        serviceList.AddRange(child.Children.Select(c => c.Code).ToList());
                    }
                }
                rolePermission.Title = RolePermissionEnum.Company.ToString();
                rolePermission.Type = RolePermissionEnum.Company;
                rolePermission.Permissions = serviceList;
                await Add(rolePermission);
            }
            if (type == RolePermissionEnum.nicc )
            {
                var serviceList = new List<string>();
                var permission = permissions.Where(x => x.Code == "0005").ToList();
                foreach (var per in permission)
                {
                    var code = per.Children.Select(x => x.Code).ToList();
                    foreach (var child in per.Children)
                    {
                        serviceList.AddRange(child.Children.Select(c => c.Code).ToList());
                    }
                }
                rolePermission.Title = RolePermissionEnum.nicc.ToString();
                rolePermission.Type = RolePermissionEnum.nicc;
                rolePermission.Permissions = serviceList;
                await Add(rolePermission);
            }
            if (type == RolePermissionEnum.InspectionOrganization )
            {
                var serviceList = new List<string>();
                var permission = permissions.Where(x => x.Code == "0005").ToList();
                foreach (var per in permission)
                {
                    var code = per.Children.Select(x => x.Code).ToList();
                    foreach (var child in per.Children)
                    {
                        serviceList.AddRange(child.Children.Select(c => c.Code).ToList());
                    }
                }
                rolePermission.Title = RolePermissionEnum.InspectionOrganization.ToString();
                rolePermission.Type = RolePermissionEnum.InspectionOrganization;
                rolePermission.Permissions = serviceList;
                await Add(rolePermission);
            }
            if (type == RolePermissionEnum.mimt)
            {
                var serviceList = new List<string>();
                var permission = permissions.Where(x => x.Code == "0005").ToList();
                foreach (var per in permission)
                {
                    var code = per.Children.Select(x => x.Code).ToList();
                    foreach (var child in per.Children)
                    {
                        serviceList.AddRange(child.Children.Select(c => c.Code).ToList());
                    }
                }
                rolePermission.Title = RolePermissionEnum.mimt.ToString();
                rolePermission.Type = RolePermissionEnum.mimt;
                rolePermission.Permissions = serviceList;
                await Add(rolePermission);
            }
            return true;
        }

        [SecuredOperation(RolePermissionServicePermissionConstants.Add)]
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

        [SecuredOperation(RolePermissionServicePermissionConstants.Update)]
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

        [SecuredOperation(RolePermissionServicePermissionConstants.GetById)]
        public async Task<RolePermissionDto> GetById(ObjectId id)
        {
            var rolepermission = await Validation(id, null);
            var rolepermissionDto = ObjectMapper.Map<RolePermission, RolePermissionDto>(rolepermission);
            return rolepermissionDto;
        }
        [SecuredOperation(RolePermissionServicePermissionConstants.Delete)]
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
