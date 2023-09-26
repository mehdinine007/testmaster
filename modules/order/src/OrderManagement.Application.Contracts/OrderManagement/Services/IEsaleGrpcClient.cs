using Volo.Abp.Application.Services;

namespace OrderManagement.Application.Contracts.Services
{
    public interface IEsaleGrpcClient : IApplicationService
    {
        Task<UserDto> GetUserId(string userId);
        Task<AdvocacyUserDto> GetUserAdvocacyByNationalCode(string nationlCode);
        Task<PaymentInformationResponseDto> GetPaymentInformation(int paymentId);
        Task<List<PaymentStatusModel>> GetPaymentStatusList(PaymentStatusDto paymentStatusDto);
        Task<List<RetryForVerifyPaymentDto>> RetryForVerify();
        Task<List<PaymentStatusModel>> GetPaymentStatusByGroupList(PaymentStatusDto paymentStatusDto);
        Task<PaymentHandShakeViewModel> HandShake(PaymentHandShakeDto handShakeDto);
        Task<PaymentResultViewModel> Verify(int paymentId);
        Task<PaymentResultViewModel> Reverse(int paymentId);
    }
}
