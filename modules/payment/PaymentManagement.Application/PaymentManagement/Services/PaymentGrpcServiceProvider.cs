using PaymentManagement.Application.Contracts.IServices;
using PaymentManagement.Application.PaymentServiceGrpc;
using Grpc.Core;
using Google.Protobuf.WellKnownTypes;
using PaymentManagement.Application.Contracts.Dtos;
using PaymentManagement.Domain.Models;

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
        public override Task<PaymentStatusViewModel> GetPaymentStatusList(PaymentGetStatusDto paymentStatusDto,ServerCallContext context)
        {

            var paymentStatus = _paymentAppService.InquiryWithFilterParam(paymentStatusDto.RelationId, paymentStatusDto.RelationIdB, paymentStatusDto.RelationIdC, paymentStatusDto.RelationIdD);
            if (paymentStatus == null)
                return Task.FromResult(new PaymentStatusViewModel());
            var paymentViewModel = new PaymentStatusViewModel();
            paymentViewModel.PaymentStatusData.AddRange(paymentStatus.Select(x => new PaymentStatusData()
            {
                Count = x.Count,
                Message = x.Message,
                Status = x.Status
            }).ToList());
            return Task.FromResult(paymentViewModel);
        }

        public override async Task<RetryForVerifyResponse> RetryForVerify(RetryForVerifyRequest retryForVerifyRequest, ServerCallContext context)
        {
            var payment = await _paymentAppService.RetryForVerify();
            if (payment == null)
                return new RetryForVerifyResponse();
            var paymentViewModel = new RetryForVerifyResponse();
            paymentViewModel.RetryForVerifyData.AddRange(payment.Select(x => new RetryForVerifyData()
            {
                PaymentId = x.PaymentId,
                PaymentStatus = (int)x.PaymentStatus,
                FilterParam1 = x.FilterParam1 ?? 0,
                FilterParam2 = x.FilterParam2 ?? 0,
                FilterParam3 = x.FilterParam3 ?? 0,
                FilterParam4 = x.FilterParam4 ?? 0
            }).ToList());
            return paymentViewModel;
        }
    }
}
