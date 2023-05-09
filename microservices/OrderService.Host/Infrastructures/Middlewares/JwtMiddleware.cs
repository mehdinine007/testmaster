using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Host.Infrastructure.Middlewares;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;

    public JwtMiddleware(RequestDelegate next)
    => _next = next;

    public async Task InvokeAsync(HttpContext httpContext)
    {
        var token = httpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(' ').Last();

        if (token != null)
        {
            AttachUserToContext(httpContext, token);
        }
        await _next.Invoke(httpContext);
    }

    private void AttachUserToContext(HttpContext httpContext, string token)
    {
        JwtSecurityTokenHandler jwtTokenHandler = new();
        //var key = Encoding.ASCII.GetBytes(TokenSecretKey);
        try
        {
            jwtTokenHandler.ValidateToken(token, new TokenValidationParameters()
            {
                ValidateIssuerSigningKey = true,
                //IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);
            var jwtToken = (JwtSecurityToken)validatedToken;
            string uid, userName, roleName;

            //var roleList = jwtToken.Claims.Where(x => x.Type.Equals(ClaimTypes.Role)).ToList();
            uid = jwtToken.Claims.SingleOrDefault(x => x.Type.Equals(ClaimTypes.NameIdentifier)).Value;
            roleName = jwtToken.Claims.SingleOrDefault(x => x.Type.Equals(ClaimTypes.Role)).Value;
            userName = jwtToken.Claims.Single(x => x.Type.Equals(ClaimTypes.Name)).Value;

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, userName),
                new Claim(ClaimTypes.NameIdentifier, uid),
                new Claim(ClaimTypes.Surname , userName),
                new Claim(ClaimTypes.Role,roleName)
            };
            //roleList.ForEach(x => claims.Add(new Claim(ClaimTypes.Role, x.Value)));
            var appIdentity = new ClaimsIdentity(claims);
            httpContext.User.AddIdentity(appIdentity);
            httpContext.Items["UserId"] = uid;
            httpContext.Items["exp"] = DateTime.Now.AddDays(1).Ticks;
        }
        catch (Exception ex)
        {

        }
    }
}
