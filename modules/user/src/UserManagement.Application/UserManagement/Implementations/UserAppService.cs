using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Text.RegularExpressions;
using UserManagement.Application.Contracts.Models;
using UserManagement.Application.Contracts.Services;
using UserManagement.Application.Contracts.UserManagement.Services;
using UserManagement.Domain.Authorization.Users;
using UserManagement.Domain.Shared;
using UserManagement.Domain.UserManagement.Advocacy;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using WorkingWithMongoDB.WebAPI.Services;

namespace UserManagement.Application.UserManagement.Implementations;

public class UserAppService : ApplicationService, IUserAppService
{
    
    private readonly IRolePermissionService _rolePermissionService;
    private readonly IRepository<UserMongo, ObjectId> _userMongoRepository;
    private readonly IConfiguration _configuration;
    private readonly IBankAppService _bankAppService;
    private readonly ICommonAppService _commonAppService;
    private readonly IDistributedCache _distributedCache;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly IBaseInformationService _baseInformationService;

    public UserAppService(IConfiguration configuration,
                          IBankAppService bankAppService,
                          ICommonAppService commonAppService,
                          IDistributedCache distributedCache,
                          IPasswordHasher<User> passwordHasher,
                          IBaseInformationService baseInformationService,
                          IRolePermissionService rolePermissionService,
                          IRepository<UserMongo, ObjectId> userMongoRepository
        )
    {
        _rolePermissionService = rolePermissionService;
        _userMongoRepository = userMongoRepository;
        _configuration = configuration;
        _bankAppService = bankAppService;
        _commonAppService = commonAppService;
        _distributedCache = distributedCache;
        _passwordHasher = passwordHasher;
        _baseInformationService = baseInformationService;
    }

    public async Task<bool> AddRole(ObjectId userid, List<string> roleCode)
    {
        var user = (await _userMongoRepository
               .GetQueryableAsync())
               .FirstOrDefault(x => x.Id == userid);

        if (user is null)
            return false;
        foreach (var cod in roleCode)
        {
            if (await _rolePermissionService.ValidationByCode(cod))
                user.Roles.Add(cod);
        }
        await _userMongoRepository.UpdateAsync(user);
        return true;
    }

