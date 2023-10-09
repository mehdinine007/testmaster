using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Auditing;
using Volo.Abp;
using Microsoft.AspNetCore.Mvc;
using OrderManagement.Application.Contracts.OrderManagement.Services;
using OrderManagement.Application.Contracts.OrderManagement;
using Volo.Abp.Application.Dtos;
using Esale.Share.Authorize;
using Microsoft.AspNetCore.Http;
using OrderManagement.Domain.Shared;
using OrderManagement.Application.Contracts;
using Esale.Core.Utility.Tools;
using OrderManagement.Application.Contracts.CompanyManagement.Services;
using OrderManagement.Application.Contracts.CompanyManagement;
using OrderManagement.Domain.Shared.CompanyManagement;

namespace OrderManagement.HttpApi.OrderManagement.Controllers;

[DisableAuditing]
[RemoteService]
[Route("api/services/app/CompanyAppService/[action]")]
[UserAuthorization]
public class CompanyController : Controller
{
    private readonly ICompanyAppService _companyAppService;
    public CompanyController(ICompanyAppService companyAppService)
        => _companyAppService = companyAppService;
    [HttpGet]
    public List<CustomersWithCars> GetCustomersAndCars(GetCustomersAndCarsDto input)
    =>_companyAppService.GetCustomersAndCars(input);
    [HttpPost]
    public Task<bool> InsertCompanyProduction(List<CompanyProductionDto> companyProductionsDto)
    =>_companyAppService.InsertCompanyProduction(companyProductionsDto);
    [HttpPost]
    public Task<bool> SubmitOrderInformations(List<ClientsOrderDetailByCompanyDto> clientsOrderDetailByCompnayDtos)
    => _companyAppService.SubmitOrderInformations(clientsOrderDetailByCompnayDtos);
}
