﻿using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PaymentManagement.Application.Contracts;
using PaymentManagement.Application.Contracts.Enums;
using PaymentManagement.Application.Contracts.IServices;
using PaymentManagement.Application.IranKish;
using PaymentManagement.Application.Utilities;
using PaymentManagement.Domain.Models;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
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
        //private readonly IUnitOfWorkManager _unitOfWorkManager;

        public PaymentAppService(
            IRepository<Psp, int> pspRepository,
            IRepository<Account, int> accountRepository,
            IRepository<PspAccount, int> pspAccountRepository,
            IRepository<Payment, int> paymentRepository,
            IRepository<PaymentLog, int> paymentLogRepository
            //IUnitOfWorkManager unitOfWorkManager
            )
        {
            _pspRepository = pspRepository;
            _accountRepository = accountRepository;
            _pspAccountRepository = pspAccountRepository;
            _paymentRepository = paymentRepository;
            _paymentLogRepository = paymentLogRepository;
            //_unitOfWorkManager = unitOfWorkManager;
        }

        public async Task<List<PspAccountDto>> GetPspsByCustomerCode(int customerCode)
        {
            //todo: آیا این روش درست است
            return await (await _pspAccountRepository.GetQueryableAsync())
                 .Include(o => o.Psp).Include(o => o.Account).ThenInclude(o => o.Customer)
                 .Where(o => o.Account.Customer.Code == customerCode && o.IsActive && o.Account.IsActive)
                 .Select(o => new PspAccountDto { Id = o.Id, AccountName = o.Account.AccountName, Psp = o.Psp.Title, Logo = o.Logo }).ToListAsync();
        }
        public async Task<List<InquiryWithFilterParamDto>> InquiryWithFilterParamAsync(int customerCode, string filterParam)
        {
            List<int> pspAcccountIds = await (await _pspAccountRepository.GetQueryableAsync())
                 .Include(o => o.Account).ThenInclude(o => o.Customer)
                 .Where(o => o.Account.Customer.Code == customerCode && o.IsActive && o.Account.IsActive)
                 .Select(o => o.Id).ToListAsync();

            return await (await _paymentRepository.GetQueryableAsync())
                 .Where(o => pspAcccountIds.Contains(o.Id) && o.FilterParam == filterParam)
                 .GroupBy(o => o.PaymentStatusId).Select(o => new InquiryWithFilterParamDto
                 {
                     Status = o.Key,
                     Message = EnumExtension.GetEnumDescription((PaymentStatusEnum)o.Key),
                     Count = o.Count()
                 }).ToListAsync();
        }

        #region HandShakeWithPsp
        public async Task<HandShakeOutputDto> HandShakeWithPspAsync(HandShakeInputDto input)
        {
            var result = new HandShakeOutputDto()
            {
                StatusCode = (int)StatusCodeEnum.Failed,
                Message = Constants.UnknownError
            };

            string pspTitle = string.Empty;

            try
            {
                Payment payment = await CreatePaymentAsync(input);

                result.PaymentId = payment.Id;

                #region ValidateInput
                //todo: validate input
                var pspAccount = await _pspAccountRepository.GetAsync(input.PspAccountId);
                if (pspAccount == null || !pspAccount.IsActive)
                {
                    result.Message = Constants.ErrorInHandShakeInput;
                    return result;
                }

                pspTitle = ((PspEnum)pspAccount.PspId).ToString();

                await _paymentLogRepository.InsertAsync(new PaymentLog
                {
                    PaymentId = payment.Id,
                    Psp = pspTitle,
                    Message = Constants.HandShakeStart,
                    Parameter = JsonConvert.SerializeObject(input),
                });

                var psp = await _pspRepository.GetAsync(pspAccount.PspId);
                if (psp == null || !psp.IsActive)
                {
                    result.Message = Constants.ErrorInHandShakeInput;
                    return result;
                }

                var account = await _accountRepository.GetAsync(pspAccount.AccountId);
                if (account == null || !account.IsActive)
                {
                    result.Message = Constants.ErrorInHandShakeInput;
                    return result;
                }

                if (input.Amount < 1000)
                {
                    result.Message = Constants.ErrorInHandShakeInput;
                    return result;
                }
                #endregion

                #region HandShake

                switch ((PspEnum)pspAccount.PspId)
                {
                    case PspEnum.BehPardakht:
                        break;

                    case PspEnum.IranKish:
                        return await HandShakeWithIranKishAsync(payment);
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

                result.Message = Constants.ErrorInConnectToPsp;
                return result;
            }
        }
        private async Task<Payment> CreatePaymentAsync(HandShakeInputDto input)
        {
            return await _paymentRepository.InsertAsync(new Payment
            {
                PspAccountId = input.PspAccountId,
                PaymentStatusId = (int)PaymentStatusEnum.Inprogress,
                Amount = input.Amount,
                CallBackUrl = input.CallBackUrl,
                NationalCode = input.NationalCode,
                Mobile = input.Mobile,
                FilterParam = input.FilterParam,
                TransactionDate = DateTime.Now,
                TransactionPersianDate = DateUtil.Now,
                TransactionCode = string.Empty
            });
        }
        private async Task<HandShakeOutputDto> HandShakeWithIranKishAsync(Payment payment)
        {
            var result = new HandShakeOutputDto()
            {
                StatusCode = (int)StatusCodeEnum.Failed,
                Message = Constants.UnknownError,
                PaymentId = payment.Id
            };
            try
            {
                var pspAccount = await _pspAccountRepository.GetAsync(payment.PspAccountId);
                //todo:باید با ادرس خودمون درخواست به درگاه بدیم
                string callBackUrl = "BackFromIranKish";

                WebHelper webHelper = new();
                IPGData iPGData = new()
                {
                    TreminalId = pspAccount.IranKishTerminalId,
                    AcceptorId = pspAccount.IranKishAcceptorId,
                    PassPhrase = pspAccount.IranKishPassPhrase,
                    RevertURL = callBackUrl,
                    Amount = (long)payment.Amount,
                    RequestId = payment.Id.ToString(),
                    NationalId = payment.NationalCode ?? string.Empty,
                    CmsPreservationId = payment.Mobile ?? string.Empty
                };

                if (!string.IsNullOrEmpty(iPGData.CmsPreservationId))
                {
                    string firstchars = iPGData.CmsPreservationId.Substring(0, 2);
                    if (firstchars != "98")
                    {
                        iPGData.CmsPreservationId = "98" + iPGData.CmsPreservationId.Substring(1);
                    }
                }

                iPGData.TransactionType = TransactionType.Purchase;
                iPGData.BillInfo = null;
                iPGData.RsaPublicKey = "<RSAKeyValue><Modulus>0QdvomuKPZfhxNxqQcOICl/wMYZIhZW4CUa6RlZOqmMJOyWgeUHfX4bA5+bWpLfv/05pWsaGcFUSFVjk4W6ljE3oXwfW8f2ccr/90wxDfx8vt1jN5KVKPk02TCTrBtZL8rBZYu6inDtt2HdjOEenIkzB9AcZ200UciYOTz0iuvs=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";

                string request = CreateJsonRequest.CreateJasonRequest(iPGData);
                Uri url = new Uri(string.Format(@"https://ikc.shaparak.ir/api/v3/tokenization/make"));
                string jresponse = webHelper.Post(url, request);
                if (jresponse != null)
                {
                    JsonResult jResult = JsonConvert.DeserializeObject<JsonResult>(jresponse);
                    if (jResult.status)
                    {
                        result.StatusCode = (int)StatusCodeEnum.Success;
                        result.PspJsonResult = jresponse;
                        result.Token = jResult.result.token;
                        result.Message = Constants.HandShakeSuccess;
                    }
                    else
                    {
                        result.PspJsonResult = jresponse;
                        result.Message = Constants.ErrorInHandShakeResult;
                    }
                }
                else
                {
                    result.Message = Constants.ErrorInHandShakeResult;
                }

                if (!string.IsNullOrEmpty(result.Token))
                {
                    //todo:آیا روش بروزرسانی صحیح است
                    payment.Token = result.Token;
                    await _paymentRepository.UpdateAsync(payment);
                }

                await _paymentLogRepository.InsertAsync(new PaymentLog
                {
                    PaymentId = payment.Id,
                    Psp = PspEnum.IranKish.ToString(),
                    Message = Constants.HandShakeResult,
                    Parameter = result.PspJsonResult,
                });

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

                result.Message = Constants.ErrorInConnectToPsp;
                return result;
            }
        }
        #endregion

        #region RedirectToPsp
        public async Task<RedirectToPspOutput> RedirectToPspAsync(int paymentId)
        {
            //todo: سلکت بزنم یا همین خوبه
            var payment = (await _paymentRepository.GetQueryableAsync()).Include(o => o.PspAccount).First(o => o.Id == paymentId);
            await _paymentLogRepository.InsertAsync(new PaymentLog
            {
                PaymentId = paymentId,
                Psp = ((PspEnum)payment.PspAccount.PspId).ToString(),
                Message = Constants.RedirectToPsp
            });

            return new RedirectToPspOutput { PaymentId = paymentId, PspId = payment.PspAccount.PspId, Token = payment.Token };
        }
        public async Task<string> GetCallBackUrlAsync(int paymentId)
        {
            return (await _paymentRepository.GetAsync(paymentId)).CallBackUrl;
        }
        #endregion

        #region BackFromPsp
        //todo:برای کدوم متدها این اتریبیوت گذاشته بشه؟
        [UnitOfWork(isTransactional: false)]
        public async Task<BackFromPspOutputDto> BackFromIranKishAsync(string pspJsonResult)
        {
            var pspResult = JsonConvert.DeserializeObject<BackFromPspResult>(pspJsonResult);

            var payment = await _paymentRepository.GetAsync(pspResult.RequestId);

            var result = new BackFromPspOutputDto()
            {
                StatusCode = (int)StatusCodeEnum.Failed,
                Message = Constants.UnknownError,
                PaymentId = payment.Id
            };

            try
            {
                payment.TransactionCode = pspResult.retrievalReferenceNumber;
                await _paymentRepository.UpdateAsync(payment);

                result.PspJsonResult = pspJsonResult;
                result.TransactionCode = pspResult.retrievalReferenceNumber;

                await _paymentLogRepository.InsertAsync(new PaymentLog
                {
                    PaymentId = payment.Id,
                    Psp = PspEnum.IranKish.ToString(),
                    Message = Constants.BackFromPsp,
                    Parameter = pspJsonResult
                });

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

                    result.StatusCode = (int)StatusCodeEnum.PaymentSuccess;
                    result.Message = Constants.PaymentSuccessDontSendVerify;
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

                    result.StatusCode = (int)StatusCodeEnum.PaymentFailed;
                    result.Message = Constants.PaymentFailedDontSendVerify;
                    return result;
                }
                if (pspResult.UrlReferrer == null || pspResult.UrlReferrerAuthority != "ikc.shaparak.ir")
                {
                    await _paymentLogRepository.InsertAsync(new PaymentLog
                    {
                        PaymentId = payment.Id,
                        Psp = PspEnum.IranKish.ToString(),
                        Message = Constants.BackFromPspWithInCorrectUrl,
                        Parameter = pspResult.UrlReferrer == null ? "UrlReferrer Is Null" : pspResult.UrlReferrerAuthority,
                    });

                    payment.PaymentStatusId = (int)PaymentStatusEnum.Failed;
                    await _paymentRepository.UpdateAsync(payment);

                    result.StatusCode = (int)StatusCodeEnum.Failed;
                    result.Message = Constants.ErrorInBackFromPspUrl;
                    return result;
                }

                var resultCode = pspResult.responseCode;
                if (string.IsNullOrEmpty(resultCode) || resultCode != "00")
                {
                    payment.PaymentStatusId = (int)PaymentStatusEnum.Failed;
                    await _paymentRepository.UpdateAsync(payment);

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

                result.StatusCode = (int)StatusCodeEnum.Failed;
                result.Message = Constants.ErrorInBackFromPsp;
                return result;
            }
        }
        #endregion

        #region VerifyToPsp
        public async Task<VerifyOutputDto> VerifyAsync(int paymentId)
        {
            var result = new VerifyOutputDto()
            {
                StatusCode = (int)StatusCodeEnum.Failed,
                Message = Constants.UnknownError,
                PaymentId = paymentId
            };

            var payment = (await _paymentRepository.GetQueryableAsync()).Include(o => o.PspAccount).First(o => o.Id == paymentId);

            switch ((PspEnum)payment.PspAccount.PspId)
            {
                case PspEnum.BehPardakht:
                    break;
                case PspEnum.IranKish:
                    return await VerifyToIranKishAsync(payment, false);
            }

            return result;
        }
        private async Task<VerifyOutputDto> VerifyToIranKishAsync(Payment payment, bool isRetryForVerify)
        {
            var result = new VerifyOutputDto()
            {
                StatusCode = (int)StatusCodeEnum.Failed,
                Message = Constants.UnknownError,
                PaymentId = payment.Id
            };

            try
            {
                WebHelper webHelper = new();

                RequestVerify requestVerify = new()
                {
                    terminalId = payment.PspAccount.IranKishTerminalId,
                    //todo: رفع خطا شد
                    //systemTraceAuditNumber = input.Params[3],
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

                Uri url = new(string.Format(@"https://ikc.shaparak.ir/api/v3/confirmation/purchase"));
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
                        await _paymentRepository.UpdateAsync(payment);

                        result.StatusCode = (int)StatusCodeEnum.Success;
                        result.Message = Constants.VerifySuccess;
                        return result;
                    }
                }

                payment.PaymentStatusId = (int)PaymentStatusEnum.Failed;
                await _paymentRepository.UpdateAsync(payment);

                result.Message = Constants.VerifyFailed;
                return result;
            }
            catch (Exception ex)
            {
                await _paymentLogRepository.InsertAsync(new PaymentLog
                {
                    PaymentId = payment.Id,
                    Message = isRetryForVerify ? Constants.RetryForVerifyException : Constants.VerifyException,
                    Parameter = ex.Message
                });

                result.Message = Constants.ErrorInVerify;
                return result;
            }
        }
        #endregion

        #region InquiryToPsp
        public async Task<InquiryOutputDto> InquiryAsync(int paymentId)
        {
            var result = new InquiryOutputDto()
            {
                StatusCode = (int)StatusCodeEnum.Failed,
                Message = Constants.UnknownError,
                PaymentId = paymentId
            };

            var payment = (await _paymentRepository.GetQueryableAsync()).Include(o => o.PspAccount).First(o => o.Id == paymentId);

            switch ((PspEnum)payment.PspAccount.PspId)
            {
                case PspEnum.BehPardakht:
                    break;
                case PspEnum.IranKish:
                    return await InquiryToIranKishAsync(payment);
            }

            return result;
        }
        private async Task<InquiryOutputDto> InquiryToIranKishAsync(Payment payment)
        {
            var result = new InquiryOutputDto()
            {
                StatusCode = (int)StatusCodeEnum.Failed,
                Message = Constants.UnknownError,
                PaymentId = payment.Id
            };

            try
            {
                string requestirankish = JsonConvert.SerializeObject(new
                {
                    passPhrase = payment.PspAccount.IranKishPassPhrase,
                    terminalId = payment.PspAccount.IranKishTerminalId,
                    requestId = payment.Id,
                    findOption = 3
                });

                await _paymentLogRepository.InsertAsync(new PaymentLog
                {
                    PaymentId = payment.Id,
                    Psp = PspEnum.IranKish.ToString(),
                    Message = Constants.InquiryStart,
                    Parameter = requestirankish,
                });

                WebHelper webHelper = new WebHelper();

                Uri url = new Uri(string.Format(@"https://ikc.shaparak.ir/api/v3/inquiry/single"));
                string jresponse = webHelper.Post(url, requestirankish);

                await _paymentLogRepository.InsertAsync(new PaymentLog
                {
                    PaymentId = payment.Id,
                    Psp = PspEnum.IranKish.ToString(),
                    Message =  Constants.InquiryResult,
                    Parameter = jresponse,
                });

                payment.PaymentStatusId = (int)PaymentStatusEnum.Success;
                await _paymentRepository.UpdateAsync(payment);

                result.StatusCode = (int)StatusCodeEnum.Success;
                result.Message = Constants.InquirySuccess;
                return result;

            }
            catch (Exception ex)
            {
                await _paymentLogRepository.InsertAsync(new PaymentLog
                {
                    PaymentId = payment.Id,
                    Message = Constants.InquiryException,
                    Parameter = ex.Message
                });

                result.Message = Constants.ErrorInInquiry;
                return result;
            }

            return result;
        }
        #endregion

        #region RetryForVerify
        public async Task RetryForVerify()
        {
            //todo:schadule in hangfire
            //todo:شرط زمان با اضافه کردن درگاه ها باید تکمیل شود
            //todo:RetryCount??
            //todo:فقط فیلدهای مورد نیاز سلکت شود که باید به مرور تکمیل شود
            var payments = await (await _paymentRepository.GetQueryableAsync())
                .Include(o => o.PspAccount)
                .Where(o => o.PaymentStatusId == (int)PaymentStatusEnum.Inprogress && (DateTime.Now - o.TransactionDate).TotalMinutes > 12)
                .Take(100).ToListAsync();

            foreach (var payment in payments)
            {
                switch ((PspEnum)payment.PspAccount.PspId)
                {
                    case PspEnum.BehPardakht:
                        break;
                    case PspEnum.IranKish:
                        await RetryForVerifyToIranKishAsync(payment);
                        break;
                }
            }
        }
        private async Task RetryForVerifyToIranKishAsync(Payment payment)
        {
            if (string.IsNullOrEmpty(payment.Token))
            {
                await InquiryToIranKishAsync(payment);
                //todo:اگر بعد از استعلام پرداخت در وضعیت در انتظار ارسال تاییدیه بود و کد تراکنش هم از استعلام دریافت شده بود باید وریفای دوباره ارسال شود
            }

            await VerifyToIranKishAsync(payment, true);
        }
        #endregion
    }
}
