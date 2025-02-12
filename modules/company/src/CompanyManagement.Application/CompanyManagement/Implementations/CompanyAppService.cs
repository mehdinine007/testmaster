﻿using CompanyManagement.Application.Contracts;
using CompanyManagement.Application.Contracts.CompanyManagement;
using CompanyManagement.Application.Contracts.CompanyManagement.Constants.Validation;
using CompanyManagement.Application.Contracts.CompanyManagement.Services;
using CompanyManagement.Application.Contracts.Services;
using CompanyManagement.Domain.CompanyManagement;
using CompanyManagement.Domain.Shared.CompanyManagement;
using CompanyManagement.EfCore.CompanyManagement.EntityFrameworkCore;
using CompanyManagement.EfCore.CompanyManagement.Repositories;
using Esale.Share.Authorize;
using IFG.Core.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using Permission.Company;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace CompanyManagement.Application.CompanyManagement.Implementations;

public class CompanyAppService : ApplicationService, ICompanyAppService
{
    private readonly IRepository<ClientsOrderDetailByCompany, long> _clientsOrderDetailByCompanyRepository;
    private readonly IRepository<CompanyProduction, long> _companyProductionRepository;
    private readonly ICompanyRepository _companyRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly OrderManagementDbContext _orderManagementDbContext;
    private readonly IRepository<UserMongo, ObjectId> _usermongoRepository;
    private readonly ICommonAppService _commonAppService;
    private readonly IOrderGrpcClientService _orderGrpcClientService;

    public CompanyAppService(IRepository<ClientsOrderDetailByCompany, long> clientsOrderDetailByCompanyRepository,
                            IRepository<CompanyProduction, long> companyProductionRepository,
                            ICompanyRepository companyRepository,
                            IHttpContextAccessor HttpContextAccessor,
                            OrderManagementDbContext orderManagementDbContext,
                            IRepository<UserMongo, ObjectId> userMongoRepository,
                            ICommonAppService commonAppService,
                            IOrderGrpcClientService orderGrpcClientService
                            )
    {
        _clientsOrderDetailByCompanyRepository = clientsOrderDetailByCompanyRepository;
        _companyProductionRepository = companyProductionRepository;
        _companyRepository = companyRepository;
        _httpContextAccessor = HttpContextAccessor;
        _orderManagementDbContext = orderManagementDbContext;
        _usermongoRepository = userMongoRepository;
        _commonAppService = commonAppService;
        _orderGrpcClientService = orderGrpcClientService;
    }

