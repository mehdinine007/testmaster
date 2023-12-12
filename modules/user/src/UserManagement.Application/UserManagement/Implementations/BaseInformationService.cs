#region NS
using Volo.Abp.Domain.Repositories;
using Microsoft.Extensions.Configuration;
using UserManagement.Application.Contracts.Models;
using UserManagement.Application.Contracts.Services;
using Volo.Abp.Application.Services;
using UserManagement.Domain.UserManagement.Advocacy;
using Volo.Abp;
using MongoDB.Driver;
using System.Security.Claims;
using UserManagement.Domain.UserManagement.Bases;
using Microsoft.AspNetCore.Http;
using UserManagement.Domain.Authorization.Users;
using MongoDB.Bson;
using Volo.Abp.Uow;
using Volo.Abp.Auditing;
using Microsoft.EntityFrameworkCore;
using UserManagement.Domain.Shared;
using UserManagement.Domain.UserManagement.CompanyService;
using IFG.Core.Caching;
using Abp.Runtime.Caching;
using UserManagement.Application.Contracts.UserManagement.Constant;
using Newtonsoft.Json;
#endregion

namespace UserManagement.Application.Implementations;

public class BaseInformationService : ApplicationService, IBaseInformationService
{
    private readonly IConfiguration _configuration;
    private readonly IRepository<AdvocacyUsers, int> _advocacyUsersRepository;
    private readonly IRepository<UserMongo, ObjectId> _userMongoRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IRepository<WhiteList, int> _whiteListRepository;
    private readonly ICommonAppService _commonAppService;
    private readonly IRepository<ClientsOrderDetailByCompany, long> _clientsOrderDetailByCompany;
    private readonly IRepository<CompanyPaypaidPrices, long> _companyPaypaidPricesRepository;
    private readonly IFG.Core.Caching.ICacheManager _cacheManager;


    public BaseInformationService(IConfiguration configuration,
                                  IRepository<AdvocacyUsers, int> advocacyUsersRepository,
                                  IHttpContextAccessor httpContextAccessor,
                                  IRepository<WhiteList, int> whiteListRepository,
                                  IRepository<UserMongo, ObjectId> userMongoRepository,
                                  ICommonAppService commonAppService,
                                  IRepository<ClientsOrderDetailByCompany, long> clientsOrderDetailByCompany,
                                  IRepository<CompanyPaypaidPrices, long> companyPaypaidPricesRepository,
                                  IFG.Core.Caching.ICacheManager cacheManager)
    {
        _configuration = configuration;
        _advocacyUsersRepository = advocacyUsersRepository;
        _userMongoRepository = userMongoRepository;
        _httpContextAccessor = httpContextAccessor;
        _whiteListRepository = whiteListRepository;
        _commonAppService = commonAppService;
        _clientsOrderDetailByCompany = clientsOrderDetailByCompany;
        _companyPaypaidPricesRepository = companyPaypaidPricesRepository;
        _cacheManager = cacheManager;
    }

    public async Task RegistrationValidationWithoutCaptcha(RegistrationValidationDto input)
    {
        if (!ValidationHelper.IsNationalCode(input.Nationalcode))
        {
            throw new UserFriendlyException(Messages.NationalCodeNotValid);
        }
        if (_configuration.GetSection("IsCheckAdvocacy").Value == "1")
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
          .OrderByDescending(x => x.Id).FirstOrDefault(x => x.nationalcode == input.Nationalcode);

            if (advocacyuser == null)
            {
                throw new UserFriendlyException("اطلاعات حساب وکالتی یافت نشد");
            }
        }

