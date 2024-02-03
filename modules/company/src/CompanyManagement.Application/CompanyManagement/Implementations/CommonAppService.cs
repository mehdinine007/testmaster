using CompanyManagement.Application.Contracts.CompanyManagement.Services;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;

namespace CompanyManagement.Application.CompanyManagement.Implementations
{
    public class CommonAppService : ApplicationService, ICommonAppService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CommonAppService(IHttpContextAccessor HttpContextAccessor)
        {
            _httpContextAccessor = HttpContextAccessor;
        }
        public Guid GetUserId()
        {
            var userIdStr = _httpContextAccessor.HttpContext.User.Claims.SingleOrDefault(x => x.Type.Equals("UBP"))?.Value ?? string.Empty;
            if (string.IsNullOrWhiteSpace(userIdStr))
                throw new UserFriendlyException("لطفا لاگین کنید");


            return new Guid(userIdStr);
        }
    }
}
