#region NS
using Newtonsoft.Json;
using Esale.Share.Authorize;
using Abp.Dependency;
using Abp.Authorization;
using Abp.Domain.Uow;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Text.RegularExpressions;
using UserManagement.Application.Contracts.Models;
using UserManagement.Application.Contracts.Services;
using UserManagement.Application.Contracts.UserManagement.Services;
using UserManagement.Domain.Authorization.Users;
using UserManagement.Domain.Shared;
using UserManagement.Domain.UserManagement.bases;
using UserManagement.Domain.UserManagement.Advocacy;
using UserManagement.Domain.UserManagement.Authorization.Users;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.EventBus;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.ObjectMapping;
using WorkingWithMongoDB.WebAPI.Services;
using wsFava;
using UserManagement.Application.Contracts.UserManagement.Constant;
using UserManagement.Application.Contracts;
using IFG.Core.Caching;
using IFG.Core.Utility.Tools;
using UserManagement.Domain.UserManagement.Enums;
#endregion

namespace UserManagement.Application.UserManagement.Implementations;

public class UserAppService : ApplicationService, IUserAppService
{

    private readonly IRolePermissionService _rolePermissionService;
    private readonly IRepository<UserMongo, ObjectId> _userMongoRepository;
    private readonly IRepository<UserSQL, long> _userSQLRepository;
    private readonly IConfiguration _configuration;
    private readonly IBankAppService _bankAppService;
    private readonly ICommonAppService _commonAppService;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly IBaseInformationService _baseInformationService;
    private readonly ICaptchaService _captchaService;
    private readonly IRepository<UserMongoWrite, ObjectId> _userMongoWriteRepository;
    private readonly IRepository<PermissionDefinitionWrite, ObjectId> _permissionDefinationRepository;
    private readonly IDistributedEventBus _distributedEventBus;
    private readonly ICacheManager _cacheManager;


    public UserAppService(IConfiguration configuration,
                          IBankAppService bankAppService,
                          ICommonAppService commonAppService,
                          IPasswordHasher<User> passwordHasher,
                          IBaseInformationService baseInformationService,
                          IRolePermissionService rolePermissionService,
                          IRepository<UserMongo, ObjectId> userMongoRepository,
                          IRepository<UserMongoWrite, ObjectId> userMongoWriteRepository,
                          ICaptchaService captchaService,
                          IDistributedEventBus distributedEventBus,
                          IRepository<UserSQL, long> UserSQLRepository,
                          IRepository<PermissionDefinitionWrite, ObjectId> permissionDefinationRepository,
                          ICacheManager cacheManager)
    {
        _rolePermissionService = rolePermissionService;
        _userMongoRepository = userMongoRepository;
        _configuration = configuration;
        _bankAppService = bankAppService;
        _commonAppService = commonAppService;
        _passwordHasher = passwordHasher;
        _baseInformationService = baseInformationService;
        _userMongoWriteRepository = userMongoWriteRepository;
        _captchaService = captchaService;
        _distributedEventBus = distributedEventBus;
        _userSQLRepository = UserSQLRepository;
        _permissionDefinationRepository = permissionDefinationRepository;
        _cacheManager = cacheManager;
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
        await _userMongoWriteRepository.UpdateAsync(ObjectMapper.Map<UserMongo, UserMongoWrite>(user));
        return true;
    }

    public class StockCountChangedEto
    {
        public Guid ProductId { get; set; }

        public int NewCount { get; set; }
    }

    //public class MyHandler
    //  : IDistributedEventHandler<StockCountChangedEto>,
    //    ITransientDependency
    //{
    //    public async Task HandleEventAsync(StockCountChangedEto eventData)
    //    {
    //        var productId = eventData.ProductId;
    //    }
    //}

