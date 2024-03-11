using IFG.Core.DataAccess;
using IFG.Core.Validation;
using Grpc.Net.Client;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using PaymentManagement.Application.Contracts.Dtos;
using PaymentManagement.Application.Contracts.Enums;
using PaymentManagement.Application.Contracts.IServices;
using PaymentManagement.Application.GatewayServiceGrpc;
using PaymentManagement.Application.IranKish;
using PaymentManagement.Application.Mellat;
using PaymentManagement.Application.Utilities;
using PaymentManagement.Domain.Models;
using System.Text;
using System.Text.Json;
using System.Xml.Serialization;
using Volo.Abp.Application.Services;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Uow;
using Microsoft.AspNetCore.Authentication;
using PaymentManagement.Application.Pasargad;
using System.Linq.Dynamic.Core.Tokenizer;
using PaymentManagement.Application.Parsian;

namespace PaymentManagement.Application.Servicess
{
    public class PaymentAppService : ApplicationService, IPaymentAppService
    {
        private readonly IRepository<Psp, int> _pspRepository;
        private readonly IRepository<Account, int> _accountRepository;
        private readonly IRepository<PspAccount, int> _pspAccountRepository;
        private readonly IRepository<Payment, int> _paymentRepository;
        private readonly IRepository<PaymentLog, int> _paymentLogRepository;
        private readonly IConfiguration _config;

        public PaymentAppService(
            IRepository<Psp, int> pspRepository,
            IRepository<Account, int> accountRepository,
            IRepository<PspAccount, int> pspAccountRepository,
            IRepository<Payment, int> paymentRepository,
            IRepository<PaymentLog, int> paymentLogRepository,
            IConfiguration config
            )
        {
            _pspRepository = pspRepository;
            _accountRepository = accountRepository;
            _pspAccountRepository = pspAccountRepository;
            _paymentRepository = paymentRepository;
            _paymentLogRepository = paymentLogRepository;
            _config = config;
        }
        private GatewayServiceGrpc.GatewayServiceGrpc.GatewayServiceGrpcClient GatewayServiceGrpcClient()
        {
            var httpHandler = new HttpClientHandler();
            httpHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            var channel = GrpcChannel.ForAddress(_config.GetValue<string>("AppUrls:GatewayGrpc"), new GrpcChannelOptions { HttpHandler = httpHandler });
            return new GatewayServiceGrpc.GatewayServiceGrpc.GatewayServiceGrpcClient(channel);
        }
        public List<PspAccountDto> GetPsps()
        {
            return _pspAccountRepository.WithDetails().AsNoTracking()
                .Include(o => o.Psp).Include(o => o.Account)
                .Where(o => o.IsActive && o.Account.IsActive && o.Psp.IsActive)
                .Select(o => new PspAccountDto { Id = o.Id, PspId = o.PspId, AccountName = o.Account.AccountName, Psp = o.Psp.Title })
                .ToList();
        }

        public List<InquiryWithFilterParamDto> InquiryWithFilterParam(int? filterParam1, int? filterParam2, int? filterParam3, int? filterParam4
            , bool? IsRelationIdGroup
            , bool? IsRelationIdBGroup
            , bool? IsRelationIdCGroup
            , bool? IsRelationIdDGroup)
        {
            IsRelationIdGroup = IsRelationIdGroup != true ? null : true;
            IsRelationIdBGroup = IsRelationIdBGroup != true ? null : true;
            IsRelationIdCGroup = IsRelationIdCGroup != true ? null : true;
            IsRelationIdDGroup = IsRelationIdDGroup != true ? null : true;

            return _paymentRepository.WithDetails().AsNoTracking()
                .Where(o => (filterParam1 == null || o.FilterParam1 == filterParam1) && (filterParam2 == null || o.FilterParam2 == filterParam2) &&
                            (filterParam3 == null || o.FilterParam3 == filterParam3) && (filterParam4 == null || o.FilterParam4 == filterParam4))
                .GroupBy(o => new
                {
                    filterParam1 = IsRelationIdGroup != null ? o.FilterParam1 : null,
                    filterParam2 = IsRelationIdBGroup != null ? o.FilterParam2 : null,
                    filterParam3 = IsRelationIdCGroup != null ? o.FilterParam3 : null,
                    filterParam4 = IsRelationIdDGroup != null ? o.FilterParam4 : null,
                    o.PaymentStatusId
                }).Select(o => new InquiryWithFilterParamDto
                {
                    Status = o.Key.PaymentStatusId,
                    filterParam1 = o.Key.filterParam1,
                    filterParam2 = o.Key.filterParam2,
                    filterParam3 = o.Key.filterParam3,
                    filterParam4 = o.Key.filterParam4,
                    Message = EnumExtension.GetEnumDescription((PaymentStatusEnum)o.Key.PaymentStatusId),
                    Count = o.Count()
                }).ToList();
        }

        public List<InquiryWithFilterParamDto> InquiryWithFilterParamGroupByParams(int? filterParam1, int? filterParam2, int? filterParam3, int? filterParam4)
        {
            return _paymentRepository.WithDetails().AsNoTracking()
                .Where(o => (filterParam1 == null || o.FilterParam1 == filterParam1) && (filterParam2 == null || o.FilterParam2 == filterParam2) &&
                            (filterParam3 == null || o.FilterParam3 == filterParam3) && (filterParam4 == null || o.FilterParam4 == filterParam4))
                .GroupBy(o => new
                {
                    filterParam1 = filterParam1 != null ? o.FilterParam1 : null,
                    filterParam2 = filterParam2 != null ? o.FilterParam2 : null,
                    filterParam3 = filterParam3 != null ? o.FilterParam3 : null,
                    filterParam4 = filterParam4 != null ? o.FilterParam4 : null,
                    o.PaymentStatusId

                }).Select(o => new InquiryWithFilterParamDto
                {
                    Status = o.Key.PaymentStatusId,
                    Message = EnumExtension.GetEnumDescription((PaymentStatusEnum)o.Key.PaymentStatusId),
                    filterParam1 = o.Key.filterParam1,
                    filterParam2 = o.Key.filterParam2,
                    filterParam3 = o.Key.filterParam3,
                    filterParam4 = o.Key.filterParam4,
                    Count = o.Count()
                }).ToList();
        }
        public string GetCallBackUrl(int paymentId)
        {
            var result = _paymentRepository.WithDetails().AsNoTracking().Select(o => new { o.Id, o.CallBackUrl }).FirstOrDefault(o => o.Id == paymentId);
            return result == null ? string.Empty : result.CallBackUrl;
        }
        [UnitOfWork(false)]
        public PaymentInfoDto GetPaymentInfo(int paymentId)
        {
            return _paymentRepository.WithDetails().AsNoTracking().Select(o => new PaymentInfoDto
            {
                PaymentId = o.Id,
                TransactionCode = o.TransactionCode,
                TransactionDate = o.TransactionDate,
                TransactionPersianDate = o.TransactionPersianDate,
                PaymentStatusId = o.PaymentStatusId
            }).FirstOrDefault(o => o.PaymentId == paymentId);
        }

        public async Task InsertPaymentLogAsync(PaymentLogDto input)
        {
            if (input.PaymentId != 0)
            {
                await _paymentLogRepository.InsertAsync(new PaymentLog
                {
                    PaymentId = input.PaymentId,
                    Psp = input.Psp,
                    Message = input.Message,
                    Parameter = input.Parameter
                });
            }
        }
        private async Task<AuthenticateJsonResult> AuthenticatePasargadAsync(int paymentId, string userName, string password)
        {
            AuthenticateInput authInput = new()
            {
                Type = PspEnum.Pasargad.ToString(),
                UserName = userName,
                Password = password
            };
            await _paymentLogRepository.InsertAsync(new PaymentLog
            {
                PaymentId = paymentId,
                Psp = PspEnum.Pasargad.ToString(),
                Message = Constants.AuthenticateStart,
                Parameter = JsonConvert.SerializeObject(authInput),
            });
            string authenticateResult = string.Empty;
            var connectToGatewayMode = _config.GetValue<bool>("App:ConnectToGatewayMode");
            if (connectToGatewayMode) //grpc
            {
                authenticateResult = (await GatewayServiceGrpcClient().AuthenticateAsync(authInput)).Result;
            }
            else //restApi
            {
                string url = _config.GetValue<string>("AppUrls:GatewayApi") + _config.GetValue<string>("AppUrls:GatewayAuthUrl");
                HttpClient client = new();
                var response = await client.PostAsync(url, new StringContent(JsonConvert.SerializeObject(authInput), Encoding.UTF8, "application/json"));
                authenticateResult = response.Content.ReadAsStringAsync().Result;
                authenticateResult = JsonDocument.Parse(authenticateResult).RootElement.GetProperty("result").ToString();
            }
            await _paymentLogRepository.InsertAsync(new PaymentLog
            {
                PaymentId = paymentId,
                Psp = PspEnum.Pasargad.ToString(),
                Message = Constants.AuthenticateResult,
                Parameter = authenticateResult,
            });

            return JsonConvert.DeserializeObject<AuthenticateJsonResult>(authenticateResult);
        }

