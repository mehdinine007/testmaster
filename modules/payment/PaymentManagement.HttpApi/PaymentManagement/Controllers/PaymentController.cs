using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PaymentManagement.Application.Contracts.Dtos;
using PaymentManagement.Application.Contracts.IServices;
using PaymentManagement.HttpApi.Utilities;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;

namespace PaymentManagement
{
    [RemoteService]
    [Area("paymentManagement")]
    [Route("api/services/app/PaymentService/[action]")]
    public class PaymentsController : AbpController
    {
        private readonly IPaymentAppService _paymentAppService;

        public PaymentsController(IPaymentAppService paymentAppService)
        {
            _paymentAppService = paymentAppService;
        }

        [HttpGet]
        public List<PspAccountDto> GetPsps()
        {
            return _paymentAppService.GetPsps();
        }

        [HttpPost]
        public async Task<HandShakeOutputDto> HandShakeAsync(HandShakeInputDto input)
        {
            return await _paymentAppService.HandShakeAsync(input);
        }

        [HttpPost]
        public async Task<ActionResult> BackFromIranKishAsync()
        {
            var keyValueList = new Dictionary<string, string>();

            foreach (string key in HttpContext.Request.Form.Keys)
            {
                keyValueList.Add(key, HttpContext.Request.Form[key]);
            }
            keyValueList.Add("OriginUrl", HttpContext.Request.Headers.Origin);

            var pspJsonResult = JsonConvert.SerializeObject(keyValueList);

            var result = await _paymentAppService.BackFromIranKishAsync(pspJsonResult);
            NavigateToPsp dp = new()
            {
                FormName = "form1",
                Method = "post",
                Url = _paymentAppService.GetCallBackUrl(result.PaymentId)
            };

            dp.AddKey("data", JsonConvert.SerializeObject(result));
            dp.Post(HttpContext);

            return null;
        }

        [HttpPost]
        public async Task<ActionResult> BackFromMellatAsync()
        {
            var keyValueList = new Dictionary<string, string>();

            foreach (string key in HttpContext.Request.Form.Keys)
            {
                keyValueList.Add(key, HttpContext.Request.Form[key]);
            }
            keyValueList.Add("OriginUrl", HttpContext.Request.Headers.Origin);

            var pspJsonResult = JsonConvert.SerializeObject(keyValueList);

            var result = await _paymentAppService.BackFromMellatAsync(pspJsonResult);
            NavigateToPsp dp = new()
            {
                FormName = "form1",
                Method = "post",
                Url = _paymentAppService.GetCallBackUrl(result.PaymentId)
            };

            dp.AddKey("data", JsonConvert.SerializeObject(result));
            dp.Post(HttpContext);

            return null;
        }

        [HttpPost]
        public async Task<VerifyOutputDto> VerifyAsync(int paymentId)
        {
            return await _paymentAppService.VerifyAsync(paymentId);
        }

        [HttpPost]
        public async Task<InquiryOutputDto> InquiryAsync(int paymentId)
        {
            return await _paymentAppService.InquiryAsync(paymentId);
        }

        [HttpPost]
        public async Task<ReverseOutputDto> ReverseAsync(int paymentId)
        {
            return await _paymentAppService.ReverseAsync(paymentId);
        }

        [HttpGet]
        public List<InquiryWithFilterParamDto> InquiryWithFilterParam(int? filterParam1, int? filterParam2, int? filterParam3, int? filterParam4)
        {
            return _paymentAppService.InquiryWithFilterParam(filterParam1, filterParam2, filterParam3, filterParam4);
        }

        [HttpPost]
        public async Task<List<RetryForVerifyOutputDto>> RetryForVerify()
        {
            return await _paymentAppService.RetryForVerify();
        }

        [HttpPost]
        public async Task<ActionResult> CustomerApi()
        {
            var data = HttpContext.Request.Form["data"];
            var result = JsonConvert.DeserializeObject(data);
            return null;
        }

        [HttpPost]
        public async Task<ActionResult> RedirctToIranKish(string token)
        {
            NavigateToPsp dp = new()
            {
                FormName = "form1",
                Method = "post"
            };
            dp.Url = "https://ikc.shaparak.ir/iuiv3/IPG/Index";
            dp.AddKey("tokenIdentity", token);
            dp.Post(HttpContext);

            return null;
        }

        [HttpPost]
        public async Task<ActionResult> RedirctToMellat(string refId, string nationalCode, string mobile)
        {
            NavigateToPsp dp = new()
            {
                FormName = "form1",
                Method = "post"
            };
            dp.Url = "https://bpm.shaparak.ir/pgwchannel/startpay.mellat";
            dp.AddKey("RefId", refId);
            if (!string.IsNullOrEmpty(nationalCode))
                dp.AddKey("Enc", nationalCode);
            if (!string.IsNullOrEmpty(mobile))
                dp.AddKey("MobileNo", mobile);

            dp.Post(HttpContext);

            return null;
        }
    }
}
