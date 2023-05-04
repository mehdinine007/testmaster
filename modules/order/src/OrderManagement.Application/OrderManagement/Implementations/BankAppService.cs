using OrderManagement.Domain;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Uow;
using System.Threading.Tasks;
using OrderManagement.Application.Contracts;
using Volo.Abp;
using OrderManagement.Application.Contracts.Services;
using System.Collections.Generic;
using System;
using Volo.Abp.Auditing;
using Volo.Abp.Application.Services;
using System.Linq;
using Abp.Authorization;
using Abp.Domain.Uow;
using Abp.Runtime.Session;

namespace OrderManagement.Application.OrderManagement.Implementations;

public class BankAppService : ApplicationService, IBankAppService 
{
    private readonly IRepository<Bank, int> _bankRepository;
    private readonly IRepository<AdvocacyUsers, int> _advocacyUsers;
    private readonly IRepository<AdvocacyUsersFromBank, int> _advocacyUsersFromBank;
    private readonly IRepository<UserRejectionFromBank, int> _userRejectionFromBankFromBank;

    private readonly IAbpSession _abpSession;
    private readonly IRepository<UserRejectionAdvocacy, int> _userRejectionAdcocacyRepository;
    private readonly IRepository<SaleDetail, int> _saleDetailRepository;
    private readonly ICommonAppService _commonAppService;
    private readonly IRepository<Gallery, int> _galleryRepository;

    //[AbpAuthorize(PermissionNames.Pages_Bank_Advocacy)]
    public async Task DeleteAdvocayUserFromBank(string nationalCode)
    {
        var userId = _abpSession.UserId;
        var ad = _advocacyUsersFromBank.WithDetails().Select(x => new
        {
            x.nationalcode,
            x.UserId,
            x.Id
        }).FirstOrDefault(x => x.nationalcode == nationalCode
        && x.UserId == userId);
        if (ad == null)
        {
            throw new UserFriendlyException("رکورد مورد نظر یافت نشد");
        }
        await _advocacyUsersFromBank.DeleteAsync(ad.Id);
    }
    [Audited]
    public List<BankDto> GetList()
    {
        var join = from b in _bankRepository.WithDetails()
                   join g in _galleryRepository.WithDetails() on b.LogoId equals g.Id into details
                   from m in details.DefaultIfEmpty()
                   select new BankDto
                   {
                       Id = b.Id,
                       LogoUrl = m.ImageUrl,
                       Title = b.Title
                   };
        var result = join.ToList();
        return result;
    }

    //[AbpAuthorize]
    //public async Task<List<UserRejectionAdvocacyDto>> GetUserRejecttionAdvocacyList()
    //{
    //    if (await _commonAppService.GetRole() != "Bank")
    //    {
    //        throw new UserFriendlyException("خطای دسترسی");
    //    }
    //    //  _advocacyUsersFromBank.Delete(x => x.UserId == userId);
    //    var userId = _abpSession.UserId;
    //    var currentUser = await _userRepository.FirstOrDefaultAsync(x => x.Id == userId) ?? throw new UserFriendlyException("کاربر پیدا نشد");
    //    if (currentUser == null)
    //    {
    //        throw new UserFriendlyException("کاربر شما در سیستم یافت نشد");
    //    }
    //    var bankId = currentUser.BankId;
    //    var userRejectionAdvocacyList = await _userRejectionAdcocacyRepository.Query(
    //        x => x.Join(_userRepository.GetAllIncluding(),
    //        x => x.NationalCode,
    //        x => x.NationalCode, (x, y) =>
    //            new {
    //                nationalCode = x.NationalCode,
    //                bankId = y.BankId,
    //                archived = x.Archived,
    //                datetime = x.datetime,
    //                ShabaNumber = x.ShabaNumber,
    //                accountNumber = x.accountNumber
    //            }
    //        ).Where(x => x.bankId == bankId).Select(x => new UserRejectionAdvocacyDto
    //        {
    //            NationalCode = x.nationalCode,
    //            datetime = x.datetime,
    //            accountNumber = x.accountNumber,
    //            ShabaNumber = x.ShabaNumber,
    //        }).ToListAsync());
    //    return userRejectionAdvocacyList;
    //    throw new Exception("خطایی پیش آمده است");
    //}

    [RemoteService(false)]
    public async Task<AdvocacyUserFromBankDto> CheckAdvocacy(string NationalCode)
    {

        var advocacyUesrQuery = await _advocacyUsers.GetQueryableAsync();

        var advocacyuser = advocacyUesrQuery
            .Select(x => new
            {
                x.shabaNumber,
                x.accountNumber,
                x.Id,
                x.nationalcode,
                x.BanksId
            })
            .OrderByDescending(x => x.Id).FirstOrDefault(x => x.nationalcode == NationalCode);
        if (advocacyuser == null)
        {
            throw new UserFriendlyException("اطلاعات حساب وکالتی یافت نشد");
        }
        AdvocacyUserFromBankDto advocacyUserFromBankDto = new AdvocacyUserFromBankDto();
        advocacyUserFromBankDto.AccountNumber = advocacyuser.accountNumber;
        advocacyUserFromBankDto.ShebaNumber = advocacyuser.shabaNumber;
        advocacyUserFromBankDto.BankId = (int)advocacyuser.BanksId;

        return advocacyUserFromBankDto;
    }

