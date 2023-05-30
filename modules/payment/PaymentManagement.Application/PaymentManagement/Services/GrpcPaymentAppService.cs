using PaymentManagement.Application.Contracts.IServices;
using PaymentManagement.Application.Contracts.Dtos;
using Volo.Abp.Application.Services;

namespace PaymentManagement.Application.PaymentManagement.Services
{
    public class GrpcPaymentAppService : ApplicationService, IGrpcPaymentAppService
    {
        private readonly IPaymentAppService _paymentAppService;
        public GrpcPaymentAppService(IPaymentAppService paymentAppService)
        {
            _paymentAppService = paymentAppService;
        }

        public List<InquiryWithFilterParamDto> GetPaymentStatusList(PaymentStatusDto paymentStatusDto)
        {
            return _paymentAppService.InquiryWithFilterParam(paymentStatusDto.RelationId, paymentStatusDto.RelationIdB, paymentStatusDto.RelationIdC, paymentStatusDto.RelationIdD);
        }

        public async Task<List<RetryForVerifyDetail>> RetryForVerify()
        {
            return await _paymentAppService.RetryForVerify();
        }
    }
}
