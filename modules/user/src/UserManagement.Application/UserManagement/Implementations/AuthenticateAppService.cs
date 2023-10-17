using Abp.Runtime.Security;
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

    public async Task<AuthenticateResultModel> Authenticate( AuthenticateModel model)
    {
        System.Diagnostics.Debugger.Launch();
        User loginResult;
        loginResult = await _userAppService.GetLoginInfromationuserFromCache(model.UserNameOrEmailAddress);

        var res =new AuthenticateResultModel();
        if (loginResult == null)
        {
            res.Success = false;
            res.Message = "نام کاربری یا کلمه عبور صحیح نمی باشد";
            res.ErrorCode = 101;
            return res;
            
        }
        if (loginResult.IsActive == false)
        {
            res.Success = false;
            res.Message = "حساب کاربری شما غیر فعال می باشد";
            res.ErrorCode = 102;
            return res;
          
        }
        var passwordResult = _passwordHasher.VerifyHashedPassword(loginResult, loginResult.Password, model.Password);
        var success = passwordResult != PasswordVerificationResult.Failed;
        if (!success)
        {
            res.Success = false;
            res.Message = "نام کاربری یا کلمه عبور صحیح نمی باشد";
            res.ErrorCode = 103;
            return res;
           
        }

        // _baseInformationService.CheckWhiteList(WhiteListEnumType.WhiteListBeforeLogin, loginResult.UserName);
        var accessToken = CreateAccessToken(CreateJwtClaims(loginResult));

         res.Data = new AuthenticateResult
        {
            AccessToken = accessToken,
            EncryptedAccessToken = GetEncryptedAccessToken(accessToken),
            ExpireInSeconds = (int)TimeSpan.FromDays(1).TotalSeconds
        };
        res.Success = true;
        return res;

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
