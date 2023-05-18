using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using OrderManagement.Application.Contracts;
using OrderManagement.Application.Contracts.Services;
using OrderManagement.Application.OrderManagement.Constants;
using OrderManagement.Application.OrderManagement.Utitlities;
using OrderManagement.Domain;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace OrderManagement.Application.OrderManagement.Implementations;

public class CommonAppService : ApplicationService, ICommonAppService
{
    private readonly IDistributedCache _distributedCache;
    private IConfiguration _configuration { get; set; }
    private readonly IRepository<Logs, long> _logsRespository;
    private readonly IRepository<UserRejectionAdvocacy, int> _userRejectionAdcocacyRepository;
    private IHttpContextAccessor _httpContextAccessor;
    private readonly IRepository<ExternalApiLogResult, int> _externalApiLogResultRepository;
    private readonly IRepository<ExternalApiResponsLog, int> _externalApiResponsLogRepository;
    private readonly IHttpContextAccessor _contextAccessor;

    public CommonAppService(IDistributedCache distributedCache,
                            IConfiguration configuration,
                            IRepository<Logs, long> LogsRespository,
                            IRepository<UserRejectionAdvocacy, int> UserRejectionAdcocacyRepository,
                            IHttpContextAccessor HttpContextAccessor,
                            IRepository<ExternalApiLogResult, int> externalApiLogResultRepository,
                            IRepository<ExternalApiResponsLog, int> externalApiResponsLogRepositor,
                            IHttpContextAccessor contextAccessor
        )
    {
        _distributedCache = distributedCache;
        _configuration = configuration;
        _logsRespository = LogsRespository;
        _userRejectionAdcocacyRepository = UserRejectionAdcocacyRepository;
        _httpContextAccessor = HttpContextAccessor;
        _externalApiLogResultRepository = externalApiLogResultRepository;
        _externalApiResponsLogRepository = externalApiResponsLogRepositor;
        _contextAccessor = contextAccessor;
    }

    //private async Task<AuthtenticateResult> AuthenticateBank()
    //{
    //    Logs logs = new Logs();
    //    logs.StartDate = DateTime.Now;
    //    try
    //    {

    //        logs.Ip = "";
    //        logs.Type = 1;
    //        logs.EndDate = DateTime.Now;
    //        logs.Method = "TejaratAuth";
    //        logs.LocationId = 3;
    //        logs.Servername = Utility.GetServerIPAddress();
    //        var authenticateResult = await _cacheManager.GetCache("AuthenticateTejarat").GetAsync("AuthenticateTejarat"
    //       , async async =>
    //       {
    //           var options = new RestClientOptions("https://ws.farava.ir")
    //           {
    //               MaxTimeout = -1,
    //           };
    //           var client = new RestClient(options);
    //           var request = new RestRequest("/api/Account/Authenticate?userName=" + _configuration.GetSection("TejaratUser").Value + "&password=" + _configuration.GetSection("TejaratPassword").Value, Method.Post);
    //           request.AddHeader("accept", "application/json");
    //           request.AddHeader("content-type", "application/json");
    //           RestResponse response = await client.ExecuteAsync(request);
    //           AuthtenticateResult authenticateResult = JsonConvert.DeserializeObject<AuthtenticateResult>(response.Content);
    //           logs.EndDate = DateTime.Now;
    //           return authenticateResult;
    //       }) as AuthtenticateResult;
    //        logs.EndDate = DateTime.Now;
    //        await _logsRespository.InsertAsync(logs);
    //        return authenticateResult;

    //    }
    //    catch (Exception ex)
    //    {
    //        logs.EndDate = DateTime.Now;
    //        logs.Type = 0;
    //        logs.Message = ex.Message;
    //        UnitOfWorkOptions unitOfWorkOptions = new UnitOfWorkOptions();
    //        unitOfWorkOptions.IsTransactional = false;
    //        unitOfWorkOptions.Scope = System.Transactions.TransactionScopeOption.RequiresNew;
    //        using (var unitOfWork = _unitOfWorkManager.Begin(unitOfWorkOptions))
    //        {
    //            await _logsRespository.InsertAsync(logs);
    //            unitOfWork.Complete();
    //        }

    //    }
    //    return null;

