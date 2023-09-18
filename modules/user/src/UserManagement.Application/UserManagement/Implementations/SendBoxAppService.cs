

using Esale.Core.Caching;
using Esale.Core.Utility.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using System.Text.Json;
using UserManagement.Application.Contracts.Services;
using UserManagement.Application.Contracts.UserManagement.Services;
using UserManagement.Domain;
using UserManagement.Domain.Authorization.Users;
using UserManagement.Domain.Shared;
using UserManagement.Domain.UserManagement.Bases;
using UserManagement.Domain.UserManagement.CommonService.Dto;
using UserManagement.Domain.UserManagement.CommonService.Dto.Getway;
using UserManagement.Domain.UserManagement.Enums;
using UserManagement.Domain.UserManagement.SendBox.Dtos;
using Volo.Abp;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.Repositories;

namespace UserManagement.Application.UserManagement.Implementations
{
    public class SendBoxAppService : ISendBoxAppService
    {
        private IConfiguration _configuration { get; set; }
        private readonly ICommonAppService _commonAppService;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IRepository<UserMongo, ObjectId> _userMongoRepository;
        private readonly IRandomGenerator _iRandomGenerator;
        private readonly IBaseInformationService _baseInformationService;
        private readonly ICacheManager _cacheManager;
        private readonly IRepository<Logs, long> _logsRepository;
        //private readonly IDistributedCache _distributedCache;
        private readonly IGetwayGrpcClient _getwayGrpcClient;




        public SendBoxAppService(IConfiguration configuration,
            ICommonAppService CommonAppService,
            IRandomGenerator iRandomGenerator,
            IBaseInformationService baseInformationService,
            IRepository<Logs, long> LogsRepository,
            ICacheManager CacheManager)
        {
            _configuration = configuration;
            _commonAppService = CommonAppService;
            _iRandomGenerator = iRandomGenerator;
            _baseInformationService = baseInformationService;
            _logsRepository = LogsRepository;
            _cacheManager = CacheManager;
        }



        [Audited]
        public async Task<Esale.Core.Utility.Results.IResult> SendSms(SendSMSDto input)
        {
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            RegistrationSMSDto sendSMSDto = new RegistrationSMSDto();
            Logs logs = new Logs();
            logs.StartDate = DateTime.Now;
            logs.LocationId = 10;
            logs.Method = "SendSMS";
            logs.Type = 0;
            logs.Ip = Utility.utilities.GetIpAddress(_contextAccessor);
            logs.Servername = Utility.utilities.GetServerIPAddress();
            await _commonAppService.ValidateVisualizeCaptcha(new VisualCaptchaInput(input.CK, input.CIT));

            // await _commonAppService.ValidateVisualizeCaptcha(new CommonService.Dto.VisualCaptchaInput(input.CT,input.CK, input.CIT));
            if (_configuration.GetSection("IsRecaptchaEnabled").Value == "1")
            {
                //var response = await _commonAppService.CheckCaptcha(new CaptchaInputDto(input.CK, "login"));
                //if (response.Success == false)
                //{
                //    throw new UserFriendlyException("خطای کپچا");
                //}


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
                    var userFromDb =(await _userMongoRepository
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
                string objectSMSString = RedisHelper.Connection.GetDatabase().StringGet(PreFix + input.Recipient + input.NationalCode);
                if (objectSMSString == null)
                {
                    objectSMSString = "";
                }
                RegistrationSMSDto sendSMSDFromCache = JsonSerializer.Deserialize<RegistrationSMSDto>(objectSMSString);

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
                try
                {
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

                        //   _ret1 = await _magfa.Send(Message, input.Recipient);
                        _ret = await _getwayGrpcClient.SendService(sendService);
                        //grpc

                    }
                }
                catch (Exception ex)
                {
                    logs.EndDate = DateTime.Now;
                    logs.Message = ex.Message;
                    _logsRepository.InsertAsync(logs);
                }


                if (_ret.Success)
                {
                    logs.EndDate = DateTime.Now;
                    logs.Message = _ret.DataResult;
                    logs.Type = 1;

                    _logsRepository.InsertAsync(logs);
                    sendSMSDto.LastSMSSend = DateTime.Now;
                    RedisHelper.Connection.GetDatabase().StringSet(PreFix + input.Recipient + input.NationalCode,
                        Newtonsoft.Json.JsonConvert.SerializeObject(sendSMSDto)
                      , TimeSpan.FromMinutes(4));
                    //  _cacheManager.GetCache("SMS").Set(PreFix + input.Recipient + input.NationalCode, sendSMSDto);
                    return new SuccsessResult();
                }
                else
                {
                    logs.Type = 0;
                    logs.EndDate = DateTime.Now;
                    logs.Message = _ret.DataResult;
                    _logsRepository.InsertAsync(logs);
                    sendSMSDto.LastSMSSend = DateTime.Now;
                    //sendSMSDto.SMSCode = "1";
                    //_cacheManager.GetCache("SMS").Set(PreFix + input.Recipient + input.NationalCode, sendSMSDto);
                    _logsRepository.InsertAsync(logs);

                }
            }
            return new ErrorResult();
        }
    }
}
