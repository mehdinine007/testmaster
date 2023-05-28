using PaymentManagement.Application.Contracts;
using System.ServiceModel;
using Volo.Abp.Application.Services;

namespace PaymentManagement.Application.Contracts.IServices
{
    public interface IPaymentAppService : IApplicationService
    {
        List<PspAccountDto> GetPsps();
        Task<HandShakeOutputDto> HandShakeAsync(HandShakeInputDto input);
        Task<BackFromPspOutputDto> BackFromIranKishAsync(string pspJsonResult);
        Task<BackFromPspOutputDto> BackFromMellatAsync(string pspJsonResult);
        string GetCallBackUrl(int paymentId);
        Task<VerifyOutputDto> VerifyAsync(int paymentId);
        Task<InquiryOutputDto> InquiryAsync(int paymentId);
        Task<ReverseOutputDto> ReverseAsync(int paymentId);
        //todo: Task<SettleOutputDto> SettleAsync(int paymentId);
        List<InquiryWithFilterParamDto> InquiryWithFilterParam(int? filterParam1, int? filterParam2, int? filterParam3, int? filterParam4);
        Task RetryForVerify();
    }
}
