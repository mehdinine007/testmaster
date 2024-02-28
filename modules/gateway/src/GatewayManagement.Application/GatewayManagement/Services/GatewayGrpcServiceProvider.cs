using GatewayManagement.Application.Contracts.Dtos;
using GatewayManagement.Application.Contracts.IServices;
using GatewayManagement.Application.GatewayServiceGrpc;
using Grpc.Core;
using Volo.Abp.Auditing;

namespace GatewayManagement.Application.Servicess
{
    public class GatewayGrpcServiceProvider : GatewayServiceGrpc.GatewayServiceGrpc.GatewayServiceGrpcBase
    {
        private readonly IGatewayAppService _gatewayAppService;
        public GatewayGrpcServiceProvider(IGatewayAppService gatewayAppService)
        {
            _gatewayAppService = gatewayAppService;
        }
        public override async Task<Output> Authenticate(AuthenticateInput input, ServerCallContext context)
        {
            var output = await _gatewayAppService.Authenticate(new AuthenticateInputDto
            {
                Type = input.Type,
                UserName = input.UserName,
                Password = input.Password
            });
            return new Output() { Result = output.Result };
        }
        public override async Task<Output> HandShakeWithIranKish(IranKishHandShakeInput input, ServerCallContext context)
        {
            var output = await _gatewayAppService.HandShakeWithIranKish(new IranKishHandShakeInputDto
            {
                TerminalId = input.TerminalId,
                AcceptorId = input.AcceptorId,
                PassPhrase = input.PassPhrase,
                CallBackUrl = input.CallBackUrl,
                Amount = input.Amount,
                RequestId = input.RequestId,
                NationalCode = input.NationalCode,
                Mobile = input.Mobile,
                RsaPublicKey = input.RsaPublicKey
            });
            return new Output() { Result = output.Result };
        }
        public override async Task<Output> HandShakeWithMellat(MellatHandShakeInput input, ServerCallContext context)
        {
            var output = await _gatewayAppService.HandShakeWithMellat(new MellatHandShakeInputDto
            {
                TerminalId = input.TerminalId,
                UserName = input.UserName,
                UserPassword = input.UserPassword,
                OrderId = input.OrderId,
                Amount = input.Amount,
                CallBackUrl = input.CallBackUrl,
                MobileNo = input.MobileNo,
                EncryptedNationalCode = input.EncryptedNationalCode,
                Switch = input.Switch
            });
            return new Output() { Result = output.Result };
        }
        public override async Task<Output> HandShakeWithParsian(ParsianHandShakeInput input, ServerCallContext context)
        {
            var output = await _gatewayAppService.HandShakeWithParsian(new ParsianHandShakeInputDto
            {
                LoginAccount = input.LoginAccount,
                CallBackUrl = input.CallBackUrl,
                Amount = input.Amount,
                OrderId = input.OrderId,
                AdditionalData = input.AdditionalData,
                Originator = input.Originator,
                Key = input.Key,
                IV = input.IV,
                ThirdPartyCode = input.ThirdPartyCode
            });
            return new Output() { Result = output.Result };
        }
        public override async Task<Output> HandShakeWithPasargad(PasargadHandShakeInput input, ServerCallContext context)
        {
            var output = await _gatewayAppService.HandShakeWithPasargad(new PasargadHandShakeInputDto
            {
                Token = input.Token,
                Amount = input.Amount,
                CallbackApi = input.CallbackApi,
                Description = input.Description,
                Invoice = input.Invoice,
                InvoiceDate = input.InvoiceDate,
                MobileNumber = input.MobileNumber,
                PayerMail = input.PayerMail,
                PayerName = input.PayerName,
                ServiceCode = input.ServiceCode,
                ServiceType = input.ServiceType,
                TerminalNumber = input.TerminalNumber,
                NationalCode = input.NationalCode,
                Key = input.Key,
                IV = input.IV
            });
            return new Output() { Result = output.Result };
        }
        public override async Task<Output> VerifyToIranKish(IranKishVerifyInput input, ServerCallContext context)
        {
            var output = await _gatewayAppService.VerifyToIranKish(new IranKishVerifyInputDto
            {
                TerminalId = input.TerminalId,
                RetrievalReferenceNumber = input.RetrievalReferenceNumber,
                SystemTraceAuditNumber = input.SystemTraceAuditNumber,
                TokenIdentity = input.TokenIdentity
            });
            return new Output() { Result = output.Result };
        }
        public override async Task<Output> VerifyToMellat(MellatVerifyInput input, ServerCallContext context)
        {
            var output = await _gatewayAppService.VerifyToMellat(new MellatVerifyInputDto
            {
                TerminalId = input.TerminalId,
                UserName = input.UserName,
                UserPassword = input.UserPassword,
                OrderId = input.OrderId,
                SaleOrderId = input.SaleOrderId,
                SaleReferenceId = input.SaleReferenceId,
                Switch = input.Switch
            });
            return new Output() { Result = output.Result };
        }
        public override async Task<Output> VerifyToParsian(ParsianVerifyInput input, ServerCallContext context)
        {
            var output = await _gatewayAppService.VerifyToParsian(new ParsianVerifyInputDto
            {
                LoginAccount = input.LoginAccount,
                Token = input.Token,
            });
            return new Output() { Result = output.Result };
        }
        public override async Task<Output> VerifyToPasargad(PasargadVerifyInput input, ServerCallContext context)
        {
            var output = await _gatewayAppService.VerifyToPasargad(new PasargadVerifyInputDto
            {
                Token = input.Token,
                Invoice = input.Invoice,
                UrlId = input.UrlId
            });
            return new Output() { Result = output.Result };
        }
        public override async Task<Output> InquiryToIranKish(IranKishInquiryInput input, ServerCallContext context)
        {
            var output = await _gatewayAppService.InquiryToIranKish(new IranKishInquiryInputDto
            {
                TerminalId = input.TerminalId,
                PassPhrase = input.PassPhrase,
                TokenIdentity = input.TokenIdentity,
                FindOption = input.FindOption
            });
            return new Output() { Result = output.Result };
        }
        public override async Task<Output> InquiryToParsian(ParsianInquiryInput input, ServerCallContext context)
        {
            var output = await _gatewayAppService.InquiryToParsian(new ParsianInquiryInputDto
            {
                OrderId = input.OrderId,
                LoginAccount = input.LoginAccount,
                ReportServiceUserName = input.ReportServiceUserName,
                ReportServicePassword = input.ReportServicePassword
            });
            return new Output() { Result = output.Result };
        }
        public override async Task<Output> InquiryToPasargad(PasargadInquiryInput input, ServerCallContext context)
        {
            var output = await _gatewayAppService.InquiryToPasargad(new PasargadInquiryInputDto
            {
                Token = input.Token,
                InvoiceId = input.InvoiceId
            });
            return new Output() { Result = output.Result };
        }
        public override async Task<Output> ReverseToIranKish(IranKishReverseInput input, ServerCallContext context)
        {
            var output = await _gatewayAppService.ReverseToIranKish(new IranKishReverseInputDto
            {
                TerminalId = input.TerminalId,
                RetrievalReferenceNumber = input.RetrievalReferenceNumber,
                SystemTraceAuditNumber = input.SystemTraceAuditNumber,
                TokenIdentity = input.TokenIdentity
            });
            return new Output() { Result = output.Result };
        }
        public override async Task<Output> ReverseToMellat(MellatReverseInput input, ServerCallContext context)
        {
            var output = await _gatewayAppService.ReverseToMellat(new MellatReverseInputDto
            {
                TerminalId = input.TerminalId,
                UserName = input.UserName,
                UserPassword = input.UserPassword,
                OrderId = input.OrderId,
                SaleOrderId = input.SaleOrderId,
                SaleReferenceId = input.SaleReferenceId,
                Switch = input.Switch
            });
            return new Output() { Result = output.Result };
        }
        public override async Task<Output> ReverseToParsian(ParsianReverseInput input, ServerCallContext context)
        {
            var output = await _gatewayAppService.ReverseToParsian(new ParsianReverseInputDto
            {
                LoginAccount = input.LoginAccount,
                Token = input.Token,
            });
            return new Output() { Result = output.Result };
        }
        public override async Task<Output> ReverseToPasargad(PasargadReverseInput input, ServerCallContext context)
        {
            var output = await _gatewayAppService.ReverseToPasargad(new PasargadReverseInputDto
            {
                Token = input.Token,
                Invoice = input.Invoice,
                UrlId = input.UrlId
            });
            return new Output() { Result = output.Result };
        }
    }
}
