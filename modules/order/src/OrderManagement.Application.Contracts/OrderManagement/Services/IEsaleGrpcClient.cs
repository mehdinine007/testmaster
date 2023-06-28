using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace OrderManagement.Application.Contracts.Services
{
    public interface IEsaleGrpcClient : IApplicationService
    {
        Task<UserDto> GetUserById(long userId);

        Task<AdvocacyUserDto> GetUserAdvocacyByNationalCode(string nationlCode);

        Task<PaymentInformationResponseDto> GetPaymentInformation(int paymentId);
        Task<List<PaymentStatusModel>> GetPaymentStatusList(PaymentStatusDto paymentStatusDto);
        Task<List<RetryForVerifyPaymentDto>> RetryForVerify();
        Task<List<PaymentStatusModel>> GetPaymentStatusByGroupList(PaymentStatusDto paymentStatusDto);
    }
}
