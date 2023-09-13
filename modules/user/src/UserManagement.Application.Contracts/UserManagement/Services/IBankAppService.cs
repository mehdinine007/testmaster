using Abp.Application.Services;
using UserManagement.Application.Contracts.Models;

namespace UserManagement.Application.Contracts.Services;

public interface IBankAppService : IApplicationService
{
    Task DeleteAdvocayUserFromBank(string nationalCode);

    Task<List<UserRejectionAdvocacyDto>> GetUserRejecttionAdvocacyList();

    AdvocacyUserFromBankDto CheckAdvocacy(string nationalCode);

    Task SaveUserRejectionFromBank(UserRejectionFromBankDto userRejectionFromBankDto);

    Task<UserRejecgtionFromBankExportDto> InquiryUserRejectionFromBank(string nationalCode);

    Task SaveAdvocacyUsersFromBank(List<AdvocacyUsersFromBankDto> advocacyUsersFromBankDto);

    Task<AdvocacyUserFromBankExportDto> InquiryAdvocacyUserReport(string nationalCode);

    Task<List<AdvocacyUsersFromBankWithCompanyDto>> GetAdvocacyUserByCompanyId();
}
