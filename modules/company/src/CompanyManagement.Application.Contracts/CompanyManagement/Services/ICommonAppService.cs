using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace CompanyManagement.Application.Contracts.CompanyManagement.Services
{
    public interface ICommonAppService : IApplicationService
    {
        Guid GetUserId();

        /// <summary>
        /// 
        /// </summary>
        /// <returns>Returns user companyId</returns>
        /// <returns>Returns -1 in case user is not authenticated</returns>
        /// <returns>Returns 0 in case user company code is empty or null</returns>
        int GetUserCompanyId();
    }
}
