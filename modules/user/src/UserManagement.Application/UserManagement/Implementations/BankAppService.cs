using Volo.Abp.Domain.Repositories;
using MongoDB.Driver;
using UserManagement.Application.Contracts.Models;
using UserManagement.Application.Contracts.Services;
using UserManagement.Domain.UserManagement.Advocacy;
using Volo.Abp.Application.Services;
using Volo.Abp.Uow;

namespace UserManagement.Application.UserManagement.Implementations;

[UnitOfWork(false)]
public class BankAppService : ApplicationService, IBankAppService
{
    private readonly IRepository<AdvocacyUsers, int> _advocacyUserRepository;

    public BankAppService(IRepository<AdvocacyUsers,int> advocacyUserRepository)
    {
        _advocacyUserRepository = advocacyUserRepository;
    }

    public AdvocacyUserFromBankDto CheckAdvocacy(string nationalCode)
    {
        var advocacyuser = _advocacyUserRepository.WithDetails()
                .Select(x => new
                {
                    x.shabaNumber,
                    x.accountNumber,
                    x.Id,
                    x.nationalcode,
                    x.BanksId
                })
                .OrderByDescending(x => x.Id).FirstOrDefault(x => x.nationalcode == nationalCode);
        if (advocacyuser == null)
        {
            return null;
        }
        AdvocacyUserFromBankDto advocacyUserFromBankDto = new AdvocacyUserFromBankDto();
        advocacyUserFromBankDto.AccountNumber = advocacyuser.accountNumber;
        advocacyUserFromBankDto.ShebaNumber = advocacyuser.shabaNumber;
        advocacyUserFromBankDto.BankId = (int)advocacyuser.BanksId;

        return advocacyUserFromBankDto;
    }
}
