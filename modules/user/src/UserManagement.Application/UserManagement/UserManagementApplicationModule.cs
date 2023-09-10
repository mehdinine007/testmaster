﻿using Abp.Dependency;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using UserManagement.Application.Contracts;
using UserManagement.Domain.Authorization.Users;
using UserManagement.Domain.UserManagement;
using UserManagement.Domain.UserManagement.Authorization;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

namespace UserManagement.Application;

[DependsOn(
    typeof(UserManagementDomainModule),
    typeof(UserManagementApplicationContractsModule))]
public class UserManagementApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<UserManagementApplicationModule>();
        });
        context.Services.AddScoped(typeof(IPasswordHasher<>),typeof(PasswordHasher<>));
    }
}