        #region HandShake
        [UnitOfWork(isTransactional: false)]
        public async Task<HandShakeOutputDto> HandShakeAsync(HandShakeInputDto input)
        {

            var result = new HandShakeOutputDto()
            {
                StatusCode = (int)StatusCodeEnum.Failed,
                Message = Constants.UnknownError
            };
            string pspTitle = string.Empty;
            PaymentDto payment = null;
            try
            {
                #region ValidateInput

                List<string> messages = new();

                if (!string.IsNullOrEmpty(input.NationalCode) && !ValidationHelper.IsValidNationalCode(input.NationalCode))
                {
                    messages.Add(Constants.ErrorInHandShakeInput_NationalCode);
                }
                if (!string.IsNullOrEmpty(input.Mobile) && !ValidationHelper.IsValidMobileNumber(input.Mobile))
                {
                    messages.Add(Constants.ErrorInHandShakeInput_Mobile);
                }
                if (input.Amount < 10000)
                {
                    messages.Add(Constants.ErrorInHandShakeInput_Amount);
                }

                var pspAccount = _pspAccountRepository.WithDetails().AsNoTracking().Select(o => new { o.Id, o.IsActive, o.PspId, o.AccountId, o.JsonProps }).FirstOrDefault(o => o.Id == input.PspAccountId);
                if (pspAccount == null || !pspAccount.IsActive)
                {
                    messages.Add(Constants.ErrorInHandShakeInput_InactiveAccount);
                }
                else
                {
                    if ((PspEnum)pspAccount.PspId == PspEnum.Parsian && string.IsNullOrEmpty(input.NationalCode))
                    { messages.Add(Constants.ErrorInHandShakeInput_NationalCode_Required); }

                    var psp = _pspRepository.WithDetails().AsNoTracking().Select(o => new { o.Id, o.IsActive }).FirstOrDefault(o => o.Id == pspAccount.PspId);
                    if (psp == null || !psp.IsActive)
                    {
                        messages.Add(Constants.ErrorInHandShakeInput_InactiveAccount);
                    }
                    else
                    {
                        var account = _accountRepository.WithDetails().AsNoTracking().Select(o => new { o.Id, o.IsActive }).FirstOrDefault(o => o.Id == pspAccount.AccountId);
                        if (account == null || !account.IsActive)
                        {
                            messages.Add(Constants.ErrorInHandShakeInput_InactiveAccount);
                        }
                    }
                }

                if (messages.Any())
                {
                    result.Message = string.Join("، ", messages);
                    return result;
                }

                #endregion

                payment = await CreatePaymentAsync(input);
                pspTitle = ((PspEnum)pspAccount.PspId).ToString();
                await _paymentLogRepository.InsertAsync(new PaymentLog
                {
                    PaymentId = payment.Id,
                    Psp = pspTitle,
                    Message = Constants.HandShakeWithPaymentStart,
                    Parameter = JsonConvert.SerializeObject(input),
                });
                result.PaymentId = payment.Id;

                #region HandShake
                switch ((PspEnum)pspAccount.PspId)
                {
                    case PspEnum.Mellat:
                        return await HandShakeWithMellatAsync(payment, pspAccount.JsonProps);

                    case PspEnum.IranKish:
                        return await HandShakeWithIranKishAsync(payment, pspAccount.JsonProps);

                    case PspEnum.Parsian:
                        return await HandShakeWithParsianAsync(payment, pspAccount.JsonProps);

                    case PspEnum.Pasargad:
                        return await HandShakeWithPasargadAsync(payment, pspAccount.JsonProps);
                }
                #endregion

                return result;
            }
            catch (Exception ex)
            {
                await _paymentLogRepository.InsertAsync(new PaymentLog
                {
                    PaymentId = result.PaymentId,
                    Psp = pspTitle,
                    Message = Constants.HandShakeException,
                    Parameter = ex.Message,
                });
                if (payment != null)
                {
                    payment.PaymentStatusId = (int)PaymentStatusEnum.Failed;
                    await _paymentRepository.AttachAsync(ObjectMapper.Map<PaymentDto, Payment>(payment), o => o.PaymentStatusId);
                }
                result.Message = Constants.ErrorInConnectToPsp;
                return result;
            }
        }
        private async Task<PaymentDto> CreatePaymentAsync(HandShakeInputDto input)
        {
            var payment = await _paymentRepository.InsertAsync(new Payment
            {
                PspAccountId = input.PspAccountId,
                PaymentStatusId = (int)PaymentStatusEnum.InProgress,
                Amount = input.Amount,
                CallBackUrl = input.CallBackUrl,
                NationalCode = input.NationalCode,
                Mobile = input.Mobile,
                AdditionalData = input.AdditionalData,
                TransactionCode = string.Empty,
                TransactionDate = DateTime.Now,
                TransactionPersianDate = DateUtil.Now,
                FilterParam1 = input.FilterParam1,
                FilterParam2 = input.FilterParam2,
                FilterParam3 = input.FilterParam3,
                FilterParam4 = input.FilterParam4
            });
            await CurrentUnitOfWork.SaveChangesAsync();
            await _paymentRepository.RemoveTracking(payment);
            var paymentDto = new PaymentDto
            {
                Id = payment.Id,
                PaymentStatusId = payment.PaymentStatusId,
                TraceNo = payment.TraceNo,
                TransactionCode = payment.TransactionCode,
                Token = payment.Token,
                PspAccountId = payment.PspAccountId,
                Amount = payment.Amount,
                Mobile = payment.Mobile,
                NationalCode = payment.NationalCode,
                TransactionDate = payment.TransactionDate
            };
            return paymentDto;
        }
        private async Task<HandShakeOutputDto> HandShakeWithIranKishAsync(PaymentDto payment, string pspAccountJsonProps)
        {
            var result = new HandShakeOutputDto()
            {
                StatusCode = (int)StatusCodeEnum.Failed,
                Message = Constants.UnknownError,
                PaymentId = payment.Id
            };
            try
            {
                var pspAccountProps = JsonConvert.DeserializeObject<IranKish.PspAccountProps>(pspAccountJsonProps);
                IranKishHandShakeInput input = new()
                {
                    TerminalId = pspAccountProps.TerminalId,
                    AcceptorId = pspAccountProps.AcceptorId,
                    PassPhrase = pspAccountProps.PassPhrase,
                    CallBackUrl = _config.GetValue<string>("AppUrls:PaymentApi") + _config.GetValue<string>("IranKishUrls:CallBackUrl"),
                    Amount = (long)payment.Amount,
                    RequestId = payment.Id.ToString(),
                    NationalCode = payment.NationalCode ?? string.Empty,
                    Mobile = string.IsNullOrEmpty(payment.Mobile)
                    ? string.Empty
                    : (payment.Mobile[..1] == "0" ? string.Concat("98", payment.Mobile[1..]) : payment.Mobile),
                    RsaPublicKey = pspAccountProps.RsaPublicKey
                };
                await _paymentLogRepository.InsertAsync(new PaymentLog
                {
                    PaymentId = payment.Id,
                    Psp = PspEnum.IranKish.ToString(),
                    Message = Constants.HandShakeWithPspStart,
                    Parameter = JsonConvert.SerializeObject(input),
                });
                string handShakeResult = string.Empty;
                var connectToGatewayMode = _config.GetValue<bool>("App:ConnectToGatewayMode");
                if (connectToGatewayMode) //grpc
                {
                    handShakeResult = (await GatewayServiceGrpcClient().HandShakeWithIranKishAsync(input)).Result;
                }
                else //restApi
                {
                    string url = _config.GetValue<string>("AppUrls:GatewayApi") + _config.GetValue<string>("IranKishUrls:HandShake");
                    HttpClient client = new();
                    var response = await client.PostAsync(url, new StringContent(JsonConvert.SerializeObject(input), Encoding.UTF8, "application/json"));
                    handShakeResult = response.Content.ReadAsStringAsync().Result;
                    handShakeResult = JsonDocument.Parse(handShakeResult).RootElement.GetProperty("result").ToString();
                }
                await _paymentLogRepository.InsertAsync(new PaymentLog
                {
                    PaymentId = payment.Id,
                    Psp = PspEnum.IranKish.ToString(),
                    Message = Constants.HandShakeResult,
                    Parameter = handShakeResult,
                });
                result.PspJsonResult = handShakeResult;
                if (handShakeResult != null)
                {
                    JsonResult jResult = JsonConvert.DeserializeObject<JsonResult>(handShakeResult);
                    if (jResult.status)
                    {
                        payment.Token = jResult.result.token;
                        await _paymentRepository.AttachAsync(ObjectMapper.Map<PaymentDto, Payment>(payment), o => o.Token);
                        result.StatusCode = (int)StatusCodeEnum.Success;
                        result.Token = jResult.result.token;
                        var inputParams = new System.Collections.Specialized.NameValueCollection
                        {
                            { "tokenIdentity", jResult.result.token }
                        };
                        result.HtmlContent = StringUtil.GenerateForm(_config.GetValue<string>("IranKishUrls:RedirectUrl"), "post", inputParams);
                        result.Message = Constants.HandShakeSuccess;
                        return result;
                    }
                }
                payment.PaymentStatusId = (int)PaymentStatusEnum.Failed;
                await _paymentRepository.AttachAsync(ObjectMapper.Map<PaymentDto, Payment>(payment), o => o.PaymentStatusId);
                result.Message = Constants.ErrorInHandShakeResult;
                return result;
            }
            catch (Exception ex)
            {
                await _paymentLogRepository.InsertAsync(new PaymentLog
                {
                    PaymentId = payment.Id,
                    Psp = PspEnum.IranKish.ToString(),
                    Message = Constants.HandShakeException,
                    Parameter = ex.Message,
                });
                payment.PaymentStatusId = (int)PaymentStatusEnum.Failed;
                await _paymentRepository.AttachAsync(ObjectMapper.Map<PaymentDto, Payment>(payment), o => o.PaymentStatusId);
                result.Message = Constants.ErrorInConnectToPsp;
                return result;
            }
        }
        private async Task<HandShakeOutputDto> HandShakeWithMellatAsync(PaymentDto payment, string pspAccountJsonProps)
        {
            var result = new HandShakeOutputDto()
            {
                StatusCode = (int)StatusCodeEnum.Failed,
                Message = Constants.UnknownError,
                PaymentId = payment.Id
            };

            try
            {
                var pspAccountProps = JsonConvert.DeserializeObject<Mellat.PspAccountProps>(pspAccountJsonProps);
                string encryptedNationalCode = string.Empty;
                if (!string.IsNullOrEmpty(payment.NationalCode))
                {
                    byte[] keyByteArray = Mellat.Encrypt.StringToByteArray("2C7D202B960A96AA");
                    encryptedNationalCode = Mellat.Encrypt.EncryptPinBlock(keyByteArray, Mellat.Encrypt.StringToByteArray(payment.NationalCode));
                }
                MellatHandShakeInput input = new()
                {
                    TerminalId = pspAccountProps.TerminalId,
                    UserName = pspAccountProps.UserName,
                    UserPassword = pspAccountProps.UserPassword,
                    OrderId = payment.Id,
                    Amount = (long)payment.Amount,
                    CallBackUrl = _config.GetValue<string>("AppUrls:PaymentApi") + _config.GetValue<string>("MellatUrls:CallBackUrl"),
                    MobileNo = string.IsNullOrEmpty(payment.Mobile)
                    ? string.Empty
                    : (payment.Mobile[..1] == "0" ? string.Concat("98", payment.Mobile[1..]) : payment.Mobile),
                    EncryptedNationalCode = encryptedNationalCode,
                    Switch = pspAccountProps.Switch
                };
                await _paymentLogRepository.InsertAsync(new PaymentLog
                {
                    PaymentId = payment.Id,
                    Psp = PspEnum.Mellat.ToString(),
                    Message = Constants.HandShakeWithPspStart,
                    Parameter = JsonConvert.SerializeObject(input),
                });

                string handShakeResult = string.Empty;
                var connectToGatewayMode = _config.GetValue<bool>("App:ConnectToGatewayMode");
                if (connectToGatewayMode) //grpc
                {
                    handShakeResult = (await GatewayServiceGrpcClient().HandShakeWithMellatAsync(input)).Result;
                }
                else //restApi
                {
                    string url = _config.GetValue<string>("AppUrls:GatewayApi") + _config.GetValue<string>("MellatUrls:HandShake");
                    HttpClient client = new();
                    var response = await client.PostAsync(url, new StringContent(JsonConvert.SerializeObject(input), Encoding.UTF8, "application/json"));
                    handShakeResult = response.Content.ReadAsStringAsync().Result;
                    handShakeResult = JsonDocument.Parse(handShakeResult).RootElement.GetProperty("result").ToString();
                }
                await _paymentLogRepository.InsertAsync(new PaymentLog
                {
                    PaymentId = payment.Id,
                    Psp = PspEnum.Mellat.ToString(),
                    Message = Constants.HandShakeResult,
                    Parameter = handShakeResult,
                });
                result.PspJsonResult = handShakeResult;
                if (!string.IsNullOrEmpty(handShakeResult))
                {
                    var res = JsonConvert.DeserializeObject<MellatOutputDto>(handShakeResult).Body.@return.Split(",".ToCharArray());

                    if (res[0] == "0")
                    {
                        payment.Token = res[1];
                        await _paymentRepository.AttachAsync(ObjectMapper.Map<PaymentDto, Payment>(payment), o => o.Token);
                        result.StatusCode = (int)StatusCodeEnum.Success;
                        result.Token = res[1];
                        result.Message = Constants.HandShakeSuccess;
                        var inputParams = new System.Collections.Specialized.NameValueCollection
                        {
                            { "RefId", res[1] }
                        };
                        if (!string.IsNullOrEmpty(encryptedNationalCode))
                        {
                            inputParams.Add("ENC", encryptedNationalCode);
                        }
                        if (!string.IsNullOrEmpty(input.MobileNo))
                        {
                            inputParams.Add("MobileNo", input.MobileNo);
                        }

                        result.HtmlContent = StringUtil.GenerateForm(_config.GetValue<string>("MellatUrls:RedirectUrl" + pspAccountProps.Switch.ToString()), "post", inputParams);
                        return result;
                    }
                }

                payment.PaymentStatusId = (int)PaymentStatusEnum.Failed;
                await _paymentRepository.AttachAsync(ObjectMapper.Map<PaymentDto, Payment>(payment), o => o.PaymentStatusId);
                result.Message = Constants.ErrorInHandShakeResult;
                return result;
            }
            catch (Exception ex)
            {
                await _paymentLogRepository.InsertAsync(new PaymentLog
                {
                    PaymentId = payment.Id,
                    Psp = PspEnum.Mellat.ToString(),
                    Message = Constants.HandShakeException,
                    Parameter = ex.Message,
                });
                payment.PaymentStatusId = (int)PaymentStatusEnum.Failed;
                await _paymentRepository.AttachAsync(ObjectMapper.Map<PaymentDto, Payment>(payment), o => o.PaymentStatusId);
                result.Message = Constants.ErrorInConnectToPsp;
                return result;
            }
        }
        private async Task<HandShakeOutputDto> HandShakeWithParsianAsync(PaymentDto payment, string pspAccountJsonProps)
        {
            var result = new HandShakeOutputDto()
            {
                StatusCode = (int)StatusCodeEnum.Failed,
                Message = Constants.UnknownError,
                PaymentId = payment.Id
            };
            try
            {
                var pspAccountProps = JsonConvert.DeserializeObject<Parsian.PspAccountProps>(pspAccountJsonProps);
                ParsianHandShakeInput input = new()
                {
                    LoginAccount = pspAccountProps.LoginAccount,
                    CallBackUrl = _config.GetValue<string>("AppUrls:PaymentApi") + _config.GetValue<string>("ParsianUrls:CallBackUrl"),
                    Amount = (long)payment.Amount,
                    OrderId = payment.Id,
                    AdditionalData = payment.NationalCode ?? string.Empty,
                    Originator = string.IsNullOrEmpty(payment.Mobile) ? string.Empty : (payment.Mobile[..1] == "0" ? payment.Mobile[1..] : (payment.Mobile[..2] == "98" ? payment.Mobile[2..] : payment.Mobile)),
                    Key = pspAccountProps.Key,
                    IV = pspAccountProps.IV,
                    ThirdPartyCode = pspAccountProps.ThirdPartyCode
                };
                await _paymentLogRepository.InsertAsync(new PaymentLog
                {
                    PaymentId = payment.Id,
                    Psp = PspEnum.Parsian.ToString(),
                    Message = Constants.HandShakeWithPspStart,
                    Parameter = JsonConvert.SerializeObject(input),
                });
                string handShakeResult = string.Empty;
                var connectToGatewayMode = _config.GetValue<bool>("App:ConnectToGatewayMode");
                if (connectToGatewayMode) //grpc
                {
                    handShakeResult = (await GatewayServiceGrpcClient().HandShakeWithParsianAsync(input)).Result;
                }
                else //restApi
                {
                    string url = _config.GetValue<string>("AppUrls:GatewayApi") + _config.GetValue<string>("ParsianUrls:HandShake");
                    HttpClient client = new();
                    var response = await client.PostAsync(url, new StringContent(JsonConvert.SerializeObject(input), Encoding.UTF8, "application/json"));
                    handShakeResult = response.Content.ReadAsStringAsync().Result;
                    handShakeResult = JsonDocument.Parse(handShakeResult).RootElement.GetProperty("result").ToString();
                }
                await _paymentLogRepository.InsertAsync(new PaymentLog
                {
                    PaymentId = payment.Id,
                    Psp = PspEnum.Parsian.ToString(),
                    Message = Constants.HandShakeResult,
                    Parameter = handShakeResult,
                });
                result.PspJsonResult = handShakeResult;
                if (handShakeResult != null)
                {
                    Parsian.HandShakeJsonResult jResult = JsonConvert.DeserializeObject<Parsian.HandShakeJsonResult>(handShakeResult);

                    result.Token = payment.Token = jResult.Token.ToString();
                    await _paymentRepository.AttachAsync(ObjectMapper.Map<PaymentDto, Payment>(payment), o => o.Token);

                    if (jResult.Status == 0 && jResult.Token > 0L)
                    {
                        result.StatusCode = (int)StatusCodeEnum.Success;
                        var inputParams = new System.Collections.Specialized.NameValueCollection { { "token", jResult.Token.ToString() } };
                        result.HtmlContent = StringUtil.GenerateForm(_config.GetValue<string>("ParsianUrls:RedirectUrl"), "get", inputParams);
                        result.Message = Constants.HandShakeSuccess;
                        return result;
                    }
                }
                payment.PaymentStatusId = (int)PaymentStatusEnum.Failed;
                await _paymentRepository.AttachAsync(ObjectMapper.Map<PaymentDto, Payment>(payment), o => o.PaymentStatusId);
                result.Message = Constants.ErrorInHandShakeResult;
                return result;
            }
            catch (Exception ex)
            {
                await _paymentLogRepository.InsertAsync(new PaymentLog
                {
                    PaymentId = payment.Id,
                    Psp = PspEnum.Parsian.ToString(),
                    Message = Constants.HandShakeException,
                    Parameter = ex.Message,
                });
                payment.PaymentStatusId = (int)PaymentStatusEnum.Failed;
                await _paymentRepository.AttachAsync(ObjectMapper.Map<PaymentDto, Payment>(payment), o => o.PaymentStatusId);
                result.Message = Constants.ErrorInConnectToPsp;
                return result;
            }
        }
        private async Task<HandShakeOutputDto> HandShakeWithPasargadAsync(PaymentDto payment, string pspAccountJsonProps)
        {
            var result = new HandShakeOutputDto()
            {
                StatusCode = (int)StatusCodeEnum.Failed,
                Message = Constants.UnknownError,
                PaymentId = payment.Id
            };
            try
            {
                var pspAccountProps = JsonConvert.DeserializeObject<Pasargad.PspAccountProps>(pspAccountJsonProps);

                var authResult = await AuthenticatePasargadAsync(payment.Id, pspAccountProps.UserName, pspAccountProps.Password);
                if (authResult == null || string.IsNullOrEmpty(authResult.Token))
                {
                    result.PspJsonResult = authResult != null ? JsonConvert.SerializeObject(authResult) : string.Empty;
                    payment.PaymentStatusId = (int)PaymentStatusEnum.Failed;
                    await _paymentRepository.AttachAsync(ObjectMapper.Map<PaymentDto, Payment>(payment), o => o.PaymentStatusId);
                    result.Message = Constants.ErrorInAuthenticate;
                    return result;
                }

                PasargadHandShakeInput input = new()
                {
                    Token = authResult.Token,
                    Amount = (long)payment.Amount,
                    CallbackApi = _config.GetValue<string>("AppUrls:PaymentApi") + _config.GetValue<string>("PasargadUrls:CallBackUrl"),
                    Description = string.Empty,
                    Invoice = payment.Id.ToString(),
                    InvoiceDate = payment.TransactionDate.ToString("yyyy-MM-dd"),
                    MobileNumber = string.IsNullOrEmpty(payment.Mobile)
                    ? string.Empty
                    : (payment.Mobile[..1] == "98" ? string.Concat("0", payment.Mobile[2..]) : payment.Mobile),
                    PayerMail = string.Empty,
                    PayerName = string.Empty,
                    ServiceCode = 8,
                    ServiceType = "PURCHASE",
                    TerminalNumber = pspAccountProps.TerminalNumber,
                    NationalCode = payment.NationalCode,
                    Key = pspAccountProps.Key,
                    IV = pspAccountProps.IV
                };

                await _paymentLogRepository.InsertAsync(new PaymentLog
                {
                    PaymentId = payment.Id,
                    Psp = PspEnum.Pasargad.ToString(),
                    Message = Constants.HandShakeWithPspStart,
                    Parameter = JsonConvert.SerializeObject(input),
                });
                string handShakeResult = string.Empty;
                var connectToGatewayMode = _config.GetValue<bool>("App:ConnectToGatewayMode");
                if (connectToGatewayMode) //grpc
                {
                    handShakeResult = (await GatewayServiceGrpcClient().HandShakeWithPasargadAsync(input)).Result;
                }
                else //restApi
                {
                    string url = _config.GetValue<string>("AppUrls:GatewayApi") + _config.GetValue<string>("PasargadUrls:HandShake");
                    HttpClient client = new();
                    var response = await client.PostAsync(url, new StringContent(JsonConvert.SerializeObject(input), Encoding.UTF8, "application/json"));
                    handShakeResult = response.Content.ReadAsStringAsync().Result;
                    handShakeResult = JsonDocument.Parse(handShakeResult).RootElement.GetProperty("result").ToString();
                }
                await _paymentLogRepository.InsertAsync(new PaymentLog
                {
                    PaymentId = payment.Id,
                    Psp = PspEnum.Pasargad.ToString(),
                    Message = Constants.HandShakeResult,
                    Parameter = handShakeResult,
                });
                result.PspJsonResult = handShakeResult;
                if (handShakeResult != null)
                {
                    Pasargad.HandShakeJsonResult jResult = JsonConvert.DeserializeObject<Pasargad.HandShakeJsonResult>(handShakeResult);

                    result.Token = payment.Token = jResult.Data.UrlId;
                    await _paymentRepository.AttachAsync(ObjectMapper.Map<PaymentDto, Payment>(payment), o => o.Token);

                    if (jResult.ResultCode == 0)
                    {
                        result.StatusCode = (int)StatusCodeEnum.Success;
                        var inputParams = new System.Collections.Specialized.NameValueCollection();
                        result.HtmlContent = StringUtil.GenerateForm(jResult.Data.Url, "get", inputParams);
                        result.Message = Constants.HandShakeSuccess;
                        return result;
                    }
                }
                payment.PaymentStatusId = (int)PaymentStatusEnum.Failed;
                await _paymentRepository.AttachAsync(ObjectMapper.Map<PaymentDto, Payment>(payment), o => o.PaymentStatusId);
                result.Message = Constants.ErrorInHandShakeResult;
                return result;
            }
            catch (Exception ex)
            {
                await _paymentLogRepository.InsertAsync(new PaymentLog
                {
                    PaymentId = payment.Id,
                    Psp = PspEnum.Pasargad.ToString(),
                    Message = Constants.HandShakeException,
                    Parameter = ex.Message,
                });
                payment.PaymentStatusId = (int)PaymentStatusEnum.Failed;
                await _paymentRepository.AttachAsync(ObjectMapper.Map<PaymentDto, Payment>(payment), o => o.PaymentStatusId);
                result.Message = Constants.ErrorInConnectToPsp;
                return result;
            }
        }
        #endregion

