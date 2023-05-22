using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PaymentManagement.Application.Contracts;
using PaymentManagement.Application.Contracts.IServices;
using PaymentManagement.HttpApi.Utilities;
using System.Xml;
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
        public async Task<List<PspAccountDto>> GetPsps()
        {
            return await _paymentAppService.GetPsps();
        }

        [HttpPost]
        //[RemoteService(IsEnabled = false)]
        public async Task<HandShakeOutputDto> HandShakeAsync(HandShakeInputDto input)
        {
            return await _paymentAppService.HandShakeAsync(input);
        }

        //[HttpPost]
        //public async Task<ActionResult> RedirectToPspAsync(int paymentId)
        //{
        //    var result = await _paymentAppService.RedirectToPspAsync(paymentId);

        //    NavigateToPsp dp = new()
        //    {
        //        FormName = "form1",
        //        Method = "post"
        //    };
        //    switch ((PspEnum)result.PspId)
        //    {
        //        case PspEnum.BehPardakht:
        //            break;
        //        case PspEnum.IranKish:
        //            dp.Url = Constants.IranKishUrl;
        //            dp.AddKey("tokenIdentity", result.Token);
        //            break;
        //    }
        //    dp.Post();
        //    return null;

        // }

        [HttpPost]
        public async Task<ActionResult> BackFromIranKishAsync()
        {
            //todo:value type?
            var keyValueList = new Dictionary<string, string>();

            foreach (string key in HttpContext.Request.Form.Keys)
            {
                keyValueList.Add(key, HttpContext.Request.Form[key]);
            }
            keyValueList.Add("OriginUrl", HttpContext.Request.Headers.Origin.ToString().Replace("https://",""));

            var pspJsonResult = JsonConvert.SerializeObject(keyValueList);

            var result = await _paymentAppService.BackFromIranKishAsync(pspJsonResult);
            var ss = JsonConvert.SerializeObject(result);
            NavigateToPsp dp = new()
            {
                FormName = "form1",
                Method = "post",
                Url = "https://localhost:44344/api/services/app/PaymentService/CustomerApi"//await _paymentAppService.GetCallBackUrlAsync(result.PaymentId)
            };

            dp.AddKey("data", ss);
            dp.Post(HttpContext);


            //string ss = "{\"StatusCode\":0,\"Message\":\"برگشت از درگاه با موفقیت انجام شد، لطفا در صورت تمایل درخواست تایید پرداخت را ارسال کنید\",\"PaymentId\":100030,\"TransactionCode\":\"203245219323\",\"PspJsonResult\":\"{\\\"token\\\":\\\"B52CD4C7FA008F41847F6863AF86E71B6894\\\",\\\"acceptorId\\\":\\\"992180008116894\\\",\\\"responseCode\\\":\\\"00\\\",\\\"paymentId\\\":\\\"null\\\",\\\"requestId\\\":\\\"100030\\\",\\\"maskedPan\\\":\\\"621986******3340\\\",\\\"sha256OfPan\\\":\\\"98AA3FADFBBD8DA1C4C4903DE4211EC7074147542529097FF4F6C611FB9EDAE7\\\",\\\"retrievalReferenceNumber\\\":\\\"203245219323\\\",\\\"systemTraceAuditNumber\\\":\\\"455361\\\",\\\"amount\\\":\\\"10000\\\",\\\"isMigratedMerchant\\\":\\\"false\\\",\\\"merchantID\\\":\\\"BCEE8\\\",\\\"ttl\\\":\\\"60000\\\",\\\"sha1OfPan\\\":\\\"59214C300A0EF04DD87485BB9FEC009BA9DF91D0\\\",\\\"transactionType\\\":\\\"Purchase\\\"}\"}";

            //var url = "https://localhost:44344/api/services/app/PaymentService/CustomerApi";

            //Response.Clear();
            //var sb = new System.Text.StringBuilder();
            //sb.Append("<html>");
            //sb.AppendFormat("<body onload='document.forms[0].submit()'>");
            //sb.AppendFormat("<form action='{0}' method='post'>", url);

            //sb.AppendFormat("<input type='hidden' name='data' value='{0}'>", ss);
            //sb.Append("</form>");
            //sb.Append("</body>");
            //sb.Append("</html>");
            //Response.WriteAsync(sb.ToString());


            return null;
        }

        [HttpPost]
        public async Task<ActionResult> CustomerApi()
        {
            var data = HttpContext.Request.Form["data"];
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
        public async Task<ActionResult> RedirctToPsp(string token)
        {
            //return Redirect("https://ikc.shaparak.ir/iuiv3/IPG/Index?tokenIdentity=C22980846DA9B2408081D27F842E836D6894");

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
