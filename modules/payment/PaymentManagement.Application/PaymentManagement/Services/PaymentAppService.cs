using Esale.Core.DataAccess;
using Esale.Core.Validation;
using MellatPaymentService;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using PaymentManagement.Application.Contracts.Dtos;
using PaymentManagement.Application.Contracts.Enums;
using PaymentManagement.Application.Contracts.IServices;
using PaymentManagement.Application.IranKish;
using PaymentManagement.Application.Utilities;
using PaymentManagement.Domain.Models;
using System.Linq.Dynamic.Core.Tokenizer;
using System.Runtime.Intrinsics.Arm;
using System.Xml;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Uow;

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
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public PaymentAppService(
            IRepository<Psp, int> pspRepository,
            IRepository<Account, int> accountRepository,
            IRepository<PspAccount, int> pspAccountRepository,
            IRepository<Payment, int> paymentRepository,
            IRepository<PaymentLog, int> paymentLogRepository,
            IUnitOfWorkManager unitOfWorkManager,
            IConfiguration config
            )
        {
            _pspRepository = pspRepository;
            _accountRepository = accountRepository;
            _pspAccountRepository = pspAccountRepository;
            _paymentRepository = paymentRepository;
            _paymentLogRepository = paymentLogRepository;
            _unitOfWorkManager = unitOfWorkManager;
            _config = config;
        }

        public List<PspAccountDto> GetPsps()
        {
            return _pspAccountRepository.WithDetails().AsNoTracking()
                .Include(o => o.Psp).Include(o => o.Account)
                .Where(o => o.IsActive && o.Account.IsActive && o.Psp.IsActive)
                .Select(o => new PspAccountDto { Id = o.Id, PspId = o.PspId, AccountName = o.Account.AccountName, Psp = o.Psp.Title })
                .ToList();
        }
        public List<InquiryWithFilterParamDto> InquiryWithFilterParam(int? filterParam1, int? filterParam2, int? filterParam3, int? filterParam4)
        {
            return _paymentRepository.WithDetails().AsNoTracking()
                .Where(o => (filterParam1 == null || o.FilterParam1 == filterParam1) && (filterParam2 == null || o.FilterParam2 == filterParam2) &&
                            (filterParam3 == null || o.FilterParam3 == filterParam3) && (filterParam4 == null || o.FilterParam4 == filterParam4))
                .GroupBy(o => o.PaymentStatusId).Select(o => new InquiryWithFilterParamDto
                {
                    Status = o.Key,
                    Message = EnumExtension.GetEnumDescription((PaymentStatusEnum)o.Key),
                    Count = o.Count()
                }).ToList();
        }
        public string GetCallBackUrl(int paymentId)
        {
            var result = _paymentRepository.WithDetails().AsNoTracking().Select(o => new { o.Id, o.CallBackUrl }).FirstOrDefault(o => o.Id == paymentId);
            return result == null ? string.Empty : result.CallBackUrl;
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
                    var paymentEntity = ObjectMapper.Map<PaymentDto, Payment>(payment);
                    await _paymentRepository.AttachAsync(paymentEntity, o => o.PaymentStatusId);
                }

                result.Message = Constants.ErrorInConnectToPsp;
                return result;
            }
        }
        private async Task<PaymentDto> CreatePaymentAsync(HandShakeInputDto input)
        {
            try
            {
                //var payment = await _paymentRepository.InsertAsync(new Payment
                //{
                //    PspAccountId = input.PspAccountId,
                //    PaymentStatusId = (int)PaymentStatusEnum.InProgress,
                //    Amount = input.Amount,
                //    CallBackUrl = input.CallBackUrl,
                //    NationalCode = input.NationalCode,
                //    Mobile = input.Mobile,
                //    TransactionDate = DateTime.Now,
                //    TransactionPersianDate = DateUtil.Now,
                //    FilterParam1 = input.FilterParam1,
                //    FilterParam2 = input.FilterParam2,
                //    FilterParam3 = input.FilterParam3,
                //    FilterParam4 = input.FilterParam4
                //});
                //await CurrentUnitOfWork.SaveChangesAsync();

                using var uow = _unitOfWorkManager.Begin(requiresNew: true, isTransactional: false);
                var payment = await _paymentRepository.InsertAsync(new Payment
                {
                    PspAccountId = input.PspAccountId,
                    PaymentStatusId = (int)PaymentStatusEnum.InProgress,
                    Amount = input.Amount,
                    CallBackUrl = input.CallBackUrl,
                    NationalCode = input.NationalCode,
                    Mobile = input.Mobile,
                    AdditionalData = input.AdditionalData,
                    TransactionDate = DateTime.Now,
                    TransactionPersianDate = DateUtil.Now,
                    FilterParam1 = input.FilterParam1,
                    FilterParam2 = input.FilterParam2,
                    FilterParam3 = input.FilterParam3,
                    FilterParam4 = input.FilterParam4
                });
                await uow.CompleteAsync();

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
                    NationalCode = payment.NationalCode
                };

                return paymentDto;
            }
            catch (Exception ex)
            {
                throw ex;
            }
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
                string callBackUrl = _config.GetValue<string>("App:IranKishCallBackUrl");

                WebHelper webHelper = new WebHelper();
                XmlDocument doc = new XmlDocument();

                string request = string.Empty;
                IPGData iPGData = new()
                {
                    TreminalId = pspAccountProps.TerminalId,
                    AcceptorId = pspAccountProps.AcceptorId,
                    PassPhrase = pspAccountProps.PassPhrase,
                    RevertURL = callBackUrl,
                    Amount = (long)payment.Amount,
                    RequestId = payment.Id.ToString(),
                    NationalId = payment.NationalCode ?? string.Empty,
                    CmsPreservationId = string.IsNullOrEmpty(payment.Mobile)
                    ? string.Empty
                    : (payment.Mobile[..1] == "0" ? string.Concat("98", payment.Mobile[1..]) : payment.Mobile)
                };

                iPGData.TransactionType = TransactionType.Purchase;
                iPGData.BillInfo = null;
                iPGData.RsaPublicKey = pspAccountProps.RsaPublicKey;

                request = CreateJsonRequest.CreateJasonRequest(iPGData);

                await _paymentLogRepository.InsertAsync(new PaymentLog
                {
                    PaymentId = payment.Id,
                    Psp = PspEnum.IranKish.ToString(),
                    Message = Constants.HandShakeWithPspStart,
                    Parameter = request,
                });

                Uri url = new(Constants.IranKishGetTokenUrl);
                string jresponse = webHelper.Post(url, request);

                await _paymentLogRepository.InsertAsync(new PaymentLog
                {
                    PaymentId = payment.Id,
                    Psp = PspEnum.IranKish.ToString(),
                    Message = Constants.HandShakeResult,
                    Parameter = jresponse,
                });

                result.PspJsonResult = jresponse;

                if (jresponse != null)
                {
                    JsonResult jResult = JsonConvert.DeserializeObject<JsonResult>(jresponse);
                    if (jResult.status)
                    {
                        payment.Token = jResult.result.token;
                        var paymentEntity1 = ObjectMapper.Map<PaymentDto, Payment>(payment);
                        await _paymentRepository.AttachAsync(paymentEntity1, o => o.Token);

                        result.StatusCode = (int)StatusCodeEnum.Success;
                        result.Token = jResult.result.token;
                        var inputParams = new System.Collections.Specialized.NameValueCollection
                        {
                            { "tokenIdentity", jResult.result.token }
                        };
                        result.HtmlContent = StringUtil.GenerateForm(Constants.IranKishRedirectUrl, "post", inputParams);
                        result.Message = Constants.HandShakeSuccess;
                        return result;
                    }
                }

                payment.PaymentStatusId = (int)PaymentStatusEnum.Failed;
                var paymentEntity = ObjectMapper.Map<PaymentDto, Payment>(payment);
                await _paymentRepository.AttachAsync(paymentEntity, o => o.PaymentStatusId);

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
                var paymentEntity = ObjectMapper.Map<PaymentDto, Payment>(payment);
                await _paymentRepository.AttachAsync(paymentEntity, o => o.PaymentStatusId);

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

                var handShakeRequest = new
                {
                    TerminalId = pspAccountProps.TerminalId,
                    UserName = pspAccountProps.UserName,
                    UserPassword = pspAccountProps.UserPassword,
                    OrderId = payment.Id,
                    Amount = (long)payment.Amount,
                    LocalDate = DateTime.Now.Year + DateTime.Now.Month.ToString().PadLeft(2, '0') + DateTime.Now.Day.ToString().PadLeft(2, '0'),
                    LocalTime = DateTime.Now.Hour.ToString().PadLeft(2, '0') + DateTime.Now.Minute.ToString().PadLeft(2, '0') + DateTime.Now.Second.ToString().PadLeft(2, '0'),
                    CallBackUrl = _config.GetValue<string>("App:MellatCallBackUrl"),
                    MobileNo = string.IsNullOrEmpty(payment.Mobile)
                    ? string.Empty
                    : (payment.Mobile[..1] == "0" ? string.Concat("98", payment.Mobile[1..]) : payment.Mobile),
                    Enc = encryptedNationalCode
                };

                await _paymentLogRepository.InsertAsync(new PaymentLog
                {
                    PaymentId = payment.Id,
                    Psp = PspEnum.Mellat.ToString(),
                    Message = Constants.HandShakeWithPspStart,
                    Parameter = JsonConvert.SerializeObject(handShakeRequest),
                });

                PaymentGatewayClient clientMellat = new();
                var handShakeResult = await clientMellat.bpPayRequestAsync(
                    handShakeRequest.TerminalId,
                    handShakeRequest.UserName,
                    handShakeRequest.UserPassword,
                    handShakeRequest.OrderId,
                    handShakeRequest.Amount,
                    handShakeRequest.LocalDate,
                    handShakeRequest.LocalTime,
                    "",//AdditionalData,
                    handShakeRequest.CallBackUrl,
                    "0",//PayerId,
                    handShakeRequest.MobileNo,
                    "",//EncPan,
                    "",//PanHiddenMode,
                    "",//CartItem,
                    handShakeRequest.Enc);

                //todo: test null serialize
                result.PspJsonResult = JsonConvert.SerializeObject(handShakeResult);

                await _paymentLogRepository.InsertAsync(new PaymentLog
                {
                    PaymentId = payment.Id,
                    Psp = PspEnum.Mellat.ToString(),
                    Message = Constants.HandShakeResult,
                    Parameter = result.PspJsonResult,
                });

                if (handShakeResult != null)
                {
                    var res = handShakeResult.Body.@return.Split(",".ToCharArray());

                    if (res[0] == "0")
                    {
                        payment.Token = res[1];
                        var paymentEntity1 = ObjectMapper.Map<PaymentDto, Payment>(payment);
                        await _paymentRepository.AttachAsync(paymentEntity1, o => o.Token);

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
                        if (!string.IsNullOrEmpty(handShakeRequest.MobileNo))
                        {
                            inputParams.Add("MobileNo", handShakeRequest.MobileNo);
                        }

                        result.HtmlContent = StringUtil.GenerateForm(Constants.MellatRedirectUrl, "post", inputParams);

                        return result;
                    }
                }

                payment.PaymentStatusId = (int)PaymentStatusEnum.Failed;
                var paymentEntity = ObjectMapper.Map<PaymentDto, Payment>(payment);
                await _paymentRepository.AttachAsync(paymentEntity, o => o.PaymentStatusId);

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
                var paymentEntity = ObjectMapper.Map<PaymentDto, Payment>(payment);
                await _paymentRepository.AttachAsync(paymentEntity, o => o.PaymentStatusId);

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
                    AdditionalData = o.AdditionalData
                })
                .First(o => o.Id == int.Parse(pspResult.requestId));

            var result = new BackFromPspOutputDto()
            {
                StatusCode = (int)StatusCodeEnum.Unknown,
                Message = Constants.UnknownError,
                PaymentId = payment.Id,
                AdditionalData = payment.AdditionalData
            };

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
                    await CurrentUnitOfWork.CompleteAsync();

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
                var paymentEntity = ObjectMapper.Map<PaymentDto, Payment>(payment);
                await _paymentRepository.AttachAsync(paymentEntity, o => o.TransactionCode, o => o.TraceNo);

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
                    await CurrentUnitOfWork.CompleteAsync();

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
                    await CurrentUnitOfWork.CompleteAsync();

                    result.StatusCode = (int)StatusCodeEnum.Failed;
                    result.Message = Constants.VerifyFailed;
                    return result;
                }
                if (string.IsNullOrEmpty(pspResult.OriginUrl) || pspResult.OriginUrl != Constants.IranKishOriginUrl)
                {
                    await _paymentLogRepository.InsertAsync(new PaymentLog
                    {
                        PaymentId = payment.Id,
                        Psp = PspEnum.IranKish.ToString(),
                        Message = Constants.BackFromPspWithInCorrectUrl,
                        Parameter = pspResult.OriginUrl == null ? "OriginUrl Is Null" : pspResult.OriginUrl,
                    });

                    payment.PaymentStatusId = (int)PaymentStatusEnum.Failed;
                    var paymentEntity1 = ObjectMapper.Map<PaymentDto, Payment>(payment);
                    using var uow = _unitOfWorkManager.Begin(requiresNew: true, isTransactional: false);
                    await _paymentRepository.AttachAsync(paymentEntity1, o => o.PaymentStatusId);
                    await uow.CompleteAsync();
                    await CurrentUnitOfWork.CompleteAsync();

                    result.StatusCode = (int)StatusCodeEnum.Failed;
                    result.Message = Constants.ErrorInBackFromPspUrl;
                    return result;
                }
                if (string.IsNullOrEmpty(pspResult.responseCode) || pspResult.responseCode != "00")
                {
                    payment.PaymentStatusId = (int)PaymentStatusEnum.Failed;
                    var paymentEntity1 = ObjectMapper.Map<PaymentDto, Payment>(payment);
                    using var uow = _unitOfWorkManager.Begin(requiresNew: true, isTransactional: false);
                    await _paymentRepository.AttachAsync(paymentEntity1, o => o.PaymentStatusId);
                    await uow.CompleteAsync();
                    await CurrentUnitOfWork.CompleteAsync();

                    result.StatusCode = (int)StatusCodeEnum.Failed;
                    result.Message = Constants.ErrorInBackFromPspResponseCode;
                    return result;
                }

                await CurrentUnitOfWork.CompleteAsync();

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
                await CurrentUnitOfWork.CompleteAsync();

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
                    AdditionalData = o.AdditionalData
                })
                .First(o => o.Id == int.Parse(pspResult.SaleOrderId));
            var result = new BackFromPspOutputDto()
            {
                StatusCode = (int)StatusCodeEnum.Unknown,
                Message = Constants.UnknownError,
                PaymentId = payment.Id,
                AdditionalData = payment.AdditionalData
            };

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
                    await CurrentUnitOfWork.CompleteAsync();

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
                var paymentEntity = ObjectMapper.Map<PaymentDto, Payment>(payment);
                await _paymentRepository.AttachAsync(paymentEntity, o => o.TransactionCode);

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
                    await CurrentUnitOfWork.CompleteAsync();

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
                    await CurrentUnitOfWork.CompleteAsync();

                    result.StatusCode = (int)StatusCodeEnum.Failed;
                    result.Message = Constants.VerifyFailed;
                    return result;
                }
                if (string.IsNullOrEmpty(pspResult.OriginUrl) || pspResult.OriginUrl != Constants.MellatOriginUrl)
                {
                    await _paymentLogRepository.InsertAsync(new PaymentLog
                    {
                        PaymentId = payment.Id,
                        Psp = PspEnum.Mellat.ToString(),
                        Message = Constants.BackFromPspWithInCorrectUrl,
                        Parameter = pspResult.OriginUrl == null ? "OriginUrl Is Null" : pspResult.OriginUrl,
                    });

                    payment.PaymentStatusId = (int)PaymentStatusEnum.Failed;
                    var paymentEntity1 = ObjectMapper.Map<PaymentDto, Payment>(payment);
                    using var uow = _unitOfWorkManager.Begin(requiresNew: true, isTransactional: false);
                    await _paymentRepository.AttachAsync(paymentEntity1, o => o.PaymentStatusId);
                    await uow.CompleteAsync();
                    await CurrentUnitOfWork.CompleteAsync();

                    result.StatusCode = (int)StatusCodeEnum.Failed;
                    result.Message = Constants.ErrorInBackFromPspUrl;
                    return result;
                }
                if (string.IsNullOrEmpty(pspResult.ResCode) || pspResult.ResCode != "0" || pspResult.RefId != payment.Token)
                {
                    payment.PaymentStatusId = (int)PaymentStatusEnum.Failed;
                    var paymentEntity1 = ObjectMapper.Map<PaymentDto, Payment>(payment);
                    using var uow = _unitOfWorkManager.Begin(requiresNew: true, isTransactional: false);
                    await _paymentRepository.AttachAsync(paymentEntity1, o => o.PaymentStatusId);
                    await uow.CompleteAsync();
                    await CurrentUnitOfWork.CompleteAsync();

                    result.StatusCode = (int)StatusCodeEnum.Failed;
                    result.Message = Constants.ErrorInBackFromPspResponseCode;
                    return result;
                }

                await CurrentUnitOfWork.CompleteAsync();

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
                await CurrentUnitOfWork.CompleteAsync();

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
                    PspAccountId = o.PspAccountId
                }).First(o => o.Id == paymentId);
            var pspAccount = _pspAccountRepository.WithDetails().AsNoTracking().Select(o => new { o.Id, o.PspId, o.JsonProps }).First(o => o.Id == payment.PspAccountId);

            switch ((PspEnum)pspAccount.PspId)
            {
                case PspEnum.Mellat:
                    return await VerifyToMellatAsync(payment, pspAccount.JsonProps, false);
                case PspEnum.IranKish:
                    return await VerifyToIranKishAsync(payment, pspAccount.JsonProps, false);
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

                WebHelper webHelper = new();

                RequestVerify requestVerify = new()
                {
                    terminalId = pspAccountProps.TerminalId,
                    systemTraceAuditNumber = payment.TraceNo,
                    retrievalReferenceNumber = payment.TransactionCode,
                    tokenIdentity = payment.Token
                };
                string requestVerifyJson = JsonConvert.SerializeObject(requestVerify);

                await _paymentLogRepository.InsertAsync(new PaymentLog
                {
                    PaymentId = payment.Id,
                    Psp = PspEnum.IranKish.ToString(),
                    Message = isRetryForVerify ? Constants.RetryForVerifyStart : Constants.VerifyStart,
                    Parameter = requestVerifyJson,
                });

                Uri url = new(Constants.IranKishVerifyUrl);
                string jresponse = webHelper.Post(url, requestVerifyJson);

                result.PspJsonResult = jresponse;

                await _paymentLogRepository.InsertAsync(new PaymentLog
                {
                    PaymentId = payment.Id,
                    Psp = PspEnum.IranKish.ToString(),
                    Message = isRetryForVerify ? Constants.RetryForVerifyResult : Constants.VerifyResult,
                    Parameter = jresponse,
                });

                if (jresponse != null)
                {
                    VerifyJsonResult jResult = JsonConvert.DeserializeObject<VerifyJsonResult>(jresponse);

                    if (jResult.status && jResult.responseCode == "00")
                    {
                        payment.PaymentStatusId = (int)PaymentStatusEnum.Success;
                        var paymentEntity1 = ObjectMapper.Map<PaymentDto, Payment>(payment);
                        await _paymentRepository.AttachAsync(paymentEntity1, o => o.PaymentStatusId);

                        result.StatusCode = (int)StatusCodeEnum.Success;
                        result.Message = Constants.VerifySuccess;
                        return result;
                    }
                }

                payment.PaymentStatusId = (int)PaymentStatusEnum.Failed;
                var paymentEntity = ObjectMapper.Map<PaymentDto, Payment>(payment);
                await _paymentRepository.AttachAsync(paymentEntity, o => o.PaymentStatusId);

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

                var verifyRequest = new
                {
                    TerminalId = pspAccountProps.TerminalId,
                    UserName = pspAccountProps.UserName,
                    UserPassword = pspAccountProps.UserPassword,
                    SequentialOrderId = payment.Id,
                    SaleOrderId = payment.Id,
                    SaleReferenceId = long.Parse(payment.TransactionCode),
                };

                await _paymentLogRepository.InsertAsync(new PaymentLog
                {
                    PaymentId = payment.Id,
                    Psp = PspEnum.Mellat.ToString(),
                    Message = isRetryForVerify ? Constants.RetryForVerifyStart : Constants.VerifyStart,
                    Parameter = JsonConvert.SerializeObject(verifyRequest),
                });

                var verifyResult = await new PaymentGatewayClient().bpVerifyRequestAsync(
                    verifyRequest.TerminalId,
                    verifyRequest.UserName,
                    verifyRequest.UserPassword,
                    verifyRequest.SequentialOrderId,
                    verifyRequest.SaleOrderId,
                    verifyRequest.SaleReferenceId);

                result.PspJsonResult = JsonConvert.SerializeObject(verifyResult);

                await _paymentLogRepository.InsertAsync(new PaymentLog
                {
                    PaymentId = payment.Id,
                    Psp = PspEnum.Mellat.ToString(),
                    Message = isRetryForVerify ? Constants.RetryForVerifyResult : Constants.VerifyResult,
                    Parameter = result.PspJsonResult,
                });

                //ResCode = 0 پرداخت موفق
                //ResCode = 43 پرداخت موفق است و درخواست تاییدیه تکراری ارسال شده است
                //ResCode = 415 session time out در این حالت پرداخت ناموفق نیست و باید مجددن وضعیت آن استعلام گرفته شود
                if (string.IsNullOrEmpty(verifyResult.Body.@return) || verifyResult.Body.@return is not ("0" or "43" or "415"))
                {
                    payment.PaymentStatusId = (int)PaymentStatusEnum.Failed;
                    var paymentEntity = ObjectMapper.Map<PaymentDto, Payment>(payment);
                    await _paymentRepository.AttachAsync(paymentEntity, o => o.PaymentStatusId);

                    result.StatusCode = (int)StatusCodeEnum.Failed;
                    result.Message = Constants.VerifyFailed;
                    return result;
                }

                if (verifyResult.Body.@return is "0" or "43")
                {
                    payment.PaymentStatusId = (int)PaymentStatusEnum.Success;
                    var paymentEntity = ObjectMapper.Map<PaymentDto, Payment>(payment);
                    await _paymentRepository.AttachAsync(paymentEntity, o => o.PaymentStatusId);

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
        #endregion

        #region Inquiry
        [UnitOfWork(isTransactional: false)]
        public async Task<InquiryOutputDto> InquiryAsync(int paymentId)
        {
            var result = new InquiryOutputDto()
            {
                StatusCode = (int)StatusCodeEnum.Failed,
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
                    PspAccountId = o.PspAccountId
                }).First(o => o.Id == paymentId);
            var pspAccount = _pspAccountRepository.WithDetails().AsNoTracking().Select(o => new { o.Id, o.PspId, o.JsonProps }).First(o => o.Id == payment.PspAccountId);

            switch ((PspEnum)pspAccount.PspId)
            {
                case PspEnum.Mellat:
                    //return await InquiryToMellatAsync(payment, pspAccount);
                    break;
                case PspEnum.IranKish:
                    return await InquiryToIranKishAsync(payment, pspAccount.JsonProps);
            }

            return result;
        }
        private async Task<InquiryOutputDto> InquiryToIranKishAsync(PaymentDto payment, string pspAccountJsonProps)
        {
            var result = new InquiryOutputDto()
            {
                StatusCode = (int)StatusCodeEnum.Failed,
                Message = Constants.UnknownError,
                PaymentId = payment.Id
            };

            try
            {
                var pspAccountProps = JsonConvert.DeserializeObject<IranKish.PspAccountProps>(pspAccountJsonProps);

                string requestirankish = JsonConvert.SerializeObject(new
                {
                    passPhrase = pspAccountProps.PassPhrase,
                    terminalId = pspAccountProps.TerminalId,
                    tokenIdentity = payment.Token,
                    findOption = 2 // جستجو بر اساس شناسه نشانه
                });

                await _paymentLogRepository.InsertAsync(new PaymentLog
                {
                    PaymentId = payment.Id,
                    Psp = PspEnum.IranKish.ToString(),
                    Message = Constants.InquiryStart,
                    Parameter = requestirankish,
                });

                WebHelper webHelper = new WebHelper();

                Uri url = new(Constants.IranKishInquiryUrl);
                string jresponse = webHelper.Post(url, requestirankish);

                result.PspJsonResult = jresponse;

                await _paymentLogRepository.InsertAsync(new PaymentLog
                {
                    PaymentId = payment.Id,
                    Psp = PspEnum.IranKish.ToString(),
                    Message = Constants.InquiryResult,
                    Parameter = jresponse,
                });

                if (jresponse != null)
                {
                    InquiryJsonResult jResult = JsonConvert.DeserializeObject<InquiryJsonResult>(jresponse);

                    if (jResult.status && jResult.responseCode == "00")
                    {
                        payment.TransactionCode = jResult.result.retrievalReferenceNumber;
                        payment.TraceNo = jResult.result.systemTraceAuditNumber;
                        payment.PaymentStatusId = jResult.result.isReversed ? (int)PaymentStatusEnum.Failed : (jResult.result.isVerified ? (int)PaymentStatusEnum.Success : payment.PaymentStatusId);
                        var paymentEntity = ObjectMapper.Map<PaymentDto, Payment>(payment);
                        await _paymentRepository.AttachAsync(paymentEntity, o => o.TransactionCode, o => o.TraceNo, o => o.PaymentStatusId);
                    }
                    else if (!jResult.status && jResult.responseCode == "30")
                    {
                        payment.PaymentStatusId = (int)PaymentStatusEnum.Failed;
                        var paymentEntity = ObjectMapper.Map<PaymentDto, Payment>(payment);
                        await _paymentRepository.AttachAsync(paymentEntity, o => o.PaymentStatusId);
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
        #endregion

        #region Reverse
        [UnitOfWork(isTransactional: false)]
        public async Task<ReverseOutputDto> ReverseAsync(int paymentId)
        {
            var result = new ReverseOutputDto()
            {
                StatusCode = (int)StatusCodeEnum.Failed,
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
                    PspAccountId = o.PspAccountId
                }).First(o => o.Id == paymentId);
            var pspAccount = _pspAccountRepository.WithDetails().AsNoTracking().Select(o => new { o.Id, o.PspId, o.JsonProps }).First(o => o.Id == payment.PspAccountId);

            switch ((PspEnum)pspAccount.PspId)
            {
                case PspEnum.Mellat:
                    return await ReverseToMellatAsync(payment, pspAccount.JsonProps);
                case PspEnum.IranKish:
                    return await ReverseToIranKishAsync(payment, pspAccount.JsonProps);
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

                RequestVerify requestVerify = new RequestVerify();
                requestVerify.terminalId = pspAccountProps.TerminalId;
                requestVerify.systemTraceAuditNumber = payment.TraceNo;
                requestVerify.retrievalReferenceNumber = payment.TransactionCode;
                requestVerify.tokenIdentity = payment.Token;
                string requestirankish = JsonConvert.SerializeObject(requestVerify);

                await _paymentLogRepository.InsertAsync(new PaymentLog
                {
                    PaymentId = payment.Id,
                    Psp = PspEnum.IranKish.ToString(),
                    Message = Constants.ReverseStart,
                    Parameter = requestirankish,
                });

                WebHelper webHelper = new WebHelper();

                Uri url = new(Constants.IranKishReverseUrl);
                string jresponse = webHelper.Post(url, requestirankish);

                result.PspJsonResult = jresponse;

                await _paymentLogRepository.InsertAsync(new PaymentLog
                {
                    PaymentId = payment.Id,
                    Psp = PspEnum.IranKish.ToString(),
                    Message = Constants.ReverseResult,
                    Parameter = jresponse,
                });

                if (jresponse != null)
                {
                    InquiryJsonResult jResult = JsonConvert.DeserializeObject<InquiryJsonResult>(jresponse);
                    if (jResult.status && jResult.responseCode == "00")
                    {
                        payment.PaymentStatusId = (int)PaymentStatusEnum.Failed;
                        var paymentEntity = ObjectMapper.Map<PaymentDto, Payment>(payment);
                        await _paymentRepository.AttachAsync(paymentEntity, o => o.PaymentStatusId);

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

                var reverseRequest = new
                {
                    TerminalId = pspAccountProps.TerminalId,
                    UserName = pspAccountProps.UserName,
                    UserPassword = pspAccountProps.UserPassword,
                    SequentialOrderId = payment.Id,
                    SaleOrderId = payment.Id,
                    SaleReferenceId = long.Parse(payment.TransactionCode),
                };

                await _paymentLogRepository.InsertAsync(new PaymentLog
                {
                    PaymentId = payment.Id,
                    Psp = PspEnum.Mellat.ToString(),
                    Message = Constants.ReverseStart,
                    Parameter = JsonConvert.SerializeObject(reverseRequest),
                });

                var reverseResult = await new PaymentGatewayClient().bpReversalRequestAsync(
                    reverseRequest.TerminalId,
                    reverseRequest.UserName,
                    reverseRequest.UserPassword,
                    reverseRequest.SequentialOrderId,
                    reverseRequest.SaleOrderId,
                    reverseRequest.SaleReferenceId);

                result.PspJsonResult = JsonConvert.SerializeObject(reverseResult);

                await _paymentLogRepository.InsertAsync(new PaymentLog
                {
                    PaymentId = payment.Id,
                    Psp = PspEnum.Mellat.ToString(),
                    Message = Constants.ReverseResult,
                    Parameter = result.PspJsonResult,
                });

                if (string.IsNullOrEmpty(reverseResult.Body.@return) || reverseResult.Body.@return == "0")
                {
                    payment.PaymentStatusId = (int)PaymentStatusEnum.Failed;
                    var paymentEntity = ObjectMapper.Map<PaymentDto, Payment>(payment);
                    await _paymentRepository.AttachAsync(paymentEntity, o => o.PaymentStatusId);

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
        #endregion

        #region RetryForVerify
        [UnitOfWork(isTransactional: false)]
        public async Task<List<RetryForVerifyOutputDto>> RetryForVerify()
        {
            //todo:شرط زمان با اضافه کردن درگاه ها باید تکمیل شود
            var deadLine = DateTime.Now.AddMinutes(-12);

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
                }

                result.Add(new RetryForVerifyOutputDto
                {
                    PaymentId = payment.Id,
                    PaymentStatus = payment.PaymentStatusId,
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
                var res = await InquiryToIranKishAsync(payment, pspAccountJsonProps);

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
            var paymentEntity = ObjectMapper.Map<PaymentDto, Payment>(payment);
            using var uow = _unitOfWorkManager.Begin(requiresNew: true, isTransactional: false);
            await _paymentRepository.AttachAsync(paymentEntity, o => o.RetryCount);
            await uow.CompleteAsync();
        }
        private async Task RetryForVerifyToMellatAsync(PaymentDto payment, string pspAccountJsonProps)
        {
            if (!string.IsNullOrEmpty(payment.TransactionCode))
            {
                await VerifyToMellatAsync(payment, pspAccountJsonProps, true);

                payment.RetryCount += 1;
                var paymentEntity = ObjectMapper.Map<PaymentDto, Payment>(payment);
                using var uow = _unitOfWorkManager.Begin(requiresNew: true, isTransactional: false);
                await _paymentRepository.AttachAsync(paymentEntity, o => o.RetryCount);
                await uow.CompleteAsync();
            }
        }
        #endregion
    }
}
