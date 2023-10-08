using AutoMapper.Internal.Mappers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using OrderManagement.Application.Contracts.CompanyManagement;
using OrderManagement.Application.Contracts.CompanyManagement.Services;
using OrderManagement.Application.Contracts.Services;
using OrderManagement.Domain.CompanyManagement;
using OrderManagement.Domain.Shared.CompanyManagement;
using OrderManagement.EfCore.CompanyManagement.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;


namespace OrderManagement.Application.CompanyManagement.Implementations
{
    public class CompanyAppService : ApplicationService, ICompanyAppService
    {
        private readonly IRepository<ClientsOrderDetailByCompany, long> _clientsOrderDetailByCompanyRepository;
        private readonly IRepository<CompanyPaypaidPrices, long> _companyPaypaidPricesRepository;
        private readonly IRepository<CompanySaleCallDates, long> _companySaleCallDatesRepository;
        private readonly IRepository<CompanyProduction, long> _companyProductionRepository;
        private IConfiguration _configuration;
        private ICompanyRepository _companyRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CompanyAppService(IRepository<ClientsOrderDetailByCompany, long> clientsOrderDetailByCompanyRepository,
                                IConfiguration configuration,
                                IRepository<CompanyPaypaidPrices, long> CompanyPaypaidPricesRepository,
                                IRepository<CompanySaleCallDates, long> CompanySaleCallDatesRepository,
                                IRepository<CompanyProduction, long> companyProductionRepository,
                                ICompanyRepository companyRepository,
                                IHttpContextAccessor HttpContextAccessor
                                )
        {
            _clientsOrderDetailByCompanyRepository = clientsOrderDetailByCompanyRepository;
            _configuration = configuration;
            _companyPaypaidPricesRepository = CompanyPaypaidPricesRepository;
            _companySaleCallDatesRepository = CompanySaleCallDatesRepository;
            _companyProductionRepository = companyProductionRepository;
            _companyRepository = companyRepository;
            _httpContextAccessor = HttpContextAccessor;
        }


        public List<CustomersWithCars> GetCustomersAndCars(GetCustomersAndCarsDto input)
        {
            if (!IsInRole("Company"))
            {
                throw new UserFriendlyException("دسترسی کافی نمی باشد");
            }
            var customersAndCarsInputDto = new CustomersAndCarsInputDto()
            {
                SaleId = input.SaleId,
                CompanyId =int.Parse(GetCompanyId())
            };

            _companyRepository.GetCustomerOrderList(customersAndCarsInputDto);

            List<CustomersWithCars> lsCustomersWithCars = new List<CustomersWithCars>();

            return lsCustomersWithCars;
        }

        public async Task InsertCompanyProduction(List<CompanyProductionDto> companyProductionsDto)
        {
            if (!IsInRole("Company"))
                throw new UserFriendlyException("عدم دسترسی کافی");
            var companyProductions = ObjectMapper.Map(companyProductionsDto, new List<CompanyProduction>());
            await _companyProductionRepository.InsertManyAsync(companyProductions);
        }

        public async Task SubmitOrderInformations(List<ClientsOrderDetailByCompanyDto> clientsOrderDetailByCompnayDtos)
        {
            if (!IsInRole("Company"))
                throw new UserFriendlyException("عدم دسترسی کافی");
            var x = ObjectMapper.Map<List<ClientsOrderDetailByCompanyDto>, List<ClientsOrderDetailByCompany>>(clientsOrderDetailByCompnayDtos, new List<ClientsOrderDetailByCompany>());

            await _clientsOrderDetailByCompanyRepository.InsertManyAsync(
                ObjectMapper.Map<List<ClientsOrderDetailByCompanyDto>, List<ClientsOrderDetailByCompany>>(clientsOrderDetailByCompnayDtos, new List<ClientsOrderDetailByCompany>()));
        }

        private bool IsInRole(string Role)
        {
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            // Get the claims values
            var role = _httpContextAccessor.HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.Role)
                               .Select(c => c.Value).SingleOrDefault();
            return role == Role;
        }

        private string GetCompanyId()
        {
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            // Get the claims values
            var CompanyId = _httpContextAccessor.HttpContext.User.Claims.Where(c => c.Type == "CompanyId")
                               .Select(c => c.Value).SingleOrDefault();
            if (CompanyId == null)
            {
                throw new UserFriendlyException("کد شرکت تعریف نشده است");
            }
            return CompanyId;
        }
    }
}
