#region NS
using IFG.Core.Caching;
using IFG.Core.Utility.Results;
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
using UserManagement.Domain.Shared;
using MongoDB.Driver;
using WorkingWithMongoDB.WebAPI.Services;
using System.Xml.Linq;
using Volo.Abp.Data;
using UserManagement.Application.Contracts;
using UserManagement.Application.Contracts.UserManagement.Models;
#endregion

namespace UserManagement.Application.UserManagement.Implementations;

public class SendBoxAppService : ApplicationService, ISendBoxAppService
{
    private IConfiguration _configuration { get; set; }
    private readonly ICommonAppService _commonAppService;
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly IRepository<UserMongo, ObjectId> _userMongoRepository;
    private readonly IBaseInformationService _baseInformationService;
    private readonly ICacheManager _cacheManager;
    private readonly IGetwayGrpcClient _getwayGrpcClient;
    private readonly ICaptchaService _captchaService;
    private readonly IAuditingManager _auditingManager;
    public SendBoxAppService(IConfiguration configuration,
        ICommonAppService CommonAppService,
        IBaseInformationService baseInformationService,
        ICacheManager cacheManager,
        IGetwayGrpcClient getwayGrpcClient,
        ICaptchaService captchaService,
         IRepository<UserMongo, ObjectId> userMongoRepository, IAuditingManager auditingManager)
    {
        _configuration = configuration;
        _commonAppService = CommonAppService;
        _baseInformationService = baseInformationService;
        _cacheManager = cacheManager;
        _getwayGrpcClient = getwayGrpcClient;
        _captchaService = captchaService;
        _userMongoRepository = userMongoRepository;
        _auditingManager = auditingManager;
    }