    [SecuredOperation(CompanyServicePermissionConstants.SaveOrderInformation)]
    public async Task SaveOrderInformation(ClientsOrderDetailByCompanyDto clientsOrderDetailByCompanyDto)
    {
        if (!ValidationHelper.IsValidNationalCode(clientsOrderDetailByCompanyDto.NationalCode))
            throw new UserFriendlyException(ValidationConstant.NationalCodeIsWrong,
                code: ValidationConstant.NationalCodeIsWrongId);

        var userCompanyId = _commonAppService.GetUserCompanyId();
        if (userCompanyId <= 0)
            throw new UserFriendlyException(ValidationConstant.UserCompanyIdNotValid, code: ValidationConstant.UserCompanyIdNotValid_Id);

        if (!clientsOrderDetailByCompanyDto.RelatedToOrganization)
        {
            var companyIdFromInquiry = await _orderGrpcClientService.GetOrderById(clientsOrderDetailByCompanyDto.OrderId);
            if (companyIdFromInquiry is null)
                throw new UserFriendlyException(ValidationConstant.CompanyIdNotFound, code: ValidationConstant.CompanyIdNotFoundId);

            if (userCompanyId != companyIdFromInquiry.OrganizationId)
                throw new UserFriendlyException(ValidationConstant.OrderIsNotRelatedToThisCompany,
                    code: ValidationConstant.OrderIsNotRelatedToThisCompanyId);
        }

        var clientOrderDetailsByCompany = (await _clientsOrderDetailByCompanyRepository.GetQueryableAsync())
            .AsNoTracking()
            .Include(x => x.Paypaidprice)
            .Where(x => x.OrderId == clientsOrderDetailByCompanyDto.OrderId &&
                x.RelatedToOrganization == clientsOrderDetailByCompanyDto.RelatedToOrganization)
            .OrderByDescending(x => x.Id)
            .FirstOrDefault();
        if (clientOrderDetailsByCompany is not null)
        {
            if (clientOrderDetailsByCompany.FactorDate.HasValue && clientOrderDetailsByCompany.FactorDate != clientsOrderDetailByCompanyDto.FactorDate)
                throw new UserFriendlyException(ValidationConstant.FactorDateConflict,
                    code: ValidationConstant.FactorDateConflictId);
            if (clientOrderDetailsByCompany.InviteDate.HasValue && clientOrderDetailsByCompany.InviteDate != clientsOrderDetailByCompanyDto.InviteDate)
                throw new UserFriendlyException(ValidationConstant.InviteDateConflict,
                    code: ValidationConstant.InviteDateConflictId);
            if (clientOrderDetailsByCompany.DeliveryDate.HasValue && clientOrderDetailsByCompany.DeliveryDate != clientsOrderDetailByCompanyDto.DeliveryDate)
                throw new UserFriendlyException(ValidationConstant.DeliveryDateConflict,
                    code: ValidationConstant.DeliveryDateConflictId);

            if (clientOrderDetailsByCompany?.Paypaidprice is not null)
            {
                if (!(clientsOrderDetailByCompanyDto.PaypaidPrice ?? new List<PaypaidPriceDto>()).Any())
                    throw new UserFriendlyException(ValidationConstant.CompanyPayedPriceIsNotAllowedToBeNull,
                        code: ValidationConstant.CompanyPayedPriceIsNotAllowedToBeNullId);

                if (clientsOrderDetailByCompanyDto.IsCanceled is not null and true)
                    throw new UserFriendlyException(ValidationConstant.UnableToCancelOrderWhichHavePayment,
                        code: ValidationConstant.UnableToCancelOrderWhichHavePaymentId);
            }
        }

        var clientsOrderDetailByCompany = ObjectMapper.Map<ClientsOrderDetailByCompanyDto, ClientsOrderDetailByCompany>(clientsOrderDetailByCompanyDto);
        await _clientsOrderDetailByCompanyRepository.InsertAsync(clientsOrderDetailByCompany);
    }

    #region Validations

    #endregion



    [SecuredOperation(CompanyServicePermissionConstants.GetCustomersAndCars)]
    public List<CustomersWithCars> GetCustomersAndCars(GetCustomersAndCarsDto input)
    {
        var companyId = _commonAppService.GetUserCompanyId();
        if (companyId <= 0)
            throw new UserFriendlyException(ValidationConstant.UserCompanyIdNotValid, code: ValidationConstant.UserCompanyIdNotValid_Id);
        var customersAndCarsInputDto = new CustomersAndCarsInputDto()
        {
            SaleId = input.SaleId,
            CompanyId = companyId,
            PageNo = input.PageNo
        };

        var lsCustomersWithCars = _companyRepository.GetCustomerOrderList(customersAndCarsInputDto);
        return lsCustomersWithCars;
    }

    [SecuredOperation(CompanyServicePermissionConstants.InsertCompanyProduction)]
    public async Task<bool> InsertCompanyProduction(List<CompanyProductionDto> companyProductionsDto)
    {
        var companyProductions = ObjectMapper.Map(companyProductionsDto, new List<CompanyProduction>());
        await _companyProductionRepository.InsertManyAsync(companyProductions);
        return true;
    }

    [SecuredOperation(CompanyServicePermissionConstants.SubmitOrderInformations)]
    public async Task<bool> SubmitOrderInformations(List<ClientsOrderDetailByCompanyDto> clientsOrderDetailByCompnayDtos)
    {
        var clientsOrderDetailByCompnay = ObjectMapper.Map<List<ClientsOrderDetailByCompanyDto>, List<ClientsOrderDetailByCompany>>(clientsOrderDetailByCompnayDtos, new List<ClientsOrderDetailByCompany>());
        var companyId = _commonAppService.GetUserCompanyId();
        if (companyId <= 0)
            throw new UserFriendlyException(ValidationConstant.UserCompanyIdNotValid, code: ValidationConstant.UserCompanyIdNotValid_Id);
        clientsOrderDetailByCompnay.ForEach(x =>
        {
            x.CompanyId = companyId;
        });
        await _clientsOrderDetailByCompanyRepository.InsertManyAsync(clientsOrderDetailByCompnay);
        return true;
    }

