using AutoMapper;
using UserManagement.Application.Contracts.Models;
using UserManagement.Application.Contracts.UserManagement;
using UserManagement.Domain.Authorization.Users;
using UserManagement.Domain.UserManagement.Advocacy;
using UserManagement.Domain.UserManagement.Authorization.RolePermissions;
using UserManagement.Domain.UserManagement.bases;
using Volo.Abp.AutoMapper;

namespace UserManagement.Application;

public class UserManagementApplciationMapperProfile : Profile
{
    public UserManagementApplciationMapperProfile()
    {
        CreateMap<PermissionDefinition, PermissionDefinitionDto>().ReverseMap();
        CreateMap<PermissionDefinitionChild, PermissionDefinitionChildDto>().ReverseMap();
        CreateMap<RolePermission, RolePermissionDto>().ReverseMap();
        CreateMap<CreateUserDto, UserMongo>();
        CreateMap<AdvocacyUsersFromBank, AdvocacyUsersFromBankWithCompanyDto>();
        CreateMap<UserMongo, UserDto>().ReverseMap();

        CreateMap<PermissionDefinition, PermissionDefinitionWrite>()
    .ReverseMap();
        CreateMap<PermissionDefinitionDto, PermissionDefinitionWrite>()
            .ReverseMap();
        CreateMap<RolePermissionWrite, RolePermissionDto>()
            .ReverseMap()
            .IgnoreFullAuditedObjectProperties();
        CreateMap<RolePermission, RolePermissionWrite>()
            .ReverseMap();
        CreateMap<UserMongo, UserMongoWrite>()
            .ReverseMap();
        CreateMap<Menu, MenuDto>().ReverseMap();
        CreateMap<MenuChild, MenuChildDto>().ReverseMap();
    }
}
