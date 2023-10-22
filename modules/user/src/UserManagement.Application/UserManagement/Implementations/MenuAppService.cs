using Abp.Runtime.Security;
using Esale.Core.Caching;
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

namespace UserManagement.Application.UserManagement.Implementations;

public class MenuAppService : ApplicationService, IMenuAppService
{
    private readonly ICacheManager _cacheManager;
    private readonly IRepository<Menu, ObjectId> _menuRepository;


    public MenuAppService(ICacheManager cacheManager, IRepository<Menu, ObjectId> menuRepository
        )
    {
        _cacheManager = cacheManager;
        _menuRepository = menuRepository;
    }

    public async Task<List<MenuDto>> GetList()
    {
        List<Menu> menu = new();

        var menus = await _menuRepository.GetQueryableAsync();
        menu = menus.ToList();
        var getmenu = ObjectMapper.Map<List<Menu>, List<MenuDto>>(menu);
        return getmenu;

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
        var menuDto = new MenuDto()
        {
            Code = "0001",
            Title = "اطلاعات پایه",
            Icon = string.Empty,
            Url = string.Empty,
            Type = (int)MenuEnum.Category,
            Children = new List<MenuChildDto>()
            {
                 new MenuChildDto()
                {
                Code = "00010001",
                    Title = "تعریف خودرو",
                    Icon = string.Empty,
                    Type = (int)MenuEnum.Category,
                    Url = string.Empty ,
                    Children= new List<MenuChildDto>()
                    {
                        new MenuChildDto() {
                        Code = "000100010001",
                        Title = "نمایش",
                        Icon = string.Empty,
                        Type = (int)MenuEnum.Form,
                        Url = string.Empty,
                        Permissions=new List<PermissionDefinitionChildDto>() {
                                  new PermissionDefinitionChildDto()
                                  {
                                     Code="",
                                     DisplayName="",
                                     Title=""
                                  }
                        }
                        } ,
                         new MenuChildDto() {
                        Code = "000100010002",
                        Title = "ایجاد",
                        Icon = string.Empty,
                        Type = (int)MenuEnum.Form,
                        Url = string.Empty ,
                          Permissions=new List<PermissionDefinitionChildDto>() {
                                  new PermissionDefinitionChildDto()
                                  {
                                     Code="",
                                     DisplayName="",
                                     Title=""
                                  }
                        }
                        } ,
                          new MenuChildDto() {
                        Code = "000100010003",
                        Title = "ویرایش",
                        Icon = string.Empty,
                        Type = (int)MenuEnum.Form,
                        Url = string.Empty ,
                          Permissions=new List<PermissionDefinitionChildDto>() {
                                  new PermissionDefinitionChildDto()
                                  {
                                     Code="",
                                     DisplayName="",
                                     Title=""
                                  }
                        }
                          },
                        new MenuChildDto() {
                        Code = "000100010004",
                        Title = "حذف",
                        Icon = string.Empty,
                        Type = (int)MenuEnum.Form,
                        Url = string.Empty ,
                          Permissions=new List<PermissionDefinitionChildDto>() {
                                  new PermissionDefinitionChildDto()
                                  {
                                     Code="",
                                     DisplayName="",
                                     Title=""
                                  }
                        }
                        }
                    }
                  } ,

            }
        };




        var map = ObjectMapper.Map<MenuDto, Menu>(menuDto);
        await _menuRepository.InsertAsync(map);
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




}