        #region BackFromPsp
        [UnitOfWork(isTransactional: false)]
        public async Task<BackFromPspOutputDto> BackFromIranKishAsync(string pspJsonResult)
        {
            var pspResult = JsonConvert.DeserializeObject<IranKish.BackFromPspResult>(pspJsonResult);
            var payment = _paymentRepository.WithDetails().AsNoTracking()
                .Select(o => new PaymentDto
                {
                    Id = o.Id,
                    TraceNo = o.TraceNo,
                    TransactionCode = o.TransactionCode,
                    Token = o.Token,
                    PspAccountId = o.PspAccountId,
                    PaymentStatusId = o.PaymentStatusId,
                    AdditionalData = o.AdditionalData
                }).FirstOrDefault(o => o.Id == int.Parse(pspResult.requestId));

            var result = new BackFromPspOutputDto()
            {
                StatusCode = (int)StatusCodeEnum.Unknown,
                Message = Constants.UnknownError,
                PaymentId = payment != null ? payment.Id : 0,
                AdditionalData = payment != null ? payment.AdditionalData : string.Empty
            };

            if (payment == null) { return result; }

            try
            {
                if (_paymentLogRepository.WithDetails().AsNoTracking()
                    .Select(o => new { o.PaymentId, o.Message })
                    .Any(o => o.PaymentId == payment.Id && o.Message == Constants.BackFromPsp))
                {
                    await _paymentLogRepository.InsertAsync(new PaymentLog
                    {
                        PaymentId = payment.Id,
                        Psp = PspEnum.IranKish.ToString(),
                        Message = Constants.DuplicateBackFromPsp
                    });
                    //    await CurrentUnitOfWork.CompleteAsync();

                    result.Message = Constants.ErrorInBackFromPsp;
                    return result;
                }

                await _paymentLogRepository.InsertAsync(new PaymentLog
                {
                    PaymentId = payment.Id,
                    Psp = PspEnum.IranKish.ToString(),
                    Message = Constants.BackFromPsp,
                    Parameter = pspJsonResult
                });

                payment.TransactionCode = pspResult.retrievalReferenceNumber;
                payment.TraceNo = pspResult.systemTraceAuditNumber;
                await _paymentRepository.AttachAsync(ObjectMapper.Map<PaymentDto, Payment>(payment), o => o.TransactionCode, o => o.TraceNo);

                result.PspJsonResult = pspJsonResult;
                result.TransactionCode = pspResult.retrievalReferenceNumber;

                //در صورتي كه وضعيت پرداخت موفق است نبايد مجددن تاييديه ارسال شود
                if (payment.PaymentStatusId == (int)PaymentStatusEnum.Success)
                {
                    await _paymentLogRepository.InsertAsync(new PaymentLog
                    {
                        PaymentId = payment.Id,
                        Psp = PspEnum.IranKish.ToString(),
                        Message = Constants.DuplicateBackFromPsp,
                        Parameter = PaymentStatusEnum.Success.ToString(),
                    });
                    //    await CurrentUnitOfWork.CompleteAsync();

                    result.StatusCode = (int)StatusCodeEnum.Success;
                    result.Message = Constants.BackFromPspSuccess;
                    return result;
                }
                //در صورتي كه وضعيت پرداخت ناموفق است نبايد مجددن تاييديه ارسال شود            
                if (payment.PaymentStatusId == (int)PaymentStatusEnum.Failed)
                {
                    await _paymentLogRepository.InsertAsync(new PaymentLog
                    {
                        PaymentId = payment.Id,
                        Psp = PspEnum.IranKish.ToString(),
                        Message = Constants.DuplicateBackFromPsp,
                        Parameter = PaymentStatusEnum.Failed.ToString(),
                    });
                    //  await CurrentUnitOfWork.CompleteAsync();

                    result.StatusCode = (int)StatusCodeEnum.Failed;
                    result.Message = Constants.VerifyFailed;
                    return result;
                }
                if (string.IsNullOrEmpty(pspResult.OriginUrl) || pspResult.OriginUrl != _config.GetValue<string>("IranKishUrls:OriginUrl"))
                {
                    await _paymentLogRepository.InsertAsync(new PaymentLog
                    {
                        PaymentId = payment.Id,
                        Psp = PspEnum.IranKish.ToString(),
                        Message = Constants.BackFromPspWithInCorrectUrl,
                        Parameter = pspResult.OriginUrl == null ? "OriginUrl Is Null" : pspResult.OriginUrl,
                    });

                    payment.PaymentStatusId = (int)PaymentStatusEnum.Failed;
                    await _paymentRepository.AttachAsync(ObjectMapper.Map<PaymentDto, Payment>(payment), o => o.PaymentStatusId);
                    //    await CurrentUnitOfWork.CompleteAsync();

                    result.StatusCode = (int)StatusCodeEnum.Failed;
                    result.Message = Constants.ErrorInBackFromPspUrl;
                    return result;
                }
                if (string.IsNullOrEmpty(pspResult.responseCode) || pspResult.responseCode != "00")
                {
                    payment.PaymentStatusId = (int)PaymentStatusEnum.Failed;
                    await _paymentRepository.AttachAsync(ObjectMapper.Map<PaymentDto, Payment>(payment), o => o.PaymentStatusId);


                    result.StatusCode = (int)StatusCodeEnum.Failed;
                    result.Message = Constants.ErrorInBackFromPspResponseCode;
                    return result;
                }
                result.StatusCode = (int)StatusCodeEnum.Success;
                result.Message = Constants.BackFromPspSuccess;
                return result;
            }
            catch (Exception ex)
            {
                await _paymentLogRepository.InsertAsync(new PaymentLog
                {
                    PaymentId = payment.Id,
                    Psp = PspEnum.IranKish.ToString(),
                    Message = Constants.BackFromPspException,
                    Parameter = ex.Message
                });

                result.Message = Constants.ErrorInBackFromPsp;
                return result;
            }
        }
        [UnitOfWork(isTransactional: false)]
        public async Task<BackFromPspOutputDto> BackFromMellatAsync(string pspJsonResult)
        {
            var pspResult = JsonConvert.DeserializeObject<Mellat.BackFromPspResult>(pspJsonResult);
            var payment = _paymentRepository.WithDetails().AsNoTracking()
                .Select(o => new PaymentDto
                {
                    Id = o.Id,
                    TraceNo = o.TraceNo,
                    TransactionCode = o.TransactionCode,
                    Token = o.Token,
                    PspAccountId = o.PspAccountId,
                    PaymentStatusId = o.PaymentStatusId,
                    AdditionalData = o.AdditionalData
                })
                .FirstOrDefault(o => o.Id == int.Parse(pspResult.SaleOrderId));

            var result = new BackFromPspOutputDto()
            {
                StatusCode = (int)StatusCodeEnum.Unknown,
                Message = Constants.UnknownError,
                PaymentId = payment != null ? payment.Id : 0,
                AdditionalData = payment != null ? payment.AdditionalData : string.Empty
            };

            if (payment == null) { return result; }

            try
            {
                if (_paymentLogRepository.WithDetails().AsNoTracking()
                    .Select(o => new { o.PaymentId, o.Message })
                    .Any(o => o.PaymentId == payment.Id && o.Message == Constants.BackFromPsp))
                {
                    await _paymentLogRepository.InsertAsync(new PaymentLog
                    {
                        PaymentId = payment.Id,
                        Psp = PspEnum.Mellat.ToString(),
                        Message = Constants.DuplicateBackFromPsp
                    });
                    //       await CurrentUnitOfWork.CompleteAsync();

                    result.Message = Constants.ErrorInBackFromPsp;
                    return result;
                }

                await _paymentLogRepository.InsertAsync(new PaymentLog
                {
                    PaymentId = payment.Id,
                    Psp = PspEnum.Mellat.ToString(),
                    Message = Constants.BackFromPsp,
                    Parameter = pspJsonResult
                });

                payment.TransactionCode = pspResult.SaleReferenceId;
                await _paymentRepository.AttachAsync(ObjectMapper.Map<PaymentDto, Payment>(payment), o => o.TransactionCode);

                result.PspJsonResult = pspJsonResult;
                result.TransactionCode = pspResult.SaleReferenceId;

                //در صورتي كه وضعيت پرداخت موفق است نبايد مجددن تاييديه ارسال شود
                if (payment.PaymentStatusId == (int)PaymentStatusEnum.Success)
                {
                    await _paymentLogRepository.InsertAsync(new PaymentLog
                    {
                        PaymentId = payment.Id,
                        Psp = PspEnum.Mellat.ToString(),
                        Message = Constants.DuplicateBackFromPsp,
                        Parameter = PaymentStatusEnum.Success.ToString(),
                    });
                    //       await CurrentUnitOfWork.CompleteAsync();

                    result.StatusCode = (int)StatusCodeEnum.Success;
                    result.Message = Constants.BackFromPspSuccess;
                    return result;
                }
                //در صورتي كه وضعيت پرداخت ناموفق است نبايد مجددن تاييديه ارسال شود            
                if (payment.PaymentStatusId == (int)PaymentStatusEnum.Failed)
                {
                    await _paymentLogRepository.InsertAsync(new PaymentLog
                    {
                        PaymentId = payment.Id,
                        Psp = PspEnum.Mellat.ToString(),
                        Message = Constants.DuplicateBackFromPsp,
                        Parameter = PaymentStatusEnum.Failed.ToString(),
                    });
                    //      await CurrentUnitOfWork.CompleteAsync();

                    result.StatusCode = (int)StatusCodeEnum.Failed;
                    result.Message = Constants.VerifyFailed;
                    return result;
                }
                if (string.IsNullOrEmpty(pspResult.OriginUrl) || pspResult.OriginUrl != _config.GetValue<string>("MellatUrls:OriginUrl"))
                {
                    await _paymentLogRepository.InsertAsync(new PaymentLog
                    {
                        PaymentId = payment.Id,
                        Psp = PspEnum.Mellat.ToString(),
                        Message = Constants.BackFromPspWithInCorrectUrl,
                        Parameter = pspResult.OriginUrl == null ? "OriginUrl Is Null" : pspResult.OriginUrl,
                    });

                    payment.PaymentStatusId = (int)PaymentStatusEnum.Failed;
                    await _paymentRepository.AttachAsync(ObjectMapper.Map<PaymentDto, Payment>(payment), o => o.PaymentStatusId);
                    //    await CurrentUnitOfWork.CompleteAsync();

                    result.StatusCode = (int)StatusCodeEnum.Failed;
                    result.Message = Constants.ErrorInBackFromPspUrl;
                    return result;
                }
                if (string.IsNullOrEmpty(pspResult.ResCode) || pspResult.ResCode != "0" || pspResult.RefId != payment.Token)
                {
                    payment.PaymentStatusId = (int)PaymentStatusEnum.Failed;
                    await _paymentRepository.AttachAsync(ObjectMapper.Map<PaymentDto, Payment>(payment), o => o.PaymentStatusId);
                    //    await CurrentUnitOfWork.CompleteAsync();

                    result.StatusCode = (int)StatusCodeEnum.Failed;
                    result.Message = Constants.ErrorInBackFromPspResponseCode;
                    return result;
                }

                //    await CurrentUnitOfWork.CompleteAsync();

                result.StatusCode = (int)StatusCodeEnum.Success;
                result.Message = Constants.BackFromPspSuccess;
                return result;
            }
            catch (Exception ex)
            {
                await _paymentLogRepository.InsertAsync(new PaymentLog
                {
                    PaymentId = payment.Id,
                    Psp = PspEnum.Mellat.ToString(),
                    Message = Constants.BackFromPspException,
                    Parameter = ex.Message
                });
                //   await CurrentUnitOfWork.CompleteAsync();

                result.Message = Constants.ErrorInBackFromPsp;
                return result;
            }
        }
        [UnitOfWork(isTransactional: false)]
        public async Task<BackFromPspOutputDto> BackFromParsianAsync(string pspJsonResult)
        {
            var pspResult = JsonConvert.DeserializeObject<Parsian.BackFromPspResult>(pspJsonResult);
            var payment = (await _paymentRepository.GetQueryableAsync()).AsNoTracking()
                .Select(o => new PaymentDto
                {
                    Id = o.Id,
                    TraceNo = o.TraceNo,
                    TransactionCode = o.TransactionCode,
                    Token = o.Token,
                    PspAccountId = o.PspAccountId,
                    PaymentStatusId = o.PaymentStatusId,
                    AdditionalData = o.AdditionalData
                }).FirstOrDefault(o => o.Id == int.Parse(pspResult.OrderId));

            var result = new BackFromPspOutputDto()
            {
                StatusCode = (int)StatusCodeEnum.Unknown,
                Message = Constants.UnknownError,
                PaymentId = payment != null ? payment.Id : 0,
                AdditionalData = payment != null ? payment.AdditionalData : string.Empty
            };

            if (payment == null) { return result; }

            try
            {
                if ((await _paymentLogRepository.GetQueryableAsync()).AsNoTracking()
                    .Select(o => new { o.PaymentId, o.Message })
                    .Any(o => o.PaymentId == payment.Id && o.Message == Constants.BackFromPsp))
                {
                    await _paymentLogRepository.InsertAsync(new PaymentLog
                    {
                        PaymentId = payment.Id,
                        Psp = PspEnum.Parsian.ToString(),
                        Message = Constants.DuplicateBackFromPsp
                    });
                    result.Message = Constants.ErrorInBackFromPsp;
                    return result;
                }

                await _paymentLogRepository.InsertAsync(new PaymentLog
                {
                    PaymentId = payment.Id,
                    Psp = PspEnum.Parsian.ToString(),
                    Message = Constants.BackFromPsp,
                    Parameter = pspJsonResult
                });

                payment.TransactionCode = pspResult.RRN;
                payment.TraceNo = pspResult.STraceNo;
                await _paymentRepository.AttachAsync(ObjectMapper.Map<PaymentDto, Payment>(payment), o => o.TraceNo, o => o.TransactionCode);

                result.PspJsonResult = pspJsonResult;
                result.TransactionCode = pspResult.RRN;

                //در صورتي كه وضعيت پرداخت موفق است نبايد مجددن تاييديه ارسال شود
                if (payment.PaymentStatusId == (int)PaymentStatusEnum.Success)
                {
                    await _paymentLogRepository.InsertAsync(new PaymentLog
                    {
                        PaymentId = payment.Id,
                        Psp = PspEnum.Parsian.ToString(),
                        Message = Constants.DuplicateBackFromPsp,
                        Parameter = PaymentStatusEnum.Success.ToString(),
                    });
                    result.StatusCode = (int)StatusCodeEnum.Success;
                    result.Message = Constants.BackFromPspSuccess;
                    return result;
                }
                //در صورتي كه وضعيت پرداخت ناموفق است نبايد مجددن تاييديه ارسال شود            
                if (payment.PaymentStatusId == (int)PaymentStatusEnum.Failed)
                {
                    await _paymentLogRepository.InsertAsync(new PaymentLog
                    {
                        PaymentId = payment.Id,
                        Psp = PspEnum.Parsian.ToString(),
                        Message = Constants.DuplicateBackFromPsp,
                        Parameter = PaymentStatusEnum.Failed.ToString(),
                    });

                    result.StatusCode = (int)StatusCodeEnum.Failed;
                    result.Message = Constants.VerifyFailed;
                    return result;
                }
                if (string.IsNullOrEmpty(pspResult.OriginUrl) || pspResult.OriginUrl != _config.GetValue<string>("ParsianUrls:OriginUrl"))
                {
                    await _paymentLogRepository.InsertAsync(new PaymentLog
                    {
                        PaymentId = payment.Id,
                        Psp = PspEnum.Parsian.ToString(),
                        Message = Constants.BackFromPspWithInCorrectUrl,
                        Parameter = pspResult.OriginUrl == null ? "OriginUrl Is Null" : pspResult.OriginUrl,
                    });

                    payment.PaymentStatusId = (int)PaymentStatusEnum.Failed;
                    await _paymentRepository.AttachAsync(ObjectMapper.Map<PaymentDto, Payment>(payment), o => o.PaymentStatusId);

                    result.StatusCode = (int)StatusCodeEnum.Failed;
                    result.Message = Constants.ErrorInBackFromPspUrl;
                    return result;
                }
                if (string.IsNullOrEmpty(pspResult.status) || pspResult.status != "0")
                {
                    payment.PaymentStatusId = (int)PaymentStatusEnum.Failed;
                    await _paymentRepository.AttachAsync(ObjectMapper.Map<PaymentDto, Payment>(payment), o => o.PaymentStatusId);


                    result.StatusCode = (int)StatusCodeEnum.Failed;
                    result.Message = Constants.ErrorInBackFromPspResponseCode;
                    return result;
                }

                result.StatusCode = (int)StatusCodeEnum.Success;
                result.Message = Constants.BackFromPspSuccess;
                return result;
            }
            catch (Exception ex)
            {
                await _paymentLogRepository.InsertAsync(new PaymentLog
                {
                    PaymentId = payment.Id,
                    Psp = PspEnum.Parsian.ToString(),
                    Message = Constants.BackFromPspException,
                    Parameter = ex.Message
                });

                result.Message = Constants.ErrorInBackFromPsp;
                return result;
            }
        }
        [UnitOfWork(isTransactional: false)]
        public async Task<BackFromPspOutputDto> BackFromPasargadAsync(string pspJsonResult)
        {
            var pspResult = JsonConvert.DeserializeObject<Pasargad.BackFromPspResult>(pspJsonResult);
            var payment = (await _paymentRepository.GetQueryableAsync()).AsNoTracking()
                .Select(o => new PaymentDto
                {
                    Id = o.Id,
                    TraceNo = o.TraceNo,
                    TransactionCode = o.TransactionCode,
                    Token = o.Token,
                    PspAccountId = o.PspAccountId,
                    PaymentStatusId = o.PaymentStatusId,
                    AdditionalData = o.AdditionalData
                }).FirstOrDefault(o => o.Id == int.Parse(pspResult.InvoiceId));

            var result = new BackFromPspOutputDto()
            {
                StatusCode = (int)StatusCodeEnum.Unknown,
                Message = Constants.UnknownError,
                PaymentId = payment != null ? payment.Id : 0,
                AdditionalData = payment != null ? payment.AdditionalData : string.Empty
            };

            if (payment == null) { return result; }

            try
            {
                if ((await _paymentLogRepository.GetQueryableAsync()).AsNoTracking()
                    .Select(o => new { o.PaymentId, o.Message })
                    .Any(o => o.PaymentId == payment.Id && o.Message == Constants.BackFromPsp))
                {
                    await _paymentLogRepository.InsertAsync(new PaymentLog
                    {
                        PaymentId = payment.Id,
                        Psp = PspEnum.Pasargad.ToString(),
                        Message = Constants.DuplicateBackFromPsp
                    });
                    result.Message = Constants.ErrorInBackFromPsp;
                    return result;
                }

                await _paymentLogRepository.InsertAsync(new PaymentLog
                {
                    PaymentId = payment.Id,
                    Psp = PspEnum.Pasargad.ToString(),
                    Message = Constants.BackFromPsp,
                    Parameter = pspJsonResult
                });

                payment.TransactionCode = pspResult.ReferenceNumber;
                payment.TraceNo = pspResult.TrackId;
                await _paymentRepository.AttachAsync(ObjectMapper.Map<PaymentDto, Payment>(payment), o => o.TraceNo, o => o.TransactionCode);

                result.PspJsonResult = pspJsonResult;
                result.TransactionCode = pspResult.ReferenceNumber;

                //در صورتي كه وضعيت پرداخت موفق است نبايد مجددن تاييديه ارسال شود
                if (payment.PaymentStatusId == (int)PaymentStatusEnum.Success)
                {
                    await _paymentLogRepository.InsertAsync(new PaymentLog
                    {
                        PaymentId = payment.Id,
                        Psp = PspEnum.Pasargad.ToString(),
                        Message = Constants.DuplicateBackFromPsp,
                        Parameter = PaymentStatusEnum.Success.ToString(),
                    });
                    result.StatusCode = (int)StatusCodeEnum.Success;
                    result.Message = Constants.BackFromPspSuccess;
                    return result;
                }
                //در صورتي كه وضعيت پرداخت ناموفق است نبايد مجددن تاييديه ارسال شود            
                if (payment.PaymentStatusId == (int)PaymentStatusEnum.Failed)
                {
                    await _paymentLogRepository.InsertAsync(new PaymentLog
                    {
                        PaymentId = payment.Id,
                        Psp = PspEnum.Pasargad.ToString(),
                        Message = Constants.DuplicateBackFromPsp,
                        Parameter = PaymentStatusEnum.Failed.ToString(),
                    });

                    result.StatusCode = (int)StatusCodeEnum.Failed;
                    result.Message = Constants.VerifyFailed;
                    return result;
                }
                if (string.IsNullOrEmpty(pspResult.OriginUrl) || pspResult.OriginUrl != _config.GetValue<string>("PasargadUrls:OriginUrl"))
                {
                    await _paymentLogRepository.InsertAsync(new PaymentLog
                    {
                        PaymentId = payment.Id,
                        Psp = PspEnum.Pasargad.ToString(),
                        Message = Constants.BackFromPspWithInCorrectUrl,
                        Parameter = pspResult.OriginUrl == null ? "OriginUrl Is Null" : pspResult.OriginUrl,
                    });

                    payment.PaymentStatusId = (int)PaymentStatusEnum.Failed;
                    await _paymentRepository.AttachAsync(ObjectMapper.Map<PaymentDto, Payment>(payment), o => o.PaymentStatusId);

                    result.StatusCode = (int)StatusCodeEnum.Failed;
                    result.Message = Constants.ErrorInBackFromPspUrl;
                    return result;
                }

                switch (pspResult.Status.ToLower())
                {
                    case "success":
                        result.StatusCode = (int)StatusCodeEnum.Success;
                        result.Message = Constants.BackFromPspSuccess;
                        return result;

                    case "failed":
                    case "cancel":
                        payment.PaymentStatusId = (int)PaymentStatusEnum.Failed;
                        await _paymentRepository.AttachAsync(ObjectMapper.Map<PaymentDto, Payment>(payment), o => o.PaymentStatusId);
                        result.StatusCode = (int)StatusCodeEnum.Failed;
                        result.Message = Constants.ErrorInBackFromPspResponseCode;
                        return result;

                    case "unknown":
                    default:
                        return result;
                }
            }
            catch (Exception ex)
            {
                await _paymentLogRepository.InsertAsync(new PaymentLog
                {
                    PaymentId = payment.Id,
                    Psp = PspEnum.Pasargad.ToString(),
                    Message = Constants.BackFromPspException,
                    Parameter = ex.Message
                });

                result.Message = Constants.ErrorInBackFromPsp;
                return result;
            }
        }
        #endregion

