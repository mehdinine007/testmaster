using PaymentManagement.Application.Contracts;
using Volo.Abp.Application.Services;

namespace PaymentManagement.Application.Contracts.IServices
{
    public interface IPaymentAppService : IApplicationService
    {
        Task<List<PspAccountDto>> GetPspsByCustomerCode(int customerCode);
        Task<HandShakeOutputDto> HandShakeWithPspAsync(HandShakeInputDto input);
        Task<RedirectToPspOutput> RedirectToPspAsync(int paymentId);
        Task<BackFromPspOutputDto> BackFromIranKishAsync(string pspJsonResult);
        Task<string> GetCallBackUrlAsync(int paymentId);
        Task<VerifyOutputDto> VerifyAsync(int paymentId);
        Task<List<InquiryWithFilterParamDto>> InquiryWithFilterParamAsync(int customerCode, string filterParam);
        Task RetryForVerify();
        //Task<string> Settle(string NationalCode);
        //Task<string> Inquiry(string NationalCode);        
        //Task<string> Reverse(string NationalCode);
        //Task<string> Refound(string NationalCode);
        //todo:ssis
        //todo:پیاده سازی سرویس های تسهیم و پرداخت قبض و سایر سرویس های درگاه های پرداخت
    }
}
