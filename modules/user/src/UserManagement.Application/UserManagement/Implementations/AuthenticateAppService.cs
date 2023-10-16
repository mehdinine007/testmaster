﻿using Abp.Runtime.Security;
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

namespace UserManagement.Application.UserManagement.Implementations;

public class AuthenticateAppService : ApplicationService, IAuthenticateAppService
{

    private readonly IUserAppService _userAppService;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly IConfiguration _configuration;


    public AuthenticateAppService(IUserAppService userAppService,
                                 IPasswordHasher<User> PasswordHasher,
                                 IConfiguration configuration
        )
    {
        _userAppService = userAppService;
        _passwordHasher = PasswordHasher;
        _configuration = configuration;
    }

    public async Task<AuthenticateResultModel> Authenticate([FromBody] AuthenticateModel model)
    {
        System.Diagnostics.Debugger.Launch();
        User loginResult;
        loginResult = await _userAppService.GetLoginInfromationuserFromCache(model.UserNameOrEmailAddress);

        if (loginResult == null)
        {
            throw new UserFriendlyException("نام کاربری یا کلمه عبور صحیح نمی باشد");
        }
        if (loginResult.IsActive == false)
        {
            throw new UserFriendlyException("حساب کاربری شما غیر فعال می باشد");
        }
        var passwordResult = _passwordHasher.VerifyHashedPassword(loginResult, loginResult.Password, model.Password);
        var success = passwordResult != PasswordVerificationResult.Failed;
        if (!success)
        {
            throw new UserFriendlyException("نام کاربری یا کلمه عبور صحیح نمی باشد");
        }

        // _baseInformationService.CheckWhiteList(WhiteListEnumType.WhiteListBeforeLogin, loginResult.UserName);
        var accessToken = CreateAccessToken(CreateJwtClaims(loginResult));
        return new AuthenticateResultModel
        {
            AccessToken = accessToken,
            EncryptedAccessToken = GetEncryptedAccessToken(accessToken),
            ExpireInSeconds = (int)TimeSpan.FromDays(1).TotalSeconds,
            UserId = loginResult.Id
        };
    }

    private string CreateAccessToken(IEnumerable<Claim> claims, TimeSpan? expiration = null)
    {
        var now = DateTime.UtcNow;
        var jwtSecurityToken = new JwtSecurityToken(
            issuer: _configuration.GetValue<string>("Authentication:JwtBearer:Issuer"),
            audience: _configuration.GetValue<string>("Authentication:JwtBearer:Audience"),
            claims: claims,
            notBefore: now,
            expires: now.Add(expiration ?? TimeSpan.FromDays(1)),
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration.GetValue<string>("Authentication:JwtBearer:SecurityKey"))), SecurityAlgorithms.HmacSha256)
        );

        return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
    }

    private List<Claim> CreateJwtClaims(User user)
    {
        var claims = new List<Claim>();

        claims.AddRange(new[]
        {
        //    new Claim(JwtRegisteredClaimNames.Sub, nameIdClaim.Value),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Role, (user?.RolesM ?? new()).Any() ? string.Join(",",user.RolesM) : string.Empty),
            new Claim("UBP", user.UID.ToString()),

            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),

            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.Now.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
        });
        return claims;
    }
    private string GetEncryptedAccessToken(string accessToken)
    {
        return SimpleStringCipher.Instance.Encrypt(accessToken);
    }


}
