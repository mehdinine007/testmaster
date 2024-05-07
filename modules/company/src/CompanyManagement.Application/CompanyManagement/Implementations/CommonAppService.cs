using CompanyManagement.Application.Contracts.CompanyManagement.Services;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using Volo.Abp;
using Volo.Abp.Application.Services;

namespace CompanyManagement.Application.CompanyManagement.Implementations;

public class CommonAppService : ApplicationService, ICommonAppService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    public CommonAppService(IHttpContextAccessor HttpContextAccessor)
    {
        _httpContextAccessor = HttpContextAccessor;
    }

    public int GetUserCompanyId()
    {
        if (!_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            return -1;
        var userCompanyIdClaim = _httpContextAccessor.HttpContext.User.Claims.First(x =>
            x.Type.Equals("CompanyId", StringComparison.OrdinalIgnoreCase))?.Value ?? "0";
        return int.TryParse(userCompanyIdClaim, out int userCompanyId) ? userCompanyId : 0;
    }

    public Guid GetUserId()
    {
        var userIdStr = _httpContextAccessor.HttpContext.User.Claims.SingleOrDefault(x => x.Type.Equals("UBP"))?.Value ?? string.Empty;
        if (string.IsNullOrWhiteSpace(userIdStr))
            throw new UserFriendlyException("لطفا لاگین کنید");


        return new Guid(userIdStr);
    }
}
