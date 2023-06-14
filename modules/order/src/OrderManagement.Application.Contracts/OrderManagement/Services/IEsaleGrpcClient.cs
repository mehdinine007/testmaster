using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace OrderManagement.Application.Contracts.Services
{
    public interface IEsaleGrpcClient : IApplicationService
    {
        Task<UserDto> GetUserById(long userId);

        Task<AdvocacyUserDto> GetUserAdvocacyByNationalCode(string nationlCode);

        Task<PaymentInformationResponseDto> GetPaymentInformation(int paymentId);
    }
}