    //}
    public bool IsInRole(string Role)
    {
        var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
        // Get the claims values
        var role = _httpContextAccessor.HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.Role)
                           .Select(c => c.Value).SingleOrDefault();
        return role == Role;
    }
    public string GetNationalCode()
    {
        var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
        // Get the claims values
        var nationalcode = _httpContextAccessor.HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.Surname)
                           .Select(c => c.Value).SingleOrDefault();
        if (nationalcode == null)
        {
            throw new UserFriendlyException("کد ملی صحیح نمی باشد");
        }
        return nationalcode;
    }
    public async Task<string> GetRole()
    {
        var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
        // Get the claims values
        var Role = _httpContextAccessor.HttpContext.User.Claims.SingleOrDefault(c => c.Type == ClaimTypes.Role).Value;
        if (string.IsNullOrWhiteSpace(Role))
        {
            throw new UserFriendlyException("دسترسی کافی نمی باشد");
        }
        return Role;
    }

    public async Task IsUserRejected()
    {
        var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
        // Get the claims values
        var Nationalcode = _httpContextAccessor.HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.Name)
                           .Select(c => c.Value).SingleOrDefault();
        if (Nationalcode == null)
        {
            throw new UserFriendlyException("کد ملی صحیح نمی باشد");
        }
        object UserRejectFromCache = null;
        UserRejectionAdvocacy userReject = null;
        //_cacheManager.GetCache("UserRejection").TryGetValue(Nationalcode, out UserRejectFromCache);
        UserRejectFromCache = await _distributedCache.GetStringAsync(string.Format(RedisConstants.UserRejectionPrefix, Nationalcode));
        if (UserRejectFromCache != null && (UserRejectFromCache as string) == "1")
        {
            throw new UserFriendlyException("شما انصراف داده اید و امکان ثبت سفارش نمی باشد");
        }
        if (UserRejectFromCache != null && (UserRejectFromCache as string) == "0")
        {
            return;
        }
        string userRejection = "";

        userRejection = _userRejectionAdcocacyRepository
       .WithDetails()
       .Select(x => x.NationalCode)
       .FirstOrDefault(x => x == Nationalcode);


        if (!string.IsNullOrEmpty(userRejection))
        {
            //await _cacheManager.GetCache("UserRejection").SetAsync(Nationalcode, "1");
            await _distributedCache.SetStringAsync(string.Format(RedisConstants.UserRejectionPrefix, Nationalcode), "1", new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = RedisConstants.UserRejectionTimeOffset
            });
            throw new UserFriendlyException("شما انصراف داده اید و امکان ثبت سفارش نمی باشد");
        }
        else
        {
            //await _cacheManager.GetCache("UserRejection").SetAsync(Nationalcode, "0");
            await _distributedCache.SetStringAsync(string.Format(RedisConstants.UserRejectionPrefix, Nationalcode), "0", new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = RedisConstants.UserRejectionTimeOffset
            });
        }
    }
    //public async Task<AdvocacyAcountResult> CheckAccount(string nationalCode, string mobileNo)
    //{
    //    Logs logs = new Logs();
    //    logs.StartDate = DateTime.Now;
    //    try
    //    {
    //        logs.Ip = "";
    //        logs.Type = 1;
    //        logs.EndDate = DateTime.Now;
    //        logs.Method = "TejaratCheck";
    //        logs.LocationId = 4;
    //        logs.Servername = Utility.GetServerIPAddress();
    //        AuthtenticateResult authenticateResult = await AuthenticateBank();
    //        if (authenticateResult == null)
    //        {
    //            return null;
    //        }
    //        var options = new RestClientOptions("https://ws.farava.ir")
    //        {
    //            MaxTimeout = -1,
    //        };
    //        var client = new RestClient(options);
    //        var request = new RestRequest("/Samt/BlockMoney?nationalCode=" + nationalCode + "&mobileNo=" + mobileNo + "&price=" + _configuration.GetSection("AdvocacyPrice").Value, Method.Get);
    //        request.AddHeader("accept", "application/json");
    //        request.AddHeader("content-type", "application/json");
    //        request.AddHeader("Authorization", "Bearer " + authenticateResult.Model.Token);
    //        var body = @"";
    //        request.AddStringBody(body, DataFormat.Json);
    //        RestResponse response = await client.ExecuteAsync(request);
    //        AdvocacyAcountResult advocacyAcountResult = JsonConvert.DeserializeObject<AdvocacyAcountResult>(response.Content);
    //        logs.EndDate = DateTime.Now;

    //        await _logsRespository.InsertAsync(logs);

    //        return advocacyAcountResult;
    //    }
    //    catch (Exception ex)
    //    {
    //        logs.EndDate = DateTime.Now;
    //        logs.Type = 0;
    //        logs.Message = ex.Message;
    //        //unitOfWorkOptions.IsTransactional = false;
    //        logs.Ip = "";
    //        logs.Type = 7;
    //        logs.EndDate = DateTime.Now;
    //        logs.Method = "Recaptcha";
    //        logs.LocationId = 4;
    //        logs.Servername = Utility.GetServerIPAddress();
    //        //unitOfWorkOptions.Scope = System.Transactions.TransactionScopeOption.RequiresNew;
    //        //using (var unitOfWork = _unitOfWorkManager.Begin(unitOfWorkOptions))
    //        //{
    //        await _logsRespository.InsertAsync(logs);
    //        await UnitOfWorkManager.Current.CompleteAsync();
    //        //}
    //    }
    //    return null;

    //}

    public async Task<bool> ValidateSMS(string Mobile, string NationalCode, string UserSMSCode, SMSType sMSType)
    {
        Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
        object ObjectSMSCode = null;
        //_cacheManager.GetCache("SMS").TryGetValue(sMSType.ToString() + Mobile + NationalCode, out ObjectSMSCode);
        ObjectSMSCode = await _distributedCache.GetStringAsync(string.Format(RedisConstants.ValidateSmsPrefix, NationalCode));
        RegistrationSMSDto smsCodeDto = ObjectSMSCode as RegistrationSMSDto;
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
    public async Task<RecaptchaResponse> CheckCaptcha(CaptchaInputDto input)
    {
        var dictionary = new Dictionary<string, string>
                    {
                        { "secret", _configuration.GetSection("RecaptchaSiteKey").Value },
                        { "response", input.Token }
                    };
        RecaptchaResponse signupResponse = null;
        var postContent = new FormUrlEncodedContent(dictionary);
        HttpResponseMessage recaptchaResponse = null;
        string stringContent = "";

        using (var http = new HttpClient())
        {
            http.Timeout = TimeSpan.FromSeconds(30);
            recaptchaResponse = await http.PostAsync("https://www.google.com/recaptcha/api/siteverify", postContent);
            try
            {
                stringContent = await recaptchaResponse.Content.ReadAsStringAsync();

            }
            catch (Exception ex)
            {
                Logs logs = new Logs();

                logs.EndDate = DateTime.Now;
                logs.Type = 0;
                logs.Message = ex.Message;
                //UnitOfWorkOptions unitOfWorkOptions = new UnitOfWorkOptions();
                //unitOfWorkOptions.IsTransactional = false;
                //unitOfWorkOptions.Scope = System.Transactions.TransactionScopeOption.RequiresNew;
                //using (var unitOfWork = _unitOfWorkManager.Begin(unitOfWorkOptions))
                //{
                await _logsRespository.InsertAsync(logs);
                await UnitOfWorkManager.Current.SaveChangesAsync();
                //unitOfWork.Complete();
                //}
                signupResponse = new RecaptchaResponse() { Success = false, Error = "captcha error", ErrorCode = "S07" };
                return signupResponse;
            }
        }
        if (!recaptchaResponse.IsSuccessStatusCode)
        {
            signupResponse = new RecaptchaResponse() { Success = false, Error = "Unable to verify recaptcha token", ErrorCode = "S03" };
            return signupResponse;
        }
        if (string.IsNullOrEmpty(stringContent))
        {
            signupResponse = new RecaptchaResponse() { Success = false, Error = "Invalid reCAPTCHA verification response", ErrorCode = "S04" };
            return signupResponse;
        }
        var googleReCaptchaResponse = JsonConvert.DeserializeObject<ReCaptchaResponse>(stringContent);
        if (!googleReCaptchaResponse.Success)
        {
            var errors = string.Join(",", googleReCaptchaResponse.ErrorCodes);
            signupResponse = new RecaptchaResponse() { Success = false, Error = errors, ErrorCode = "S05" };
            return signupResponse;
        }

        if (googleReCaptchaResponse.Score < 0.5)
        {
            signupResponse = new RecaptchaResponse() { Success = false, Error = "This is a potential bot. Signup request rejected", ErrorCode = "S07" };
            return signupResponse;
        }
        return new RecaptchaResponse() { Success = true };
    }

    public HttpClient GetHttpClientWithRquiredHeaderForFaraBoom()
    {
        var httpclient = new HttpClient();
        httpclient.DefaultRequestHeaders.Add("Accept-Language", _configuration.GetValue<string>("FaraBoom:Headers:Accept-Language"));
        httpclient.DefaultRequestHeaders.Add("App-Key", _configuration.GetValue<string>("FaraBoom:Headers:App-Key"));
        httpclient.DefaultRequestHeaders.Add("Device-Id", _configuration.GetValue<string>("FaraBoom:Headers:Device-Id"));
        httpclient.DefaultRequestHeaders.Add("Token-Id", _configuration.GetValue<string>("FaraBoom:Headers:Token-Id"));
        httpclient.DefaultRequestHeaders.Add("CLIENT-DEVICE-ID", _configuration.GetValue<string>("FaraBoom:Headers:CLIENT-DEVICE-ID"));
        httpclient.DefaultRequestHeaders.Add("CLIENT-IP-ADDRESS", _configuration.GetValue<string>("FaraBoom:Headers:CLIENT-IP-ADDRESS"));
        httpclient.DefaultRequestHeaders.Add("CLIENT-USER-AGENT", _configuration.GetValue<string>("FaraBoom:Headers:CLIENT-USER-AGENT"));
        httpclient.DefaultRequestHeaders.Add("CLIENT-USER-ID", _configuration.GetValue<string>("FaraBoom:Headers:CLIENT-USER-ID"));
        httpclient.DefaultRequestHeaders.Add("App-Secret", _configuration.GetValue<string>("FaraBoom:Headers:App-Secret"));
        return httpclient;
    }

    public async Task ValidateCustomerBirthDate(List<UserOrderDto> users, CancellationToken cancellationToken)
    {
        var serviceKey = _configuration.GetValue<string>("FaraBoom:Services:Birth-Date:Key");
        var client = GetHttpClientWithRquiredHeaderForFaraBoom();
        string serviceAddress = $"{_configuration.GetValue<string>("FaraBoom:Base-Address")}{_configuration.GetValue<string>("FaraBoom:Services:Birth-Date:Address")}";

        foreach (var user in users)
        {
            var logPerNationalCode = await _externalApiLogResultRepository.FirstOrDefaultAsync(x => EF.Functions.Like(x.ServiceName, serviceKey) &&
                x.NationalCode == user.NationalCode &&
                x.SaleId == user.SaleId);
            if (logPerNationalCode != null)
                continue;
            var content = new StringContent(JsonConvert.SerializeObject(new { birth_date = user.BirthDate.ToString("yyyy-MM-dd"), national_code = user.NationalCode }), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(serviceAddress, content, cancellationToken);
            var responseContentString = await response.Content.ReadAsStringAsync();
            var responseContent = JsonConvert.DeserializeObject<FaraboomBirthDateServiceResponse>(responseContentString);
            await _externalApiResponsLogRepository.InsertAsync(new ExternalApiResponsLog
            {
                NationalCode = user.NationalCode,
                OrderId = user.SaleDetailId,
                Result = responseContent.match,
                ServiceName = serviceKey,
                SaleId = user.SaleId
            });
            await _externalApiLogResultRepository.InsertAsync(new ExternalApiLogResult
            {
                NationalCode = user.NationalCode,
                OrderId = user.SaleDetailId,
                SaleId = user.SaleId,
                Resopnse = responseContentString,
                ServiceName = serviceKey,
                Body = content.ToString()
            });
        }
    }

    public async Task ValidateCustomerPhoneNumber(List<UserOrderMobileDto> userOrders, CancellationToken cancellationToken)
    {
        var serviceKey = _configuration.GetValue<string>("FaraBoom:Services:National-Code:Key");
        var client = GetHttpClientWithRquiredHeaderForFaraBoom();
        var serviceAddress = $"{_configuration.GetValue<string>("FaraBoom:Base-Address")}{_configuration.GetValue<string>("FaraBoom:Services:National-Code:Address")}";

        foreach (var userOrder in userOrders)
        {
            var logPerNationalCode = await _externalApiLogResultRepository.FirstOrDefaultAsync(x => EF.Functions.Like(x.ServiceName, serviceKey) &&
                x.NationalCode == userOrder.NationalCode &&
                x.SaleId == userOrder.SaleId);
            if (logPerNationalCode != null)
                continue;

            var content = new StringContent(JsonConvert.SerializeObject(new { national_code = userOrder.NationalCode, mobile = userOrder.Mobile }), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(serviceAddress, content, cancellationToken);
            var responseContentString = await response.Content.ReadAsStringAsync();
            var responseContent = JsonConvert.DeserializeObject<FaraboomBirthDateServiceResponse>(responseContentString);
            await _externalApiResponsLogRepository.InsertAsync(new ExternalApiResponsLog
            {
                NationalCode = userOrder.NationalCode,
                OrderId = userOrder.SaleDetailId,
                Result = responseContent.match,
                ServiceName = serviceKey,
                SaleId = userOrder.SaleId
            });
            await _externalApiLogResultRepository.InsertAsync(new ExternalApiLogResult
            {
                NationalCode = userOrder.NationalCode,
                OrderId = userOrder.SaleDetailId,
                SaleId = userOrder.SaleId,
                Resopnse = responseContentString,
                ServiceName = serviceKey,
                Body = content.ToString()
            });
        }
    }

    public long GetUserId()
    {
        var userIdStr = _httpContextAccessor.HttpContext.User.Claims.SingleOrDefault(x => x.Type.Equals("UserIdLong"))?.Value ?? string.Empty;
        if (string.IsNullOrWhiteSpace(userIdStr))
            throw new UserFriendlyException("لطفا لاگین کنید");

        long.TryParse(userIdStr, out var userId);
        return userId;
    }
}