    private bool IsInRole(string Role)
    {
        var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
        // Get the claims values
        var role = _httpContextAccessor.HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.Role)
                           .Select(c => c.Value).SingleOrDefault();
        return role == Role;
    }

    [SecuredOperation(CompanyServicePermissionConstants.GetRecentCustomerAndOrder)]
    public async Task<CompaniesCustomerDto> GetRecentCustomerAndOrder(string nationalCode, int saleId)
    {
        if (nationalCode.AsParallel().Any(x => !char.IsDigit(x)) || nationalCode.Length != 10)
            throw new UserFriendlyException("کد ملی مشتری صحیح نیست");

        var companyId = _commonAppService.GetUserCompanyId();
        if (companyId <= 0)
            throw new UserFriendlyException(ValidationConstant.UserCompanyIdNotValid, code: ValidationConstant.UserCompanyIdNotValid_Id);
        var user = (await _usermongoRepository.GetQueryableAsync())
            .Select(x => new
            {
                x.NationalCode,
                x.Mobile,
                x.Name,
                x.Surname,
                x.FatherName,
                x.BirthCertId,
                x.BirthDate,
                x.Gender,
                ProvienceId = x.HabitationProvinceId,
                CityId = x.HabitationCityId,
                x.Tel,
                x.PostalCode,
                x.Address,
                x.IssuingDate,
                x.Shaba,
                x.UID
            })
            .FirstOrDefault(x => x.NationalCode == nationalCode)
            ?? throw new UserFriendlyException("مشتری یافت نشد");

        var paramArray = new object[]
        {
            new SqlParameter("@saleId",SqlDbType.Int){Value = saleId},
            new SqlParameter("@companyId",SqlDbType.Int){Value = companyId},
            new SqlParameter("@userId",SqlDbType.UniqueIdentifier){Value = new Guid(user.UID)},
            new SqlParameter("@provienceId",SqlDbType.Int){Value = user.ProvienceId.HasValue ? user.ProvienceId.Value : -1}
        };
        var companiesCustomer = _orderManagementDbContext.Set<CompaniesCustomer>().FromSqlRaw(
            string.Format("EXEC {0} {1}", "[dbo].[GetRecentCustomerAndOrder]", "@saleId,@companyId,@userId,@provienceId"), paramArray)
            .AsEnumerable()
            .FirstOrDefault()
            ?? throw new UserFriendlyException("سفارشی برای این شخص وجود ندارد");
        //"EXEC [dbo].[GetCompaniesCustomer] @saleId,@companyId,@nationalCode", paramArray).FirstOrDefault();

        return new()
        {
            OrderId = companiesCustomer.OrderId,
            ProvienceId = user.ProvienceId,
            NationalCode = user.NationalCode,
            Address = user.Address,
            BirthCertId = user.BirthCertId,
            BirthDate = user.BirthDate,
            CompanyName = companiesCustomer.CompanyName,
            CompanySaleId = companiesCustomer.CompanySaleId,
            DeliveryDateDescription = companiesCustomer.DeliveryDateDescription,
            ESaleTypeId = companiesCustomer.ESaleTypeId,
            FatherName = user.FatherName,
            Gender = (int)user.Gender,
            Id = companiesCustomer.Id,
            IssuingDate = user.IssuingDate,
            Mobile = user.Mobile,
            Name = user.Name,
            OrderRejectionStatus = companiesCustomer.OrderRejectionStatus,
            PostalCode = user.PostalCode,
            ProductId = companiesCustomer.ProductId,
            ProductName = companiesCustomer.ProductName,
            ProvienceName = companiesCustomer.ProvienceName,
            Shaba = user.Shaba,
            Surname = user.Surname,
            Tel = user.Tel,
            TrackingCode = companiesCustomer.TrackingCode,
        };
    }
}