    public async Task<UserDto> CreateAsync(CreateUserDto input)
    {


      //  if (!string.IsNullOrEmpty(_configuration.GetSection("CloseRegisterDate").Value)
      //      && DateTime.Now > DateTime.Parse(_configuration.GetSection("CloseRegisterDate").Value))
      //  {
      //      throw new UserFriendlyException("زمان ثبت نام به پایان رسیده است");
      //  }
      //  //   _baseInformationService.RegistrationValidation()
      //  //Logs logs = new Logs();
      //  //logs.StartDate = DateTime.Now;
      //  object captcha = null;
      //  //_cacheManager.GetCache("Redis").TryGetValue(input.UserName, out captcha);
      //  //if(captcha == null)
      //  //{
      //  //    throw new UserFriendlyException("کپچا صحیح نمی باشد");
      //  //}
      //  //else
      //  //{
      //  //    if (input.cit != captcha as string)
      //  //    {
      //  //        await _cacheManager.GetCache("Redis").RemoveAsync(input.NationalCode);
      //  //        throw new UserFriendlyException("کپچا صحیح نمی باشد");
      //  //    }
      //  //}
      //  var useInquiryForUserAddress = _configuration.GetValue<bool?>("UseInquiryForUserAddress") ?? false;
      //  if (!string.IsNullOrWhiteSpace(input.Vin) &&
      //      !string.IsNullOrWhiteSpace(input.ChassiNo) &&
      //      !string.IsNullOrWhiteSpace(input.EngineNo) &&
      //      !string.IsNullOrWhiteSpace(input.Vehicle))
      //  { }
      //  else
      //  {
      //      throw new UserFriendlyException("لطفا در صورت داشتن خودرو فرسوده همه ی فیلد های مربوط به آن را وارد کنید");
      //  }

      //  if (_configuration.GetSection("IsIranCellActive").Value == "1")
      //  {
      //      _baseInformationService.RegistrationValidationWithoutCaptcha(new RegistrationValidationDto(input.NationalCode));
      //      // TODO: pending baseinformtion
      //      if (!input.BirthCityId.HasValue)
      //      {
      //          throw new UserFriendlyException("شهر محل تولد رو انتخاب نمایید");
      //      }
      //      if (input.BirthDate >= DateTime.Now)
      //          throw new UserFriendlyException("تارخ تولد نمیتواند با تاریخ جاری برابر یا بزرگتر باشد");
      //      if (input.IssuingDate >= DateTime.Now)
      //          throw new UserFriendlyException("تارخ صدور شناسنامه نمیتواند با تاریخ جاری برابر یا بزرگتر باشد");
      //      if (!input.IssuingCityId.HasValue)
      //      {
      //          throw new UserFriendlyException("شهر محل صدور شناسنامه رو انتخاب نمایید");
      //      }
      //      if (!input.HabitationCityId.HasValue)
      //      {
      //          throw new UserFriendlyException("شهر محل سکونت رو انتخاب نمایید");
      //      }
      //      if (!input.BirthProvinceId.HasValue)
      //      {
      //          throw new UserFriendlyException("استان محل تولد رو انتخاب نمایید");
      //      }
      //      if (!input.IssuingProvinceId.HasValue)
      //      {
      //          throw new UserFriendlyException("استان محل صدور شناسنامه رو انتخاب نمایید");
      //      }
      //      if (!input.HabitationProvinceId.HasValue)
      //      {
      //          throw new UserFriendlyException("استان محل سکونت رو انتخاب نمایید");
      //      }

      //      if (string.IsNullOrEmpty(input.PreTel))
      //      {
      //          throw new UserFriendlyException("پیش شماره تلفن را وارد نمایید");

      //      }
      //      if (string.IsNullOrEmpty(input.Street))
      //      {
      //          throw new UserFriendlyException("خیابان را وارد نمایید");

      //      }
      //      if (string.IsNullOrEmpty(input.Pelaq))
      //      {
      //          throw new UserFriendlyException("پلاک را وارد نمایید");

      //      }
      //      if (DateTime.Now.Subtract(input.BirthDate).TotalDays > 73200)
      //      {
      //          throw new UserFriendlyException("تاریخ تولد صحیح نمی باشد");
      //      }
      //      if (DateTime.Now.Subtract(input.IssuingDate.Value).TotalDays > 73200)
      //      {
      //          throw new UserFriendlyException("تاریخ صدور شناسنامه صحیح نمی باشد");

      //      }

      //      if (string.IsNullOrEmpty(input.Alley))
      //      {
      //          throw new UserFriendlyException("کوچه را وارد نمایید");
      //      }
      //      if (!input.IssuingDate.HasValue)
      //      {
      //          throw new UserFriendlyException("تاریخ صدور شناسنامه را وارد نمایید");
      //      }
      //      if (!input.RegionId.HasValue)
      //      {
      //          throw new UserFriendlyException("کد منطقه را وارد نمایید");
      //      }
      //      if (string.IsNullOrWhiteSpace(input.NationalCode) || input.NationalCode.Length > 10)
      //      {
      //          throw new UserFriendlyException("کد ملی خالی است یا محدودیت تعداد کارکتر را نقض کرده است");
      //      }
      //      if (string.IsNullOrWhiteSpace(input.FatherName) || input.FatherName.Length > 150)
      //      {
      //          throw new UserFriendlyException("نام پدر خالی است یا محدودیت تعداد کارکتر را نقض کرده است");
      //      }
      //      if (string.IsNullOrWhiteSpace(input.BirthCertId) || input.BirthCertId.Length > 11)
      //      {
      //          throw new UserFriendlyException("شناسه محل تولد خالی است یا محدودیت تعداد کارکتر را نقض کرده است");
      //      }
      //      if (string.IsNullOrWhiteSpace(input.PostalCode) || input.PostalCode.Length > 10)
      //      {
      //          throw new UserFriendlyException("کد پستی خالی است یا محدودیت تعداد کارکتر را نقض کرده است");
      //      }
      //      if (string.IsNullOrWhiteSpace(input.Mobile) || input.Mobile.Length > 11)
      //      {
      //          throw new UserFriendlyException("شماره موبایل خالی است یا محدودیت تعداد کارکتر را نقض کرده است");
      //      }
      //      if (string.IsNullOrWhiteSpace(input.Tel) || input.Tel.Length > 11)
      //      {
      //          throw new UserFriendlyException("شماره تلفن خالی است یا محدودیت تعداد کارکتر را نقض کرده است");
      //      }
      //      if (!useInquiryForUserAddress && (string.IsNullOrWhiteSpace(input.Address) || input.Address.Length > 255))
      //      {
      //          throw new UserFriendlyException("آدرس خالی است یا محدودیت تعداد کارکتر را نقض کرده است");
      //      }
      //      if (string.IsNullOrWhiteSpace(input.PreTel) || input.PreTel.Length > 6)
      //      {
      //          throw new UserFriendlyException("پیش شماره تلفن خالی است یا محدودیت تعداد کارکتر را نقض کرده است");
      //      }
      //      if (!useInquiryForUserAddress && string.IsNullOrWhiteSpace(input.Street) || input.Street.Length > 100)
      //      {
      //          throw new UserFriendlyException("نام خیابان خالی است یا محدودیت تعداد کارکتر را نقض کرده است");
      //      }
      //      if (!useInquiryForUserAddress && string.IsNullOrWhiteSpace(input.Pelaq) || input.Pelaq.Length > 10)
      //      {
      //          throw new UserFriendlyException("شماره پلاک خالی است یا محدودیت تعداد کارکتر را نقض کرده است");
      //      }
      //      if (!useInquiryForUserAddress && string.IsNullOrWhiteSpace(input.Alley) || input.Alley.Length > 100)
      //      {
      //          throw new UserFriendlyException("نام کوچه خالی است یا محدودیت تعداد کارکتر را نقض کرده است");
      //      }
      //      if (!string.IsNullOrWhiteSpace(input.EngineNo) && input.EngineNo.Length > 20)
      //          throw new UserFriendlyException("فرمت شماره موتور صحیح نیست");
      //      if (!string.IsNullOrWhiteSpace(input.ChassiNo) && input.ChassiNo.Length > 20)
      //          throw new UserFriendlyException("فرمت شماره شاسی صحیح نیست");
      //      //if (!string.IsNullOrWhiteSpace(input.Vehicle))
      //      //    throw new UserFriendlyException("نام خودرو به درستی وارد نشده است");
      //      if (_configuration.GetSection("IsCheckAdvocacy").Value == "1")
      //      {
      //          var advocacyuser = _bankAppService.CheckAdvocacy(input.NationalCode);
      //          input.Shaba = advocacyuser.ShebaNumber;
      //          input.AccountNumber = advocacyuser.AccountNumber;
      //          input.BankId = (int)advocacyuser.BankId;
      //      }
      //      else
      //      {
      //          input.Shaba = "...";
      //          input.AccountNumber = "...";
      //          input.BankId = 3;
      //      }

      //      //if (!ValidationHelper.IsShaba(input.Shaba))
      //      //{
      //      //    throw new UserFriendlyException(Messages.ShabaNotValid);
      //      //}
      //  }
      //  else
      //  {

      //      if (_configuration.GetSection("IsRecaptchaEnabled").Value == "1")
      //      {
      //          var response = await _commonAppService.CheckCaptcha(new CaptchaInputDto(input.ck, "CreateUser"));
      //          if (response.Success == false)
      //          {
      //              throw new UserFriendlyException("خطای کپچا");
      //          }

      //      }
      //      input.Shaba = input.Shaba.Replace(" ", "");
      //      if (!ValidationHelper.IsShaba(input.Shaba))
      //      {
      //          throw new UserFriendlyException(Messages.ShabaNotValid);
      //      }
      //      if (_configuration.GetValue<bool?>("AccountNumberIsRequired") ?? false && string.IsNullOrEmpty(input.AccountNumber))
      //      {
      //          throw new UserFriendlyException("شماره حساب وارد نشده است");

      //      }
      //      if (input.BankId <= 0)
      //      {
      //          throw new UserFriendlyException("لطفا بانک را انتخاب نمایید.");
      //      }

      //      if (_configuration.GetSection("IsRecaptchaEnabled").Value == "1")
      //      {
      //          var response = await _commonAppService.CheckCaptcha(new CaptchaInputDto(input.ck, "CreateUser"));
      //          if (response.Success == false)
      //          {
      //              throw new UserFriendlyException("خطای کپچا");
      //          }

      //      }

      //  }

      //  var hasNumber = new Regex(@"[0-9]+");
      //  var hasUpperChar = new Regex(@"[A-Z]+");
      //  var hasLowerChar = new Regex(@"[a-z]+");
      //  var hasSymbols = new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|./?,-]");

      //  if (!hasLowerChar.IsMatch(input.Password))
      //  {
      //      throw new UserFriendlyException("ساختار کلمه عبور صحیح نمی باشد");
      //  }
      //  else if (!hasUpperChar.IsMatch(input.Password))
      //  {
      //      throw new UserFriendlyException("ساختار کلمه عبور صحیح نمی باشد");
      //  }

      //  else if (!hasNumber.IsMatch(input.Password))
      //  {
      //      throw new UserFriendlyException("ساختار کلمه عبور صحیح نمی باشد");
      //  }

      //  else if (!hasSymbols.IsMatch(input.Password))
      //  {
      //      throw new UserFriendlyException("ساختار کلمه عبور صحیح نمی باشد");
      //  }





      //  if (!ValidationHelper.IsNationalCode(input.NationalCode))
      //  {
      //      throw new UserFriendlyException(Messages.NationalCodeNotValid);
      //  }
      //  var useShahkarInquiry = _configuration.GetValue<bool?>("UseShahkarInquiryInRegister") ?? false;
      //  var shahkarResult = useShahkarInquiry
      //      ? await _commonAppService.ValidateMobileNumber(input.NationalCode, input.Mobile)
      //      : false;
      //  if (useShahkarInquiry && !shahkarResult)
      //  {
      //      throw new UserFriendlyException("شماره موبایل به این کد ملی تعلق ندارد");
      //  }

      //  await _commonAppService.ValidateSMS(input.Mobile, input.NationalCode, input.SMSCode, SMSType.Register);
      //  //object userObject = null;
      //  //_cacheManager.GetCache("RegisterdUsers").TryGetValue(input.UserName, out userObject);
      //  //_distributedCache.GetAsync("")
      ////  User userFromCache = null;


      //  //User userFromCache = await _cacheManager.GetCache("RegisterdUsers").GetAsync(input.UserName
      //  //    , async async =>
      //  //    {
      //  //        var user = await _userRepository.FirstOrDefaultAsync(x => x.UserName == input.UserName);
      //  //        if (user == null)
      //  //        {
      //  //            return "";
      //  //        }
      //  //        else
      //  //        {
      //  //            return Task.FromResult(user) as object;
      //  //        }
      //  //    }) as User;

      //  //if (userFromCache == null)
      //  {
      //      var user = ObjectMapper.Map<CreateUserDto,UserMongo>(input);
      //      user.IsActive = true;
      //      //user.TenantId = CurrentTenant.Id;
      //      user.IsEmailConfirmed = true;
      //      user.EmailAddress = input.NationalCode + "@fava.com";
      //      //user.NormalizedEmailAddress = user.EmailAddress.ToUpper();
      //      user.UserName = input.NationalCode;
      //      user.NormalizedUserName = user.UserName.ToUpper();
      //      user.ConcurrencyStamp = null;
      //      user.UID = Guid.NewGuid().ToString().ToLower();
      //      user.IsDeleted = false;
      //      user.CreationTime = DateTime.Now;
      //      user.Password = _passwordHasher.HashPassword(new User(), input.Password);
      //      List<string> lsRols = new List<string>();
      //      lsRols.Add("Customer");
      //      user.RolesM = lsRols;
      //      //_userManager.InitializeOptions(AbpSession.TenantId);
      //      user.Address = useInquiryForUserAddress
      //          ? user.Address = await _commonAppService.GetAddressByZipCode(user.PostalCode, user.NationalCode)
      //          : user.Address = input.Address;
      //      try
      //      {
      //          user._Id = ObjectId.GenerateNewId().ToString();
      //          await(await _userMongoService.GetUserCollection()).InsertOneAsync(user);
      //      }
      //      catch (Exception ex)
      //      {
      //          var sqlex = ex.InnerException as SqlException;
      //          int errorCode = 0;
      //          if (sqlex != null)
      //              errorCode = sqlex.Number;
      //          switch (errorCode)
      //          {
      //              case 2601://duplicate
      //                  throw new UserFriendlyException(Messages.NationalCodeExists);
      //              default:
      //                  throw ex;
      //          }
      //      }
      //      //   _cacheManager.GetCache("RegisterdUsers").Set(input.UserName, user);
      //  }
      //  //if (userFromCache != null)
      //  //{
      //  //    throw new UserFriendlyException("شما قبلا ثبت نام نموده اید");
      //  //}

        return null;
    }




    public async Task<User> GetLoginInfromationuserFromCache(string Username)
    {
        Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
        object UserFromCache = null;

        var user = (await _userMongoRepository
                .GetQueryableAsync())
                .Where(x => x.NormalizedUserName == Username.ToUpper()
                  )
                .Select(x => new User
                {
                    TempUID = x.UID,
                    UserName = x.UserName,
                    Password = x.Password,
                    IsActive = x.IsActive,
                    RolesM = x.Roles,
                    NormalizedUserName = x.NormalizedUserName
                })
                .FirstOrDefault();
        if (user != null)
        {
            user.UID = new Guid(user.TempUID);
        }
        return user;
    }
}
