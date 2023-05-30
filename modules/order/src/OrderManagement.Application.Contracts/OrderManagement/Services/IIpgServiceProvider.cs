using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace OrderManagement.Application.Contracts.Services
{
    public interface IIpgServiceProvider : IApplicationService
    {
        Task<List<PspDto>> GetPsps();

        Task<ApiResult<IpgApiResult>> HandShakeWithPsp(PspHandShakeRequest handShakeRequest);

        Task<ApiResult<PspInteractionResult>> VerifyTransaction(int paymentId);

        Task ReverseTransaction(int paymentId);
        Task RetryForVerify();
    }
}
