using CompanyManagement.Application.Contracts.CompanyManagement.Dto;
using CompanyManagement.Application.Contracts.CompanyManagement.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Auditing;

namespace CompanyManagement.HttpApi.CompanyManagement.Controllers
{
    [DisableAuditing]
    [RemoteService]
    [Route("api/services/app/ClientOrderDetailCompany/[action]")]
    public class ClientOrderDetailCompanyController: Controller
    {

        private readonly IClientOrderDetailCompanyService _clientOrderDetailCompanyService;
        public ClientOrderDetailCompanyController(IClientOrderDetailCompanyService clientOrderDetailCompanyService)
        => _clientOrderDetailCompanyService = clientOrderDetailCompanyService;

        [HttpGet]
        public async Task<List<ClientOrderDetailDto>> GetList(string nationalCode)
        => await _clientOrderDetailCompanyService.GetList(nationalCode);
    }
}
