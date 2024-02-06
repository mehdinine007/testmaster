using GatewayManagement.Application.Contracts.Dtos;
using GatewayManagement.Application.Contracts.IServices;
using GatewayManagement.Application.IranKish;
using GatewayManagement.Application.Parsian;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Org.BouncyCastle.Ocsp;
using ParsianConfirmService;
using ParsianReverseService;
using ParsianSaleService;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using Volo.Abp.Application.Services;
using Volo.Abp.Auditing;

namespace GatewayManagement.Application.Servicess
{
    public class GatewayAppService : ApplicationService, IGatewayAppService
    {
        private readonly IConfiguration _config;

        public GatewayAppService(IConfiguration config)
        {
            _config = config;
        }

        [Audited]
        public async Task<OutputDto> HandShakeWithIranKish(IranKishHandShakeInputDto input)
        {
            WebHelper webHelper = new();
            IPGData iPGData = new()
            {
                TreminalId = input.TerminalId,
                AcceptorId = input.AcceptorId,
                PassPhrase = input.PassPhrase,
                RevertURL = input.CallBackUrl,
                Amount = input.Amount,
                RequestId = input.RequestId,
                NationalId = input.NationalCode,
                CmsPreservationId = input.Mobile,
                TransactionType = TransactionType.Purchase,
                BillInfo = null,
                RsaPublicKey = input.RsaPublicKey
            };

            string request = CreateJsonRequest.CreateJasonRequest(iPGData);
            Uri url = new(_config.GetValue<string>("PSP:IranKishGetTokenUrl"));
            string jresponse = webHelper.Post(url, request);
            return new() { Result = jresponse };
        }
        [Audited]
        public async Task<OutputDto> HandShakeWithMellat(MellatHandShakeInputDto input)
        {
            string LocalDate = DateTime.Now.Year + DateTime.Now.Month.ToString().PadLeft(2, '0') + DateTime.Now.Day.ToString().PadLeft(2, '0');
            string LocalTime = DateTime.Now.Hour.ToString().PadLeft(2, '0') + DateTime.Now.Minute.ToString().PadLeft(2, '0') + DateTime.Now.Second.ToString().PadLeft(2, '0');

            if (input.Switch == 1)
            {
                var handShakeResult = await new MellatPaymentServiceSwitch1.PaymentGatewayClient().bpPayRequestAsync(
                    input.TerminalId,
                    input.UserName,
                    input.UserPassword,
                    input.OrderId,
                    input.Amount,
                    LocalDate,
                    LocalTime,
                    "",//AdditionalData,
                    input.CallBackUrl,
                    "0",//PayerId,
                    input.MobileNo,
                    "",//EncPan,
                    "",//PanHiddenMode,
                    "",//CartItem,
                    input.EncryptedNationalCode);

                return new() { Result = JsonConvert.SerializeObject(handShakeResult) };
            }
            else if (input.Switch == 2)
            {
                var handShakeResult = await new MellatPaymentServiceSwitch2.PaymentGatewayClient().bpPayRequestAsync(
                    input.TerminalId,
                    input.UserName,
                    input.UserPassword,
                    input.OrderId,
                    input.Amount,
                    LocalDate,
                    LocalTime,
                    "",//AdditionalData,
                    input.CallBackUrl,
                    "0",//PayerId,
                    input.MobileNo,
                    "",//EncPan,
                    "",//PanHiddenMode,
                    "",//CartItem,
                    input.EncryptedNationalCode);

                return new() { Result = JsonConvert.SerializeObject(handShakeResult) };
            }

            return new() { Result = string.Empty };
        }
        [Audited]
        public async Task<OutputDto> HandShakeWithParsian(ParsianHandShakeInputDto input)
        {
            var service = new SaleServiceSoapClient(SaleServiceSoapClient.EndpointConfiguration.SaleServiceSoap);
            var request = new ClientSaleRequestData
            {
                LoginAccount = input.LoginAccount,
                CallBackUrl = input.CallBackUrl,
                Amount = input.Amount,
                OrderId = input.OrderId,
                AdditionalData = JsonConvert.SerializeObject(new { NationalEncryptedId = AesOperation.EncryptString("0|" + input.AdditionalData + "|12345678|", input.Key, input.IV), input.ThirdPartyCode, Data = "" }),
                Originator = input.Originator
            };

            var response = await service.SalePaymentRequestAsync(request);
            return new() { Result = JsonConvert.SerializeObject(response.Body.SalePaymentRequestResult) };
        }
        [Audited]
        public async Task<OutputDto> VerifyToIranKish(IranKishVerifyInputDto input)
        {
            WebHelper webHelper = new();
            string requestVerifyJson = JsonConvert.SerializeObject(input);
            Uri url = new(_config.GetValue<string>("PSP:IranKishVerifyUrl"));
            string jresponse = webHelper.Post(url, requestVerifyJson);
            return new() { Result = jresponse };
        }
        [Audited]
        public async Task<OutputDto> VerifyToMellat(MellatVerifyInputDto input)
        {
            if (input.Switch == 1)
            {
                var verifyResult = await new MellatPaymentServiceSwitch1.PaymentGatewayClient().bpVerifyRequestAsync(
                    input.TerminalId,
                    input.UserName,
                    input.UserPassword,
                    input.OrderId,
                    input.SaleOrderId,
                    input.SaleReferenceId);
                return new() { Result = JsonConvert.SerializeObject(verifyResult) };
            }
            else if (input.Switch == 2)
            {
                var verifyResult = await new MellatPaymentServiceSwitch2.PaymentGatewayClient().bpVerifyRequestAsync(
                    input.TerminalId,
                    input.UserName,
                    input.UserPassword,
                    input.OrderId,
                    input.SaleOrderId,
                    input.SaleReferenceId);
                return new() { Result = JsonConvert.SerializeObject(verifyResult) };
            }
            return new() { Result = string.Empty };
        }
        [Audited]
        public async Task<OutputDto> VerifyToParsian(ParsianVerifyInputDto input)
        {
            var service = new ConfirmServiceSoapClient(ConfirmServiceSoapClient.EndpointConfiguration.ConfirmServiceSoap);

            var request = new ClientConfirmRequestData
            {
                LoginAccount = input.LoginAccount,
                Token = input.Token
            };

            var response = await service.ConfirmPaymentAsync(request);
            return new() { Result = JsonConvert.SerializeObject(response.Body.ConfirmPaymentResult) };
        }
        [Audited]
        public async Task<OutputDto> InquiryToIranKish(IranKishInquiryInputDto input)
        {
            WebHelper webHelper = new();
            string requestVerifyJson = JsonConvert.SerializeObject(input);
            Uri url = new(_config.GetValue<string>("PSP:IranKishInquiryUrl"));
            string jresponse = webHelper.Post(url, requestVerifyJson);
            return new() { Result = jresponse };
        }
        [Audited]
        public async Task<OutputDto> InquiryToMellat(MellatInquiryInputDto input)
        {
            var mellatInputDto = new WcfServiceLibrary.MellatInquiryInputDto
            {
                TerminalId = input.TerminalId,
                ReportServiceUserName = input.ReportServiceUserName,
                ReportServicePassword = input.ReportServicePassword,
                OrderId = input.OrderId,
                Switch = input.Switch
            };

            WcfServiceLibrary.Service1Client client = new();
            var inquiryResponse = await client.MellatGetTransactionStatusByTerminalIdAndOrderIdAsync(mellatInputDto);
            return new() { Result = inquiryResponse };
        }
        [Audited]
        public async Task<OutputDto> InquiryToParsian(ParsianInquiryInputDto input)
        {
            var data = new { input.OrderId, input.LoginAccount };

            Uri baseAddressUri = new(_config.GetValue<string>("PSP:ParsianInquiryUrl"), UriKind.Absolute);

            string authParams = string.Format("{0}|{1}", input.ReportServiceUserName, input.ReportServicePassword);
            byte[] bytes = Encoding.UTF8.GetBytes(authParams);
            string encodedAuthParams = Convert.ToBase64String(bytes);

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(AuthenticationSchemes.Basic.ToString(), encodedAuthParams);
            client.DefaultRequestHeaders.Add("apiVersion", "1.0");

            var requestJson = JsonConvert.SerializeObject(data);

            HttpContent queryString = new StringContent(requestJson, Encoding.UTF8, "application/json");

            var res = client.PostAsync(baseAddressUri, queryString).Result;

            var jresponse = res.Content.ReadAsStringAsync().Result;

            return new() { Result = jresponse };
        }
        [Audited]
        public async Task<OutputDto> ReverseToIranKish(IranKishReverseInputDto input)
        {
            WebHelper webHelper = new();
            string requestVerifyJson = JsonConvert.SerializeObject(input);
            Uri url = new(_config.GetValue<string>("PSP:IranKishReverseUrl"));
            string jresponse = webHelper.Post(url, requestVerifyJson);
            return new() { Result = jresponse };
        }
        [Audited]
        public async Task<OutputDto> ReverseToMellat(MellatReverseInputDto input)
        {
            if (input.Switch == 1)
            {
                var verifyResult = await new MellatPaymentServiceSwitch1.PaymentGatewayClient().bpReversalRequestAsync(
                    input.TerminalId,
                    input.UserName,
                    input.UserPassword,
                    input.OrderId,
                    input.SaleOrderId,
                    input.SaleReferenceId);
                return new() { Result = JsonConvert.SerializeObject(verifyResult) };
            }
            else if (input.Switch == 2)
            {
                var verifyResult = await new MellatPaymentServiceSwitch2.PaymentGatewayClient().bpReversalRequestAsync(
                    input.TerminalId,
                    input.UserName,
                    input.UserPassword,
                    input.OrderId,
                    input.SaleOrderId,
                    input.SaleReferenceId);
                return new() { Result = JsonConvert.SerializeObject(verifyResult) };
            }
            return new() { Result = string.Empty };
        }
        [Audited]
        public async Task<OutputDto> ReverseToParsian(ParsianReverseInputDto input)
        {
            var service = new ReversalServiceSoapClient(ReversalServiceSoapClient.EndpointConfiguration.ReversalServiceSoap);

            var request = new ClientReversalRequestData
            {
                LoginAccount = input.LoginAccount,
                Token = input.Token
            };

            var response = await service.ReversalRequestAsync(request);
            return new() { Result = JsonConvert.SerializeObject(response.Body.ReversalRequestResult) };
        }
    }
}