        #region Verify
        [UnitOfWork(isTransactional: false)]
        public async Task<VerifyOutputDto> VerifyAsync(int paymentId)
        {
            var result = new VerifyOutputDto()
            {
                StatusCode = (int)StatusCodeEnum.Unknown,
                Message = Constants.UnknownError,
                PaymentId = paymentId
            };
            var payment = _paymentRepository.WithDetails().AsNoTracking()
                .Select(o => new PaymentDto
                {
                    Id = o.Id,
                    TraceNo = o.TraceNo,
                    TransactionCode = o.TransactionCode,
                    Token = o.Token,
                    PspAccountId = o.PspAccountId,
                    PaymentStatusId = o.PaymentStatusId
                }).FirstOrDefault(o => o.Id == paymentId);
            if (payment == null) { return result; }
            var pspAccount = _pspAccountRepository.WithDetails().AsNoTracking().Select(o => new { o.Id, o.PspId, o.JsonProps }).First(o => o.Id == payment.PspAccountId);
            switch ((PspEnum)pspAccount.PspId)
            {
                case PspEnum.Mellat:
                    return await VerifyToMellatAsync(payment, pspAccount.JsonProps, false);
                case PspEnum.IranKish:
                    return await VerifyToIranKishAsync(payment, pspAccount.JsonProps, false);
                case PspEnum.Parsian:
                    return await VerifyToParsianAsync(payment, pspAccount.JsonProps, false);
                case PspEnum.Pasargad:
                    return await VerifyToPasargadAsync(payment, pspAccount.JsonProps, false);
            }
            return result;
        }
        private async Task<VerifyOutputDto> VerifyToIranKishAsync(PaymentDto payment, string pspAccountJsonProps, bool isRetryForVerify)
        {
            var result = new VerifyOutputDto()
            {
                StatusCode = (int)StatusCodeEnum.Unknown,
                Message = Constants.UnknownError,
                PaymentId = payment.Id
            };
            try
            {
                var pspAccountProps = JsonConvert.DeserializeObject<IranKish.PspAccountProps>(pspAccountJsonProps);
                IranKishVerifyInput input = new()
                {
                    TerminalId = pspAccountProps.TerminalId,
                    SystemTraceAuditNumber = payment.TraceNo,
                    RetrievalReferenceNumber = payment.TransactionCode,
                    TokenIdentity = payment.Token
                };
                await _paymentLogRepository.InsertAsync(new PaymentLog
                {
                    PaymentId = payment.Id,
                    Psp = PspEnum.IranKish.ToString(),
                    Message = isRetryForVerify ? Constants.RetryForVerifyStart : Constants.VerifyStart,
                    Parameter = JsonConvert.SerializeObject(input)
                });
                string verifyResult = string.Empty;
                var connectToGatewayMode = _config.GetValue<bool>("App:ConnectToGatewayMode");
                if (connectToGatewayMode) //grpc
                {
                    verifyResult = (await GatewayServiceGrpcClient().VerifyToIranKishAsync(input)).Result;
                }
                else //restApi
                {
                    string url = _config.GetValue<string>("AppUrls:GatewayApi") + _config.GetValue<string>("IranKishUrls:Verify");
                    HttpClient client = new();
                    var response = await client.PostAsync(url, new StringContent(JsonConvert.SerializeObject(input), Encoding.UTF8, "application/json"));
                    verifyResult = response.Content.ReadAsStringAsync().Result;
                    verifyResult = JsonDocument.Parse(verifyResult).RootElement.GetProperty("result").ToString();
                }
                await _paymentLogRepository.InsertAsync(new PaymentLog
                {
                    PaymentId = payment.Id,
                    Psp = PspEnum.IranKish.ToString(),
                    Message = isRetryForVerify ? Constants.RetryForVerifyResult : Constants.VerifyResult,
                    Parameter = verifyResult,
                });
                result.PspJsonResult = verifyResult;
                if (verifyResult != null)
                {
                    IranKish.VerifyJsonResult jResult = JsonConvert.DeserializeObject<IranKish.VerifyJsonResult>(verifyResult);
                    if (jResult.status && jResult.responseCode == "00")
                    {
                        payment.PaymentStatusId = (int)PaymentStatusEnum.Success;
                        await _paymentRepository.AttachAsync(ObjectMapper.Map<PaymentDto, Payment>(payment), o => o.PaymentStatusId);

                        result.StatusCode = (int)StatusCodeEnum.Success;
                        result.Message = Constants.VerifySuccess;
                        return result;
                    }
                }
                payment.PaymentStatusId = (int)PaymentStatusEnum.Failed;
                await _paymentRepository.AttachAsync(ObjectMapper.Map<PaymentDto, Payment>(payment), o => o.PaymentStatusId);
                result.StatusCode = (int)StatusCodeEnum.Failed;
                result.Message = Constants.VerifyFailed;
                return result;
            }
            catch (Exception ex)
            {
                await _paymentLogRepository.InsertAsync(new PaymentLog
                {
                    PaymentId = payment.Id,
                    Psp = PspEnum.IranKish.ToString(),
                    Message = isRetryForVerify ? Constants.RetryForVerifyException : Constants.VerifyException,
                    Parameter = ex.Message
                });
                result.Message = Constants.ErrorInVerify;
                return result;
            }
        }
        private async Task<VerifyOutputDto> VerifyToMellatAsync(PaymentDto payment, string pspAccountJsonProps, bool isRetryForVerify)
        {
            var result = new VerifyOutputDto()
            {
                StatusCode = (int)StatusCodeEnum.Unknown,
                Message = Constants.UnknownError,
                PaymentId = payment.Id
            };
            try
            {
                var pspAccountProps = JsonConvert.DeserializeObject<Mellat.PspAccountProps>(pspAccountJsonProps);
                MellatVerifyInput input = new()
                {
                    TerminalId = pspAccountProps.TerminalId,
                    UserName = pspAccountProps.UserName,
                    UserPassword = pspAccountProps.UserPassword,
                    OrderId = payment.Id,
                    SaleOrderId = payment.Id,
                    SaleReferenceId = long.Parse(payment.TransactionCode),
                    Switch = pspAccountProps.Switch
                };
                await _paymentLogRepository.InsertAsync(new PaymentLog
                {
                    PaymentId = payment.Id,
                    Psp = PspEnum.Mellat.ToString(),
                    Message = isRetryForVerify ? Constants.RetryForVerifyStart : Constants.VerifyStart,
                    Parameter = JsonConvert.SerializeObject(input),
                });
                string verifyResult = string.Empty;
                var connectToGatewayMode = _config.GetValue<bool>("App:ConnectToGatewayMode");
                if (connectToGatewayMode) //grpc
                {
                    verifyResult = (await GatewayServiceGrpcClient().VerifyToMellatAsync(input)).Result;
                }
                else //restApi
                {
                    string url = _config.GetValue<string>("AppUrls:GatewayApi") + _config.GetValue<string>("MellatUrls:Verify");
                    HttpClient client = new();
                    var response = await client.PostAsync(url, new StringContent(JsonConvert.SerializeObject(input), Encoding.UTF8, "application/json"));
                    verifyResult = response.Content.ReadAsStringAsync().Result;
                    verifyResult = JsonDocument.Parse(verifyResult).RootElement.GetProperty("result").ToString();
                }
                await _paymentLogRepository.InsertAsync(new PaymentLog
                {
                    PaymentId = payment.Id,
                    Psp = PspEnum.Mellat.ToString(),
                    Message = isRetryForVerify ? Constants.RetryForVerifyResult : Constants.VerifyResult,
                    Parameter = verifyResult,
                });
                result.PspJsonResult = verifyResult;
                var verifyRes = JsonConvert.DeserializeObject<MellatOutputDto>(result.PspJsonResult);

                //ResCode = 0 پرداخت موفق
                //ResCode = 43 پرداخت موفق است و درخواست تاییدیه تکراری ارسال شده است
                //ResCode = 415 session time out در این حالت پرداخت ناموفق نیست و باید مجددن وضعیت آن استعلام گرفته شود
                if (string.IsNullOrEmpty(result.PspJsonResult) || verifyRes.Body.@return is not ("0" or "43" or "415"))
                {
                    payment.PaymentStatusId = (int)PaymentStatusEnum.Failed;
                    await _paymentRepository.AttachAsync(ObjectMapper.Map<PaymentDto, Payment>(payment), o => o.PaymentStatusId);

                    result.StatusCode = (int)StatusCodeEnum.Failed;
                    result.Message = Constants.VerifyFailed;
                    return result;
                }
                if (verifyRes.Body.@return is "0" or "43")
                {
                    payment.PaymentStatusId = (int)PaymentStatusEnum.Success;
                    await _paymentRepository.AttachAsync(ObjectMapper.Map<PaymentDto, Payment>(payment), o => o.PaymentStatusId);

                    result.StatusCode = (int)StatusCodeEnum.Success;
                    result.Message = Constants.VerifySuccess;
                    return result;
                }
                result.Message = Constants.ErrorInVerify;
                return result;
            }
            catch (Exception ex)
            {
                await _paymentLogRepository.InsertAsync(new PaymentLog
                {
                    PaymentId = payment.Id,
                    Psp = PspEnum.Mellat.ToString(),
                    Message = isRetryForVerify ? Constants.RetryForVerifyException : Constants.VerifyException,
                    Parameter = ex.Message
                });
                result.Message = Constants.ErrorInVerify;
                return result;
            }
        }
        private async Task<VerifyOutputDto> VerifyToParsianAsync(PaymentDto payment, string pspAccountJsonProps, bool isRetryForVerify)
        {
            var result = new VerifyOutputDto()
            {
                StatusCode = (int)StatusCodeEnum.Unknown,
                Message = Constants.UnknownError,
                PaymentId = payment.Id
            };
            try
            {
                var pspAccountProps = JsonConvert.DeserializeObject<Parsian.PspAccountProps>(pspAccountJsonProps);
                ParsianVerifyInput input = new()
                {
                    LoginAccount = pspAccountProps.LoginAccount,
                    Token = long.Parse(payment.Token)
                };
                await _paymentLogRepository.InsertAsync(new PaymentLog
                {
                    PaymentId = payment.Id,
                    Psp = PspEnum.Parsian.ToString(),
                    Message = isRetryForVerify ? Constants.RetryForVerifyStart : Constants.VerifyStart,
                    Parameter = JsonConvert.SerializeObject(input)
                });
                string verifyResult = string.Empty;
                var connectToGatewayMode = _config.GetValue<bool>("App:ConnectToGatewayMode");
                if (connectToGatewayMode) //grpc
                {
                    verifyResult = (await GatewayServiceGrpcClient().VerifyToParsianAsync(input)).Result;
                }
                else //restApi
                {
                    string url = _config.GetValue<string>("AppUrls:GatewayApi") + _config.GetValue<string>("ParsianUrls:Verify");
                    HttpClient client = new();
                    var response = await client.PostAsync(url, new StringContent(JsonConvert.SerializeObject(input), Encoding.UTF8, "application/json"));
                    verifyResult = response.Content.ReadAsStringAsync().Result;
                    verifyResult = JsonDocument.Parse(verifyResult).RootElement.GetProperty("result").ToString();
                }
                await _paymentLogRepository.InsertAsync(new PaymentLog
                {
                    PaymentId = payment.Id,
                    Psp = PspEnum.Parsian.ToString(),
                    Message = isRetryForVerify ? Constants.RetryForVerifyResult : Constants.VerifyResult,
                    Parameter = verifyResult,
                });
                result.PspJsonResult = verifyResult;
                if (verifyResult != null)
                {
                    Parsian.VerifyJsonResult jResult = JsonConvert.DeserializeObject<Parsian.VerifyJsonResult>(verifyResult);
                    if (jResult.Status == 0)
                    {
                        payment.PaymentStatusId = (int)PaymentStatusEnum.Success;
                        await _paymentRepository.AttachAsync(ObjectMapper.Map<PaymentDto, Payment>(payment), o => o.PaymentStatusId);

                        result.StatusCode = (int)StatusCodeEnum.Success;
                        result.Message = Constants.VerifySuccess;
                        return result;
                    }
                }
                payment.PaymentStatusId = (int)PaymentStatusEnum.Failed;
                await _paymentRepository.AttachAsync(ObjectMapper.Map<PaymentDto, Payment>(payment), o => o.PaymentStatusId);
                result.StatusCode = (int)StatusCodeEnum.Failed;
                result.Message = Constants.VerifyFailed;
                return result;
            }
            catch (Exception ex)
            {
                await _paymentLogRepository.InsertAsync(new PaymentLog
                {
                    PaymentId = payment.Id,
                    Psp = PspEnum.Parsian.ToString(),
                    Message = isRetryForVerify ? Constants.RetryForVerifyException : Constants.VerifyException,
                    Parameter = ex.Message
                });
                result.Message = Constants.ErrorInVerify;
                return result;
            }
        }
        private async Task<VerifyOutputDto> VerifyToPasargadAsync(PaymentDto payment, string pspAccountJsonProps, bool isRetryForVerify)
        {
            var result = new VerifyOutputDto()
            {
                StatusCode = (int)StatusCodeEnum.Unknown,
                Message = Constants.UnknownError,
                PaymentId = payment.Id
            };
            try
            {
                var pspAccountProps = JsonConvert.DeserializeObject<Pasargad.PspAccountProps>(pspAccountJsonProps);
                var authResult = await AuthenticatePasargadAsync(payment.Id, pspAccountProps.UserName, pspAccountProps.Password);
                if (authResult == null || string.IsNullOrEmpty(authResult.Token))
                {
                    result.PspJsonResult = authResult != null ? JsonConvert.SerializeObject(authResult) : string.Empty;
                    result.Message = Constants.ErrorInAuthenticate;
                    return result;
                }

                var inquiryResult = await InquiryToPasargadAsync(payment, pspAccountJsonProps);
                if (inquiryResult.StatusCode != (int)StatusCodeEnum.Success || payment.PaymentStatusId != (int)PaymentStatusEnum.InProgress)
                {
                    if (inquiryResult.StatusCode != (int)StatusCodeEnum.Success)
                    {
                        result.StatusCode = (int)StatusCodeEnum.Unknown;
                        result.Message = inquiryResult.Message;
                    }
                    else
                    {
                        if (payment.PaymentStatusId == (int)PaymentStatusEnum.Success)
                        {
                            result.StatusCode = (int)StatusCodeEnum.Success;
                            result.Message = Constants.VerifySuccess;

                        }
                        else if (payment.PaymentStatusId == (int)PaymentStatusEnum.Failed)
                        {
                            result.StatusCode = (int)StatusCodeEnum.Failed;
                            result.Message = Constants.VerifyFailed;
                        }
                    }
                    result.PaymentId = inquiryResult.PaymentId;
                    result.PspJsonResult = inquiryResult.PspJsonResult;
                    return result;
                }

                PasargadVerifyInput input = new()
                {
                    Token = authResult.Token,
                    Invoice = payment.Id.ToString(),
                    UrlId = payment.Token
                };

                await _paymentLogRepository.InsertAsync(new PaymentLog
                {
                    PaymentId = payment.Id,
                    Psp = PspEnum.Pasargad.ToString(),
                    Message = isRetryForVerify ? Constants.RetryForVerifyStart : Constants.VerifyStart,
                    Parameter = JsonConvert.SerializeObject(input)
                });
                string verifyResult = string.Empty;
                var connectToGatewayMode = _config.GetValue<bool>("App:ConnectToGatewayMode");
                if (connectToGatewayMode) //grpc
                {
                    verifyResult = (await GatewayServiceGrpcClient().VerifyToPasargadAsync(input)).Result;
                }
                else //restApi
                {
                    string url = _config.GetValue<string>("AppUrls:GatewayApi") + _config.GetValue<string>("PasargadUrls:Verify");
                    HttpClient client = new();
                    var response = await client.PostAsync(url, new StringContent(JsonConvert.SerializeObject(input), Encoding.UTF8, "application/json"));
                    verifyResult = response.Content.ReadAsStringAsync().Result;
                    verifyResult = JsonDocument.Parse(verifyResult).RootElement.GetProperty("result").ToString();
                }
                await _paymentLogRepository.InsertAsync(new PaymentLog
                {
                    PaymentId = payment.Id,
                    Psp = PspEnum.Pasargad.ToString(),
                    Message = isRetryForVerify ? Constants.RetryForVerifyResult : Constants.VerifyResult,
                    Parameter = verifyResult,
                });
                result.PspJsonResult = verifyResult;
                if (verifyResult != null)
                {
                    Pasargad.Verify.VerifyJsonResult jResult = JsonConvert.DeserializeObject<Pasargad.Verify.VerifyJsonResult>(verifyResult);
                    //13031 => تراکنش در انتظار تایید است
                    //13029 => تراکنش قبلا تایید شده است 
                    if (jResult.ResultCode is 0 or 2 or 13029 or 13031)
                    {
                        payment.PaymentStatusId = (int)PaymentStatusEnum.Success;
                        await _paymentRepository.AttachAsync(ObjectMapper.Map<PaymentDto, Payment>(payment), o => o.PaymentStatusId);
                        result.StatusCode = (int)StatusCodeEnum.Success;
                        result.Message = Constants.VerifySuccess;
                        return result;
                    }
                }
                payment.PaymentStatusId = (int)PaymentStatusEnum.Failed;
                await _paymentRepository.AttachAsync(ObjectMapper.Map<PaymentDto, Payment>(payment), o => o.PaymentStatusId);
                result.StatusCode = (int)StatusCodeEnum.Failed;
                result.Message = Constants.VerifyFailed;
                return result;
            }
            catch (Exception ex)
            {
                await _paymentLogRepository.InsertAsync(new PaymentLog
                {
                    PaymentId = payment.Id,
                    Psp = PspEnum.Pasargad.ToString(),
                    Message = isRetryForVerify ? Constants.RetryForVerifyException : Constants.VerifyException,
                    Parameter = ex.Message
                });
                result.Message = Constants.ErrorInVerify;
                return result;
            }
        }
        #endregion

