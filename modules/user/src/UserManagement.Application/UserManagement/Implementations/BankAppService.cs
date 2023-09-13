using Volo.Abp.Domain.Repositories;
using MongoDB.Driver;
using UserManagement.Application.Contracts.Models;
using UserManagement.Application.Contracts.Services;
using UserManagement.Domain.UserManagement.Advocacy;
using Volo.Abp.Application.Services;
using Volo.Abp.Uow;
using Abp.Runtime.Session;

namespace UserManagement.Application.UserManagement.Implementations;

[UnitOfWork(false)]
public class BankAppService : ApplicationService, IBankAppService
{
    private readonly IRepository<AdvocacyUsers, int> _advocacyUsersRepository;

    public BankAppService(IRepository<AdvocacyUsers,int> advocacyUserRepository)
    {
        _advocacyUsersRepository = advocacyUserRepository;
    }

    public AdvocacyUserFromBankDto CheckAdvocacy(string nationalCode)
    {
        var advocacyuser = _advocacyUsersRepository.WithDetails()
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

    public async Task DeleteAdvocayUserFromBank(string nationalCode)
    {
        var userId = CurrentUser.Id;
        //var ad = await _advocacyUsersFromBank.GetAll().Select(x => new
        //{
        //    x.nationalcode,
        //    x.UserId,
        //    x.Id
        //}).FirstOrDefaultAsync(x => x.nationalcode == nationalCode
        //&& x.UserId == userId);
        //if (ad == null)
        //{
        //    throw new UserFriendlyException("رکورد مورد نظر یافت نشد");
        //}
        //await _advocacyUsersFromBank.DeleteAsync(ad.Id);
    }

    public Task<List<AdvocacyUsersFromBankWithCompanyDto>> GetAdvocacyUserByCompanyId()
    {
        throw new NotImplementedException();
    }

    public Task<List<UserRejectionAdvocacyDto>> GetUserRejecttionAdvocacyList()
    {
        throw new NotImplementedException();
    }

    public Task<AdvocacyUserFromBankExportDto> InquiryAdvocacyUserReport(string nationalCode)
    {
        throw new NotImplementedException();
    }

    public Task<UserRejecgtionFromBankExportDto> InquiryUserRejectionFromBank(string nationalCode)
    {
        throw new NotImplementedException();
    }

    public Task SaveAdvocacyUsersFromBank(List<AdvocacyUsersFromBankDto> advocacyUsersFromBankDto)
    {
        throw new NotImplementedException();
    }

    public Task SaveUserRejectionFromBank(UserRejectionFromBankDto userRejectionFromBankDto)
    {
        throw new NotImplementedException();
    }
}
