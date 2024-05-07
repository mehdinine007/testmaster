using CompanyManagement.Application.CompanyManagement.Implementations;
using CompanyManagement.Application.Contracts.CompanyManagement;
using CompanyManagement.Application.Contracts.CompanyManagement.Dto;
using CompanyManagement.Application.Contracts.CompanyManagement.Services;
using CompanyManagement.Domain.Shared.CompanyManagement;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;

namespace CompanyManagement.HttpApi.CompanyManagement.Controllers
{
    [RemoteService]
    [Route("api/services/app/BlackList/[action]")]

    public class BlackListController : Controller
    {
        private readonly IBlackListService _blackListService;
        public BlackListController(IBlackListService blackListService)
        {
            _blackListService = blackListService;
        }

        [HttpGet]
        public async Task<bool> Inquiry(string nationalCode)
    => await _blackListService.Inquiry(nationalCode);
    }
}
