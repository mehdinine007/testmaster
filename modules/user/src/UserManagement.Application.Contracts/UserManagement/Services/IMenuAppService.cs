using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using UserManagement.Application.Contracts.Models;
using UserManagement.Domain.Authorization.Users;
using UserManagement.Domain.UserManagement.Authorization;
using Volo.Abp.Application.Services;

namespace UserManagement.Application.Contracts.UserManagement.Services;

public interface IMenuAppService : IApplicationService
{
    Task<MenuDto> GetList();
    Task<MenuDto> GetById(ObjectId Id);

    Task<MenuDto> Add(MenuDto input);

    Task<MenuDto> Update(MenuDto input);
    Task<bool> Delete(ObjectId Id);
     Task InsertList();
    Task UpdateMenuPolicy();
}
