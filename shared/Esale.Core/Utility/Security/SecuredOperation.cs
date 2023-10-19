using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Esale.Core.Caching;
using Esale.Core.IOC;
using Volo.Abp;
using Esale.Core.Constant;

namespace Esale.Share.Authorize
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class | AttributeTargets.Interface)]
    public class SecuredOperation : Attribute
    {
        private string _permission;
        public SecuredOperation(string permission)
        {
            var _httpContextAccessor = CheckAuthenticate();
            _permission = permission;
            if (string.IsNullOrWhiteSpace(_permission))
                throw new UserFriendlyException(CoreMessage.AuthenticationDenied, CoreMessage.AuthenticationDeniedId);
            var roles = _httpContextAccessor.HttpContext.User.Claims
                .Where(x => x.Type.Equals(ClaimTypes.Role))
                .ToList();
            if (roles is null || roles.Count == 0)
            {
                throw new UserFriendlyException(CoreMessage.AuthenticationDenied, CoreMessage.AuthenticationDeniedId);
            }
            var _cacheManager = ServiceTool.Resolve<ICacheManager>();
            foreach (var role in roles)
            {
                var rolePermission = _cacheManager.Get<List<string>>(role.ToString(),
                RedisCoreConstant.RolePermissionPrefix,
                new CacheOptions()
                {
                    Provider = CacheProviderEnum.Hybrid
                });
                if (rolePermission != null && rolePermission.Count > 0)
                {
                    if (rolePermission.Any(x => x.Equals(_permission)))
                        return;
                }
            }
            throw new UserFriendlyException(CoreMessage.AuthenticationDenied, CoreMessage.AuthenticationDeniedId);
        }

        private IHttpContextAccessor CheckAuthenticate()
        {
            var _httpContextAccessor = ServiceTool.Resolve<IHttpContextAccessor>();
            if (_httpContextAccessor is null)
                return default;
            if (!_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                throw new UserFriendlyException(CoreMessage.AuthenticationDenied, CoreMessage.AuthenticationDeniedId);
            }
            return _httpContextAccessor;
        }

    }
}