        #region Inquiry
        [UnitOfWork(isTransactional: false)]
        public async Task<InquiryOutputDto> InquiryAsync(int paymentId)
        {
            var result = new InquiryOutputDto()
            {
                StatusCode = (int)StatusCodeEnum.Unknown,
                Message = Constants.UnknownError,
                PaymentId = paymentId
            };
            var payment = _paymentRepository.WithDetails().AsNoTracking()
                .Select(o => new PaymentDto
                {
                    Id = o.Id,
                    TraceNo = o.TraceNo,
                    TransactionCode = o.TransactionCode,
                    Token = o.Token,
                    PspAccountId = o.PspAccountId,
                    PaymentStatusId = o.PaymentStatusId
                }).FirstOrDefault(o => o.Id == paymentId);

            if (payment == null) { return result; }
            var pspAccount = _pspAccountRepository.WithDetails().AsNoTracking().Select(o => new { o.Id, o.PspId, o.JsonProps }).First(o => o.Id == payment.PspAccountId);
            switch ((PspEnum)pspAccount.PspId)
            {
                case PspEnum.Mellat:
                    return await InquiryToMellatAsync(payment, pspAccount.JsonProps);
                case PspEnum.IranKish:
                    return await InquiryToIranKishAsync(payment, pspAccount.JsonProps);
                case PspEnum.Parsian:
                    return await InquiryToParsianAsync(payment, pspAccount.JsonProps);
                case PspEnum.Pasargad:
                    return await InquiryToPasargadAsync(payment, pspAccount.JsonProps);
            }
            return result;
        }
        private async Task<InquiryOutputDto> InquiryToIranKishAsync(PaymentDto payment, string pspAccountJsonProps)
        {
            var result = new InquiryOutputDto()
            {
                StatusCode = (int)StatusCodeEnum.Unknown,
                Message = Constants.UnknownError,
                PaymentId = payment.Id
            };
            try
            {
                var pspAccountProps = JsonConvert.DeserializeObject<IranKish.PspAccountProps>(pspAccountJsonProps);

                IranKishInquiryInput input = new()
                {
                    TerminalId = pspAccountProps.TerminalId,
                    PassPhrase = pspAccountProps.PassPhrase,
                    TokenIdentity = payment.Token,
                    FindOption = 2 // جستجو بر اساس شناسه نشانه
                };
                await _paymentLogRepository.InsertAsync(new PaymentLog
                {
                    PaymentId = payment.Id,
                    Psp = PspEnum.IranKish.ToString(),
                    Message = Constants.InquiryStart,
                    Parameter = JsonConvert.SerializeObject(input),
                });
                string inquiryResult = string.Empty;
                var connectToGatewayMode = _config.GetValue<bool>("App:ConnectToGatewayMode");
                if (connectToGatewayMode) //grpc
                {
                    inquiryResult = (await GatewayServiceGrpcClient().InquiryToIranKishAsync(input)).Result;
                }
                else //restApi
                {
                    string url = _config.GetValue<string>("AppUrls:GatewayApi") + _config.GetValue<string>("IranKishUrls:Inquiry");
                    HttpClient client = new();
                    var response = await client.PostAsync(url, new StringContent(JsonConvert.SerializeObject(input), Encoding.UTF8, "application/json"));
                    inquiryResult = response.Content.ReadAsStringAsync().Result;
                    inquiryResult = JsonDocument.Parse(inquiryResult).RootElement.GetProperty("result").ToString();
                }
                await _paymentLogRepository.InsertAsync(new PaymentLog
                {
                    PaymentId = payment.Id,
                    Psp = PspEnum.IranKish.ToString(),
                    Message = Constants.InquiryResult,
                    Parameter = inquiryResult
                });
                result.PspJsonResult = inquiryResult;
                if (inquiryResult != null)
                {
                    IranKish.InquiryJsonResult jResult = JsonConvert.DeserializeObject<IranKish.InquiryJsonResult>(inquiryResult);

                    if (jResult.status && jResult.responseCode == "00")
                    {
                        payment.TransactionCode = jResult.result.retrievalReferenceNumber;
                        payment.TraceNo = jResult.result.systemTraceAuditNumber;
                        payment.PaymentStatusId = jResult.result.isReversed ? (int)PaymentStatusEnum.Failed : (jResult.result.isVerified ? (int)PaymentStatusEnum.Success : payment.PaymentStatusId);
                        await _paymentRepository.AttachAsync(ObjectMapper.Map<PaymentDto, Payment>(payment), o => o.TransactionCode, o => o.TraceNo, o => o.PaymentStatusId);
                    }
                    else if (!jResult.status && jResult.responseCode == "30")
                    {
                        payment.PaymentStatusId = (int)PaymentStatusEnum.Failed;
                        await _paymentRepository.AttachAsync(ObjectMapper.Map<PaymentDto, Payment>(payment), o => o.PaymentStatusId);
                    }
                    //todo:باید سایر حالات خروجی استعلام بررسی شود و این قسمت تکمیل شود
                    result.PaymentStatus = payment.PaymentStatusId;
                    result.PaymentStatusDescription = EnumExtension.GetEnumDescription((PaymentStatusEnum)payment.PaymentStatusId);
                    result.StatusCode = (int)StatusCodeEnum.Success;
                    result.Message = Constants.InquirySuccess;
                    return result;
                }
                result.Message = Constants.InquiryFailed;
                return result;
            }
            catch (Exception ex)
            {
                await _paymentLogRepository.InsertAsync(new PaymentLog
                {
                    PaymentId = payment.Id,
                    Psp = PspEnum.IranKish.ToString(),
                    Message = Constants.InquiryException,
                    Parameter = ex.Message
                });
                result.Message = Constants.ErrorInInquiry;
                return result;
            }
        }
        private async Task<InquiryOutputDto> InquiryToMellatAsync(PaymentDto payment, string pspAccountJsonProps)
        {
            var result = new InquiryOutputDto()
            {
                StatusCode = (int)StatusCodeEnum.Unknown,
                Message = Constants.UnknownError,
                PaymentId = payment.Id
            };
            try
            {
                var pspAccountProps = JsonConvert.DeserializeObject<Mellat.PspAccountProps>(pspAccountJsonProps);
                MellatInquiryInput input = new()
                {
                    TerminalId = pspAccountProps.TerminalId,
                    ReportServiceUserName = pspAccountProps.ReportServiceUserName,
                    ReportServicePassword = pspAccountProps.ReportServicePassword,
                    OrderId = payment.Id,
                    Switch = pspAccountProps.Switch
                };
                await _paymentLogRepository.InsertAsync(new PaymentLog
                {
                    PaymentId = payment.Id,
                    Psp = PspEnum.Mellat.ToString(),
                    Message = Constants.InquiryStart,
                    Parameter = JsonConvert.SerializeObject(input),
                });
                string inquiryResult = string.Empty;
                var connectToGatewayMode = _config.GetValue<bool>("App:ConnectToGatewayMode");
                if (connectToGatewayMode) //grpc
                {
                    inquiryResult = (await GatewayServiceGrpcClient().InquiryToMellatAsync(input)).Result;
                }
                else //restApi
                {
                    string url = _config.GetValue<string>("AppUrls:GatewayApi") + _config.GetValue<string>("MellatUrls:Inquiry");
                    HttpClient client = new();
                    var response = await client.PostAsync(url, new StringContent(JsonConvert.SerializeObject(input), Encoding.UTF8, "application/json"));
                    inquiryResult = response.Content.ReadAsStringAsync().Result;
                    inquiryResult = JsonDocument.Parse(inquiryResult).RootElement.GetProperty("result").ToString();
                }
                await _paymentLogRepository.InsertAsync(new PaymentLog
                {
                    PaymentId = payment.Id,
                    Psp = PspEnum.Mellat.ToString(),
                    Message = Constants.InquiryResult,
                    Parameter = inquiryResult,
                });
                result.PspJsonResult = inquiryResult;
                if (inquiryResult != null)
                {
                    var serializer = new XmlSerializer(typeof(Response));
                    using TextReader reader = new StringReader(inquiryResult);
                    Response response = (Response)serializer.Deserialize(reader);
                    //914=>Transaction Not Found
                    if (response.Record.First().Field.First().Value == "000" || response.Record.First().Field.First().Value == "914")
                    {
                        response.Record.RemoveAt(0);
                        if (response.Record.Any())
                        {
                            var record = response.Record.First();
                            payment.TransactionCode = record.Field.First(o => o.Name == "internalRefrenceId" || o.Name == "internalReferenceId").Value;
                            var transactionStatus = record.Field.First(o => o.Name == "transactionStatus").Value;
                            //todo:این قسمت با حالت های مختلف باید تست و تکمیل شود
                            payment.PaymentStatusId = transactionStatus == "Successful" ? (int)PaymentStatusEnum.Success : payment.PaymentStatusId;
                            await _paymentRepository.AttachAsync(ObjectMapper.Map<PaymentDto, Payment>(payment), o => o.TransactionCode, o => o.PaymentStatusId);
                        }
                        result.PaymentStatus = payment.PaymentStatusId;
                        result.PaymentStatusDescription = EnumExtension.GetEnumDescription((PaymentStatusEnum)payment.PaymentStatusId);
                        result.StatusCode = (int)StatusCodeEnum.Success;
                        result.Message = Constants.InquirySuccess;
                        return result;
                    }
                }
                result.Message = Constants.InquiryFailed;
                return result;
            }
            catch (Exception ex)
            {
                await _paymentLogRepository.InsertAsync(new PaymentLog
                {
                    PaymentId = payment.Id,
                    Psp = PspEnum.IranKish.ToString(),
                    Message = Constants.InquiryException,
                    Parameter = ex.Message
                });
                result.Message = Constants.ErrorInInquiry;
                return result;
            }
        }
        private async Task<InquiryOutputDto> InquiryToParsianAsync(PaymentDto payment, string pspAccountJsonProps)
        {
            var result = new InquiryOutputDto()
            {
                StatusCode = (int)StatusCodeEnum.Unknown,
                Message = Constants.UnknownError,
                PaymentId = payment.Id
            };
            try
            {
                var pspAccountProps = JsonConvert.DeserializeObject<Parsian.PspAccountProps>(pspAccountJsonProps);

                ParsianInquiryInput input = new()
                {
                    OrderId = payment.Id,
                    LoginAccount = pspAccountProps.LoginAccount,
                    ReportServiceUserName = pspAccountProps.ReportServiceUserName,
                    ReportServicePassword = pspAccountProps.ReportServicePassword,
                };
                await _paymentLogRepository.InsertAsync(new PaymentLog
                {
                    PaymentId = payment.Id,
                    Psp = PspEnum.Parsian.ToString(),
                    Message = Constants.InquiryStart,
                    Parameter = JsonConvert.SerializeObject(input),
                });
                string inquiryResult = string.Empty;
                var connectToGatewayMode = _config.GetValue<bool>("App:ConnectToGatewayMode");
                if (connectToGatewayMode) //grpc
                {
                    inquiryResult = (await GatewayServiceGrpcClient().InquiryToParsianAsync(input)).Result;
                }
                else //restApi
                {
                    string url = _config.GetValue<string>("AppUrls:GatewayApi") + _config.GetValue<string>("ParsianUrls:Inquiry");
                    HttpClient client = new();
                    var response = await client.PostAsync(url, new StringContent(JsonConvert.SerializeObject(input), Encoding.UTF8, "application/json"));
                    inquiryResult = response.Content.ReadAsStringAsync().Result;
                    inquiryResult = JsonDocument.Parse(inquiryResult).RootElement.GetProperty("result").ToString();
                }
                await _paymentLogRepository.InsertAsync(new PaymentLog
                {
                    PaymentId = payment.Id,
                    Psp = PspEnum.Parsian.ToString(),
                    Message = Constants.InquiryResult,
                    Parameter = inquiryResult
                });
                result.PspJsonResult = inquiryResult;
                if (inquiryResult != null)
                {
                    Parsian.InquiryJsonResult jResult = JsonConvert.DeserializeObject<Parsian.InquiryJsonResult>(inquiryResult);
                    if (jResult.Status == 0)
                    {
                        if (jResult.Data != null)
                        {
                            if (jResult.Data.PGWStatusId == 0)
                            {
                                payment.TransactionCode = jResult.Data.RRN.ToString();
                                payment.TraceNo = jResult.Data.PTraceNo.ToString();
                                payment.PaymentStatusId = (int)PaymentStatusEnum.Success;
                                await _paymentRepository.AttachAsync(ObjectMapper.Map<PaymentDto, Payment>(payment), o => o.TransactionCode, o => o.PaymentStatusId, o => o.TraceNo);
                            }
                            else if (jResult.Data.PGWStatusId is not (-1425 or -1532 or -1533 or -1542 or -1544))
                            {
                                payment.PaymentStatusId = (int)PaymentStatusEnum.Failed;
                                await _paymentRepository.AttachAsync(ObjectMapper.Map<PaymentDto, Payment>(payment), o => o.PaymentStatusId);
                            }
                        }
                        result.PaymentStatus = payment.PaymentStatusId;
                        result.PaymentStatusDescription = EnumExtension.GetEnumDescription((PaymentStatusEnum)payment.PaymentStatusId);
                        result.StatusCode = (int)StatusCodeEnum.Success;
                        result.Message = Constants.InquirySuccess;
                        return result;
                    }
                }
                result.Message = Constants.InquiryFailed;
                return result;
            }
            catch (Exception ex)
            {
                await _paymentLogRepository.InsertAsync(new PaymentLog
                {
                    PaymentId = payment.Id,
                    Psp = PspEnum.Parsian.ToString(),
                    Message = Constants.InquiryException,
                    Parameter = ex.Message
                });
                result.Message = Constants.ErrorInInquiry;
                return result;
            }
        }
        private async Task<InquiryOutputDto> InquiryToPasargadAsync(PaymentDto payment, string pspAccountJsonProps)
        {
            var result = new InquiryOutputDto()
            {
                StatusCode = (int)StatusCodeEnum.Unknown,
                Message = Constants.UnknownError,
                PaymentId = payment.Id
            };
            try
            {
                var pspAccountProps = JsonConvert.DeserializeObject<Pasargad.PspAccountProps>(pspAccountJsonProps);
                var authResult = await AuthenticatePasargadAsync(payment.Id, pspAccountProps.UserName, pspAccountProps.Password);
                if (authResult == null || string.IsNullOrEmpty(authResult.Token))
                {
                    result.PspJsonResult = authResult != null ? JsonConvert.SerializeObject(authResult) : string.Empty;
                    result.Message = Constants.ErrorInAuthenticate;
                    return result;
                }

                PasargadInquiryInput input = new()
                {
                    Token = authResult.Token,
                    InvoiceId = payment.Id.ToString()
                };
                await _paymentLogRepository.InsertAsync(new PaymentLog
                {
                    PaymentId = payment.Id,
                    Psp = PspEnum.Pasargad.ToString(),
                    Message = Constants.InquiryStart,
                    Parameter = JsonConvert.SerializeObject(input),
                });
                string inquiryResult = string.Empty;
                var connectToGatewayMode = _config.GetValue<bool>("App:ConnectToGatewayMode");
                if (connectToGatewayMode) //grpc
                {
                    inquiryResult = (await GatewayServiceGrpcClient().InquiryToPasargadAsync(input)).Result;
                }
                else //restApi
                {
                    string url = _config.GetValue<string>("AppUrls:GatewayApi") + _config.GetValue<string>("PasargadUrls:Inquiry");
                    HttpClient client = new();
                    var response = await client.PostAsync(url, new StringContent(JsonConvert.SerializeObject(input), Encoding.UTF8, "application/json"));
                    inquiryResult = response.Content.ReadAsStringAsync().Result;
                    inquiryResult = JsonDocument.Parse(inquiryResult).RootElement.GetProperty("result").ToString();
                }
                await _paymentLogRepository.InsertAsync(new PaymentLog
                {
                    PaymentId = payment.Id,
                    Psp = PspEnum.Pasargad.ToString(),
                    Message = Constants.InquiryResult,
                    Parameter = inquiryResult
                });
                result.PspJsonResult = inquiryResult;
                if (inquiryResult != null)
                {
                    Pasargad.Inquiry.InquiryJsonResult jResult = JsonConvert.DeserializeObject<Pasargad.Inquiry.InquiryJsonResult>(inquiryResult);
                    if (jResult.ResultCode == 0 && jResult.Data != null)
                    {
                        //if (jResult.Data.Status == 0)
                        //{
                        //    payment.TransactionCode = jResult.Data.ReferenceNumber;
                        //    payment.TraceNo = jResult.Data.TrackId;
                        //    payment.PaymentStatusId = (int)PaymentStatusEnum.Success;
                        //    await _paymentRepository.AttachAsync(ObjectMapper.Map<PaymentDto, Payment>(payment), o => o.TransactionCode, o => o.PaymentStatusId, o => o.TraceNo);
                        //}
                        //else 
                        if (jResult.Data.Status is not (2 or 13031)) //13031 => تراکنش در انتظار تایید است 
                        {
                            payment.PaymentStatusId = (int)PaymentStatusEnum.Failed;
                            await _paymentRepository.AttachAsync(ObjectMapper.Map<PaymentDto, Payment>(payment), o => o.PaymentStatusId);
                        }

                        result.PaymentStatus = payment.PaymentStatusId;
                        result.PaymentStatusDescription = EnumExtension.GetEnumDescription((PaymentStatusEnum)payment.PaymentStatusId);
                        result.StatusCode = (int)StatusCodeEnum.Success;
                        result.Message = Constants.InquirySuccess;
                        return result;
                    }
                }
                result.Message = Constants.InquiryFailed;
                return result;
            }
            catch (Exception ex)
            {
                await _paymentLogRepository.InsertAsync(new PaymentLog
                {
                    PaymentId = payment.Id,
                    Psp = PspEnum.Pasargad.ToString(),
                    Message = Constants.InquiryException,
                    Parameter = ex.Message
                });
                result.Message = Constants.ErrorInInquiry;
                return result;
            }
        }
        #endregion