    [Audited]
    public async Task<IFG.Core.Utility.Results.IResult> SendSms(SendSMSDto input)
    {
        List<SendSmsLog> comments = new List<SendSmsLog>();
        var _inputLogData = JsonConvert.DeserializeObject<SendSMSDto>(JsonConvert.SerializeObject(input));
        if (!string.IsNullOrEmpty(input.NationalCode) && !ValidationHelper.IsNationalCode(input.NationalCode))
        {
            throw new UserFriendlyException(Messages.NationalCodeNotValid);
        }
        if (!string.IsNullOrEmpty(input.Recipient) && !ValidationHelper.IsMobileNumber(input.Recipient))
        {
            throw new UserFriendlyException(Messages.IsMobileNumberMessage);
        }
        Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
        RegistrationSMSDto sendSMSDto = new RegistrationSMSDto();
        await _commonAppService.ValidateVisualizeCaptcha(new VisualCaptchaInput(input.CK, input.CIT));
        var _magfa = new MagfaSendSms(new MagfaConfig()
        {
            BaseUrl = _configuration.GetValue<string>("SendBoxConfig:Sms:Magfa:BaseUrl"),
            Domain = _configuration.GetValue<string>("SendBoxConfig:Sms:Magfa:Domain"),
            SenderNumber = _configuration.GetValue<string>("SendBoxConfig:Sms:Magfa:SenderNumber"),
            UserName = _configuration.GetValue<string>("SendBoxConfig:Sms:Magfa:UserName"),
            Password = _configuration.GetValue<string>("SendBoxConfig:Sms:Magfa:Password")
        });
        if (_configuration.GetSection("IsRecaptchaEnabled").Value == "1")
        {
            var response = await _captchaService.ReCaptcha(new CaptchaInputDto(input.CK, "CreateUser"));
            if (response.Success == false)
            {
                throw new UserFriendlyException(UserMessageConstant.CaptchaErorr, UserMessageConstant.CaptchaErorrId);
            }

        }

        (string Message, string PreFix, UserMongo userFromDb) = (string.Empty, string.Empty, null);
        sendSMSDto.SMSCode = Core.Utility.Tools.RandomGenerator.GetUniqueInt(6).ToString();
        switch (input.SMSLocation)
        {
            case SMSType smsType when input.SMSLocation
                is SMSType.Register
                or SMSType.UpdateProfile
                or SMSType.AnonymousQuestionnaireSubmitation
                or SMSType.UpdatePhoneNumber:

                PreFix = smsType.ToString();
                Message = _configuration.GetSection("RegisterText").Value.Replace("{0}", sendSMSDto.SMSCode);

                break;
            case SMSType.UserRejectionAdvocacy:
                PreFix = SMSType.UserRejectionAdvocacy.ToString();

                await _baseInformationService.CheckWhiteListAsync(WhiteListEnumType.WhiteListOrder, input.NationalCode);
                Message = _configuration.GetSection("RegisterText").Value.Replace("{0}", sendSMSDto.SMSCode);
                break;
            case SMSType.Login:
                userFromDb = (await _userMongoRepository
                              .GetQueryableAsync())
                              .FirstOrDefault(a => a.NationalCode == input.NationalCode);

                if (userFromDb == null)
                {
                    throw new UserFriendlyException(UserMessageConstant.UsernameIsNotCorrect, UserMessageConstant.UsernameIsNotCorrectId);
                }
                input.Recipient = userFromDb.Mobile;
                PreFix = SMSType.UpdateProfile.ToString();
                Message = _configuration.GetSection("RegisterText").Value.Replace("{0}", sendSMSDto.SMSCode);
                break;
            case SMSType.ForgetPassword:
                userFromDb = (await _userMongoRepository
                              .GetQueryableAsync())
                              .FirstOrDefault(a => a.NationalCode == input.NationalCode && a.IsDeleted == false);

                if (userFromDb == null || userFromDb.Mobile.Replace(" ", "") != input.Recipient)
                {
                    throw new UserFriendlyException(UserMessageConstant.NationalCodeOrMobileNotCorrect, UserMessageConstant.NationalCodeOrMobileNotCorrectId);
                }
                PreFix = SMSType.ForgetPassword.ToString();
                Message = _configuration.GetSection("ForgetPassText").Value.Replace("{0}", sendSMSDto.SMSCode);
                break;
            case SMSType.ChangePassword:
                string uid = _commonAppService.GetUID().ToString();
                var filter = Builders<UserMongo>.Filter.Where(e => e.UID == uid && e.IsDeleted == false);

                userFromDb = (await _userMongoRepository
                                .GetQueryableAsync())
                                .FirstOrDefault(a => a.UID == uid && a.IsDeleted == false);

                if (userFromDb == null)
                {
                    throw new UserFriendlyException("خطایی ");
                }
                PreFix = SMSType.ChangePassword.ToString();
                Message = _configuration.GetSection("RegisterText").Value.Replace("{0}", sendSMSDto.SMSCode);
                input.Recipient = userFromDb.Mobile;
                input.NationalCode = userFromDb.NationalCode;
                break;
            default:
                throw new ArgumentException($"unhandeled exception occurred for type {input.SMSLocation}");
        }

        string objectSMSString = await _cacheManager.GetStringAsync(
            input.Recipient + input.NationalCode,
            PreFix, new() { Provider = CacheProviderEnum.Redis, RedisHash = false })
            ?? string.Empty;
        RegistrationSMSDto sendSMSDFromCache = Newtonsoft.Json.JsonConvert.DeserializeObject<RegistrationSMSDto>(objectSMSString);

        if (sendSMSDFromCache != null)
        {
            if (!string.IsNullOrEmpty(_configuration.GetSection("SMSValidation").Value))
            {
                if (sendSMSDFromCache != null)
                {
                    if (DateTime.Now.Subtract(sendSMSDFromCache.LastSMSSend).TotalSeconds < 120)
                    {
                        throw new UserFriendlyException(UserMessageConstant.TextSent2MinsAgo, UserMessageConstant.TextSent2MinsAgoId);
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
                await _cacheManager.SetStringAsync(input.Recipient + input.NationalCode, PreFix, JsonConvert.SerializeObject(sendSMSDto)
                    , new CacheOptions()
                    {
                        Provider = CacheProviderEnum.Redis,
                        RedisHash = false
                    }, 120);
            }
            else
            {

                comments.Add(new SendSmsLog
                {
                    Description = "Getway GrpcClient",
                    Data = JsonConvert.DeserializeObject<Dictionary<string, object>>(JsonConvert.SerializeObject(_inputLogData))
                });
                using (var auditingScope = _auditingManager.BeginScope())
                {
                    _auditingManager.Current.Log.SetProperty("SendSms", comments);
                    _auditingManager.Current.Log.Comments.Add(JsonConvert.SerializeObject(new Dictionary<string, object>
                      {
                         { "Success",_retgrpc.Success},
                         { "DataResult",_retgrpc.DataResult},
                         { "Message",_retgrpc.Message},
                         { "MessageCode",_retgrpc.MessageCode}
                       }));
                    await auditingScope.SaveAsync();
                }
                throw new UserFriendlyException(UserMessageConstant.SendSmsErorr,UserMessageConstant.SendSmsErorrId);
            }

            return new SuccsessResult();
            //grpc

        }
    }
}
