﻿using Esale.UserServiceGrpc;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Application.Contracts.Services;

namespace UserManagement.Application.UserManagement.Implementations
{
    public class GrpcUserService : UserServiceGrpc.UserServiceGrpcBase
    {
        private readonly IBaseInformationService _baseInformationSevice;
        private readonly IBankAppService _bankAppService;

        public GrpcUserService(IBaseInformationService baseInformationService,
                               IBankAppService bankAppService)
        {
            _baseInformationSevice = baseInformationService;
            _bankAppService = bankAppService;
        }

        public override Task<UserAdvocacy> GetUserAdvocacy(UserAdvocacyRequest request, ServerCallContext context)
        {
            var userAdvocacy = _bankAppService.CheckAdvocacy(request.NationalCode);
            if (userAdvocacy == null)
                return Task.FromResult(new UserAdvocacy());

            return Task.FromResult(new UserAdvocacy()
            {
                AccountNumber = userAdvocacy.AccountNumber,
                BankId = userAdvocacy.BankId,
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
                    SurName = user.SurName
                };
            }
            catch (Exception)
            {

                throw;
            }

            
        }


    }
}
