using AutoMapper;
using UserManagement.Application.Contracts.UserManagement;
using UserManagement.Application.Contracts.UserManagement.UserDto;
using UserManagement.Domain.UserManagement.Advocacy;
using UserManagement.Domain.UserManagement.Authorization.RolePermissions;
using UserManagement.Domain.UserManagement.bases;

namespace UserManagement.Application;

public class UserManagementApplciationMapperProfile : Profile
{
    public UserManagementApplciationMapperProfile()
    {


        CreateMap<PermissionDefinition, PermissionDefinitionDto>().ReverseMap();
        CreateMap<PermissionDefinitionChild, PermissionDefinitionChildDto>().ReverseMap();
        CreateMap<RolePermission, RolePermissionDto>().ReverseMap();


    }
}
