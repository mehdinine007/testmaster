using Abp.Authorization;
using MongoDB.Bson;
using UserManagement.Application.Contracts.UserManagement;
using UserManagement.Application.Contracts.UserManagement.Services;
using UserManagement.Domain.UserManagement.bases;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace UserManagement.Application.UserManagement.Implementations
{
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
            try
            {
                var permissionQuery = await _permissionRepository.GetQueryableAsync();
                permissions = permissionQuery.ToList();
                var getpermission = ObjectMapper.Map<List<PermissionDefinition>, List<PermissionDefinitionDto>>(permissions);
                return getpermission;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        // public async Task InsertList()
        // {
        //var permissionDto = new PermissionDefinitionDto()
        //{
        //    title = "esale",
        //    nodeCode = "0001",
        //    childs = new List<PermissionDefinitionDto>()
        //{
        //    new PermissionDefinitionDto()
        //    {
        //        title = "order",
        //        nodeCode = "00010001"
        //    }
        //    ,new PermissionDefinitionDto()
        //    {
        //        title = "user",
        //        nodeCode = "00010002"
        //    }
        //}
        //};
        //await _permissionRepository.InsertAsync(ObjectMapper.Map<PermissionDefinitionDto, PermissionDefinition>(permissionDto));
        //var permissions = (await _permissionRepository.GetQueryableAsync())
        //    .ToList();
    //}

       

        public async Task<List<PermissionDefinitionDto>> Insert(PermissionDefinitionDto permission)
        {
            var permissionQuery = await _permissionRepository.GetQueryableAsync();
            var getpermission = ObjectMapper.Map<PermissionDefinitionDto, PermissionDefinition>(permission);
            var a = await _permissionRepository.InsertAsync(getpermission);
            return null;
        }
    }
}
