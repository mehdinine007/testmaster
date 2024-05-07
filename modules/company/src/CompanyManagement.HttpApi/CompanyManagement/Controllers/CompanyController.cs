using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Auditing;
using Volo.Abp;
using Microsoft.AspNetCore.Mvc;
using CompanyManagement.Application.Contracts.CompanyManagement.Services;
using CompanyManagement.Application.Contracts.CompanyManagement;
using CompanyManagement.Domain.Shared.CompanyManagement;
using CompanyManagement.Application.Contracts;
using Esale.Share.Authorize;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using CompanyManagement.Application.CompanyManagement.Implementations;

namespace CompanyManagement.HttpApi.OrderManagement.Controllers;

[DisableAuditing]
[RemoteService]
[Route("api/services/app/Company/[action]")]
public class CompanyController : Controller
{
    private readonly ICompanyAppService _companyAppService;
    private readonly IClientOrderDetailCompanyService _clientOrderDetailCompanyService;
    public CompanyController(ICompanyAppService companyAppService, IClientOrderDetailCompanyService clientOrderDetailCompanyService)
    {
        _companyAppService = companyAppService;
        _clientOrderDetailCompanyService = clientOrderDetailCompanyService;
    }

    [HttpPost]
    public List<CustomersWithCars> GetCustomersAndCars(GetCustomersAndCarsDto input)
    => _companyAppService.GetCustomersAndCars(input);
    [HttpPost]
    public async Task<bool> InsertCompanyProduction(List<CompanyProductionDto> companyProductionsDto)
    {
        return await _companyAppService.InsertCompanyProduction(companyProductionsDto);
    }
    [HttpPost]
    public async Task<bool> SubmitOrderInformations(List<ClientsOrderDetailByCompanyDto> clientsOrderDetailByCompnayDtos)
    {
        return await _clientOrderDetailCompanyService.Save(clientsOrderDetailByCompnayDtos);
    }

    [HttpGet]
    public async Task<CompaniesCustomerDto> GetRecentCustomerAndOrder(string nationalCode, int saleId)
        => await _companyAppService.GetRecentCustomerAndOrder(nationalCode, saleId);

    [HttpPost]
    public async Task<bool> SaveOrderInformation(ClientsOrderDetailByCompanyDto request)
    {
        await _companyAppService.SaveOrderInformation(request);
        return true;
    }
}
