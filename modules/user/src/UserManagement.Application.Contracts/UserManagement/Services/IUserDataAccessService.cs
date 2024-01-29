using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Application.Contracts.UserManagement.Models;



namespace UserManagement.Application.Contracts.UserManagement.Services
{
    public interface IUserDataAccessService:IApplicationService
    {
        Task<UserDataAccessDto> GetListByNationalcode(string nationalcode);
        Task<UserDataAccessDto> GetListByUserId(Guid userId);
    }
}
