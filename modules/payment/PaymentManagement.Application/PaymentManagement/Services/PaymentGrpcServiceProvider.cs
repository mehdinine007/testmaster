using PaymentManagement.Application.Contracts.IServices;
using PaymentManagement.Application.PaymentServiceGrpc;
using Grpc.Core;
using Google.Protobuf.WellKnownTypes;
using PaymentManagement.Application.Contracts.Dtos;
using PaymentManagement.Domain.Models;
using NPOI.POIFS.Properties;

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
                PaymentStatusId = paymentInformation.PaymentStatusId
            });
        }
        public override Task<PaymentStatusViewModel> GetPaymentStatusList(PaymentGetStatusDto paymentStatusDto,ServerCallContext context)
        {
            var paymentStatus = _paymentAppService.InquiryWithFilterParam(paymentStatusDto.RelationId, paymentStatusDto.RelationIdB, paymentStatusDto.RelationIdC, paymentStatusDto.RelationIdD
                , paymentStatusDto.IsRelationIdGroup
                , paymentStatusDto.IsRelationIdBGroup
                , paymentStatusDto.IsRelationIdCGroup
                , paymentStatusDto.IsRelationIdDGroup
                );
            if (paymentStatus == null)
                return Task.FromResult(new PaymentStatusViewModel());
            var paymentViewModel = new PaymentStatusViewModel();
            paymentViewModel.PaymentStatusData.AddRange(paymentStatus.Select(x => new PaymentStatusData()
            {
                Count = x.Count,
                Message = x.Message,
                Status = x.Status,
                F1 = x.filterParam1 == null ? 0 : (int)x.filterParam1,
                F2 = x.filterParam2 == null ? 0 : (int)x.filterParam2,
                F3 = x.filterParam3 == null ? 0 : (int)x.filterParam3,
                F4 = x.filterParam4 == null ? 0 : (int)x.filterParam4,

            }).ToList());
            return Task.FromResult(paymentViewModel);
        }
        public override Task<PaymentStatusGroupViewModel> GetPaymentStatusByGroupList(PaymentGetStatusDto paymentStatusDto, ServerCallContext context)
        {

            var paymentStatus = _paymentAppService.InquiryWithFilterParamGroupByParams(paymentStatusDto.RelationId, paymentStatusDto.RelationIdB, paymentStatusDto.RelationIdC, paymentStatusDto.RelationIdD);
            if (paymentStatus == null)
                return Task.FromResult(new PaymentStatusGroupViewModel());
            var paymentViewModel = new PaymentStatusGroupViewModel();
            paymentViewModel.PaymentStatusData.AddRange(paymentStatus.Select(x => new PaymentStatusDataGroup()
            {
                Count = x.Count,
                Message = x.Message,
                Status = x.Status,
                F1 = x.filterParam1 == null ? 0: (int)x.filterParam1,
                F2 = x.filterParam2 == null ? 0: (int)x.filterParam2,
                F3 = x.filterParam3 == null ? 0: (int)x.filterParam3,
                F4 = x.filterParam4 == null ? 0: (int)x.filterParam4,

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
                FilterParam1 = x.FilterParam1,
                FilterParam2 = x.FilterParam2,
                FilterParam3 = x.FilterParam3,
                FilterParam4 = x.FilterParam4
            }).ToList());
            return paymentViewModel;
        }

        public override async Task<HandShakeViewModel> HandShake(HandShakeDto handShakeDto, ServerCallContext context)
        {
            var handShake = await _paymentAppService.HandShakeAsync(
                 new HandShakeInputDto()
                 {
                     AdditionalData = handShakeDto.AdditionalData,
                     Amount = handShakeDto.Amount,
                     CallBackUrl = handShakeDto.CallBackUrl,
                     Mobile = handShakeDto.Mobile,
                     NationalCode = handShakeDto.NationalCode,
                     PspAccountId = handShakeDto.PspAccountId,
                     FilterParam1 = handShakeDto.FilterParam1,
                     FilterParam2 = handShakeDto.FilterParam2,
                     FilterParam3 = handShakeDto.FilterParam3,
                     FilterParam4 = handShakeDto.FilterParam4
                 }
                );
            if (handShake == null)
            {
                return new HandShakeViewModel();
            }
            return new HandShakeViewModel()
            {
                Token = handShake.Token,
                HtmlContent = handShake.HtmlContent,
                Message = handShake.Message,
                PaymentId = handShake.PaymentId,
                PspJsonResult = handShake.PspJsonResult,
                StatusCode = handShake.StatusCode
            };
        }
        public override async Task<PaymentOutputViewModel> Verify(PaymentInputDto paymentInputDto, ServerCallContext context)
        {
            var verify = await _paymentAppService.VerifyAsync(paymentInputDto.PaymentId);
            if (verify == null)
            {
                return new PaymentOutputViewModel();
            }
            return new PaymentOutputViewModel()
            {
                Message = verify.Message,
                PaymentId = verify.PaymentId,
                PspJsonResult = verify.PspJsonResult,
                StatusCode = verify.StatusCode
            };
        }
        public override async Task<PaymentOutputViewModel> Reverse(PaymentInputDto paymentInputDto, ServerCallContext context)
        {
            var reverse = await _paymentAppService.ReverseAsync(paymentInputDto.PaymentId);
            if (reverse == null)
            {
                return new PaymentOutputViewModel();
            }
            return new PaymentOutputViewModel()
            {
                Message = reverse.Message,
                PaymentId = reverse.PaymentId,
                PspJsonResult = reverse.PspJsonResult,
                StatusCode = reverse.StatusCode
            };
        }

    }
}
