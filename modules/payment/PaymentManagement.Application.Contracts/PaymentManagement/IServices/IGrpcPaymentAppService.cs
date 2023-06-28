using PaymentManagement.Application.Contracts.Dtos;
using System.ServiceModel;

namespace PaymentManagement.Application.Contracts.IServices
{
    [ServiceContract]
    public interface IGrpcPaymentAppService
    {
        List<InquiryWithFilterParamDto> GetPaymentStatusList(PaymentStatusDto paymentStatusDto);
        Task<List<RetryForVerifyOutputDto>> RetryForVerify();
        List<InquiryWithFilterParamDto> GetPaymentStatusByGroupList(PaymentStatusDto paymentStatusDto);

    }
}
