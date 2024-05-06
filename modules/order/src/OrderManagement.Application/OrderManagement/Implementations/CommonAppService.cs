#region NS
using EasyCaching.Core;
using IFG.Core.Caching;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using OrderManagement.Application.Contracts;
using OrderManagement.Application.Contracts.OrderManagement;
using OrderManagement.Application.Contracts.OrderManagement.Services;
using OrderManagement.Application.Contracts.Services;
using OrderManagement.Domain;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
#endregion

namespace OrderManagement.Application.OrderManagement.Implementations;

public class CommonAppService : ApplicationService, ICommonAppService
{
    private readonly ICacheManager _cacheManager;
    private IConfiguration _configuration { get; set; }
    private readonly IRepository<Logs, long> _logsRespository;
    private readonly IRepository<UserRejectionAdvocacy, int> _userRejectionAdcocacyRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IRepository<ExternalApiLogResult, int> _externalApiLogResultRepository;
    private readonly IRepository<ExternalApiResponsLog, int> _externalApiResponsLogRepository;
    private readonly IHttpContextAccessor _contextAccessor;


    public CommonAppService(IConfiguration configuration,
                            IRepository<Logs, long> LogsRespository,
                            IRepository<UserRejectionAdvocacy, int> UserRejectionAdcocacyRepository,
                            IHttpContextAccessor HttpContextAccessor,
                            IRepository<ExternalApiLogResult, int> externalApiLogResultRepository,
                            IRepository<ExternalApiResponsLog, int> externalApiResponsLogRepositor,
                            IHttpContextAccessor contextAccessor,
                            ICacheManager cacheManager)
    {
        _cacheManager = cacheManager;
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
        var nationalcode = _httpContextAccessor.HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.Name)
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
        bool result;
        bool.TryParse(_configuration.GetSection("IsEnableCheckUserRejection").Value, out result);
        if (result)
        {
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            // Get the claims values
            var Nationalcode = _httpContextAccessor.HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.Name)
                               .Select(c => c.Value).SingleOrDefault();
            if (Nationalcode == null)
            {
                throw new UserFriendlyException("کد ملی صحیح نمی باشد");
            }

            string userRejection = _userRejectionAdcocacyRepository
           .WithDetails()
           .Select(x => x.NationalCode)
           .FirstOrDefault(x => x == Nationalcode);


