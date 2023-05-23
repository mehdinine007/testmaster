using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PaymentManagement.Application.Contracts;
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
        public async Task<List<PspAccountDto>> GetPsps()
        {
            return await _paymentAppService.GetPsps();
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
                Url = await _paymentAppService.GetCallBackUrlAsync(result.PaymentId)
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

        [HttpGet]
        public async Task<List<InquiryWithFilterParamDto>> InquiryWithFilterParamAsync(int filterParam)
        {
            return await _paymentAppService.InquiryWithFilterParamAsync(filterParam);
        }

        [HttpPost]
        public async Task RetryForVerify()
        {
            await _paymentAppService.RetryForVerify();
        }

        [HttpPost]
        public async Task<ActionResult> CustomerApi()
        {
            var data = HttpContext.Request.Form["data"];
            var result = JsonConvert.DeserializeObject(data);
            return null;
        }

        [HttpPost]
        public async Task<ActionResult> RedirctToPsp(string token)
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
    }
}