    [Audited]
    [AbpAuthorize]
    public async Task SaveUserRejectionFromBank(UserRejectionFromBankDto userRejectionFromBankDto)
    {
        if (await _commonAppService.GetRole() != "Bank")
        {
            throw new UserFriendlyException("خطای دسترسی");
        }
        if (!_abpSession.UserId.HasValue)
            throw new InvalidOperationException();
        UserRejectionFromBank userRejectionFromBank = ObjectMapper.Map<UserRejectionFromBankDto, UserRejectionFromBank>(userRejectionFromBankDto, new UserRejectionFromBank());
        var userId = _commonAppService.GetUserId();
        userRejectionFromBank.UserId = userId;
        await _userRejectionFromBankFromBank.InsertAsync(userRejectionFromBank);
    }
    [Audited]
    //[HttpGet]
    //[AbpAuthorize]
    public async Task<UserRejecgtionFromBankExportDto> InquiryUserRejectionFromBank(string nationalCode)
    {
        if (await _commonAppService.GetRole() != "Bank")
        {
            throw new UserFriendlyException("خطای دسترسی");
        }
        if (!_abpSession.UserId.HasValue)
            throw new InvalidOperationException();
        var userId = _abpSession.UserId;
        var ad = _userRejectionFromBankFromBank.WithDetails()
            .OrderByDescending(x => x.Id).Select(x => new
            {
                x.accountNumber,
                x.nationalcode,
                x.shabaNumber,
                x.UserId,
                x.price,
                x.dateTime
            }).FirstOrDefault(x => x.nationalcode == nationalCode && x.UserId == userId);
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
    [AbpAuthorize]
    [Audited]
    public async Task SaveAdvocacyUsersFromBank(List<AdvocacyUsersFromBankDto> advocacyUsersFromBankDto)
    {
        if (!advocacyUsersFromBankDto.Any())
            return;
        if (await _commonAppService.GetRole() != "Bank")
        {
            throw new UserFriendlyException("خطای دسترسی");
        }
        if (!_abpSession.UserId.HasValue)
            throw new InvalidOperationException();
        var userId = _abpSession.UserId;
        List<AdvocacyUsersFromBank> advocacyUsersFromBanks = new List<AdvocacyUsersFromBank>();
        UnitOfWorkOptions unitOfWorkOptions = new UnitOfWorkOptions();
        unitOfWorkOptions.IsTransactional = false;
        unitOfWorkOptions.Scope = System.Transactions.TransactionScopeOption.RequiresNew;
        //using (var unitOfWork = _unitOfWorkManager.Begin(unitOfWorkOptions))
        //{

        //  _advocacyUsersFromBank.Delete(x => x.UserId == userId);
        //advocacyUsersFromBanks = ObjectMapper.Map<List<AdvocacyUsersFromBankDto>, List<AdvocacyUsersFromBank>>(advocacyUsersFromBankDto, new List<AdvocacyUsersFromBank>());
        advocacyUsersFromBanks = ObjectMapper.Map<List<AdvocacyUsersFromBankDto>, List<AdvocacyUsersFromBank>>(advocacyUsersFromBankDto);
        advocacyUsersFromBanks.ForEach(x =>
        {
            x.UserId = userId.Value;
        });
        await _advocacyUsersFromBank.InsertManyAsync(advocacyUsersFromBanks);
        //unitOfWork.Complete();
        await CurrentUnitOfWork.SaveChangesAsync();
        //}


    }
    [Audited]
    //[UnitOfWork( System.Transactions.IsolationLevel.ReadUncommitted)]
    [Volo.Abp.Uow.UnitOfWork(false,isolationLevel: System.Data.IsolationLevel.ReadUncommitted)]
    //[AbpAuthorize(PermissionNames.Pages_Bank_Advocacy)]
    public async Task<AdvocacyUserFromBankExportDto> InquiryAdvocacyUserReport(string nationalCode)
    {
        var userId = _abpSession.UserId;
        var ad = _advocacyUsersFromBank.WithDetails().Select(x => new
        {
            x.accountNumber,
            x.nationalcode,
            x.shabaNumber,
            x.UserId,
            x.price
        }).FirstOrDefault(x => x.nationalcode == nationalCode && x.UserId == userId);
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


    public async Task<List<AdvocacyUsersFromBankWithCompanyDto>> GetAdvocacyUserByCompanyId()
    {
        if (!_commonAppService.IsInRole("Company"))
        {
            throw new UserFriendlyException("دسترسی شما کافی نمی باشد");
        }
        List<AdvocacyUsersFromBankWithCompanyDto> advocacyUsersFromBankDto = null;
        List<AdvocacyUsersFromBankWithCompanyDto> advocacyUsersFromBanksDto = new List<AdvocacyUsersFromBankWithCompanyDto>();
        var userId = _abpSession.UserId;
        //var user = _userRepository.FirstOrDefault(x => x.Id == userId);
        //TODO: add company id here some how
        int userCompanyId = default;
        var advocacyUsersFromBanks = _advocacyUsersFromBank.WithDetails().Where(x => x.CompanyId == userCompanyId).ToList();
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

}
