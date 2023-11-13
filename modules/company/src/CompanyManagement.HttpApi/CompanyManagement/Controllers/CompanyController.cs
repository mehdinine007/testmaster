using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Auditing;
using Volo.Abp;
using Microsoft.AspNetCore.Mvc;
using CompanyManagement.Application.Contracts.CompanyManagement.Services;
using CompanyManagement.Application.Contracts.CompanyManagement;
using CompanyManagement.Domain.Shared.CompanyManagement;

namespace CompanyManagement.HttpApi.OrderManagement.Controllers;

[DisableAuditing]
[RemoteService]
[Route("api/services/app/Company/[action]")]
public class CompanyController : Controller
{
    private readonly ICompanyAppService _companyAppService;
    public CompanyController(ICompanyAppService companyAppService)
        => _companyAppService = companyAppService;
    [HttpGet]
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
        return await _companyAppService.SubmitOrderInformations(clientsOrderDetailByCompnayDtos);
    }

    [HttpGet]
    public CompaniesCustomerDto GetRecentCustomerAndOrder(string nationalCode, int saleId)
        => _companyAppService.GetRecentCustomerAndOrder(nationalCode, saleId);
}
