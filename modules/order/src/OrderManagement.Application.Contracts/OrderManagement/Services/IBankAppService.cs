using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace OrderManagement.Application.Contracts.Services
{
    public interface IBankAppService : IApplicationService
    {
        List<BankDto> GetList();
        Task SaveAdvocacyUsersFromBank(List<AdvocacyUsersFromBankDto> advocacyUsersFromBankDto);
        //Task<List<UserRejectionAdvocacyDto>> GetUserRejecttionAdvocacyList();
        Task DeleteAdvocayUserFromBank(string nationalCode);
        Task<AdvocacyUserFromBankDto> CheckAdvocacy(string NationalCode);
        Task<AdvocacyUserFromBankExportDto> InquiryAdvocacyUserReport(string nationalCode);
        Task<List<AdvocacyUsersFromBankWithCompanyDto>> GetAdvocacyUserByCompanyId();
    }
}
