using Esale.Core.Caching;
using Esale.Core.Utility.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using UserManagement.Application.Contracts.Models.SendBox;
using UserManagement.Application.Contracts.Services;
using UserManagement.Application.Contracts.UserManagement.Services;
using UserManagement.Domain.Authorization.Users;
using UserManagement.Domain.Shared.SendBox;
using UserManagement.Domain.UserManagement.Bases;
using Volo.Abp;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.Repositories;
using UserManagement.Application.Contracts.Models;
using Volo.Abp.Application.Services;
using Newtonsoft.Json;

namespace UserManagement.Application.UserManagement.Implementations;

public class SendBoxAppService : ApplicationService, ISendBoxAppService
{
    private IConfiguration _configuration { get; set; }
    private readonly ICommonAppService _commonAppService;
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly IRepository<UserMongo, ObjectId> _userMongoRepository;
    private readonly IRandomGenerator _iRandomGenerator;
    private readonly IBaseInformationService _baseInformationService;
    private readonly ICacheManager _cacheManager;
    //private readonly IRepository<Logs, long> _logsRepository;
    //private readonly IDistributedCache _distributedCache;
    private readonly IGetwayGrpcClient _getwayGrpcClient;




    public SendBoxAppService(IConfiguration configuration,
        ICommonAppService CommonAppService,
        IRandomGenerator iRandomGenerator,
        IBaseInformationService baseInformationService,
        //IRepository<Logs, long> LogsRepository,
        ICacheManager cacheManager,
        IGetwayGrpcClient getwayGrpcClient)
    {
        _configuration = configuration;
        _commonAppService = CommonAppService;
        _iRandomGenerator = iRandomGenerator;
        _baseInformationService = baseInformationService;
        //_logsRepository = LogsRepository;
        _cacheManager = cacheManager;
        _getwayGrpcClient = getwayGrpcClient;
    }



