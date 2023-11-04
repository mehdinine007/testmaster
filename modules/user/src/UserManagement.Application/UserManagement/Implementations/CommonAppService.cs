#region NS
using IFG.Core.Caching;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Security.Claims;
using UserManagement.Application.Contracts.Models;
using UserManagement.Application.Contracts.Services;
using UserManagement.Application.InquiryService;
using UserManagement.Domain.Shared;
using Volo.Abp;
using Volo.Abp.Application.Services;
#endregion

namespace UserManagement.Application.UserManagement.Implementations;

public class CommonAppService : ApplicationService, ICommonAppService
{
    private readonly IConfiguration _configuration;
    private readonly IDistributedCache _distributedCache;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ICacheManager _cacheManager;

    public CommonAppService(IConfiguration configuration,
                            IDistributedCache distributedCache,
                            IHttpContextAccessor httpContextAccessor,
                            ICacheManager cacheManager)
    {
        _configuration = configuration;
        _distributedCache = distributedCache;
        _httpContextAccessor = httpContextAccessor;
        _cacheManager = cacheManager;
    }

    public Guid GetUID()
    {
        var userIdStr = _httpContextAccessor.HttpContext.User.Claims.SingleOrDefault(x => x.Type.Equals("UBP"))?.Value.ToUpper() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(userIdStr))
            throw new UserFriendlyException("لطفا لاگین کنید");

