using AutoMapper.Internal.Mappers;
using CompanyManagement.Application.Contracts.CompanyManagement;
using CompanyManagement.Application.Contracts.CompanyManagement.Services;
using CompanyManagement.Domain.CompanyManagement;
using CompanyManagement.Domain.Shared.CompanyManagement;
using CompanyManagement.EfCore.CompanyManagement.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
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


namespace CompanyManagement.Application.CompanyManagement.Implementations
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

        public async Task<bool> InsertCompanyProduction(List<CompanyProductionDto> companyProductionsDto)
        {
            if (!IsInRole("Company"))
                throw new UserFriendlyException("عدم دسترسی کافی");
            var companyProductions = ObjectMapper.Map(companyProductionsDto, new List<CompanyProduction>());
            await _companyProductionRepository.InsertManyAsync(companyProductions);
            return true;
        }

        public async Task<bool> SubmitOrderInformations(List<ClientsOrderDetailByCompanyDto> clientsOrderDetailByCompnayDtos)
        {
            if (!IsInRole("Company"))
                throw new UserFriendlyException("عدم دسترسی کافی");
            var x = ObjectMapper.Map<List<ClientsOrderDetailByCompanyDto>, List<ClientsOrderDetailByCompany>>(clientsOrderDetailByCompnayDtos, new List<ClientsOrderDetailByCompany>());

            await _clientsOrderDetailByCompanyRepository.InsertManyAsync(
                ObjectMapper.Map<List<ClientsOrderDetailByCompanyDto>, List<ClientsOrderDetailByCompany>>(clientsOrderDetailByCompnayDtos, new List<ClientsOrderDetailByCompany>()));
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
