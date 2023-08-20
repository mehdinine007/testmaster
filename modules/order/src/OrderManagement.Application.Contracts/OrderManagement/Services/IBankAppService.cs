﻿using OrderManagement.Application.Contracts.OrderManagement;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace OrderManagement.Application.Contracts.Services
{
    public interface IBankAppService : IApplicationService
    {
        Task<BankDto> GetById(int id);
        Task<BankDto> Add(BankDto bankDto);
        Task<BankDto> Update(BankDto bankDto);
        Task<List<BankDto>> GetList();
        Task<bool> Delete(int id);
        Task<bool> UploadFile(UploadFileDto uploadFile);
        Task SaveAdvocacyUsersFromBank(List<AdvocacyUsersFromBankDto> advocacyUsersFromBankDto);
        //Task<List<UserRejectionAdvocacyDto>> GetUserRejecttionAdvocacyList();
        Task DeleteAdvocayUserFromBank(string nationalCode);
        Task<AdvocacyUserFromBankDto> CheckAdvocacy(string NationalCode);
        Task<AdvocacyUserFromBankExportDto> InquiryAdvocacyUserReport(string nationalCode);
        Task<List<AdvocacyUsersFromBankWithCompanyDto>> GetAdvocacyUserByCompanyId();
    }
}
