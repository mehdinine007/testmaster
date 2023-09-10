using Abp.Authorization;
using Esale.Core.Caching;
using Esale.Core.IOC;
using MongoDB.Bson;
using Polly.Caching;
using UserManagement.Application.Contracts.UserManagement;
using UserManagement.Application.Contracts.UserManagement.Services;
using UserManagement.Application.Contracts.UserManagement.UserDto;
using UserManagement.Domain.UserManagement.bases;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace UserManagement.Application.UserManagement.Implementations;

public class PermissionDefinitionService : ApplicationService, IPermissionDefinitionService
{
    private readonly IRepository<PermissionDefinition, ObjectId> _permissionRepository;
    public PermissionDefinitionService(IRepository<PermissionDefinition, ObjectId> permissionRepository)
    {
        _permissionRepository = permissionRepository;
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
        var permissionDto = new PermissionDefinitionDto()
        {
            //title = "esale",
            //nodeCode = "0001",
            //childs = new List<PermissionDefinitionDto>()
            //{
            //    new PermissionDefinitionDto()
            //    {
            //        title = "order",
            //        nodeCode = "00010001"
            //    }
            //    , new PermissionDefinitionDto()
            //    {
            //        title = "user",
            //        nodeCode = "00010002"
            //    }
            //}

            Title = "OrderModule",
            Code = "0001",
            Children = new List<PermissionDefinitionChildDto>()
            {
                new PermissionDefinitionChildDto()
                {
                    Title = "SaleSchema",
                    Code = "00010001",
                    Children = new List<PermissionDefinitionChildDto>()
                    {
                        new PermissionDefinitionChildDto()
                        {
                            Title = "SaleSchemaGetList",
                            Code = "000100020001",
                        },
                        new PermissionDefinitionChildDto()
                        {
                            Title = "SaleSchemaAdd",
                            Code = "000100020002",
                        },
                        new PermissionDefinitionChildDto()
                        {
                            Title = "SaleSchemaUpdate",
                            Code = "000100020003",
                        },
                        new PermissionDefinitionChildDto()
                        {
                            Title = "SaleSchemaDelete",
                            Code = "000100020004",
                        },
                        new PermissionDefinitionChildDto()
                        {
                            Title = "SaleSchemaGetById",
                            Code = "000100020005",
                        },
                         new PermissionDefinitionChildDto()
                         {
                            Title = "SaleSchemaUploadFile",
                            Code = "000100020006",
                         },
                    }
                }

                ,new PermissionDefinitionChildDto()
                {
                    Title = "SaleDetail",
                    Code = "00010002",
                     Children = new List<PermissionDefinitionChildDto>()
                     {
                        new PermissionDefinitionChildDto()
                        {
                            Title = "SaleDetailGetSaleDetails",
                            Code = "000100030001",
                        },
                        new PermissionDefinitionChildDto()
                        {
                            Title = "SaleDetailSave",
                            Code = "000100030002",
                        },
                        new PermissionDefinitionChildDto()
                        {
                            Title = "SaleSchemaUpdate",
                            Code = "000100030003",
                        },
                        new PermissionDefinitionChildDto()
                        {
                            Title = "SaleSchemaDelete",
                            Code = "000100030004",
                        },
                        new PermissionDefinitionChildDto()
                        {
                            Title = "SaleDetailGetById",
                            Code = "000100030005",
                        },
                         new PermissionDefinitionChildDto()
                         {
                            Title = "SaleDetailGetActiveList",
                            Code = "000100030007",
                         },
                    }
                }
            }
        };
        var b = ObjectMapper.Map<PermissionDefinitionDto, PermissionDefinition>(permissionDto);
       var a =  await _permissionRepository.InsertAsync(b);
        var permissions = (await _permissionRepository.GetQueryableAsync())
            .ToList();
    }



    public async Task<List<PermissionDefinitionDto>> Insert(PermissionDefinitionDto permission)
    {
        var permissionQuery = await _permissionRepository.GetQueryableAsync();
        var getpermission = ObjectMapper.Map<PermissionDefinitionDto, PermissionDefinition>(permission);
        var a = await _permissionRepository.InsertAsync(getpermission);
        return null;
    }
}
