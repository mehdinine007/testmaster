using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace OrderManagement.Application.Contracts.Services
{
    public interface IIpgServiceProvider : IApplicationService
    {
        Task<List<PspDto>> GetPsps();

        Task<HandShakeResponseDto> HandShakeWithPsp(PspHandShakeRequest handShakeRequest);

        Task<PspInteractionResult> VerifyTransaction(int paymentId);
        Task RetryForVerify();
    }
}