    public async Task UpsertUserIntoSqlServer(UserSQL input)
    {
        if (input.EditMode == true)
        {
            var iqUser = await _userSQLRepository.GetQueryableAsync();

            var user = iqUser.AsNoTracking()
                .Select(x => new { x.Id, x.UID }).FirstOrDefault(x => x.UID == input.UID);
            if (user == null)
            {
                throw new UserFriendlyException("کاربر وجود ندارد:" + input.UID);
            }
            input.SetId(user.Id);
            await _userSQLRepository.UpdateAsync(input);
        }
        else
        {
            await _userSQLRepository.InsertAsync(input);

        }
        // await CurrentUnitOfWork.CompleteAsync();
    }
    public async Task<UserDto> CreateAsync(CreateUserDto input)
    {



        if (!string.IsNullOrEmpty(_configuration.GetSection("CloseRegisterDate").Value)
            && DateTime.Now > DateTime.Parse(_configuration.GetSection("CloseRegisterDate").Value))
        {
            throw new UserFriendlyException("زمان ثبت نام به پایان رسیده است");
        }
        //   _baseInformationService.RegistrationValidation()
        //Logs logs = new Logs();
        //logs.StartDate = DateTime.Now;
        object captcha = null;
        //_cacheManager.GetCache("Redis").TryGetValue(input.UserName, out captcha);
        //if(captcha == null)
        //{
        //    throw new UserFriendlyException("کپچا صحیح نمی باشد");
        //}
        //else
        //{
        //    if (input.cit != captcha as string)
        //    {
        //        await _cacheManager.GetCache("Redis").RemoveAsync(input.NationalCode);
        //        throw new UserFriendlyException("کپچا صحیح نمی باشد");
        //    }
        //}
        var useInquiryForUserAddress = _configuration.GetValue<bool?>("Inquiry:UseInquiryForUserAddress") ?? false;
        //if (!string.IsNullOrWhiteSpace(input.Vin) &&
        //    !string.IsNullOrWhiteSpace(input.ChassiNo) &&
        //    !string.IsNullOrWhiteSpace(input.EngineNo) &&
        //    !string.IsNullOrWhiteSpace(input.Vehicle))
        //{ }
        //else
        //{
        //    throw new UserFriendlyException("لطفا در صورت داشتن خودرو فرسوده همه ی فیلد های مربوط به آن را وارد کنید");
        //}

        if (_configuration.GetSection("IsIranCellActive").Value == "1")
        {
            await _baseInformationService.RegistrationValidationWithoutCaptcha(new RegistrationValidationDto()
            {
                Nationalcode = input.NationalCode
            });
            // TODO: pending baseinformtion
            if (!input.BirthCityId.HasValue)
            {
                throw new UserFriendlyException("شهر محل تولد رو انتخاب نمایید");
            }
            if (input.BirthDate >= DateTime.Now)
                throw new UserFriendlyException("تاریخ تولد نمیتواند با تاریخ جاری برابر یا بزرگتر باشد");
            if (input.IssuingDate >= DateTime.Now)
                throw new UserFriendlyException("تاریخ صدور شناسنامه نمیتواند با تاریخ جاری برابر یا بزرگتر باشد");
            if (input.BirthDate > input.IssuingDate)
                throw new UserFriendlyException("تاریخ تولد نمیتواند با صدور شناسنامه بزرگتر باشد");

            if (!EnumExtension.GetEnumValuesAndDescriptions<Gender>().Any(x => x.Key == (int)input.Gender))
            {
                throw new UserFriendlyException("جنست انتخاب شده،درست نمی باشد");
            }
            if (!input.IssuingCityId.HasValue)
            {
                throw new UserFriendlyException("شهر محل صدور شناسنامه رو انتخاب نمایید");
            }
            if (!input.HabitationCityId.HasValue)
            {
                throw new UserFriendlyException("شهر محل سکونت رو انتخاب نمایید");
            }
            if (!input.BirthProvinceId.HasValue)
            {
                throw new UserFriendlyException("استان محل تولد رو انتخاب نمایید");
            }
            if (!input.IssuingProvinceId.HasValue)
            {
                throw new UserFriendlyException("استان محل صدور شناسنامه رو انتخاب نمایید");
            }
            if (!input.HabitationProvinceId.HasValue)
            {
                throw new UserFriendlyException("استان محل سکونت رو انتخاب نمایید");
            }

            if (string.IsNullOrEmpty(input.PreTel))
            {
                throw new UserFriendlyException("پیش شماره تلفن را وارد نمایید");

            }
            if (DateTime.Now.Subtract(input.BirthDate).TotalDays > 73200)
            {
                throw new UserFriendlyException("تاریخ تولد صحیح نمی باشد");
            }
            if (DateTime.Now.Subtract(input.IssuingDate.Value).TotalDays > 73200)
            {
                throw new UserFriendlyException("تاریخ صدور شناسنامه صحیح نمی باشد");

            }
            if (!input.IssuingDate.HasValue)
            {
                throw new UserFriendlyException("تاریخ صدور شناسنامه را وارد نمایید");
            }
            if (!input.RegionId.HasValue)
            {
                throw new UserFriendlyException("کد منطقه را وارد نمایید");
            }
            if (string.IsNullOrWhiteSpace(input.NationalCode) || input.NationalCode.Length > 10)
            {
                throw new UserFriendlyException("کد ملی خالی است یا محدودیت تعداد کارکتر را نقض کرده است");
            }
            if (string.IsNullOrWhiteSpace(input.FatherName) || input.FatherName.Length > 150)
            {
                throw new UserFriendlyException("نام پدر خالی است یا محدودیت تعداد کارکتر را نقض کرده است");
            }
            if (string.IsNullOrWhiteSpace(input.BirthCertId) || input.BirthCertId.Length > 11)
            {
                throw new UserFriendlyException("شناسه محل تولد خالی است یا محدودیت تعداد کارکتر را نقض کرده است");
            }
            if (string.IsNullOrWhiteSpace(input.PostalCode) || input.PostalCode.Length != 10)
            {
                throw new UserFriendlyException("کد پستی خالی است یا محدودیت تعداد کارکتر را نقض کرده است");
            }

            var mob = new Regex(@"^09[0-39][0-9]{8}$");
            if (string.IsNullOrWhiteSpace(input.Mobile) || !mob.IsMatch(input.Mobile)) //(input.Mobile.Length != 11 && input.Mobile.StartsWith("09")))
            {
                throw new UserFriendlyException("شماره موبایل خالی است یا محدودیت تعداد کارکتر را نقض کرده است");
            }
            if (string.IsNullOrWhiteSpace(input.Tel) || input.Tel.Length > 11)
            {
                throw new UserFriendlyException("شماره تلفن خالی است یا محدودیت تعداد کارکتر را نقض کرده است");
            }
            if (!useInquiryForUserAddress && (string.IsNullOrWhiteSpace(input.Address) || input.Address.Length > 255))
            {
                throw new UserFriendlyException("آدرس خالی است یا محدودیت تعداد کارکتر را نقض کرده است");
            }
            if (string.IsNullOrWhiteSpace(input.PreTel) || input.PreTel.Length > 6)
            {
                throw new UserFriendlyException("پیش شماره تلفن خالی است یا محدودیت تعداد کارکتر را نقض کرده است");
            }
            if (!useInquiryForUserAddress && string.IsNullOrWhiteSpace(input.Street) || input.Street.Length > 100)
            {
                throw new UserFriendlyException("نام خیابان خالی است یا محدودیت تعداد کارکتر را نقض کرده است");
            }
            if (!useInquiryForUserAddress && string.IsNullOrWhiteSpace(input.Pelaq) || input.Pelaq.Length > 10)
            {
                throw new UserFriendlyException("شماره پلاک خالی است یا محدودیت تعداد کارکتر را نقض کرده است");
            }
            if (!useInquiryForUserAddress && string.IsNullOrWhiteSpace(input.Alley) || input.Alley.Length > 100)
            {
                throw new UserFriendlyException("نام کوچه خالی است یا محدودیت تعداد کارکتر را نقض کرده است");
            }
            if (!string.IsNullOrWhiteSpace(input.EngineNo) && input.EngineNo.Length > 20)
                throw new UserFriendlyException("فرمت شماره موتور صحیح نیست");
            if (!string.IsNullOrWhiteSpace(input.ChassiNo) && input.ChassiNo.Length > 20)
                throw new UserFriendlyException("فرمت شماره شاسی صحیح نیست");
            //if (!string.IsNullOrWhiteSpace(input.Vehicle))
            //    throw new UserFriendlyException("نام خودرو به درستی وارد نشده است");
            if (_configuration.GetSection("IsCheckAdvocacy").Value == "1")
            {
                var advocacyuser = _bankAppService.CheckAdvocacy(input.NationalCode);
                input.Shaba = advocacyuser.ShebaNumber;
                input.AccountNumber = advocacyuser.AccountNumber;
                input.BankId = (int)advocacyuser.BankId;
            }
            else
            {
                input.Shaba = "...";
                input.AccountNumber = "...";
                input.BankId = null;
            }

            //if (!ValidationHelper.IsShaba(input.Shaba))
            //{
            //    throw new UserFriendlyException(Messages.ShabaNotValid);
            //}
        }
        else
        {

            if (_configuration.GetSection("IsRecaptchaEnabled").Value == "1")
            {
                var response = await _captchaService.ReCaptcha(new CaptchaInputDto(input.ck, "CreateUser"));
                if (response.Success == false)
                {
                    throw new UserFriendlyException("خطای کپچا");
                }

            }
            input.Shaba = input.Shaba.Replace(" ", "");
            if (!ValidationHelper.IsShaba(input.Shaba))
            {
                throw new UserFriendlyException(Messages.ShabaNotValid);
            }
            if (_configuration.GetValue<bool?>("AccountNumberIsRequired") ?? false && string.IsNullOrEmpty(input.AccountNumber))
            {
                throw new UserFriendlyException("شماره حساب وارد نشده است");

            }
            if (input.BankId <= 0)
            {
                throw new UserFriendlyException("لطفا بانک را انتخاب نمایید.");
            }

            if (_configuration.GetSection("IsRecaptchaEnabled").Value == "1")
            {
                var response = await _captchaService.ReCaptcha(new CaptchaInputDto(input.ck, "CreateUser"));
                if (response.Success == false)
                {
                    throw new UserFriendlyException("خطای کپچا");
                }

            }

        }

        var hasNumber = new Regex(@"[0-9]+");
        var hasUpperChar = new Regex(@"[A-Z]+");
        var hasLowerChar = new Regex(@"[a-z]+");
        var hasSymbols = new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|./?,-]");

