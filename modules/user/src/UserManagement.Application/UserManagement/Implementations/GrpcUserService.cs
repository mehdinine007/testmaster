﻿using Azure.Core;
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
using UserManagement.Domain.UserManagement.Authorization;

namespace UserManagement.Application.UserManagement.Implementations
{
    public class GrpcUserService : UserServiceGrpc.UserServiceGrpcBase
    {
        private readonly IBaseInformationService _baseInformationSevice;
        private readonly IBankAppService _bankAppService;
        private readonly IAuthenticateAppService _authenticateAppService;


        public GrpcUserService(IBaseInformationService baseInformationService,
                               IBankAppService bankAppService,
                               IAuthenticateAppService authenticateAppService)
        {
            _baseInformationSevice = baseInformationService;
            _bankAppService = bankAppService;
            _authenticateAppService= authenticateAppService;
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
                    BankId = user.BankId,
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
                    Uid = user.Uid
                };
            }
            catch (Exception)
            {

                throw;
            }


        }
        public override async Task<ClientOrderDetailResponse> CheckOrderDeliveryDate(ClientOrderDetailRequest request, ServerCallContext context)
        {
            //var clientsOrderDeliveryDateValidation =await _baseInformationSevice.CheckOrderDeliveryDate(request.NationalCode, request.OrderId);
            //if (clientsOrderDeliveryDateValidation)
            //{
            var orderDelay = await _baseInformationSevice.GetOrderDelivery(request.NationalCode, request.OrderId);
            var ClientOrderDetail = await Task.FromResult(new ClientOrderDetailResponse()
            {
                NationalCode = orderDelay.NationalCode,
                TranDate = orderDelay.TranDate.HasValue ? Timestamp.FromDateTimeOffset(orderDelay.TranDate.Value) : new(),
                PayedPrice = orderDelay.PayedPrice,
                ContRowId = orderDelay.ContRowId,
                Vin = orderDelay.Vin,
                DeliveryDate = orderDelay.DeliveryDate.HasValue ? Timestamp.FromDateTimeOffset(orderDelay.TranDate.Value) : new(),
                BodyNumber = orderDelay.BodyNumber,
                FinalPrice = orderDelay.FinalPrice,
                CarDesc = orderDelay.CarDesc
            });
            return ClientOrderDetail;
            //}
            //return null;
        }

        public override async Task<AuthenticateResponse> Authenticate(AuthenticateRequest request, ServerCallContext context)
        {
            var model = new AuthenticateModel()
            {
                UserNameOrEmailAddress = request.UserNameOrEmailAddress,
                Password = request.Password
            };
            var auth =await _authenticateAppService.Authenticate(model);
            if (auth == null)
                   return new AuthenticateResponse();

            return new AuthenticateResponse
            {
                AccessToken = auth.AccessToken,
                EncryptedAccessToken = auth.EncryptedAccessToken,
                ExpireInSeconds = auth.ExpireInSeconds,
                UserId = auth.UserId
            };
        }

    }
}
