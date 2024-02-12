
using IFG.Core.Utility.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Application.Contracts.UserManagement.Models;
using UserManagement.Domain.Shared.UserManagement.Enums;
using Volo.Abp.Application.Services;



namespace UserManagement.Application.Contracts.UserManagement.Services
{
    public interface IUserDataAccessService: IApplicationService
    {
        Task<List<UserDataAccessDto>> GetListByNationalcode(string nationalcode,RoleTypeEnum roleType);
        Task<List<UserDataAccessDto>> GetListByUserId(Guid userId, RoleTypeEnum roleType);

        Task<IResult> CheckNationalCode(string nationalcode, RoleTypeEnum roleType);
    }
}
