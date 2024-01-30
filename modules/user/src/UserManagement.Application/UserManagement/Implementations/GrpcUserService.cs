using Azure.Core;
using Esale.UserServiceGrpc;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Application.Contracts.Models;
using UserManagement.Application.Contracts.Services;
using UserManagement.Application.Contracts.UserManagement.Services;
using UserManagement.Domain.Shared.UserManagement.Enums;
using UserManagement.Domain.UserManagement.Authorization;

namespace UserManagement.Application.UserManagement.Implementations
{
    public class GrpcUserService : UserServiceGrpc.UserServiceGrpcBase
    {
        private readonly IBaseInformationService _baseInformationSevice;
        private readonly IBankAppService _bankAppService;
        private readonly IAuthenticateAppService _authenticateAppService;
        private readonly IUserDataAccessService _userDataAccessService;

        public GrpcUserService(IBaseInformationService baseInformationService,
                               IBankAppService bankAppService,
                               IAuthenticateAppService authenticateAppService,
                               IUserDataAccessService userDataAccessService)
        {
            _baseInformationSevice = baseInformationService;
            _bankAppService = bankAppService;
            _authenticateAppService = authenticateAppService;
            _userDataAccessService = userDataAccessService;
        }

        public override async Task<UserDataAccessResponse> GetUDAByNationalCode(GetUDAByNationalCodeRequest request, ServerCallContext context)
        {
            var getUserDataAccess = await _userDataAccessService.GetListByNationalcode(request.NationalCode,(RoleTypeEnum)request.Type);
            var userDataAccess = new UserDataAccessResponse();
            userDataAccess.UserDataAccessModel.AddRange(getUserDataAccess.Select(x=> new UserDataAccessModel
            {
                UserId = x.UserId !=null ? x.UserId.ToString():""  ,
                Nationalcode = x.Nationalcode,
                RoleTypeId = (int)x.RoleTypeId,
                Data = x.Data
            }));
            return userDataAccess;
        }

        public override async Task<UserDataAccessResponse> GetUDAByUserId(GetUDAByUserIdRequest request, ServerCallContext context)
        {
            var getUserDataAccess = await _userDataAccessService.GetListByUserId(Guid.Parse(request.UserId), (RoleTypeEnum)request.Type);
            var userDataAccess = new UserDataAccessResponse();
            userDataAccess.UserDataAccessModel.AddRange(getUserDataAccess.Select(x => new UserDataAccessModel
            {
                UserId = x.UserId.ToString(),
                Nationalcode = x.Nationalcode,
                RoleTypeId = (int)x.RoleTypeId,
                Data = x.Data
            }));
            return userDataAccess;
        }

        public override Task<UserAdvocacy> GetUserAdvocacy(UserAdvocacyRequest request, ServerCallContext context)
        {
            var userAdvocacy = _bankAppService.CheckAdvocacy(request.NationalCode);
            if (userAdvocacy == null)
                return Task.FromResult(new UserAdvocacy());

            return Task.FromResult(new UserAdvocacy()
            {
                AccountNumber = userAdvocacy.AccountNumber,
                BankId = userAdvocacy.BankId??0,
                ShebaNumber = userAdvocacy.ShebaNumber,
                //GenderCode = userAdvocacy.GenderCode
            });
        }

        public override async Task<UserModel> GetUserById(GetUserModel request, ServerCallContext context)
        {
                       try
            {
                var user = await _baseInformationSevice.GetUserByIdAsync(request.UserId);
                if (user == null)
                    return new UserModel();

                return new UserModel()
                {
                    AccountNumber = user.AccountNumber,
                    BankId = user.BankId == null? 0: (int)user.BankId,
                    BirthCityId = user.BirthCityId,
                    BirthProvinceId = user.BirthProvinceId,
                    HabitationCityId = user.HabitationCityId,
                    HabitationProvinceId = user.HabitationProvinceId,
                    IssuingCityId = user.IssuingCityId,
                    IssuingProvinceId = user.IssuingProvinceId,
                    NationalCode = user.NationalCode,
                    Shaba = user.Shaba,
                    MobileNumber = user.MobileNumber,
                    GenderCode = user.GenderCode,
                    CompanyId = user.CompanyId,
                    Name = user.Name,
                    SurName = user.SurName,
                    Uid = user.Uid,
                    Priority = user.Priority,
                };
            }
            catch (Exception)
            {

                throw;
            }


        }
        public override async Task<AuthenticateResponse> Authenticate(AuthenticateRequest request, ServerCallContext context)
        {
            var model = new AuthenticateModel()
            {
                UserNameOrEmailAddress = request.UserNameOrEmailAddress,
                Password = request.Password
            };
            var auth = await _authenticateAppService.Authenticate(model);
            var res = new AuthenticateResponse();
            if (!auth.Success)
            {
                res.Success = false;
                res.Message = auth.Message;
                res.ErrorCode = auth.ErrorCode;
                return res;
            }

            res.Success = auth.Success;
            res.Data = new AuthenticateDataModel();
            res.Data.AccessToken = auth.Data.AccessToken;
            res.Data.EncryptedAccessToken = auth.Data.EncryptedAccessToken;
            res.Data.ExpireInSeconds = auth.Data.ExpireInSeconds;

            return res;

        }

    }
}
