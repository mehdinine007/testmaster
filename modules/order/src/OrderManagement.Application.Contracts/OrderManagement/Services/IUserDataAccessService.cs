using IFG.Core.Utility.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace OrderManagement.Application.Contracts
{
    public interface IUserDataAccessService : IApplicationService
    {
        Task<List<UserDataAccessProductDto>> ProductGetList(string nationalCode);
        Task<IResult> CheckProductAccess(string nationalCode,int productId);
    }
}
