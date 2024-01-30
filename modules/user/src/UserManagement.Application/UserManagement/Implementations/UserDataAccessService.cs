
using Esale.Share.Authorize;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Application.Contracts.Models;
using UserManagement.Application.Contracts.UserManagement.Constant.Permission;
using UserManagement.Application.Contracts.UserManagement.Models;
using UserManagement.Application.Contracts.UserManagement.Services;
using UserManagement.Domain.Shared.UserManagement.Enums;
using UserManagement.Domain.UserManagement;
using UserManagement.Domain.UserManagement.Authorization.RolePermissions;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace UserManagement.Application.UserManagement.Implementations
{
    public class UserDataAccessService : ApplicationService, IUserDataAccessService
    {
        private readonly IRepository<UserDataAccess, int> _userDataAccessRepository;
     

        public UserDataAccessService(IRepository<UserDataAccess, int> userDataAccessRepository)
        {
            _userDataAccessRepository = userDataAccessRepository;
        }
        public async Task<List<UserDataAccessDto>> GetListByNationalcode(string nationalcode, RoleTypeEnum roleType)
        {
            var userDataAccessQuery = (await _userDataAccessRepository.GetQueryableAsync());
            var userDataAccess = userDataAccessQuery.Where(x => x.Nationalcode == nationalcode && x.RoleTypeId == roleType).ToList();
            return ObjectMapper.Map<List<UserDataAccess>, List<UserDataAccessDto>>(userDataAccess);
        }

        public async  Task<List<UserDataAccessDto>> GetListByUserId(Guid userId, RoleTypeEnum roleType)
        {

            var userDataAccessQuery = (await _userDataAccessRepository.GetQueryableAsync());
            var userDataAccess = userDataAccessQuery.Where(x => x.UserId == userId && x.RoleTypeId == roleType).ToList();
            return ObjectMapper.Map<List<UserDataAccess>, List<UserDataAccessDto>>(userDataAccess);
        }
    }
}