        if (!hasNumber.IsMatch(input.PostalCode))
        {
            throw new UserFriendlyException("ساختار کدپستی صحیح نمی باشد");
        }


        if (!hasLowerChar.IsMatch(input.Password))
        {
            throw new UserFriendlyException("ساختار کلمه عبور صحیح نمی باشد");
        }
        else if (!hasUpperChar.IsMatch(input.Password))
        {
            throw new UserFriendlyException("ساختار کلمه عبور صحیح نمی باشد");
        }

        else if (!hasNumber.IsMatch(input.Password))
        {
            throw new UserFriendlyException("ساختار کلمه عبور صحیح نمی باشد");
        }

        else if (!hasSymbols.IsMatch(input.Password))
        {
            throw new UserFriendlyException("ساختار کلمه عبور صحیح نمی باشد");
        }

        if (!ValidationHelper.IsNationalCode(input.NationalCode))
        {
            throw new UserFriendlyException(Messages.NationalCodeNotValid);
        }
        var useShahkarInquiry = _configuration.GetValue<bool?>("Inquiry:UseShahkarInquiryInRegister") ?? false;
        var shahkarResult = useShahkarInquiry
            ? await _commonAppService.ValidateMobileNumber(input.NationalCode, input.Mobile)
            : false;
        if (useShahkarInquiry && !shahkarResult)
        {
            throw new UserFriendlyException("شماره موبایل به این کد ملی تعلق ندارد");
        }

