using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using Newtonsoft.Json;
using PaymentManagement.Application.Contracts;
using PaymentManagement.Application.Contracts.Enums;
using PaymentManagement.Application.Contracts.IServices;
using PaymentManagement.HttpApi.Utilities;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;

namespace PaymentManagement
{
    [RemoteService]
    [Area("paymentManagement")]
    [Route("api/services/app/PaymentService/[action]")]

    public class PaymentsController : AbpController//, IPaymentAppService
    {
        private readonly IPaymentAppService _paymentAppService;

        public PaymentsController(IPaymentAppService paymentAppService)
        {
            _paymentAppService = paymentAppService;
        }

        [HttpGet]
        //[RemoteService(IsEnabled = false)]
        public async Task<List<PspAccountDto>> GetPspsByCustomerCode(int customerCode)
        {
            return await _paymentAppService.GetPspsByCustomerCode(customerCode);
        }

        [HttpPost]
        //[RemoteService(IsEnabled = false)]
        public async Task<HandShakeOutputDto> HandShakeWithPspAsync(HandShakeInputDto input)
        {
            return await _paymentAppService.HandShakeWithPspAsync(input);
        }

        [HttpPost]
        public async Task<ActionResult> RedirectToPspAsync(int paymentId)
        {
            var result = await _paymentAppService.RedirectToPspAsync(paymentId);

            NavigateToPsp dp = new()
            {
                FormName = "form1",
                Method = "post"
            };
            switch ((PspEnum)result.PspId)
            {
                case PspEnum.BehPardakht:
                    break;
                case PspEnum.IranKish:
                    dp.Url = Constants.IranKishUrl;
                    dp.AddKey("tokenIdentity", result.Token);
                    break;
            }
            dp.Post();
            return null;

        }

        [HttpPost]
        public async Task<ActionResult> BackFromIranKishAsync()
        {
            var pspJsonResult = "";
            //                JsonConvert.SerializeObject("token: " + HttpContext.Current.Request["token"] + "-" +
            //"acceptorId: " + HttpContext.Current.Request["acceptorId"] + "-" +
            //"responseCode: " + HttpContext.Current.Request["responseCode"] + "-" +
            //"paymentId:" + HttpContext.Current.Request["paymentId"] + "-" +
            //"RequestId:" + HttpContext.Current.Request["RequestId"] + "-" +
            //"sha256OfPan:" + HttpContext.Current.Request["sha256OfPan"] + "-" +
            //"retrievalReferenceNumber:" + HttpContext.Current.Request["retrievalReferenceNumber"] + "-" +
            //"amount:" + HttpContext.Current.Request["amount"] + "-" +
            //"maskedPan" + HttpContext.Current.Request["maskedPan"] +
            //"systemTraceAuditNumber" + HttpContext.Current.Request["systemTraceAuditNumber"]);

            var result = await _paymentAppService.BackFromIranKishAsync(pspJsonResult);
            NavigateToPsp dp = new()
            {
                FormName = "form1",
                Method = "post",
                Url = await _paymentAppService.GetCallBackUrlAsync(result.PaymentId)
            };
            dp.AddKey("data", JsonConvert.SerializeObject(result));
            dp.Post();

            return null;
        }

        [HttpPost]
        public async Task<VerifyOutputDto> VerifyAsync(int paymentId)
        {
            return await _paymentAppService.VerifyAsync(paymentId);
        }

        [HttpGet]
        public async Task<List<InquiryWithFilterParamDto>> InquiryWithFilterParamAsync(int customerCode, string filterParam)
        {
            return await _paymentAppService.InquiryWithFilterParamAsync(customerCode, filterParam);
        }
        [HttpPost]
        public async Task RetryForVerify()
        {
            await _paymentAppService.RetryForVerify();
        }
    }
}