        #region Reverse
        [UnitOfWork(isTransactional: false)]
        public async Task<ReverseOutputDto> ReverseAsync(int paymentId)
        {
            var result = new ReverseOutputDto()
            {
                StatusCode = (int)StatusCodeEnum.Unknown,
                Message = Constants.UnknownError,
                PaymentId = paymentId
            };

            var payment = _paymentRepository.WithDetails().AsNoTracking()
                .Select(o => new PaymentDto
                {
                    Id = o.Id,
                    TraceNo = o.TraceNo,
                    TransactionCode = o.TransactionCode,
                    Token = o.Token,
                    PspAccountId = o.PspAccountId,
                    PaymentStatusId = o.PaymentStatusId
                }).FirstOrDefault(o => o.Id == paymentId);

            if (payment == null) { return result; }

            var pspAccount = _pspAccountRepository.WithDetails().AsNoTracking().Select(o => new { o.Id, o.PspId, o.JsonProps }).First(o => o.Id == payment.PspAccountId);

            switch ((PspEnum)pspAccount.PspId)
            {
                case PspEnum.Mellat:
                    return await ReverseToMellatAsync(payment, pspAccount.JsonProps);
                case PspEnum.IranKish:
                    return await ReverseToIranKishAsync(payment, pspAccount.JsonProps);
                case PspEnum.Parsian:
                    return await ReverseToParsianAsync(payment, pspAccount.JsonProps);
                case PspEnum.Pasargad:
                    return await ReverseToPasargadAsync(payment, pspAccount.JsonProps);
            }

            return result;
        }
        private async Task<ReverseOutputDto> ReverseToIranKishAsync(PaymentDto payment, string pspAccountJsonProps)
        {
            var result = new ReverseOutputDto()
            {
                StatusCode = (int)StatusCodeEnum.Unknown,
                Message = Constants.UnknownError,
                PaymentId = payment.Id
            };
            try
            {
                var pspAccountProps = JsonConvert.DeserializeObject<IranKish.PspAccountProps>(pspAccountJsonProps);
                IranKishReverseInput input = new()
                {
                    TerminalId = pspAccountProps.TerminalId,
                    SystemTraceAuditNumber = payment.TraceNo,
                    RetrievalReferenceNumber = payment.TransactionCode,
                    TokenIdentity = payment.Token
                };
                await _paymentLogRepository.InsertAsync(new PaymentLog
                {
                    PaymentId = payment.Id,
                    Psp = PspEnum.IranKish.ToString(),
                    Message = Constants.ReverseStart,
                    Parameter = JsonConvert.SerializeObject(input)
                });
                string reverseResult = string.Empty;
                var connectToGatewayMode = _config.GetValue<bool>("App:ConnectToGatewayMode");
                if (connectToGatewayMode) //grpc
                {
                    reverseResult = (await GatewayServiceGrpcClient().ReverseToIranKishAsync(input)).Result;
                }
                else //restApi
                {
                    string url = _config.GetValue<string>("AppUrls:GatewayApi") + _config.GetValue<string>("IranKishUrls:Reverse");
                    HttpClient client = new();
                    var response = await client.PostAsync(url, new StringContent(JsonConvert.SerializeObject(input), Encoding.UTF8, "application/json"));
                    reverseResult = response.Content.ReadAsStringAsync().Result;
                    reverseResult = JsonDocument.Parse(reverseResult).RootElement.GetProperty("result").ToString();
                }
                await _paymentLogRepository.InsertAsync(new PaymentLog
                {
                    PaymentId = payment.Id,
                    Psp = PspEnum.IranKish.ToString(),
                    Message = Constants.ReverseResult,
                    Parameter = reverseResult,
                });
                result.PspJsonResult = reverseResult;
                if (reverseResult != null)
                {
                    IranKish.ReverseJsonResult jResult = JsonConvert.DeserializeObject<IranKish.ReverseJsonResult>(reverseResult);
                    if (jResult.status && jResult.responseCode == "00")
                    {
                        payment.PaymentStatusId = (int)PaymentStatusEnum.Failed;
                        await _paymentRepository.AttachAsync(ObjectMapper.Map<PaymentDto, Payment>(payment), o => o.PaymentStatusId);

                        result.StatusCode = (int)StatusCodeEnum.Success;
                        result.Message = Constants.ReverseSuccess;
                        return result;
                    }
                }
                result.Message = Constants.ReverseFailed;
                return result;
            }
            catch (Exception ex)
            {
                await _paymentLogRepository.InsertAsync(new PaymentLog
                {
                    PaymentId = payment.Id,
                    Psp = PspEnum.IranKish.ToString(),
                    Message = Constants.ReverseException,
                    Parameter = ex.Message
                });

                result.Message = Constants.ErrorInReverse;
                return result;
            }
        }
        private async Task<ReverseOutputDto> ReverseToMellatAsync(PaymentDto payment, string pspAccountJsonProps)
        {
            var result = new ReverseOutputDto()
            {
                StatusCode = (int)StatusCodeEnum.Unknown,
                Message = Constants.UnknownError,
                PaymentId = payment.Id
            };
            try
            {
                var pspAccountProps = JsonConvert.DeserializeObject<Mellat.PspAccountProps>(pspAccountJsonProps);
                MellatReverseInput input = new()
                {
                    TerminalId = pspAccountProps.TerminalId,
                    UserName = pspAccountProps.UserName,
                    UserPassword = pspAccountProps.UserPassword,
                    OrderId = payment.Id,
                    SaleOrderId = payment.Id,
                    SaleReferenceId = long.Parse(payment.TransactionCode),
                    Switch = pspAccountProps.Switch
                };
                await _paymentLogRepository.InsertAsync(new PaymentLog
                {
                    PaymentId = payment.Id,
                    Psp = PspEnum.Mellat.ToString(),
                    Message = Constants.ReverseStart,
                    Parameter = JsonConvert.SerializeObject(input),
                });
                string reverseResult = string.Empty;
                var connectToGatewayMode = _config.GetValue<bool>("App:ConnectToGatewayMode");
                if (connectToGatewayMode) //grpc
                {
                    reverseResult = (await GatewayServiceGrpcClient().ReverseToMellatAsync(input)).Result;
                }
                else //restApi
                {
                    string url = _config.GetValue<string>("AppUrls:GatewayApi") + _config.GetValue<string>("MellatUrls:Reverse");
                    HttpClient client = new();
                    var response = await client.PostAsync(url, new StringContent(JsonConvert.SerializeObject(input), Encoding.UTF8, "application/json"));
                    reverseResult = response.Content.ReadAsStringAsync().Result;
                    reverseResult = JsonDocument.Parse(reverseResult).RootElement.GetProperty("result").ToString();
                }
                await _paymentLogRepository.InsertAsync(new PaymentLog
                {
                    PaymentId = payment.Id,
                    Psp = PspEnum.Mellat.ToString(),
                    Message = Constants.ReverseResult,
                    Parameter = reverseResult,
                });
                result.PspJsonResult = reverseResult;
                var reverseRes = JsonConvert.DeserializeObject<MellatOutputDto>(reverseResult);
                if (!string.IsNullOrEmpty(result.PspJsonResult) && reverseRes.Body.@return == "0")
                {
                    payment.PaymentStatusId = (int)PaymentStatusEnum.Failed;
                    await _paymentRepository.AttachAsync(ObjectMapper.Map<PaymentDto, Payment>(payment), o => o.PaymentStatusId);

                    result.StatusCode = (int)StatusCodeEnum.Success;
                    result.Message = Constants.ReverseSuccess;
                    return result;
                }
                result.StatusCode = (int)StatusCodeEnum.Failed;
                result.Message = Constants.ReverseFailed;
                return result;
            }
            catch (Exception ex)
            {
                await _paymentLogRepository.InsertAsync(new PaymentLog
                {
                    PaymentId = payment.Id,
                    Psp = PspEnum.Mellat.ToString(),
                    Message = Constants.ReverseException,
                    Parameter = ex.Message
                });
                result.Message = Constants.ErrorInReverse;
                return result;
            }
        }
        private async Task<ReverseOutputDto> ReverseToParsianAsync(PaymentDto payment, string pspAccountJsonProps)
        {
            var result = new ReverseOutputDto()
            {
                StatusCode = (int)StatusCodeEnum.Unknown,
                Message = Constants.UnknownError,
                PaymentId = payment.Id
            };
            try
            {
                var pspAccountProps = JsonConvert.DeserializeObject<Parsian.PspAccountProps>(pspAccountJsonProps);
                ParsianReverseInput input = new()
                {
                    LoginAccount = pspAccountProps.LoginAccount,
                    Token = long.Parse(payment.Token)
                };
                await _paymentLogRepository.InsertAsync(new PaymentLog
                {
                    PaymentId = payment.Id,
                    Psp = PspEnum.Parsian.ToString(),
                    Message = Constants.ReverseStart,
                    Parameter = JsonConvert.SerializeObject(input)
                });
                string reverseResult = string.Empty;
                var connectToGatewayMode = _config.GetValue<bool>("App:ConnectToGatewayMode");
                if (connectToGatewayMode) //grpc
                {
                    reverseResult = (await GatewayServiceGrpcClient().ReverseToParsianAsync(input)).Result;
                }
                else //restApi
                {
                    string url = _config.GetValue<string>("AppUrls:GatewayApi") + _config.GetValue<string>("ParsianUrls:Reverse");
                    HttpClient client = new();
                    var response = await client.PostAsync(url, new StringContent(JsonConvert.SerializeObject(input), Encoding.UTF8, "application/json"));
                    reverseResult = response.Content.ReadAsStringAsync().Result;
                    reverseResult = JsonDocument.Parse(reverseResult).RootElement.GetProperty("result").ToString();
                }
                await _paymentLogRepository.InsertAsync(new PaymentLog
                {
                    PaymentId = payment.Id,
                    Psp = PspEnum.Parsian.ToString(),
                    Message = Constants.ReverseResult,
                    Parameter = reverseResult,
                });
                result.PspJsonResult = reverseResult;
                if (reverseResult != null)
                {
                    Parsian.ReverseJsonResult jResult = JsonConvert.DeserializeObject<Parsian.ReverseJsonResult>(reverseResult);
                    if (jResult.Status == 0)
                    {
                        payment.PaymentStatusId = (int)PaymentStatusEnum.Failed;
                        await _paymentRepository.AttachAsync(ObjectMapper.Map<PaymentDto, Payment>(payment), o => o.PaymentStatusId);
                        result.StatusCode = (int)StatusCodeEnum.Success;
                        result.Message = Constants.ReverseSuccess;
                        return result;
                    }
                }
                result.Message = Constants.ReverseFailed;
                return result;
            }
            catch (Exception ex)
            {
                await _paymentLogRepository.InsertAsync(new PaymentLog
                {
                    PaymentId = payment.Id,
                    Psp = PspEnum.Parsian.ToString(),
                    Message = Constants.ReverseException,
                    Parameter = ex.Message
                });

                result.Message = Constants.ErrorInReverse;
                return result;
            }
        }
        private async Task<ReverseOutputDto> ReverseToPasargadAsync(PaymentDto payment, string pspAccountJsonProps)
        {
            var result = new ReverseOutputDto()
            {
                StatusCode = (int)StatusCodeEnum.Unknown,
                Message = Constants.UnknownError,
                PaymentId = payment.Id
            };
            try
            {
                var pspAccountProps = JsonConvert.DeserializeObject<Pasargad.PspAccountProps>(pspAccountJsonProps);
                var authResult = await AuthenticatePasargadAsync(payment.Id, pspAccountProps.UserName, pspAccountProps.Password);
                if (authResult == null || string.IsNullOrEmpty(authResult.Token))
                {
                    result.PspJsonResult = authResult != null ? JsonConvert.SerializeObject(authResult) : string.Empty;
                    result.Message = Constants.ErrorInAuthenticate;
                    return result;
                }

                PasargadReverseInput input = new()
                {
                    Token = authResult.Token,
                    Invoice = payment.Id.ToString(),
                    UrlId = payment.Token
                };
                await _paymentLogRepository.InsertAsync(new PaymentLog
                {
                    PaymentId = payment.Id,
                    Psp = PspEnum.Pasargad.ToString(),
                    Message = Constants.ReverseStart,
                    Parameter = JsonConvert.SerializeObject(input)
                });
                string reverseResult = string.Empty;
                var connectToGatewayMode = _config.GetValue<bool>("App:ConnectToGatewayMode");
                if (connectToGatewayMode) //grpc
                {
                    reverseResult = (await GatewayServiceGrpcClient().ReverseToPasargadAsync(input)).Result;
                }
                else //restApi
                {
                    string url = _config.GetValue<string>("AppUrls:GatewayApi") + _config.GetValue<string>("PasargadUrls:Verify");
                    HttpClient client = new();
                    var response = await client.PostAsync(url, new StringContent(JsonConvert.SerializeObject(input), Encoding.UTF8, "application/json"));
                    reverseResult = response.Content.ReadAsStringAsync().Result;
                    reverseResult = JsonDocument.Parse(reverseResult).RootElement.GetProperty("result").ToString();
                }
                await _paymentLogRepository.InsertAsync(new PaymentLog
                {
                    PaymentId = payment.Id,
                    Psp = PspEnum.Pasargad.ToString(),
                    Message = Constants.ReverseResult,
                    Parameter = reverseResult,
                });
                result.PspJsonResult = reverseResult;
                if (reverseResult != null)
                {
                    Pasargad.Reverse.ReverseJsonResult jResult = JsonConvert.DeserializeObject<Pasargad.Reverse.ReverseJsonResult>(reverseResult);
                    if (jResult.ResultCode == 0)
                    {
                        payment.PaymentStatusId = (int)PaymentStatusEnum.Failed;
                        await _paymentRepository.AttachAsync(ObjectMapper.Map<PaymentDto, Payment>(payment), o => o.PaymentStatusId);
                        result.StatusCode = (int)StatusCodeEnum.Success;
                        result.Message = Constants.ReverseSuccess;
                        return result;
                    }
                }
                result.Message = Constants.ReverseFailed;
                return result;
            }
            catch (Exception ex)
            {
                await _paymentLogRepository.InsertAsync(new PaymentLog
                {
                    PaymentId = payment.Id,
                    Psp = PspEnum.Pasargad.ToString(),
                    Message = Constants.ReverseException,
                    Parameter = ex.Message
                });
                result.Message = Constants.ErrorInReverse;
                return result;
            }
        }
        #endregion

