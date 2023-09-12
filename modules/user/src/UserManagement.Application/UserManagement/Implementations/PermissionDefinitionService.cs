using Abp.Authorization;
using Abp.UI;
using Esale.Core.Caching;
using Esale.Core.IOC;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;
using MongoDB.Bson;
using Nest;
using Polly.Caching;
using UserManagement.Application.Contracts.UserManagement;
using UserManagement.Application.Contracts.UserManagement.Constant;
using UserManagement.Application.Contracts.UserManagement.Services;
using UserManagement.Application.Contracts.UserManagement.UserDto;
using UserManagement.Domain.UserManagement.Authorization.RolePermissions;
using UserManagement.Domain.UserManagement.bases;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace UserManagement.Application.UserManagement.Implementations
{
    public class PermissionDefinitionService : ApplicationService//, IPermissionDefinitionService
    {
        private readonly IRepository<PermissionDefinition, ObjectId> _permissionRepository;
        private readonly IRepository<RolePermission, ObjectId> _rolepermissionRepository;
        private readonly ICacheManager _cacheManager;

        public PermissionDefinitionService(IRepository<PermissionDefinition, ObjectId> permissionRepository,
        IRepository<RolePermission, ObjectId> rolepermissionRepository,
        ICacheManager cacheManager)
        {
            _permissionRepository = permissionRepository;
            _rolepermissionRepository = rolepermissionRepository;
            _cacheManager = cacheManager;
        }

        public async Task<List<PermissionDefinitionDto>> GetList()
        {
            List<PermissionDefinition> permissions = new();

            var permissionQuery = await _permissionRepository.GetQueryableAsync();
            permissions = permissionQuery.ToList();
            var getpermission = ObjectMapper.Map<List<PermissionDefinition>, List<PermissionDefinitionDto>>(permissions);
            return getpermission;

        }
        public async Task InsertList()
        {

            var a = "Role_0001";
            var s = new List<string>() { "00010001", "000100020001" };
            var dd = new PermissionDefinition();
            await _cacheManager.SetAsync(a,
               "n:RolePermission:", s, 5000
               , new CacheOptions()
               {
                   Provider = CacheProviderEnum.Hybrid
               });
            // var permissionDto = new PermissionDefinitionDto()
            // {
            //     //title = "esale",
            //     //nodeCode = "0001",
            //     //childs = new List<PermissionDefinitionDto>()
            //     //{
            //     //    new PermissionDefinitionDto()
            //     //    {
            //     //        title = "order",
            //     //        nodeCode = "00010001"
            //     //    }
            //     //    , new PermissionDefinitionDto()
            //     //    {
            //     //        title = "user",
            //     //        nodeCode = "00010002"
            //     //    }
            //     //}

            //     Title = "OrderModule",
            //     Code = "0001",
            //     Children = new List<PermissionDefinitionChildDto>()
            //     {
            //         new PermissionDefinitionChildDto()
            //         {
            //             Title = "SaleSchema",
            //             Code = "00010001",
            //             Children = new List<PermissionDefinitionChildDto>()
            //             {
            //                 new PermissionDefinitionChildDto()
            //                 {
            //                     Title = "SaleSchemaGetList",
            //                     Code = "000100020001",
            //                 },
            //                 new PermissionDefinitionChildDto()
            //                 {
            //                     Title = "SaleSchemaAdd",
            //                     Code = "000100020002",
            //                 },
            //                 new PermissionDefinitionChildDto()
            //                 {
            //                     Title = "SaleSchemaUpdate",
            //                     Code = "000100020003",
            //                 },
            //                 new PermissionDefinitionChildDto()
            //                 {
            //                     Title = "SaleSchemaDelete",
            //                     Code = "000100020004",
            //                 },
            //                 new PermissionDefinitionChildDto()
            //                 {
            //                     Title = "SaleSchemaGetById",
            //                     Code = "000100020005",
            //                 },
            //                  new PermissionDefinitionChildDto()
            //                  {
            //                     Title = "SaleSchemaUploadFile",
            //                     Code = "000100020006",
            //                  },
            //             }
            //         }

            //         ,new PermissionDefinitionChildDto()
            //         {
            //             Title = "SaleDetail",
            //             Code = "00010002",
            //              Children = new List<PermissionDefinitionChildDto>()
            //              {
            //                 new PermissionDefinitionChildDto()
            //                 {
            //                     Title = "SaleDetailGetSaleDetails",
            //                     Code = "000100030001",
            //                 },
            //                 new PermissionDefinitionChildDto()
            //                 {
            //                     Title = "SaleDetailSave",
            //                     Code = "000100030002",
            //                 },
            //                 new PermissionDefinitionChildDto()
            //                 {
            //                     Title = "SaleSchemaUpdate",
            //                     Code = "000100030003",
            //                 },
            //                 new PermissionDefinitionChildDto()
            //                 {
            //                     Title = "SaleSchemaDelete",
            //                     Code = "000100030004",
            //                 },
            //                 new PermissionDefinitionChildDto()
            //                 {
            //                     Title = "SaleDetailGetById",
            //                     Code = "000100030005",
            //                 },
            //                  new PermissionDefinitionChildDto()
            //                  {
            //                     Title = "SaleDetailGetActiveList",
            //                     Code = "000100030007",
            //                  },
            //             }
            //         }
            //     }
            // };
            // var b = ObjectMapper.Map<PermissionDefinitionDto, PermissionDefinition>(permissionDto);
            //var a =  await _permissionRepository.InsertAsync(b);
            // var permissions = (await _permissionRepository.GetQueryableAsync())
            //     .ToList();
        }



        public async Task<List<PermissionDefinitionDto>> Insert(PermissionDefinitionDto permission)
        {
            var permissionQuery = await _permissionRepository.GetQueryableAsync();
            var getpermission = ObjectMapper.Map<PermissionDefinitionDto, PermissionDefinition>(permission);
            var a = await _permissionRepository.InsertAsync(getpermission);
            return null;
        }

        public async Task<PermissionDefinitionDto> GetById(ObjectId Id)
        {
            var permissionDefinition = await Validation(Id, null);
            var result = ObjectMapper.Map<PermissionDefinition, PermissionDefinitionDto>(permissionDefinition);
            return result;
        }

        private async Task<PermissionDefinition> Validation(ObjectId id, PermissionDefinitionDto value)
        {
            var permissiondefenition = (await _permissionRepository.GetQueryableAsync()).FirstOrDefault(x => x.Id == id);
            if (permissiondefenition is null)
            {
                throw new UserFriendlyException(PermissionConstant.PermissionNotFound, PermissionConstant.PermissionNotFoundId);
            }
            return permissiondefenition;
        }

        public async Task<PermissionDefinitionDto> Add(PermissionDefinitionDto permission)
        {
           var permissiondefinition = ObjectMapper.Map<PermissionDefinitionDto, PermissionDefinition>(permission);
            var result = await _permissionRepository.InsertAsync(permissiondefinition);
            return ObjectMapper.Map<PermissionDefinition, PermissionDefinitionDto>(result);
        }

        public async Task<PermissionDefinitionDto> Update(PermissionDefinitionDto input)
        {
            var rolePermission = (await _rolepermissionRepository.GetQueryableAsync()).FirstOrDefault(x => x.Code == input.Code);
            var permissiondefenition = (await _permissionRepository.GetQueryableAsync()).FirstOrDefault(x => x.Id == ObjectId.Parse(input.Id) && x.Code == input.Code);

            //var permission =  ObjectMapper.Map<PermissionDefinition, PermissionDefinitionDto>(permissiondefenition);
            if (!string.IsNullOrEmpty(input.Title))
                permissiondefenition.Title = input.Title;
            if (!string.IsNullOrEmpty(input.Code))
                permissiondefenition.Code = input.Code;
            //if(permission.Children.Count() > 0)
            //{
            //    permission.Children = new List<PermissionDefinitionChildDto>();
            //    foreach (var child in permission.Children)
            //    {
            //        permission.Children.Add(new PermissionDefinitionChildDto()
            //        {
            //            Title = child.Title,
            //            Code = child.Code,
            //            Children = new List<PermissionDefinitionChildDto>()
            //        });
            //    }

            //}

                      
            var result = await _permissionRepository.UpdateAsync(permissiondefenition);
            return ObjectMapper.Map<PermissionDefinition, PermissionDefinitionDto>(result);

        }

        public async Task<bool> Delete(ObjectId Id)
        {
            var permissiondefenission = await GetById(Id);
            var rolePermission = await _rolepermissionRepository.GetQueryableAsync();
            if (rolePermission != null)
                throw new UserFriendlyException("این دسته بندی در حال استفاده است");
            await _permissionRepository.DeleteAsync(Id);
            return true;

        }
    }
}