        Guid.TryParse(userIdStr, out var userId);
        return userId;
    }

    public async Task<string> GetAddressByZipCode(string zipCode, string nationalCode)
    {
        if (zipCode.Length != 10 || zipCode.AsParallel().Any(x => !char.IsDigit(x)))
            throw new UserFriendlyException("کد پستی صحیح نیست");

        if (nationalCode.Length != 10 || nationalCode.AsParallel().Any(x => !char.IsDigit(x)))
            throw new UserFriendlyException("کد ملی صحیح نیست");

        const string cacheKey = "zip_{0}, ntnl_{1}";
        var zipCache = await _distributedCache.GetStringAsync(string.Format(cacheKey, zipCode, nationalCode));
        if (string.IsNullOrWhiteSpace(zipCache))
        {
            if (_configuration.GetValue<bool?>("Inquiry:FakeAddressInquiryMode") ?? false)
            {
                return "sample address to bypass inquiry for temporary usage";
            }
            var inquiryBuilder = new InquiryBuilder(new FaraBoomInquiryConfig()
            {
                BaseUrl = _configuration.GetValue<string>("Inquiry:FaraBoom:BaseUrl"),
                Headers = _configuration.GetSection("Inquiry:FaraBoom:Headers").Get<FaraBoomHeader>()
            });
            var inquiry = await inquiryBuilder.FaraBoom(zipCode);
            if (inquiry.Success)
            {
                var inquiryResponse = JsonConvert.DeserializeObject<FaraBoomZipCodeInquiryResponse>(inquiry.Data);
                await _distributedCache.SetStringAsync(string.Format(cacheKey, zipCode, nationalCode), inquiryResponse.Addresss);
                return inquiryResponse.Addresss;
            }
            throw new UserFriendlyException("استعلام کدپستی با خطا مواجه شد");
        }
        return zipCache;
    }

    public async Task<bool> ValidateMobileNumber(string nationalCode, string mobileNo)
    {
        if (nationalCode.Length != 10 || nationalCode.AsParallel().Any(x => !char.IsDigit(x)))
            throw new UserFriendlyException("کد ملی صحیح نیست");
        if (mobileNo.AsParallel().Any(x => !char.IsDigit(x)))
            throw new UserFriendlyException("شماره موبایل صحیح نیست");

        const string cacheKey = "mobileNo_{0}, ntnl{1}";
        var validationResultCache = await _distributedCache.GetStringAsync(string.Format(cacheKey, mobileNo, nationalCode));
        if (string.IsNullOrWhiteSpace(validationResultCache))
        {
            var inquiryBuilder = new InquiryBuilder(new FaraBoomInquiryConfig()
            {
                BaseUrl = _configuration.GetSection("Inquiry:FaraBoom:BaseUrl").Value,
                Headers = _configuration.GetSection("Inquiry:FaraBoom:Headers").Get<FaraBoomHeader>()
            });
            var inquiry = await inquiryBuilder.FaraBoom(mobileNo, nationalCode);
            if (inquiry.Success)
            {
                var inquiryResponse = JsonConvert.DeserializeObject<FaraBoomInquiryResponse>(inquiry.Data);
                await _distributedCache.SetStringAsync(string.Format(cacheKey, mobileNo, nationalCode), inquiryResponse.match.ToString());
                return inquiryResponse.match;
            }
            if (!inquiry.Success && inquiry.MessageId.Equals("200", StringComparison.InvariantCultureIgnoreCase))
            {
                var inquiryResponse = JsonConvert.DeserializeObject<FaraBoomInquiryResponse>(inquiry.Data);
                await _distributedCache.SetStringAsync(string.Format(cacheKey, mobileNo, nationalCode), inquiryResponse.match.ToString());
                return inquiryResponse.match;
            }
            var serializedException = JsonConvert.SerializeObject(inquiry);
            throw new UserFriendlyException("استعلام شاهکار با خطا مواجه شد");
        }
        if (!bool.TryParse(validationResultCache, out bool isValid))
        {
            _distributedCache.Remove(string.Format(cacheKey, mobileNo, nationalCode));
            return false;
        }
        return isValid;
    }

    public async Task<bool> ValidateSMS(string Mobile, string NationalCode, string UserSMSCode, SMSType sMSType)

    {
        Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
        // _cacheManager.GetCache("SMS").TryGetValue(sMSType.ToString() + Mobile + NationalCode, out ObjectSMSCode);
        var checkSmsValidation = _configuration.GetValue<bool?>("SMSValidation") ?? false;
        if (!checkSmsValidation)
            return true;

        //string ObjectSMSCode = RedisHelper.GetDatabase().StringGet(sMSType.ToString() + Mobile + NationalCode);
        //var ObjectSMSCode = await _distributedCache.GetStringAsync(sMSType.ToString() + Mobile + NationalCode);
        var ObjectSMSCode = await _cacheManager.GetStringAsync(Mobile + NationalCode, sMSType.ToString(), new() { Provider = CacheProviderEnum.Redis,RedisHash = false });


        if (ObjectSMSCode == null)
        {
            throw new UserFriendlyException("کد پیامک ارسالی صحیح نمی باشد");
        }
        var smsCodeDto = JsonConvert.DeserializeObject<RegistrationSMSDto>(ObjectSMSCode);
        if (smsCodeDto == null)
        {
            throw new UserFriendlyException("کد پیامک ارسالی صحیح نمی باشد");
        }
        if (string.IsNullOrEmpty(smsCodeDto.SMSCode))
        {
            throw new UserFriendlyException("کد پیامک ارسالی صحیح نمی باشد");
        }
        else
        {
            if (UserSMSCode != smsCodeDto.SMSCode)
            {
                throw new UserFriendlyException("کد پیامک ارسالی صحیح نمی باشد");
            }
        }
        return true;
    }

    public async Task ValidateVisualizeCaptcha(VisualCaptchaInput input)
    {
        if (_configuration.GetSection("IsVisualizeCaptchaEnabled").Value == "1")
        {
            //var httpContext = _contextAccessor.HttpContext;
            //var validatorService = httpContext.RequestServices.GetRequiredService<IDNTCaptchaValidatorService>();
            //if (!validatorService.HasRequestValidCaptchaEntry(Language.Persian, DisplayMode.SumOfTwoNumbers, input.CT, input.CIT, input.CK)) 
            //{
            //    throw new UserFriendlyException(Messages.CaptchaNotValid);
            //}
            string objectCaptcha = await _distributedCache.GetStringAsync("cap_" + input.CIT);
            await _distributedCache.RemoveAsync("cap_" + input.CIT);
            if (objectCaptcha == null)
            {
                throw new UserFriendlyException("کپچای وارد شده صحیح نمی باشد");
            }
            else if (objectCaptcha != input.CK)
            {
                throw new UserFriendlyException("کپچای وارد شده صحیح نمی باشد");
            }
        }
    }
    public bool IsInRole(string Role)
    {
        var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
        // Get the claims values
        var role = _httpContextAccessor.HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.Role)
                           .Select(c => c.Value).SingleOrDefault();
        return role == Role;
    }

    public string GetRole()
    {
        var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
        // Get the claims values
        var Role = _httpContextAccessor.HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.Role)
                           .Select(c => c.Value).SingleOrDefault();
        if (Role == null)
        {
            throw new UserFriendlyException("دسترسی کافی نمی باشد");
        }
        return Role;
    }
}
