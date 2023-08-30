using OrderManagement.Application.Contracts.OrderManagement;
using OrderManagement.Domain.Shared;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace OrderManagement.Application.Contracts.Services
{
    public interface IBankAppService : IApplicationService
    {
        Task<BankDto> GetById(int id, List<AttachmentEntityTypeEnum>? attachmentType = null);
        Task<BankDto> Add(BankCreateOrUpdateDto bankCreateOrUpdateDto);
        Task<BankDto> Update(BankCreateOrUpdateDto bankCreateOrUpdateDto);
        Task<List<BankDto>> GetList(List<AttachmentEntityTypeEnum>? attachmentType=null);
        Task<bool> Delete(int id);
        Task<bool> UploadFile(UploadFileDto uploadFile);
        Task SaveAdvocacyUsersFromBank(List<AdvocacyUsersFromBankDto> advocacyUsersFromBankDto);
        //Task<List<UserRejectionAdvocacyDto>> GetUserRejecttionAdvocacyList();
        Task DeleteAdvocayUserFromBank(string nationalCode);
        Task<AdvocacyUserFromBankDto> CheckAdvocacy(string NationalCode);
        Task<AdvocacyUserFromBankExportDto> InquiryAdvocacyUserReport(string nationalCode);
        Task<List<AdvocacyUsersFromBankWithCompanyDto>> GetAdvocacyUserByCompanyId();
    }
}
