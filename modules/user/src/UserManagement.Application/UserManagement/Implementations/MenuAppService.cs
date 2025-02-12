﻿using Abp.Runtime.Security;
using IFG.Core.Caching;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson;
using MongoDB.Driver;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using UserManagement.Application.Contracts.Models;
using UserManagement.Application.Contracts.Services;
using UserManagement.Application.Contracts.UserManagement.Services;
using UserManagement.Domain.Authorization.Users;
using UserManagement.Domain.Shared;
using UserManagement.Domain.UserManagement.Authorization;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;
using WorkingWithMongoDB.WebAPI.Services;
using Polly.Caching;
using UserManagement.Domain.UserManagement.bases;
using UserManagement.Application.Contracts.UserManagement;
using UserManagement.Application.Contracts.UserManagement.Constant;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;
using UserManagement.Domain.UserManagement.Enums;
using Microsoft.AspNetCore.Http;
using Abp.Authorization;
using IFG.Core.Constant;
using IFG.Core.IOC;
using UserManagement.Domain.UserManagement.Authorization.RolePermissions;

using MongoDB.Bson.Serialization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.JsonPatch.Internal;

using System.Xml.Linq;
using NuGet.ContentModel;
using NuGet.Packaging;
using System.ComponentModel;
using System.Linq;
using System.Collections.Generic;
using Castle.MicroKernel.Registration;

using MongoDB.Driver.Core.Operations;
using System.Security;
using System.Net;
using Newtonsoft.Json;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace UserManagement.Application.UserManagement.Implementations;

public class MenuAppService : ApplicationService, IMenuAppService
{
    private readonly ICacheManager _cacheManager;
    private readonly IRepository<Menu, ObjectId> _menuRepository;
    private readonly IRepository<RolePermission, ObjectId> _rolePermissionRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;


