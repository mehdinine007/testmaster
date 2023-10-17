using Volo.Abp.Auditing;
using Volo.Abp;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Application.Contracts.UserManagement.Services;
using UserManagement.Domain.UserManagement.Authorization;
using Microsoft.AspNetCore.Identity;
using UserManagement.Domain.Authorization.Users;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Abp.Runtime.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using UserManagement.Application.Contracts.Models;
using MongoDB.Bson;
using UserManagement.Application.Contracts.UserManagement.Models.User;

namespace UserManagement.HttpApi.UserManagement.Controllers;

[DisableAuditing]
[RemoteService]
[Route("api/TokenAuth/[action]")]
public class TokenAuthController : Controller
{
    private readonly IAuthenticateAppService _authenticateAppService;


    public TokenAuthController(IAuthenticateAppService authenticateAppService)
    {
        _authenticateAppService = authenticateAppService;
    }


    [HttpPost]
    public async Task<AuthenticateResultModel> Authenticate( AuthenticateModel model)
    {
        return await _authenticateAppService.Authenticate(model);
    }


}
