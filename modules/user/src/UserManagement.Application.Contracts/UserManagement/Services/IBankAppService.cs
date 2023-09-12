using Abp.Application.Services;
using UserManagement.Application.Contracts.Models;

namespace UserManagement.Application.Contracts.Services;

public interface IBankAppService : IApplicationService
{
    AdvocacyUserFromBankDto CheckAdvocacy(string nationalCode);
}
