
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Application.Contracts.UserManagement.Models;
using Volo.Abp.Application.Services;



namespace UserManagement.Application.Contracts.UserManagement.Services
{
    public interface IUserDataAccessService: IApplicationService
    {
        Task<List<UserDataAccessDto>> GetListByNationalcode(string nationalcode);
        Task<List<UserDataAccessDto>> GetListByUserId(Guid userId);
    }
}