        var user = (await _userMongoRepository
                    .GetQueryableAsync())
                    .FirstOrDefault(a => a.NormalizedUserName == input.Nationalcode && a.IsDeleted == false);
        if (user != null)
        {

            throw new UserFriendlyException("این کد ملی قبلا ثبت نام شده است");
        }
    }

    public async Task<bool> CheckWhiteListAsync(WhiteListEnumType whiteListEnumType, string Nationalcode)
    {
        if (_configuration.GetSection(whiteListEnumType.ToString()).Value == "1")
        {
            if (string.IsNullOrEmpty(Nationalcode))
            {
                Console.WriteLine("dakhel");
                var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
                // Get the claims values
                string nc = _httpContextAccessor.HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.Name)
                    .Select(c => c.Value).SingleOrDefault();
                if (Nationalcode == null)
                {
                    throw new UserFriendlyException("کد ملی صحیح نمی باشد");
                }
                Nationalcode = nc;
            }
            Console.WriteLine("biron");

            var WhiteList = (await _whiteListRepository.GetQueryableAsync())
                .Select(x => new { x.NationalCode, x.WhiteListType })
                .FirstOrDefault(x => x.NationalCode == Nationalcode && x.WhiteListType == whiteListEnumType);
            if (WhiteList == null)
            {
                //unitOfWork.Complete();
                throw new UserFriendlyException(_configuration.GetValue<string>("ErrorMessages:InsertUserRejectionAdvocacy"));
            }
            //unitOfWork.Complete();
        }

        return true;
    }

    [UnitOfWork(false)]
    [Audited]
    [RemoteService(false)]
    public async Task<UserGrpcDto> GetUserByIdAsync(string userId)
    {
        System.Diagnostics.Debugger.Launch();
        object UserFromCache = null;
      
        string prefix = $"{RedisConstants.GetUserById}";
        string cacheKey = "GetUser_"+ userId;

        var cachedData = await _cacheManager.GetStringAsync(cacheKey, prefix, new CacheOptions
        { Provider = CacheProviderEnum.Hybrid });

        if (!string.IsNullOrEmpty(cachedData)) {
            return JsonConvert.DeserializeObject<UserGrpcDto>(cachedData);
        }

        var userQueryable = await _userMongoRepository.GetQueryableAsync();
        var user = userQueryable
            .Select(x => new
            {
                x.AccountNumber,
                x.BankId,
                x.BirthCityId,
                x.BirthProvinceId,
                x.HabitationCityId,
                x.HabitationProvinceId,
                x.IssuingCityId,
                x.IssuingProvinceId,
                x.Mobile,
                x.NationalCode,
                x.Shaba,
                x.CompanyId,
                x.Gender,
                x.Name,
                x.Surname,
                x.UID
            })
            .FirstOrDefault(x => x.UID == userId.ToLower());

        if (user == null)
            return null;

        var usergrpcdto = new UserGrpcDto
        {
            AccountNumber = user.AccountNumber,
            BankId = user.BankId,
            BirthCityId = user.BirthCityId,
            BirthProvinceId = user.BirthProvinceId,
            HabitationCityId = user.HabitationCityId,
            HabitationProvinceId = user.HabitationProvinceId,
            IssuingCityId = user.IssuingCityId,
            IssuingProvinceId = user.IssuingProvinceId,
            MobileNumber = user.Mobile,
            NationalCode = user.NationalCode,
            Shaba = user.Shaba,
            GenderCode = (int)user.Gender,
            CompanyId = user.CompanyId,
            SurName = user.Surname,
            Name = user.Name,
            Uid=user.UID
        };

        await _cacheManager.SetStringAsync(cacheKey, prefix, JsonConvert.SerializeObject(usergrpcdto), new CacheOptions
        { Provider = CacheProviderEnum.Hybrid }, TimeSpan.FromMinutes(5).TotalSeconds);

        return usergrpcdto;
    }

    [Audited]
    public async Task<bool> RegistrationValidationAsync(RegistrationValidationDto input)
    {
        await _commonAppService.ValidateVisualizeCaptcha(new VisualCaptchaInput(input.CK, input.CIT));
        Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
        if (!ValidationHelper.IsNationalCode(input.Nationalcode))
        {
            throw new UserFriendlyException(Messages.NationalCodeNotValid);
        }
        if (!string.IsNullOrEmpty(_configuration.GetSection("CloseRegisterDate").Value)
            && DateTime.Now > DateTime.Parse(_configuration.GetSection("CloseRegisterDate").Value))
        {
            throw new UserFriendlyException("زمان ثبت نام به پایان رسیده است");
        }

        if (_configuration.GetSection("IsCheckAdvocacy").Value == "1")
        {
            var advocacyuser =(await _advocacyUsersRepository.GetQueryableAsync())
           .Select(x => new
           {
               x.shabaNumber,
               x.accountNumber,
               x.Id,
               x.nationalcode,
               x.BanksId
           })
           .FirstOrDefault(x => x.nationalcode == input.Nationalcode);

            if (advocacyuser == null)
            {
                throw new UserFriendlyException("حساب وکالتی یافت نشد");
            }
        }

        var user = (await _userMongoRepository.GetQueryableAsync())
            .FirstOrDefault(x => x.NormalizedUserName == input.Nationalcode);

        if (user != null)
        {
            throw new UserFriendlyException("این کد ملی قبلا ثبت نام شده است");
        }

        return true;
    }

    public async Task<string> AddressInquiry(AddressInquiryDto input)

    {
        if (input.nationalCode == null)
        {
            throw new UserFriendlyException("کد ملی را وارد نمایید");
        }
        if (input.zipCod == null)
        {
            throw new UserFriendlyException("کدپستی را وارد نمایید");
        }
        var useInquiryForUserAddress = _configuration.GetValue<bool?>("Inquiry:UseInquiryForUserAddress") ?? false;
        if (!useInquiryForUserAddress)
            throw new UserFriendlyException("استعلام شماره موبایل ممکن نیست");
        var zipCodeInquiry =await _commonAppService.GetAddressByZipCode(input.zipCod, input.nationalCode);
        return zipCodeInquiry;
    }

}