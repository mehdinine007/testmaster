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
using Microsoft.AspNetCore.Http;
using Abp.Authorization;
using Esale.Core.Constant;
using Esale.Core.IOC;
using UserManagement.Domain.UserManagement.Authorization.RolePermissions;
using Newtonsoft.Json;





namespace UserManagement.Application.UserManagement.Implementations;

public class MenuAppService : ApplicationService, IMenuAppService
{
    private readonly ICacheManager _cacheManager;
    private readonly IRepository<Menu, ObjectId> _menuRepository;
    private readonly IRepository<RolePermission, ObjectId> _rolePermissionRepository;


    public MenuAppService(ICacheManager cacheManager,
                         IRepository<Menu, ObjectId> menuRepository,
                         IRepository<RolePermission, ObjectId> rolePermissionRepository

        )
    {
        _cacheManager = cacheManager;
        _menuRepository = menuRepository;
        _rolePermissionRepository = rolePermissionRepository;
    }

    public async Task<MenuDto> GetList()
    {
        var menus = (await _menuRepository.GetQueryableAsync())
                    .ToList();
        var getmenu = ObjectMapper.Map<List<Menu>, List<MenuDto>>(menus);
        foreach ( var menu in getmenu )
        {
         var x=   GetMenuChildDtos(menu);
            return new MenuDto();
        }
        return new MenuDto();

    }

    private List<MenuChildDto> GetMenuChildDtos(MenuDto parent)
    {        
        

        //var ParentN =   ParentN == null ? new List<MenuDto>();
         
        var permissionlist = new List<PermissionDefinitionChildDto>();
        if (parent.Children.Any())
        {
            List<MenuDto> ParentN = new List<MenuDto>();
            List<MenuChildDto> childN = new List<MenuChildDto>();
            ParentN.Add(new MenuDto { Code=parent.Code,Title=parent.Title});
            foreach (var child in parent.Children)
            {
                childN.Add(new MenuChildDto { Code=child.Code,Title=child.Title ,Children=child.Children,Permissions=child.Permissions
                  //Permissions=  permissionlist.Add(new PermissionDefinitionChildDto
                  //  {
                  //      Code = child.Permissions.Code,
                  //      DisplayName = child.DisplayName,
                  //      Title = child.Title,

                  //  })


            });
               // GetMenuChildDtos(new MenuDto { Code=child.Code,Title=child.Title,Children=child.Children,Permissions=child.Permissions});
            }
        }
        return new List<MenuChildDto>();
       
    }
        //private List<MenuChildDto> GetMenuChildDtos(List<MenuChildDto> children)
        //{
        //    var permissionlist = new List<PermissionDefinitionChildDto>();
        //    foreach (var child in children)
        //    {
        //        if (child.Children.Count > 0)
        //        {
        //            children = child.Children;
        //            if (children.Any(c => c.Children.Count >0)) continue;

        //            else
        //            {
        //                if (children[0].Permissions.Count > 0)
        //                {
        //                    foreach (var per in children[0].Permissions)
        //                    {
        //                        {
        //                            permissionlist.Add(new PermissionDefinitionChildDto
        //                            {
        //                                Code = per.Code,
        //                                DisplayName = per.DisplayName,
        //                                Title = per.Title,

        //                            });
        //                        };
        //                    }
        //                }
        //                else continue;
        //            }
        //        }

        //        List<MenuChildDto> result = children.SelectMany(x => x.Children).ToList();
        //        //var MenuDto = new MenuDto
        //        //{

        //        //    Title=child.Title,
        //        //    Code = child.Code,
        //        //    Icon = child.Icon,
        //        //    Url = child.Url,
        //        //    Permissions = permissionlist
        //        //};

        //    //var menuchild = new MenuChildDto
        //    //    {
        //    //        Title = child.Title,
        //    //        Code = child.Code,
        //    //        Icon = child.Icon,
        //    //        Url = child.Url,
        //    //        Permissions = permissionlist
        //    //    };

        //            return result;

        //    }

        //    return new List<MenuChildDto>();// { child };
        //}
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
                        Type = (int)MenuEnum.Category

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

    public async Task UpdateMenuPolicy()
    {
        var currentDirectory = Environment.CurrentDirectory;
        const string fileName = "UpdateMenuPolicy.json";
        var fullPath = Path.Combine(currentDirectory, fileName);
        var content = File.ReadAllText(fileName);
        var ls = new List<Menu>(JsonConvert.DeserializeObject<List<Menu>>(content));
        await _menuRepository.InsertManyAsync(ls);
    }
    
}
