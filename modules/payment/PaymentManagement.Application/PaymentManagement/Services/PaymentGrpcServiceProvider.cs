using PaymentManagement.Application.Contracts.IServices;
using PaymentManagement.Application.PaymentServiceGrpc;
using Grpc.Core;
using Google.Protobuf.WellKnownTypes;

namespace PaymentManagement.Application.PaymentManagement.Services
{
    public class PaymentGrpcServiceProvider : PaymentServiceGrpc.PaymentServiceGrpc.PaymentServiceGrpcBase
    {
        private readonly IPaymentAppService _paymentAppService;

        public PaymentGrpcServiceProvider(IPaymentAppService paymentAppService)
        {
            _paymentAppService = paymentAppService;
        }

        public override Task<PaymentInformationResponse> GetPaymentInformation(PaymentInformationRequest request, ServerCallContext context)
        {
            
            var paymentInformation = _paymentAppService.GetPaymentInfo(request.PaymentId);
            if (paymentInformation == null)
                throw new InvalidOperationException();
            return Task.FromResult(new PaymentInformationResponse()
            {
                PaymentId = paymentInformation.PaymentId,
                TransactionCode  = paymentInformation.TransactionCode,
                TransactionDate = Timestamp.FromDateTimeOffset(paymentInformation.TransactionDate),
                TransactionPersianDate =  paymentInformation.TransactionPersianDate,
            });
        }
    }
}
