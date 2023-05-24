using Volo.Abp.Application.Services;

namespace PaymentManagement.Application.Contracts.IServices
{
    public interface IPaymentAppService : IApplicationService
    {
        Task<List<PspAccountDto>> GetPsps();
        Task<HandShakeOutputDto> HandShakeAsync(HandShakeInputDto input);
        Task<BackFromPspOutputDto> BackFromIranKishAsync(string pspJsonResult);
        Task<string> GetCallBackUrlAsync(int paymentId);
        Task<VerifyOutputDto> VerifyAsync(int paymentId);
        Task<InquiryOutputDto> InquiryAsync(int paymentId);
        Task<ReverseOutputDto> ReverseAsync(int paymentId);
        //todo: Task<SettleOutputDto> SettleAsync(int paymentId);
        Task<List<InquiryWithFilterParamDto>> InquiryWithFilterParamAsync(int filterParam);
        Task RetryForVerify();
    }
}
