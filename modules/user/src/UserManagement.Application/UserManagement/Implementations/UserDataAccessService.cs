
using Esale.Share.Authorize;
using IFG.Core.Utility.Results;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Application.Contracts;
using UserManagement.Application.Contracts.Models;
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

        public async Task<IResult> CheckNationalCode(string nationalcode, RoleTypeEnum roleType)
        {
            var userDataAccess = await GetListByNationalcode(nationalcode, roleType);
            if (!userDataAccess.Any())
                return new ErrorResult(UserMessageConstant.UserDataAccessNationalCodeNotFound, UserMessageConstant.UserDataAccessNationalCodeNotFoundId);
            return new SuccsessResult();
        }

        public async Task<List<UserDataAccessDto>> GetListByNationalcode(string nationalcode, RoleTypeEnum roleType)
        {
            var userDataAccessQuery = (await _userDataAccessRepository.GetQueryableAsync()).AsNoTracking();
            var userDataAccess= userDataAccessQuery.Select(x => new UserDataAccessDto
            {
                Nationalcode = x.Nationalcode,
                Data = x.Data,
                RoleTypeId = x.RoleTypeId,
                UserId = x.UserId

            }).Where(x => x.Nationalcode == nationalcode && x.RoleTypeId == roleType).ToList();
            return userDataAccess;
        }

        public async Task<List<UserDataAccessDto>> GetListByUserId(Guid userId, RoleTypeEnum roleType)
        {

            var userDataAccessQuery = (await _userDataAccessRepository.GetQueryableAsync()).AsNoTracking();
            var userDataAccess = userDataAccessQuery.Select(x => new UserDataAccessDto
            {
                Nationalcode = x.Nationalcode,
                Data = x.Data,
                RoleTypeId = x.RoleTypeId,
                UserId = x.UserId

            }).Where(x => x.UserId == userId && x.RoleTypeId == roleType).ToList();
            return userDataAccess;
        }
    }
}