        await _commonAppService.ValidateSMS(input.Mobile, input.NationalCode, input.SMSCode, SMSType.Register);
        //object userObject = null;
        //_cacheManager.GetCache("RegisterdUsers").TryGetValue(input.UserName, out userObject);
        //_distributedCache.GetAsync("")
        //  User userFromCache = null;


        //User userFromCache = await _cacheManager.GetCache("RegisterdUsers").GetAsync(input.UserName
        //    , async async =>
        //    {
        //        var user = await _userRepository.FirstOrDefaultAsync(x => x.UserName == input.UserName);
        //        if (user == null)
        //        {
        //            return "";
        //        }
        //        else
        //        {
        //            return Task.FromResult(user) as object;
        //        }
        //    }) as User;

        //if (userFromCache == null)
        {

            var user = ObjectMapper.Map<CreateUserDto, UserMongo>(input);
            user.IsActive = true;
            //user.TenantId = CurrentTenant.Id;
            user.IsEmailConfirmed = true;
            user.EmailAddress = input.NationalCode + "@fava.com";
            //user.NormalizedEmailAddress = user.EmailAddress.ToUpper();
            user.UserName = input.NationalCode;
            user.NormalizedUserName = user.UserName.ToUpper();
            user.ConcurrencyStamp = null;
            user.UID = Guid.NewGuid().ToString().ToLower();
            user.IsDeleted = false;
            user.Password = _passwordHasher.HashPassword(new User(), input.Password);
            List<string> lsRols = new List<string>();
            var defultRoleCode = _configuration.GetValue<string>("CreateUser:RoleCode");
            if (string.IsNullOrWhiteSpace(defultRoleCode))
                throw new UserFriendlyException(UserMessageConstant.CreateUserDefultRoleCodeNotFound, UserMessageConstant.CreateUserDefultRoleCodeNotFoundId);
            lsRols.Add(defultRoleCode);
            user.Roles = lsRols;
            //_userManager.InitializeOptions(AbpSession.TenantId);
            user.Address = useInquiryForUserAddress
                ? user.Address = await _commonAppService.GetAddressByZipCode(user.PostalCode, user.NationalCode)
                : user.Address = input.Address;
            if (user.BankId == 0)
            {
                user.BankId = null;
            }
            try
            {
                //user._Id = ObjectId.GenerateNewId().ToString();
                await _userMongoWriteRepository.InsertAsync(ObjectMapper.Map<UserMongo, UserMongoWrite>(user));


                await _distributedEventBus.PublishAsync<UserSQL>(
                     ObjectMapper.Map<UserMongo, UserSQL>(user)
                    );
            }
            catch (Exception ex)
            {
                var sqlex = ex.InnerException as SqlException;
                int errorCode = 0;
                if (sqlex != null)
                    errorCode = sqlex.Number;
                switch (errorCode)
                {
                    case 2601://duplicate
                        throw new UserFriendlyException(Messages.NationalCodeExists);
                    default:
                        throw ex;
                }
            }
            //   _cacheManager.GetCache("RegisterdUsers").Set(input.UserName, user);
        }
        //if (userFromCache != null)
        //{
        //    throw new UserFriendlyException("شما قبلا ثبت نام نموده اید");
        //}

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
                    NormalizedUserName = x.NormalizedUserName,
                    CompanyId = x.CompanyId,
                })
                .FirstOrDefault();
        if (user != null)
        {
            user.UID = new Guid(user.TempUID);
        }
        return user;
    }

    [SecuredOperation(UserServicePermissionConstants.GetUserProfile)]
    public async Task<UserDto> GetUserProfile()
    {
        Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
        string prefix = $"{RedisConstants.GetUserProfile}";
        Guid userId = _commonAppService.GetUID();
        string cacheKey = userId.ToString();
        var cachedData = await _cacheManager.GetStringAsync(cacheKey, prefix, new CacheOptions
        { Provider = CacheProviderEnum.Hybrid });

        if (!string.IsNullOrEmpty(cachedData))
        {
            return JsonConvert.DeserializeObject<UserDto>(cachedData);
        }

        var user = (await _userMongoRepository.GetQueryableAsync())
        .FirstOrDefault(a => a.UID == userId.ToString().ToLower() && !a.IsDeleted);

        if (user != null)
        {
            await _cacheManager.SetStringAsync(cacheKey, prefix, JsonConvert.SerializeObject(user), new CacheOptions
            { Provider = CacheProviderEnum.Hybrid }, TimeSpan.FromMinutes(8).TotalSeconds);
            var userDto = ObjectMapper.Map<UserMongo, UserDto>(user);
            return userDto;
        }
        return null;
    }

    public async Task<bool> ForgotPassword(ForgetPasswordDto forgetPasswordDto)
    {
        Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");

        var hasNumber = new Regex(@"[0-9]+");
        var hasUpperChar = new Regex(@"[A-Z]+");
        var hasLowerChar = new Regex(@"[a-z]+");
        var hasSymbols = new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|./?,-]");

        if (!hasLowerChar.IsMatch(forgetPasswordDto.PassWord))
        {
            throw new UserFriendlyException("ساختار کلمه عبور صحیح نمی باشد");
        }
        else if (!hasUpperChar.IsMatch(forgetPasswordDto.PassWord))
        {
            throw new UserFriendlyException("ساختار کلمه عبور صحیح نمی باشد");

        }

        else if (!hasNumber.IsMatch(forgetPasswordDto.PassWord))
        {
            throw new UserFriendlyException("ساختار کلمه عبور صحیح نمی باشد");

        }

        else if (!hasSymbols.IsMatch(forgetPasswordDto.PassWord))
        {
            throw new UserFriendlyException("ساختار کلمه عبور صحیح نمی باشد");

        }


        await _commonAppService.ValidateSMS(forgetPasswordDto.Mobile, forgetPasswordDto.NationalCode, forgetPasswordDto.SMSCode, SMSType.ForgetPassword);
        var userFromDb = (await _userMongoRepository.GetQueryableAsync())
            .SingleOrDefault(x => x.NationalCode == forgetPasswordDto.NationalCode && x.IsDeleted == false);



        if (userFromDb == null || userFromDb.Mobile.Replace(" ", "") != forgetPasswordDto.Mobile)
        {
            throw new UserFriendlyException("کد ملی یا شماره موبایل صحیح نمی باشد");
        }
        userFromDb.Password = _passwordHasher.HashPassword(new User(), forgetPasswordDto.PassWord);
        var filter = Builders<UserMongo>.Filter.Where(_ => _.NationalCode == forgetPasswordDto.NationalCode && _.IsDeleted == false);
        var update = Builders<UserMongo>.Update.Set(_ => _.Password, userFromDb.Password)
            .Set(_ => _.LastModificationTime, DateTime.Now);

        (await _userMongoRepository.GetCollectionAsync())
            .UpdateOne(filter, update);
        var userSql = ObjectMapper.Map<UserMongo, UserSQL>(userFromDb);
        userSql.EditMode = true;
        await _distributedEventBus.PublishAsync<UserSQL>(
               userSql
               );

        return true;
    }

    [SecuredOperation(UserServicePermissionConstants.ChangePassword)]
    public async Task<bool> ChangePassword(ChangePasswordDto input)
    {
        Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");

        var hasNumber = new Regex(@"[0-9]+");
        var hasUpperChar = new Regex(@"[A-Z]+");
        var hasLowerChar = new Regex(@"[a-z]+");
        var hasSymbols = new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|./?,-]");

        if (!hasLowerChar.IsMatch(input.NewPassWord))
        {
            throw new UserFriendlyException("ساختار کلمه عبور صحیح نمی باشد");
        }
        else if (!hasUpperChar.IsMatch(input.NewPassWord))
        {
            throw new UserFriendlyException("ساختار کلمه عبور صحیح نمی باشد");

        }

        else if (!hasNumber.IsMatch(input.NewPassWord))
        {
            throw new UserFriendlyException("ساختار کلمه عبور صحیح نمی باشد");

        }

        else if (!hasSymbols.IsMatch(input.NewPassWord))
        {
            throw new UserFriendlyException("ساختار کلمه عبور صحیح نمی باشد");

        }
        string uid = _commonAppService.GetUID().ToString();

        if (uid == null)
        {
            throw new UserFriendlyException("خطایی پیش آمده است،لطفا با پشتیبانی تماس بگیرید. کد خطا 1");
        }

        UserMongo userFromDb = await (await _userMongoRepository.GetCollectionAsync())
       .Find(x => x.UID == uid
          && x.IsDeleted == false)
       .FirstOrDefaultAsync();

        await _commonAppService.ValidateSMS(userFromDb.Mobile, userFromDb.NationalCode, input.SMSCode, SMSType.ChangePassword);

        userFromDb.Password = _passwordHasher.HashPassword(new User(), input.NewPassWord);
        var filter = Builders<UserMongo>.Filter.Where(_ => _.UID == uid && _.IsDeleted == false);
        var update = Builders<UserMongo>.Update.Set(_ => _.Password, userFromDb.Password)
            .Set(_ => _.LastModificationTime, DateTime.Now);
        (await _userMongoRepository.GetCollectionAsync())
            .UpdateOne(filter, update);
        var userSql = ObjectMapper.Map<UserMongo, UserSQL>(userFromDb);
        userSql.EditMode = true;
        await _distributedEventBus.PublishAsync<UserSQL>(
                  userSql
                 );
        return true;

    }

    [SecuredOperation(UserServicePermissionConstants.UpdateUserProfile)]
    public async Task<bool> UpdateUserProfile(UserDto inputUser)
    {
        Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
        if (!string.IsNullOrEmpty(_configuration.GetSection("CloseUpdateProfile").Value)
           && DateTime.Now > DateTime.Parse(_configuration.GetSection("CloseUpdateProfile").Value))
        {
            throw new UserFriendlyException("زمان ویرایش اطلاعات به پایان رسیده است");
        }

        object captcha = null;
        if (_configuration.GetSection("IsRecaptchaEnabled").Value == "1")
        {
            //var response1 = await _commonAppService.CheckCaptcha(new CaptchaInputDto(inputUser.ck, "CreateUser"));
            var response = await _captchaService.ReCaptcha(new CaptchaInputDto(inputUser.ck, "CreateUser"));
            if (response.Success == false)
            {
                throw new UserFriendlyException("خطای کپچا");
            }
        }
        if (_configuration.GetSection("IsIranCellActive").Value == "0")
        {
            inputUser.Shaba = inputUser.Shaba.Replace(" ", "");
            if (!ValidationHelper.IsShaba(inputUser.Shaba))
            {
                throw new UserFriendlyException(Messages.ShabaNotValid);
            }
        }
        string prefix = $"{RedisConstants.GetUserProfile}";
        Guid userId = _commonAppService.GetUID();
        string cacheKey = userId.ToString();
        await _cacheManager.RemoveAsync(cacheKey, prefix, new CacheOptions
        { Provider = CacheProviderEnum.Hybrid });

        var user = await (await _userMongoRepository.GetCollectionAsync())
            .Find(x => x.UID == _commonAppService.GetUID().ToString().ToLower()
                && x.IsDeleted == false)
            .FirstOrDefaultAsync();

        var userFromDb = ObjectMapper.Map<UserMongo, UserMongoWrite>(user);
        if (userFromDb == null)
        {
            throw new UserFriendlyException("خطایی رخ داده است!");
        }

        if (userFromDb.Mobile != inputUser.Mobile)
        {
            await _commonAppService.ValidateSMS(inputUser.Mobile, inputUser.NationalCode, inputUser.smsCode, SMSType.UpdateProfile);
            userFromDb.Mobile = inputUser.Mobile;
        }

        var useInquiryForUserAddress = _configuration.GetValue<bool?>("Inquiry:UseInquiryForUserAddress") ?? false;

        if (_configuration.GetSection("IsIranCellActive").Value == "0")
        {
            userFromDb.Shaba = inputUser.Shaba;
            userFromDb.BankId = inputUser.BankId;
            userFromDb.AccountNumber = inputUser.AccountNumber;
        }
        else
        {
            if (!inputUser.BirthCityId.HasValue)
            {
                throw new UserFriendlyException("شهر محل تولد رو انتخاب نمایید");
            }
            if (inputUser.BirthDate >= DateTime.Now)
                throw new UserFriendlyException("تارخ تولد نمیتواند با تاریخ جاری برابر یا بزرگتر باشد");
            if (inputUser.IssuingDate >= DateTime.Now)
                throw new UserFriendlyException("تارخ صدور شناسنامه نمیتواند با تاریخ جاری برابر یا بزرگتر باشد");
            //var requiredBirthDate = DateTime.Now.AddYears(-18);
            //if (inputUser.BirthDate > requiredBirthDate)
            //    throw new UserFriendlyException("سن متقاضی باید بیش تر از 18 سال باشد");
            if (!EnumExtension.GetEnumValuesAndDescriptions<Gender>().Any(x => x.Key == (int)inputUser.Gender))
            {
                throw new UserFriendlyException("جنست انتخاب شده،درست نمی باشد");
            }
            if (!inputUser.IssuingCityId.HasValue)
            {
                throw new UserFriendlyException("شهر محل صدور شناسنامه رو انتخاب نمایید");
            }
            if (!inputUser.HabitationCityId.HasValue)
            {
                throw new UserFriendlyException("شهر محل سکونت رو انتخاب نمایید");
            }
            if (!inputUser.BirthProvinceId.HasValue)
            {
                throw new UserFriendlyException("استان محل تولد رو انتخاب نمایید");
            }
            if (!inputUser.IssuingProvinceId.HasValue)
            {
                throw new UserFriendlyException("استان محل صدور شناسنامه رو انتخاب نمایید");
            }
            if (!inputUser.HabitationProvinceId.HasValue)
            {
                throw new UserFriendlyException("استان محل سکونت رو انتخاب نمایید");
            }

            if (string.IsNullOrEmpty(inputUser.PreTel))
            {
                throw new UserFriendlyException("پیش شماره تلفن را وارد نمایید");

            }
            if (!inputUser.IssuingDate.HasValue)
            {
                throw new UserFriendlyException("تاریخ صدور شناسنامه را وارد نمایید");
            }
            if (!inputUser.RegionId.HasValue || inputUser.RegionId.Value == 0)
            {
                throw new UserFriendlyException("کد منطقه را وارد نمایید");
            }
            if (string.IsNullOrWhiteSpace(inputUser.NationalCode) || inputUser.NationalCode.Length > 10)
            {
                throw new UserFriendlyException("کد ملی خالی است یا محدودیت تعداد کارکتر را نقض کرده است");
            }
            if (string.IsNullOrWhiteSpace(inputUser.FatherName) || inputUser.FatherName.Length > 150)
            {
                throw new UserFriendlyException("نام پدر خالی است یا محدودیت تعداد کارکتر را نقض کرده است");
            }
            if (string.IsNullOrWhiteSpace(inputUser.BirthCertId) || inputUser.BirthCertId.Length > 11)
            {
                throw new UserFriendlyException("شناسه محل تولد خالی است یا محدودیت تعداد کارکتر را نقض کرده است");
            }
            if (string.IsNullOrWhiteSpace(inputUser.PostalCode) || inputUser.PostalCode.Length > 10)
            {
                throw new UserFriendlyException("کد پستی خالی است یا محدودیت تعداد کارکتر را نقض کرده است");
            }
            if (string.IsNullOrWhiteSpace(inputUser.Mobile) || inputUser.Mobile.Length > 11)
            {
                throw new UserFriendlyException("شماره موبایل خالی است یا محدودیت تعداد کارکتر را نقض کرده است");
            }
            if (string.IsNullOrWhiteSpace(inputUser.Tel) || inputUser.Tel.Length > 11)
            {
                throw new UserFriendlyException("شماره تلفن خالی است یا محدودیت تعداد کارکتر را نقض کرده است");
            }
            //if (string.IsNullOrWhiteSpace(inputUser.Address) || inputUser.Address.Length > 255)
            if (!useInquiryForUserAddress && (string.IsNullOrWhiteSpace(inputUser.Address) || inputUser.Address.Length > 255))
            {
                throw new UserFriendlyException("آدرس خالی است یا محدودیت تعداد کارکتر را نقض کرده است");
            }
            if (string.IsNullOrWhiteSpace(inputUser.PreTel) || inputUser.PreTel.Length > 6)
            {
                throw new UserFriendlyException("پیش شماره تلفن است یا محدودیت تعداد کارکتر را نقض کرده است");
            }
            if (!useInquiryForUserAddress && string.IsNullOrWhiteSpace(inputUser.Street) || inputUser.Street.Length > 100)
            {
                throw new UserFriendlyException("نام خیابان خالی است یا محدودیت تعداد کارکتر را نقض کرده است");
            }
            if (!useInquiryForUserAddress && string.IsNullOrWhiteSpace(inputUser.Pelaq) || inputUser.Pelaq.Length > 10)
            {
                throw new UserFriendlyException("شماره پلاک خالی است یا محدودیت تعداد کارکتر را نقض کرده است");
            }
            if (!useInquiryForUserAddress && string.IsNullOrWhiteSpace(inputUser.Alley) || inputUser.Alley.Length > 100)
            {
                throw new UserFriendlyException("نام کوچه خالی است یا محدودیت تعداد کارکتر را نقض کرده است");
            }
            if (!string.IsNullOrWhiteSpace(inputUser.EngineNo) && inputUser.EngineNo.Length > 20)
                throw new UserFriendlyException("فرمت شماره موتور صحیح نیست");
            if (!string.IsNullOrWhiteSpace(inputUser.ChassiNo) && inputUser.ChassiNo.Length > 20)
                throw new UserFriendlyException("فرمت شماره شاسی صحیح نیست");
            //if (!string.IsNullOrWhiteSpace(inputUser.Vehicle))
            //    throw new UserFriendlyException("نام خودرو به درستی وارد نشده است");
            var useShahkarInquiry = _configuration.GetValue<bool?>("Inquiry:UseShahkarInquiryInRegister") ?? false;
            var shahkarResult = useShahkarInquiry
            ? await _commonAppService.ValidateMobileNumber(inputUser.NationalCode, inputUser.Mobile)
            : false;

            if (useShahkarInquiry &&
            !string.Equals(userFromDb.Mobile, inputUser.Mobile, StringComparison.InvariantCultureIgnoreCase) &&
            !shahkarResult)
            {
                throw new UserFriendlyException("شماره موبایل به این کد ملی تعلق ندارد");
            }
            userFromDb.IssuingDate = inputUser.IssuingDate;
            userFromDb.Alley = inputUser.Alley;
            userFromDb.Street = inputUser.Street;
            userFromDb.RegionId = inputUser.RegionId;
            userFromDb.PreTel = inputUser.PreTel;
            userFromDb.Pelaq = inputUser.Pelaq;
            userFromDb.BirthCityId = inputUser.BirthCityId;
            userFromDb.IssuingCityId = inputUser.IssuingCityId;
            userFromDb.HabitationCityId = inputUser.HabitationCityId;
            userFromDb.BirthProvinceId = inputUser.BirthProvinceId;
            userFromDb.IssuingProvinceId = inputUser.IssuingProvinceId;
            userFromDb.HabitationProvinceId = inputUser.HabitationProvinceId;
            userFromDb.Vin = inputUser.Vin;
            userFromDb.EngineNo = inputUser.EngineNo;
            userFromDb.ChassiNo = inputUser.ChassiNo;
            userFromDb.Vehicle = inputUser.Vehicle;
        }

        userFromDb.Tel = inputUser.Tel;
        userFromDb.PostalCode = inputUser.PostalCode;
        userFromDb.Gender = inputUser.Gender;
        userFromDb.FatherName = inputUser.FatherName;
        userFromDb.Name = inputUser.Name;
        userFromDb.Surname = inputUser.Surname;
        userFromDb.BirthCertId = inputUser.BirthCertId;
        userFromDb.BirthDate = inputUser.BirthDate;

        userFromDb.Address = useInquiryForUserAddress
            ? userFromDb.Address = await _commonAppService.GetAddressByZipCode(userFromDb.PostalCode, userFromDb.NationalCode)
            : userFromDb.Address = inputUser.Address;
        userFromDb.LastModifierId = _commonAppService.GetUID();
        userFromDb.LastModificationTime = DateTime.Now;

        var filter = Builders<UserMongoWrite>.Filter.Eq("UID", userFromDb.UID);
        await (await _userMongoWriteRepository.GetCollectionAsync()).ReplaceOneAsync(filter, userFromDb);
        var userSql = ObjectMapper.Map<UserMongo, UserSQL>(userFromDb);
        await _distributedEventBus.PublishAsync<UserSQL>(
                  userSql
                 );
        return true;
    }
}
