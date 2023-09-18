using Volo.Abp.Domain.Repositories;
using MongoDB.Driver;
using UserManagement.Application.Contracts.Models;
using UserManagement.Application.Contracts.Services;
using UserManagement.Domain.UserManagement.Advocacy;
using Volo.Abp.Application.Services;
using Volo.Abp.Uow;
using Volo.Abp;
using UserManagement.Domain.Authorization.Users;
using MongoDB.Bson;

namespace UserManagement.Application.UserManagement.Implementations;

[UnitOfWork(false)]
public class BankAppService : ApplicationService, IBankAppService
{
    private readonly IRepository<AdvocacyUsers, int> _advocacyUsersRepository;
    private readonly ICommonAppService _commonAppService;
    private readonly IRepository<UserMongo, ObjectId> _userMongoRepository;
    private readonly IRepository<AdvocacyUsersFromBank, int> _advocacyUsersFromBank;
    private readonly IRepository<UserRejectionAdvocacy, int> _userRejectionAdcocacyRepository;
    private readonly IRepository<UserRejectionFromBank, int> _userRejectionFromBankFromBank;

    public BankAppService(IRepository<AdvocacyUsers, int> advocacyUserRepository,
                          IRepository<UserMongo, ObjectId> userMongoRepository,
                          ICommonAppService commonAppService,
                          IRepository<AdvocacyUsersFromBank, int> advocacyUsersFromBank,
                          IRepository<UserRejectionFromBank, int> userRejectionFromBankFromBank
        )
    {
        _userMongoRepository = userMongoRepository;
        _advocacyUsersRepository = advocacyUserRepository;
        _commonAppService = commonAppService;
        _advocacyUsersFromBank = advocacyUsersFromBank;
        _userRejectionFromBankFromBank = userRejectionFromBankFromBank;
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

    public async Task<List<AdvocacyUsersFromBankWithCompanyDto>> GetAdvocacyUserByCompanyId()
    {
        if (!_commonAppService.IsInRole("Company"))
        {
            throw new UserFriendlyException("دسترسی شما کافی نمی باشد");
        }
        List<AdvocacyUsersFromBankWithCompanyDto> advocacyUsersFromBankDto = null;
        List<AdvocacyUsersFromBankWithCompanyDto> advocacyUsersFromBanksDto = new List<AdvocacyUsersFromBankWithCompanyDto>();
        var userId = _commonAppService.GetUID();
        var user = (await _userMongoRepository.GetQueryableAsync()).FirstOrDefault(x => x.UID == userId.ToString());
        var advocacyUsersFromBanks = await _advocacyUsersFromBank.GetListAsync(x => x.CompanyId == user.CompanyId);
        if (advocacyUsersFromBanks == null)
        {
            return null;
        }
        else
        {
            advocacyUsersFromBankDto = ObjectMapper.Map<List<AdvocacyUsersFromBank>, List<AdvocacyUsersFromBankWithCompanyDto>>(advocacyUsersFromBanks, new List<AdvocacyUsersFromBankWithCompanyDto>());
        }

        return advocacyUsersFromBankDto;
    }

    public async Task<List<UserRejectionAdvocacyDto>> GetUserRejecttionAdvocacyList()
    {
        if (_commonAppService.GetRole() != "Bank")
        {
            throw new UserFriendlyException("خطای دسترسی");
        }


        //  _advocacyUsersFromBank.Delete(x => x.UserId == userId);
        var userId = _commonAppService.GetUID();
        var currentUser = await _userMongoRepository.FirstOrDefaultAsync(x => x.UID == userId.ToString()) ?? throw new UserFriendlyException("کاربر پیدا نشد");
        if (currentUser == null)
        {
            throw new UserFriendlyException("کاربر شما در سیستم یافت نشد");
        }

        var bankId = currentUser.BankId;
        //var userRejectionAdvocacyList = await _userRejectionAdcocacyRepository.Query(
        //    x => x.Join(_userRepository.GetAllIncluding(),
        //    x => x.NationalCode,
        //    x => x.NationalCode, (x, y) =>
        //        new
        //        {
        //            nationalCode = x.NationalCode,
        //            bankId = y.BankId,
        //            archived = x.Archived,
        //            x.datetime,
        //            x.ShabaNumber,
        //            x.accountNumber
        //        }
        //    ).Where(x => x.bankId == bankId).Select(x => new UserRejectionAdvocacyDto
        //    {
        //        NationalCode = x.nationalCode,
        //        datetime = x.datetime,
        //        accountNumber = x.accountNumber,
        //        ShabaNumber = x.ShabaNumber,
        //    }).ToListAsync());
        var users = await _userMongoRepository.GetListAsync(x => x.BankId == bankId);
        var usersNationalCode = users.Select(x => x.NationalCode).ToList();
        var userRejectionAdvocacyList = (await _userRejectionAdcocacyRepository.GetQueryableAsync()).Where(x => usersNationalCode.Any(y => y == x.NationalCode))
            .ToList();
        var joinResult = userRejectionAdvocacyList.Select(x => new UserRejectionAdvocacyDto
        {
            NationalCode = x.NationalCode,
            datetime = x.datetime,
            accountNumber = x.accountNumber,
            ShabaNumber = x.ShabaNumber
        }).ToList();

        return joinResult;

        throw new Exception("خطایی پیش آمده است");

    }

    public async Task<AdvocacyUserFromBankExportDto> InquiryAdvocacyUserReport(string nationalCode)
    {
        var userId = _commonAppService.GetUID();
        //var ad = await _advocacyUsersFromBank.GetAll().Select(x => new
        //{
        //    x.accountNumber,
        //    x.nationalcode,
        //    x.shabaNumber,
        //    x.UserId,
        //    x.price
        //}).FirstOrDefaultAsync(x => x.nationalcode == nationalCode
        //&& x.UserId == userId);
        var ad = (await _advocacyUsersFromBank.GetQueryableAsync())
            .Select(x => new
            {
                x.accountNumber,
                x.nationalcode,
                x.shabaNumber,
                x.UserId,
                x.price
            })
            .FirstOrDefault(x => x.nationalcode == nationalCode && x.UserId == userId);

        if (ad == null)
        {
            return null;
        }
        else
        {
            AdvocacyUserFromBankExportDto advocacyUserFromBankExportDto = new AdvocacyUserFromBankExportDto();
            advocacyUserFromBankExportDto.NationalCode = ad.nationalcode;
            advocacyUserFromBankExportDto.ShebaNumber = ad.shabaNumber;
            advocacyUserFromBankExportDto.AccountNumber = ad.accountNumber;
            advocacyUserFromBankExportDto.Price = ad.price;
            return advocacyUserFromBankExportDto;

        }

    }


    public async Task<UserRejecgtionFromBankExportDto> InquiryUserRejectionFromBank(string nationalCode)
    {
        if (_commonAppService.GetRole() != "Bank")
        {
            throw new UserFriendlyException("خطای دسترسی");
        }
        //if (!_abpSession.UserId.HasValue)
        //    throw new InvalidOperationException();
        var userId = _commonAppService.GetUID();
        var ad = (await _userRejectionFromBankFromBank.GetQueryableAsync())
            .OrderByDescending(x => x.Id).Select(x => new
            {
                x.accountNumber,
                x.nationalcode,
                x.shabaNumber,
                x.UserId,
                x.price,
                x.dateTime
            }).FirstOrDefault(x => x.nationalcode == nationalCode
            && x.UserId == userId);
        if (ad == null)
        {
            return null;
        }
        else
        {
            UserRejecgtionFromBankExportDto advocacyUserFromBankExportDto = new UserRejecgtionFromBankExportDto();
            advocacyUserFromBankExportDto.NationalCode = ad.nationalcode;
            advocacyUserFromBankExportDto.ShebaNumber = ad.shabaNumber;
            advocacyUserFromBankExportDto.AccountNumber = ad.accountNumber;
            advocacyUserFromBankExportDto.Price = ad.price;
            advocacyUserFromBankExportDto.dateTime = ad.dateTime;

            return advocacyUserFromBankExportDto;

        }

    }

    public async Task SaveAdvocacyUsersFromBank(List<AdvocacyUsersFromBankDto> advocacyUsersFromBankDto)
    {
        if (!advocacyUsersFromBankDto.Any())
            return;
        if (_commonAppService.GetRole() != "Bank")
        {
            throw new UserFriendlyException("خطای دسترسی");
        }
        //if (!_abpSession.UserId.HasValue)
        //    throw new InvalidOperationException();
        var userId = _commonAppService.GetUID();
        List<AdvocacyUsersFromBank> advocacyUsersFromBanks = new List<AdvocacyUsersFromBank>();
        //unitOfWorkOptions.IsTransactional = false;
        //unitOfWorkOptions.Scope = System.Transactions.TransactionScopeOption.RequiresNew;
        //using (var unitOfWork = _unitOfWorkManager.Begin(unitOfWorkOptions))
        //{

        //  _advocacyUsersFromBank.Delete(x => x.UserId == userId);
        advocacyUsersFromBanks = ObjectMapper.Map<List<AdvocacyUsersFromBankDto>, List<AdvocacyUsersFromBank>>(advocacyUsersFromBankDto, new List<AdvocacyUsersFromBank>());
        advocacyUsersFromBanks.ForEach(x =>
        {
            x.UserId = userId;
        });
        await _advocacyUsersFromBank.InsertManyAsync(advocacyUsersFromBanks, autoSave: true);
        //CurrentUnitOfWork.SaveChanges();
        //    unitOfWork.Complete();
        //}


    }

    public async Task SaveUserRejectionFromBank(UserRejectionFromBankDto userRejectionFromBankDto)
    {
        if (_commonAppService.GetRole() != "Bank")
        {
            throw new UserFriendlyException("خطای دسترسی");
        }
        //if (!_abpSession.UserId.HasValue)
        //    throw new InvalidOperationException();
        UserRejectionFromBank userRejectionFromBank = ObjectMapper.Map<UserRejectionFromBankDto, UserRejectionFromBank>(userRejectionFromBankDto, new UserRejectionFromBank());
        userRejectionFromBank.UserId = _commonAppService.GetUID();
        await _userRejectionFromBankFromBank.InsertAsync(userRejectionFromBank);
    }
}