            if (!string.IsNullOrEmpty(userRejection))
            {

                throw new UserFriendlyException("شما انصراف داده اید و امکان ثبت سفارش نمی باشد");
            }


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
        string cacheKey = $"{sMSType.ToString()}{Mobile}{NationalCode}";
        string prefix = "";
        var stringCache = await _cacheManager.GetStringAsync(cacheKey, prefix, new CacheOptions
        { Provider = CacheProviderEnum.Redis , RedisHash = false});
        if (string.IsNullOrEmpty(stringCache))
        {
            throw new UserFriendlyException("کد پیامک ارسالی صحیح نمی باشد");
        }
        SMSDto smsCodeDto = JsonConvert.DeserializeObject<SMSDto>(stringCache);
        if (smsCodeDto == null)
        {
            throw new UserFriendlyException("کد پیامک ارسالی صحیح نمی باشد");
        }
        if (smsCodeDto.SMSCode != UserSMSCode)
        {
            throw new UserFriendlyException("کد پیامک ارسالی صحیح نمی باشد");
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

            string objectCaptcha = await _cacheManager.GetStringAsync("cap_" + input.CIT, "", new CacheOptions() { Provider = CacheProviderEnum.Redis });
            await _cacheManager.RemoveAsync("cap_" + input.CIT, "", new CacheOptions() { Provider = CacheProviderEnum.Redis });

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
    public string GetUserUBPId()
    {
        var userIdStr = _httpContextAccessor.HttpContext.User.Claims.SingleOrDefault(x => x.Type.Equals("UBP"))?.Value ?? string.Empty;
        if (string.IsNullOrWhiteSpace(userIdStr))
            throw new UserFriendlyException("لطفا لاگین کنید");

        return userIdStr;
    }

    public Guid GetUserId()
    {
        var userIdStr = _httpContextAccessor.HttpContext.User.Claims.SingleOrDefault(x => x.Type.Equals("UBP"))?.Value ?? string.Empty;
        if (string.IsNullOrWhiteSpace(userIdStr))
            throw new UserFriendlyException("لطفا لاگین کنید");


        
        return new Guid(userIdStr);
    }

    public Guid? SoftGetUserId()
    {
        var userIdStr = _httpContextAccessor.HttpContext.User.Claims.SingleOrDefault(x => x.Type.Equals("UBP"))?.Value ?? string.Empty;
        if (string.IsNullOrWhiteSpace(userIdStr))
            return null;


        return new Guid(userIdStr);
    }

    public string GetIncomigToken()
        => _contextAccessor.HttpContext.Request.Headers.TryGetValue("Authorization", out var token)
        ? token
        : throw new UserFriendlyException("لطفا مجددا لاگین کنید");

    public async Task<bool> SetOrderStep(OrderStepEnum orderStep, Guid? userId = null)
    {
        //if (userId == null)
        //    userId = GetUserId();
        //var orderStepDto = new OrderStepDto();
        //if (orderStep == OrderStepEnum.Start)
        //{
        //    orderStepDto.StartTime = DateTime.Now;
        //}
        //else
        //{
        //    orderStepDto = await GetOrderStep(userId);
        //}
        //orderStepDto.Step = orderStep;
        //string cacheKey = string.Format(RedisConstants.OrderStepCacheKey, userId.ToString());
        //await _distributedCache.SetStringAsync(cacheKey, JsonConvert.SerializeObject(orderStepDto),
        //    new DistributedCacheEntryOptions()
        //    {
        //        AbsoluteExpiration = new DateTimeOffset(DateTime.Now.AddSeconds(int.Parse(_configuration.GetSection("OrderStepTTL").Value)))
        //    });
        return true;
    }

    public async Task<bool> ValidateOrderStep(OrderStepEnum orderStep)
    {
        //var orderStepDto = await GetOrderStep();
        //var orderSteps = new List<OrderStepEnum>();
        //if (!bool.Parse(_configuration.GetSection("PaymentMethodGranted").Value))
        //    orderSteps = OrderConstant.OrderStepWithoutPayment;
        //else
        //    orderSteps = OrderConstant.OrderStepWithPayment;
        //var beforeStep = orderSteps
        //    .OrderByDescending(x => (int)x)
        //    .FirstOrDefault(x => (int)x < (int)orderStep);
        //if (beforeStep == null || beforeStep != orderStepDto.Step)
        //{
        //    throw new UserFriendlyException(OrderConstant.NoValidFlowOrderStep, OrderConstant.NoValidFlowOrderStepId);
        //}
        return true;
    }

    private async Task<OrderStepDto> GetOrderStep(Guid? userId = null)
    {
        if (userId == null)
            userId = GetUserId();


        string cacheKey = string.Format(RedisConstants.OrderStepCacheKey, userId.ToString());
        string getOrderStep = await _cacheManager.GetStringAsync(cacheKey, "", new CacheOptions() { Provider = CacheProviderEnum.Redis });


        if (string.IsNullOrEmpty(getOrderStep))
        {
            throw new UserFriendlyException(OrderConstant.NoValidFlowOrderStep, OrderConstant.NoValidFlowOrderStepId);
        }
        return JsonConvert.DeserializeObject<OrderStepDto>(getOrderStep);
    }

    public async Task<IkcoApiResult<IkcoInquiry[]>> IkcoOrderStatusInquiryAsync(string nationalCode, int orderId, string accessToken)
    {
        if (string.IsNullOrWhiteSpace(nationalCode) || nationalCode.AsParallel().Any(x => !char.IsDigit(x)))
            throw new UserFriendlyException("خطا در استعلام سفارش. کدملی صحیح نمیباشد");

        if (string.IsNullOrWhiteSpace(accessToken))
            accessToken = await GetIkcoAccessTokenAsync();

        using var client = new HttpClient();
        client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
        var serviceRequest = await client.GetAsync(string.Format(_configuration.GetValue<string>("IkcoOrderInquiryAddresses:InquiryAddress"), nationalCode, orderId));
        if (serviceRequest.StatusCode == HttpStatusCode.Unauthorized)
        {
            await _cacheManager.RemoveAsync(RedisConstants.IkcoBearerToken, RedisConstants.OrderStatusPrefix, new CacheOptions() { Provider = CacheProviderEnum.Hybrid });
            accessToken = await GetIkcoAccessTokenAsync();
            serviceRequest = await client.GetAsync(string.Format(_configuration.GetValue<string>("IkcoOrderInquiryAddresses:InquiryAddress"), nationalCode, orderId));
        }
        if (!serviceRequest.IsSuccessStatusCode)
            throw new UserFriendlyException("خطا در استعلام سفارش");

        var strContent = await serviceRequest.Content.ReadAsStringAsync();
        var response = JsonConvert.DeserializeObject<IkcoApiResult<IkcoInquiry[]>>(strContent);
        return response;
    }

    public async Task<string> GetIkcoAccessTokenAsync()
    {
        
        var cachedAccessToken = await _cacheManager.GetAsync<string>(RedisConstants.IkcoBearerToken,RedisConstants.OrderStatusPrefix, new CacheOptions { Provider = CacheProviderEnum.Hybrid });

        if (!string.IsNullOrEmpty(cachedAccessToken))
        {
            return cachedAccessToken;
        }

        using var client = new HttpClient();
        //var loginModel = _configuration.GetValue<IkcoOrderInquiryProfile>(nameof(IkcoOrderInquiryProfile));
        IkcoOrderInquiryProfile loginModel = new();
        _configuration.GetSection(nameof(IkcoOrderInquiryProfile)).Bind(loginModel);
        var stringContent = new StringContent(
            JsonConvert.SerializeObject(loginModel), new MediaTypeHeaderValue("application/json"));

        var serviceRequest = await client.PostAsync(_configuration.GetValue<string>("IkcoOrderInquiryAddresses:LoginAddresss"), stringContent);
        if (!serviceRequest.IsSuccessStatusCode)
            throw new UserFriendlyException("خطا در فرآیند لاگین سرویس استعلام سفارش");

        var strContent = await serviceRequest.Content.ReadAsStringAsync();
        var response = JsonConvert.DeserializeObject<IkcoApiResult<IkcoLogin>>(strContent);
        if (response.Data == null)
            throw new UserFriendlyException(response?.Errors ?? "خطا در فرآیند لاگین سرویس استعلام سفارش");

        await _cacheManager.SetAsync<string>(RedisConstants.IkcoBearerToken,RedisConstants.OrderStatusPrefix, response.Data.AccessToken, 
            TimeSpan.FromDays(1).Seconds,
            new CacheOptions
            {
                Provider = CacheProviderEnum.Hybrid
            });

        return response.Data.AccessToken;
    }

    public Task<BahmanLoginResult> GetBahmanAccessToken(bool useCache = true)
    {
        throw new NotImplementedException();
    }
}
