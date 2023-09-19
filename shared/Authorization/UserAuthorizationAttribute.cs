using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Esale.Share.Authorize
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class UserAuthorization : Attribute, IAuthorizationFilter
    {
        private string _permissions;

        private bool _disabled = false;
        //private readonly IConfiguration _configuration;
        public UserAuthorization(string permissions)
        {
            _permissions = permissions;
            //   _currentUser = currentUser;

            //_configuration = ServiceTool.Resolve<IConfiguration>();
        }

        public UserAuthorization()
        {

        }

        public UserAuthorization(bool disabled = false)
        {
            _disabled = disabled;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context.HttpContext.Items["UserId"] == null)
            {
                context.Result = new UnauthorizedResult();
                return;
            }
           // var a = "Role_0001";
           // var _configuration = ServiceTool.Resolve<IConfiguration>();
           // var _cacheManager = ServiceTool.Resolve<ICacheManager>();
           // var RolePermission = _cacheManager.Get<List<string>>(a,
           // "",
           // new CacheOptions()
           // {
           //     Provider = CacheProviderEnum.Hybrid
           // });
           //// string permission = "000100020001";
           // // Here I can get userId from my params.
           // if (!string.IsNullOrEmpty(_permissions))
           // {
           //     var claimsPermisions = RolePermission.Contains(_permissions);            
           //     if (!RolePermission.Contains(_permissions))
           //     {
           //         context.Result = new UnauthorizedResult();
           //         return;
           //     }
           // }
         

            if (_disabled)
                return;

            if (context.HttpContext.Items["UserId"] == null)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            if (int.TryParse(context.HttpContext.Items["UserId"].ToString(), out var user) && user <= 0)
            {
                // not logged in
                context.Result = new UnauthorizedResult();
                return;
            }
            var exp = context.HttpContext.Items["exp"];
            if (exp == null)
            {
                context.Result = new UnauthorizedResult();
                return;
            }


            var permClaims = new List<Claim>();
            permClaims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            permClaims.Add(new Claim("user", "some_username"));
            //if(DateTime.Now > UnixTimeStampToDateTime(long.Parse(exp.ToString()) / 1000))
            //{
            //    context.Result = new UnauthorizedResult();
            //    return;
            //}


            //  var stream = context.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            //// var user = (string)context.HttpContext.Items["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"];

            // if (string.IsNullOrEmpty(stream))
            // {
            //     context.Result = new UnauthorizedResult();

            // }

            //var ClaimsPermisions = tokenS.Claims.FirstOrDefault(x => x.Type == "10").Value;
            //// Here I can get userId from my params.
            //if(!string.IsNullOrEmpty(_permissions))
            //{
            //    if (!ClaimsPermisions.ToLower().Contains(_permissions))
            //    {
            //        context.Result = new UnauthorizedResult();
            //    }
            //}


            // It is then being checked against current user claims.
            // The user is only authorized if the userId is equals to ClaimsType.Value and claims Type is equals to NameIdentifier. 
            //var isUserAuthorized = context.HttpContext.User.Claims.Any(c => c.Type == ClaimTypes.NameIdentifier && c.Value == userId);

            //if (!isUserAuthorized)
            //{
            //    context.Result = new UnauthorizedResult();
            //}
        }
        public DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }
    }

}