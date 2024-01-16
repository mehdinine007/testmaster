using CompanyManagement.Application.CompanyManagement.Implementations;
using CompanyManagement.Application.Contracts.CompanyManagement.Dto.BankDtos;
using CompanyManagement.Application.Contracts.CompanyManagement.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Auditing;

namespace CompanyManagement.HttpApi.BankManagement.Controllers
{
    [DisableAuditing]
    [RemoteService]
    [Route("api/services/app/Bank/[action]")]
    public class BankController: Controller
    {
        private readonly IBankAppService _bankAppService;

        public BankController(IBankAppService bankAppService)
       => _bankAppService = bankAppService;
        /// <summary class="header">
        ///حذف حساب وکالتی از طرف بانک 
        /// </summary>
        [HttpPost]
        public async Task<bool> DeleteAdvocayUserFromBank(string nationalCode)
        =>await _bankAppService.DeleteAdvocayUserFromBank(nationalCode);
        //--------------------------------------------------
        /// <summary class="header">
        ///لیست افراد انصراف داده از طریق بانک 
        /// </summary>
        [HttpGet]
        public async Task<UserRejecgtionFromBankExportDto> InquiryUserRejectionFromBank(string nationalCode)
        =>await _bankAppService.InquiryUserRejectionFromBank(nationalCode);
        //--------------------------------------------------
        /// <summary class="header">
        ///ثبت حساب وکالتی از طرف بانک 
        /// </summary>
        [HttpPost]
        public async Task<bool> SaveAdvocacyUsersFromBank(List<AdvocacyUsersFromBankDto> advocacyUsersFromBankDto)
        => await _bankAppService.SaveAdvocacyUsersFromBank(advocacyUsersFromBankDto);
        //--------------------------------------------------
        /// <summary class="header">
        ///ثبت انصراف از طریق بانک 
        /// </summary>
        [HttpPost]
        public async Task<bool> SaveUserRejectionFromBank(UserRejectionFromBankDto userRejectionFromBankDto)
        =>await _bankAppService.SaveUserRejectionFromBank(userRejectionFromBankDto);
    }
}
