using CompanyManagement.Application.Contracts.CompanyManagement.Dto.BankDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace CompanyManagement.Application.Contracts.CompanyManagement.Services
{
    public interface IBankAppService: IApplicationService
    {
        Task<bool> SaveAdvocacyUsersFromBank(List<AdvocacyUsersFromBankDto> advocacyUsersFromBankDto);
        Task<bool> DeleteAdvocayUserFromBank(string nationalCode);
        Task<UserRejecgtionFromBankExportDto> InquiryUserRejectionFromBank(string nationalCode);
        Task<bool> SaveUserRejectionFromBank(UserRejectionFromBankDto userRejectionFromBankDto);
     
    }
}