    [Audited]
    public async Task<Esale.Core.Utility.Results.IResult> SendSms(SendSMSDto input)
    {
        Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
        RegistrationSMSDto sendSMSDto = new RegistrationSMSDto();
        //Logs logs = new Logs();
        //logs.StartDate = DateTime.Now;
        //logs.LocationId = 10;
        //logs.Method = "SendSMS";
        //logs.Type = 0;
        //logs.Ip = Utility.utilities.GetIpAddress(_contextAccessor);
        //logs.Servername = Utility.utilities.GetServerIPAddress();
        await _commonAppService.ValidateVisualizeCaptcha(new VisualCaptchaInput(input.CK, input.CIT));
        var _magfa = new MagfaSendSms(new MagfaConfig()
        {
            BaseUrl = _configuration.GetValue<string>("SendBoxConfig:Sms:Magfa:BaseUrl"),
            Domain = _configuration.GetValue<string>("SendBoxConfig:Sms:Magfa:Domain"),
            SenderNumber = _configuration.GetValue<string>("SendBoxConfig:Sms:Magfa:SenderNumber"),
            UserName = _configuration.GetValue<string>("SendBoxConfig:Sms:Magfa:UserName"),
            Password = _configuration.GetValue<string>("SendBoxConfig:Sms:Magfa:Password")
        });
        // await _commonAppService.ValidateVisualizeCaptcha(new CommonService.Dto.VisualCaptchaInput(input.CT,input.CK, input.CIT));
        if (_configuration.GetSection("IsRecaptchaEnabled").Value == "1")
        {
            var response = await _commonAppService.CheckCaptcha(new CaptchaInputDto(input.CK, "login"));
            if (response.Success == false)
            {
                throw new UserFriendlyException("خطای کپچا");
            }


        }

        // if (input.type == ProviderSmsTypeEnum.Magfa)
        {
            //  var _magfa = new MagfaSendSms(JsonConvert.DeserializeObject<MagfaConfig>(_configuration.GetSection("SendBoxConfig:Sms:Magfa").Value));
            string Message = "";
            string PreFix = "";
            sendSMSDto.SMSCode = _iRandomGenerator.GetUniqueInt().ToString();
            if (input.SMSLocation == SMSType.Register)
            {
                PreFix = SMSType.Register.ToString();
                Message = _configuration.GetSection("RegisterText").Value.Replace("{0}", sendSMSDto.SMSCode);
            }
            if (input.SMSLocation == SMSType.UpdateProfile)
            {
                PreFix = SMSType.UpdateProfile.ToString();
                Message = _configuration.GetSection("RegisterText").Value.Replace("{0}", sendSMSDto.SMSCode);
            }
            if (input.SMSLocation == SMSType.UserRejectionAdvocacy)
            {
                PreFix = SMSType.UserRejectionAdvocacy.ToString();

                _baseInformationService.CheckWhiteListAsync(WhiteListEnumType.WhiteListOrder, input.NationalCode);
                Message = _configuration.GetSection("RegisterText").Value.Replace("{0}", sendSMSDto.SMSCode);
            }
            if (input.SMSLocation == SMSType.Login)
            {
                var userFromDb = (await _userMongoRepository
                                  .GetQueryableAsync())
                                  .FirstOrDefault(a => a.NationalCode == input.NationalCode);

                if (userFromDb == null)
                {
                    throw new UserFriendlyException("نام کاربری صحیح نمی باشد");
                }
                input.Recipient = userFromDb.Mobile;
                PreFix = SMSType.UpdateProfile.ToString();
                Message = _configuration.GetSection("RegisterText").Value.Replace("{0}", sendSMSDto.SMSCode);
            }
            else if (input.SMSLocation == SMSType.ForgetPassword)
            {
                var userFromDb = (await _userMongoRepository
                                  .GetQueryableAsync())
                                  .FirstOrDefault(a => a.NationalCode == input.NationalCode);

                if (userFromDb == null || userFromDb.Mobile.Replace(" ", "") != input.Recipient)
                {
                    throw new UserFriendlyException("کد ملی یا شماره موبایل صحیح نمی باشد");
                }
                PreFix = SMSType.ForgetPassword.ToString();
                Message = _configuration.GetSection("RegisterText").Value.Replace("{0}", sendSMSDto.SMSCode);
            }
            //_cacheManager.GetCache("SMS").TryGetValue(PreFix + input.Recipient + input.NationalCode, out objectSMS);
            //string objectSMSString = RedisHelper.Connection.GetDatabase().StringGet(PreFix + input.Recipient + input.NationalCode);
            string objectSMSString = await _cacheManager.GetStringAsync(input.Recipient + input.NationalCode, PreFix, new() { Provider = CacheProviderEnum.Redis });
            if (objectSMSString == null)
            {
                objectSMSString = "";
            }
            RegistrationSMSDto sendSMSDFromCache = Newtonsoft.Json.JsonConvert.DeserializeObject<RegistrationSMSDto>(objectSMSString);

            if (sendSMSDFromCache != null)
            {
                if (string.IsNullOrEmpty(_configuration.GetSection("SMSValidation").Value))
                {
                    if (sendSMSDFromCache != null)
                    {
                        if (DateTime.Now.Subtract(sendSMSDFromCache.LastSMSSend).TotalSeconds < 120)
                        {
                            throw new UserFriendlyException("در دو دقیقه گذشته برای شما پیامک ارسال شده است");
                        }
                    }
                }

            }
            IDataResult<string> _ret1 = null;
            SendBoxServiceDto _ret = null;
          
                if (!string.IsNullOrWhiteSpace(_configuration.GetSection("DisableSendSMS").Value))
                {
                    sendSMSDto.SMSCode = "1";

                    return new SuccsessResult();

                }
                else
                {
                    //_ret = await _magfa.Send(Message, input.Recipient);
                    //grpc
                    SendBoxServiceInput sendService = new SendBoxServiceInput
                    {
                        Recipient = input.Recipient,
                        Text = Message,
                        Provider = ProviderSmsTypeEnum.Magfa,
                        Type = TypeMessageEnum.Sms
                    };

                    //_ret = await _magfa.Send(Message, input.Recipient);
                    //_ret = (IDataResult<string>)await _getwayGrpcClient.SendService(sendService);
                    var _retgrpc = await _getwayGrpcClient.SendService(sendService);
                    if (_retgrpc.Success)
                    {
                        sendSMSDto.LastSMSSend = DateTime.Now;
                        await _cacheManager.SetStringAsync(input.Recipient + input.NationalCode, PreFix, JsonConvert.SerializeObject(sendSMSDto), new() { Provider = CacheProviderEnum.Redis });
                    }
                   
                    //return new SuccsessResult()
                    //grpc

                }
        }
        return new ErrorResult();
    }
}