        #region RetryForVerify
        [UnitOfWork(isTransactional: false)]
        public async Task<List<RetryForVerifyOutputDto>> RetryForVerify()
        {
            var deadLine = DateTime.Now.AddMinutes(_config.GetValue<int>("App:RetryForVerifyFromDateMinute"));
            var retryCount = _config.GetValue<int>("App:RetryCount");
            var condidateCount = _config.GetValue<int>("App:CondidateCount");

            var payments = _paymentRepository.WithDetails().AsNoTracking()
                .Include(o => o.PspAccount)
                .Where(o => o.PaymentStatusId == (int)PaymentStatusEnum.InProgress && o.TransactionDate < deadLine && o.RetryCount < retryCount)
                .Select(o => new PaymentDto
                {
                    Id = o.Id,
                    TraceNo = o.TraceNo,
                    TransactionCode = o.TransactionCode,
                    Token = o.Token,
                    PspAccountId = o.PspAccountId,
                    RetryCount = o.RetryCount,
                    PaymentStatusId = o.PaymentStatusId,
                    FilterParam1 = o.FilterParam1,
                    FilterParam2 = o.FilterParam2,
                    FilterParam3 = o.FilterParam3,
                    FilterParam4 = o.FilterParam4
                })
                .Take(condidateCount).ToList();
            var result = new List<RetryForVerifyOutputDto>();
            foreach (var payment in payments)
            {
                var pspAccount = _pspAccountRepository.WithDetails().AsNoTracking().Select(o => new { o.Id, o.PspId, o.JsonProps }).First(o => o.Id == payment.PspAccountId);
                switch ((PspEnum)pspAccount.PspId)
                {
                    case PspEnum.Mellat:
                        await RetryForVerifyToMellatAsync(payment, pspAccount.JsonProps);
                        break;
                    case PspEnum.IranKish:
                        await RetryForVerifyToIranKishAsync(payment, pspAccount.JsonProps);
                        break;
                    case PspEnum.Parsian:
                        await RetryForVerifyToParsianAsync(payment, pspAccount.JsonProps);
                        break;
                    case PspEnum.Pasargad:
                        await RetryForVerifyToPasargadAsync(payment, pspAccount.JsonProps);
                        break;
                }
                result.Add(new RetryForVerifyOutputDto
                {
                    PaymentId = payment.Id,
                    PaymentStatus = (PaymentStatusEnum)payment.PaymentStatusId,
                    FilterParam1 = payment.FilterParam1,
                    FilterParam2 = payment.FilterParam2,
                    FilterParam3 = payment.FilterParam3,
                    FilterParam4 = payment.FilterParam4
                });
            }
            return result;
        }
        private async Task RetryForVerifyToIranKishAsync(PaymentDto payment, string pspAccountJsonProps)
        {
            if (string.IsNullOrEmpty(payment.TransactionCode) || string.IsNullOrEmpty(payment.TraceNo))
            {
                await InquiryToIranKishAsync(payment, pspAccountJsonProps);

                if (!string.IsNullOrEmpty(payment.TransactionCode) &&
                    !string.IsNullOrEmpty(payment.TraceNo) &&
                    payment.PaymentStatusId == (int)PaymentStatusEnum.InProgress)
                {
                    await VerifyToIranKishAsync(payment, pspAccountJsonProps, true);
                }
            }
            else
            {
                await VerifyToIranKishAsync(payment, pspAccountJsonProps, true);
            }
            payment.RetryCount += 1;
            await _paymentRepository.AttachAsync(ObjectMapper.Map<PaymentDto, Payment>(payment), o => o.RetryCount);
        }
        private async Task RetryForVerifyToMellatAsync(PaymentDto payment, string pspAccountJsonProps)
        {
            if (string.IsNullOrEmpty(payment.TransactionCode))
            {
                //await InquiryToMellatAsync(payment, pspAccountJsonProps);
                //if (!string.IsNullOrEmpty(payment.TransactionCode) &&
                //    payment.PaymentStatusId == (int)PaymentStatusEnum.InProgress)
                //{
                //    await VerifyToMellatAsync(payment, pspAccountJsonProps, true);
                //}

                payment.PaymentStatusId = (int)PaymentStatusEnum.Failed;
                await _paymentRepository.AttachAsync(ObjectMapper.Map<PaymentDto, Payment>(payment), o => o.PaymentStatusId);
            }
            else
            {
                await VerifyToMellatAsync(payment, pspAccountJsonProps, true);
            }
            payment.RetryCount += 1;
            await _paymentRepository.AttachAsync(ObjectMapper.Map<PaymentDto, Payment>(payment), o => o.RetryCount);
        }
        private async Task RetryForVerifyToParsianAsync(PaymentDto payment, string pspAccountJsonProps)
        {
            if (string.IsNullOrEmpty(payment.Token))
            {
                payment.PaymentStatusId = (int)PaymentStatusEnum.Failed;
                await _paymentRepository.AttachAsync(ObjectMapper.Map<PaymentDto, Payment>(payment), o => o.PaymentStatusId);
            }
            else
            {
                //await InquiryToParsianAsync(payment, pspAccountJsonProps);
                //if (payment.PaymentStatusId == (int)PaymentStatusEnum.InProgress)
                { await VerifyToParsianAsync(payment, pspAccountJsonProps, true); }
            }
            payment.RetryCount += 1;
            await _paymentRepository.AttachAsync(ObjectMapper.Map<PaymentDto, Payment>(payment), o => o.RetryCount);
        }
        private async Task RetryForVerifyToPasargadAsync(PaymentDto payment, string pspAccountJsonProps)
        {
            await VerifyToParsianAsync(payment, pspAccountJsonProps, true);
            payment.RetryCount += 1;
            await _paymentRepository.AttachAsync(ObjectMapper.Map<PaymentDto, Payment>(payment), o => o.RetryCount);
        }
        #endregion
    }
}
