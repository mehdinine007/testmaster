using PaymentManagement.Application.Contracts.Dtos;
using Volo.Abp.Application.Services;

namespace PaymentManagement.Application.Contracts.IServices
{
    public interface IPaymentAppService : IApplicationService
    {
        List<PspAccountDto> GetPsps();
        PaymentInfoDto GetPaymentInfo(int paymentId);
        Task<HandShakeOutputDto> HandShakeAsync(HandShakeInputDto input);
        Task<BackFromPspOutputDto> BackFromIranKishAsync(string pspJsonResult);
        Task<BackFromPspOutputDto> BackFromMellatAsync(string pspJsonResult);
        Task<BackFromPspOutputDto> BackFromParsianAsync(string pspJsonResult);
        Task<BackFromPspOutputDto> BackFromPasargadAsync(string pspJsonResult);
        string GetCallBackUrl(int paymentId);
        Task<VerifyOutputDto> VerifyAsync(int paymentId);
        Task<InquiryOutputDto> InquiryAsync(int paymentId);
        Task<ReverseOutputDto> ReverseAsync(int paymentId);
        //todo: Task<SettleOutputDto> SettleAsync(int paymentId);
        List<InquiryWithFilterParamDto> InquiryWithFilterParam(int? filterParam1, int? filterParam2, int? filterParam3, int? filterParam4
            , bool? IsRelationIdGroup = null
            , bool? IsRelationIdBGroup = null
            , bool? IsRelationIdCGroup = null 
            , bool? IsRelationIdDGroup = null );
        List<InquiryWithFilterParamDto> InquiryWithFilterParamGroupByParams(int? filterParam1, int? filterParam2, int? filterParam3, int? filterParam4);
        Task<List<RetryForVerifyOutputDto>> RetryForVerify();
        Task InsertPaymentLogAsync(PaymentLogDto input);
    }
}
