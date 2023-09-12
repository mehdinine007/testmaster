using EasyCaching.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using WorkFlowManagement.Application.Contracts.WorkFlowManagement.IServices;

namespace WorkFlowManagement.Application.WorkFlowManagement.Implementations
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
