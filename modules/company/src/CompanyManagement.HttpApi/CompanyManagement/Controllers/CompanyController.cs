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
[Route("api/services/app/CompanyAppService/[action]")]
public class CompanyController : Controller
{
    private readonly ICompanyAppService _companyAppService;
    public CompanyController(ICompanyAppService companyAppService)
        => _companyAppService = companyAppService;
    [HttpGet]
    public List<CustomersWithCars> GetCustomersAndCars(GetCustomersAndCarsDto input)
    =>_companyAppService.GetCustomersAndCars(input);
    [HttpPost]
    public Task InsertCompanyProduction(List<CompanyProductionDto> companyProductionsDto)
    =>_companyAppService.InsertCompanyProduction(companyProductionsDto);
    [HttpPost]
    public Task SubmitOrderInformations(List<ClientsOrderDetailByCompanyDto> clientsOrderDetailByCompnayDtos)
    => _companyAppService.SubmitOrderInformations(clientsOrderDetailByCompnayDtos);
}