    public MenuAppService(ICacheManager cacheManager,
                         IRepository<Menu, ObjectId> menuRepository,
                         IRepository<RolePermission, ObjectId> rolePermissionRepository,
                         IHttpContextAccessor httpContextAccessor
        )
    {
        _cacheManager = cacheManager;
        _menuRepository = menuRepository;
        _rolePermissionRepository = rolePermissionRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<List<MenuDto>> GetList()
    {

        var permissions= GetPermission();

        var menus = (await _menuRepository.GetQueryableAsync())
              .ToList();
        var getmenu = ObjectMapper.Map<List<Menu>, List<MenuDto>>(menus);

        if (!permissions.Any())
            throw new UserFriendlyException(PermissionConstant.PermissionNotFound, PermissionConstant.PermissionNotFoundId);


        var activeMenu = new List<MenuChildDto>();
        foreach (var menu in getmenu)
        {
            if (menu.Children.Any())
                activeMenu.AddRange(getFirstParent(menu.Children, permissions,new List<MenuChildDto>()));
        }

      

        var _menu = new List<MenuDto>();

        foreach (var menu in getmenu)
        {
            if (activeMenu.Any(x => x.Code.Substring(0, menu.Code.Length) == menu.Code))
            {
                var _child = GetListWithParent(menu.Children, activeMenu,permissions);
                menu.Children = _child;
                _menu.Add(menu);
            }
        }
        return _menu;
    }

    private List<MenuChildDto> getFirstParent(List<MenuChildDto> getmenu, List<string> permissions, List<MenuChildDto> menulst)
    {
        foreach (var menu in getmenu)
        {
            if (menu.Children.Any())
                 getFirstParent(menu.Children, permissions, menulst);
            else
            {
                if (menu.Permissions.Any(x => permissions.Any(p => p == x.Code)))
                {
                    menulst.Add(menu);
                }
            }
        }
        return menulst;
    }

    private List<string> GetPermission()
    {

        var role = _httpContextAccessor.HttpContext.User.Claims
                .Where(x => x.Type.Equals(ClaimTypes.Role))
                .FirstOrDefault();
        if (role is null)
        {
            throw new UserFriendlyException(CoreMessage.AuthenticationDenied, CoreMessage.AuthenticationDeniedId);
        }

        var rolePermission = _cacheManager.Get<List<string>>("Role" + role.Value,
        RedisCoreConstant.RolePermissionPrefix,
        new CacheOptions()
        {
            Provider = CacheProviderEnum.Hybrid
        });
        return rolePermission;


    }
    private List<MenuChildDto> GetListWithParent(List<MenuChildDto> children, List<MenuChildDto> activeMenu, List<string> permissions)
    {

        var _child = new List<MenuChildDto>();
        foreach (var per in children)
        {
            if (activeMenu.Any(x => x.Code.Substring(0, per.Code.Length) == per.Code))
            {
                if (per.Children != null && per.Children.Count > 0)
                    per.Children = GetListWithParent(per.Children, activeMenu, permissions);
                if (per.Permissions != null && per.Permissions.Count > 0)
                    per.Permissions = per.Permissions
                        .Where(x => permissions.Any(p => p == x.Code))
                        .ToList();
                _child.Add(per);
            }
        }
        return _child;
    }
   

    public async Task<MenuDto> GetById(ObjectId Id)
    {
        var menu = await Validation(Id, null);
        var result = ObjectMapper.Map<Menu, MenuDto>(menu);
        return result;
    }

    public async Task<MenuDto> Add(MenuDto input)
    {
        var menu = ObjectMapper.Map<MenuDto, Menu>(input);
        var result = await _menuRepository.InsertAsync(menu);
        return ObjectMapper.Map<Menu, MenuDto>(result);
    }

    public async Task<MenuDto> Update(MenuDto input)
    {
        var menu = (await _menuRepository.GetQueryableAsync()).FirstOrDefault(x => x.Id == ObjectId.Parse(input.Id));
        if (menu is null)
            throw new UserFriendlyException(PermissionConstant.MenuNotFound, PermissionConstant.MenuNotFoundId);

        if (!string.IsNullOrEmpty(input.Title))
            menu.Title = input.Title;
        if (!string.IsNullOrEmpty(input.Code))
            menu.Code = input.Code;
        if (input.Type > 0 && input.Type != menu.Type)
            menu.Type = input.Type;
        if (!string.IsNullOrEmpty(input.Url))
            menu.Url = input.Url;

        var result = await _menuRepository.UpdateAsync(menu);
        return ObjectMapper.Map<Menu, MenuDto>(result);

    }

    public async Task<bool> Delete(ObjectId Id)
    {
        var menu = await GetById(Id);
        var menus = await _menuRepository.GetQueryableAsync();
        if (menus != null)
            throw new UserFriendlyException("این منو در حال استفاده است");
        await _menuRepository.DeleteAsync(Id);
        return true;

    }
    public async Task InsertList()
    {

        var a = "Menu_0001";
        var s = new List<string>() { "00010001", "000100020001" };
        var dd = new Menu();
        await _cacheManager.SetAsync(a,
           "n:Menu:", s, 5000
           , new CacheOptions()
           {
               Provider = CacheProviderEnum.Hybrid
           });

        var menusDto = new List<MenuDto>(){

            new MenuDto()
            {
                Code = "0001",
                Title = "پنل خودروساز",
                Icon = string.Empty,
                Url = string.Empty,
                Type = (int)MenuEnum.Category,
                Children=new List<MenuChildDto>()
                {
                     new MenuChildDto()
                     {
                        Code = "00010001",
                        Title = "اطلاعات پایه",
                        Icon = string.Empty,
                        Url = string.Empty,
                        Type = (int)MenuEnum.Category,
                        Children = new List<MenuChildDto>()
                     {
                                new MenuChildDto()
                                {
                                    Code = "000100010001",
                                    Title = "بخشنامه",
                                    Icon = string.Empty,
                                    Type = (int)MenuEnum.Form,
                                    Url = string.Empty,
                                    Permissions= new List<PermissionDefinitionChildDto>()
                                    {
                                        new PermissionDefinitionChildDto
                                        {
                                             Title="view",
                                             Code="0001000100010001",
                                             DisplayName="نمایش"
                                        },
                                           new PermissionDefinitionChildDto
                                        {
                                             Title="create",
                                             Code="0001000100010002",
                                             DisplayName="ایجاد"
                                        },
                                              new PermissionDefinitionChildDto
                                        {
                                             Title="update",
                                             Code="0001000100010003",
                                             DisplayName="ویرایش"
                                        },
                                                 new PermissionDefinitionChildDto
                                        {
                                             Title="delete",
                                             Code="0001000100010004",
                                             DisplayName="حذف"
                                        },
                                    }
                                }
                     }

                } ,

                     new MenuChildDto()
                     {
                        Code = "00010002",
                        Title = "گزارشات",
                        Icon = string.Empty,
                        Url = string.Empty,
                        Type = (int)MenuEnum.Category

                     } ,

                     new MenuChildDto()
                     {
                        Code = "00010003",
                        Title = "لیست مشتریان",
                        Icon = string.Empty,
                        Url = string.Empty,
                        Type = (int)MenuEnum.Category ,
                         Permissions= new List<PermissionDefinitionChildDto>()
                            {
                                new PermissionDefinitionChildDto
                                {
                                        Title="view",
                                        Code="000100030001",
                                        DisplayName="نمایش"
                                },
                                    new PermissionDefinitionChildDto
                                {
                                        Title="create",
                                        Code="000100030002",
                                        DisplayName="ایجاد"
                                },
                                        new PermissionDefinitionChildDto
                                {
                                        Title="update",
                                        Code="000100030003",
                                        DisplayName="ویرایش"
                                },
                                            new PermissionDefinitionChildDto
                                {
                                        Title="delete",
                                        Code="000100030004",
                                        DisplayName="حذف"
                                },
                            }
                        }

                }

            }


        };

        var map = ObjectMapper.Map<List<MenuDto>, List<Menu>>(menusDto);
        await _menuRepository.InsertManyAsync(map);
        var menus = (await _menuRepository.GetQueryableAsync())
            .ToList();
    }

    private async Task<Menu> Validation(ObjectId id, MenuDto value)
    {
        var menu = (await _menuRepository.GetQueryableAsync()).FirstOrDefault(x => x.Id == id);
        if (menu is null)
        {
            throw new UserFriendlyException(PermissionConstant.MenuNotFound, PermissionConstant.MenuNotFoundId);
        }
        return menu;
    }

    public async Task SeedMenu()
    {
        var currentDirectory = Environment.CurrentDirectory;
        const string fileName = "UpdateMenuPolicy.json";
        var fullPath = Path.Combine(currentDirectory, fileName);
        var content = File.ReadAllText(fileName);
        var ls = new List<Menu>(JsonConvert.DeserializeObject<List<Menu>>(content));
        await _menuRepository.InsertManyAsync(ls);
    }

}
