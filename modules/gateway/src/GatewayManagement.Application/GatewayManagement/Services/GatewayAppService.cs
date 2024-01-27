using GatewayManagement.Application.Contracts.Dtos;
using GatewayManagement.Application.Contracts.IServices;
using GatewayManagement.Application.IranKish;
using GatewayManagement.Application.Utilities;
using Newtonsoft.Json;
using ParsianSaleService;
using System.Net;
using Volo.Abp.Application.Services;
using Volo.Abp.Auditing;

namespace GatewayManagement.Application.Servicess
{
    public class GatewayAppService : ApplicationService, IGatewayAppService
    {
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
            Uri url = new(Constants.IranKishGetTokenUrl);
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
            var parsianService = new SaleServiceSoapClient(SaleServiceSoapClient.EndpointConfiguration.SaleServiceSoap12);
            ServicePointManager.ServerCertificateValidationCallback = (o, xc, xch, sslP) => true;
        
            var saleRequest = new ClientSaleRequestData
            {
                LoginAccount = input.LoginAccount,
                CallBackUrl = input.CallBackUrl,
                Amount = input.Amount,
                OrderId = input.OrderId,
                AdditionalData = input.AdditionalData,
                Originator = input.Originator
            };

            SalePaymentRequestResponse saleResponse = await parsianService.SalePaymentRequestAsync(saleRequest);
            return new() { Result = JsonConvert.SerializeObject(saleResponse) };
        }
        [Audited]
        public async Task<OutputDto> VerifyToIranKish(IranKishVerifyInputDto input)
        {
            WebHelper webHelper = new();
            string requestVerifyJson = JsonConvert.SerializeObject(input);
            Uri url = new(Constants.IranKishVerifyUrl);
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
        public async Task<OutputDto> InquiryToIranKish(IranKishInquiryInputDto input)
        {
            WebHelper webHelper = new();
            string requestVerifyJson = JsonConvert.SerializeObject(input);
            Uri url = new(Constants.IranKishInquiryUrl);
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
        public async Task<OutputDto> ReverseToIranKish(IranKishReverseInputDto input)
        {
            WebHelper webHelper = new();
            string requestVerifyJson = JsonConvert.SerializeObject(input);
            Uri url = new(Constants.IranKishReverseUrl);
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
    }
}
