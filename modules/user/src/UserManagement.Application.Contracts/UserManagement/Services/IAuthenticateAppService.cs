using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using UserManagement.Application.Contracts.Models;
using UserManagement.Domain.Authorization.Users;
using UserManagement.Domain.UserManagement.Authorization;
using Volo.Abp.Application.Services;

namespace UserManagement.Application.Contracts.UserManagement.Services;

public interface IAuthenticateAppService : IApplicationService
{

    Task<AuthenticateResultModel> Authenticate( AuthenticateModel model);
  
}
