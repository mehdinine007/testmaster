using OrderManagement.Application.Contracts.OrderManagement.Services;
using OrderManagement.Application.Contracts;
using OrderManagement.Application.Contracts.Services;
using OrderManagement.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using OrderManagement.Application.Contracts.OrderManagement;
using OrderManagement.Domain.Shared;
using OrderManagement.Domain.OrderManagement;
using Volo.Abp.ObjectMapping;
using Volo.Abp;
using Esale.Core.DataAccess;
using MongoDB.Driver;
using Microsoft.EntityFrameworkCore;


namespace OrderManagement.Application.OrderManagement.Implementations
{
    public class BankAppService : ApplicationService, IBankAppService
    {

        private readonly IRepository<Bank, int> _bankRepository;
        private readonly IAttachmentService _attachmentService;
        public BankAppService(IRepository<Bank, int> bankRepository, IAttachmentService attachmentService)
        {
            _bankRepository = bankRepository;
            _attachmentService = attachmentService;
        }
        public async Task<List<BankDto>> GetList(AttachmentEntityTypeEnum attachmentType)
        {
            var banks = (await _bankRepository.GetQueryableAsync()).ToList();
            var attachments = await _attachmentService.GetList(AttachmentEntityEnum.Bank, banks.Select(x => x.Id).ToList(),new List<AttachmentEntityTypeEnum> { attachmentType });
            var banksDto = ObjectMapper.Map<List<Bank>, List<BankDto>>(banks);
            banksDto.ForEach(x =>
            {
                var attachment = attachments.Where(y => y.EntityId == x.Id).ToList();
                x.Attachments = ObjectMapper.Map<List<AttachmentDto>, List<AttachmentViewModel>>(attachment);
            });
           
            return banksDto;
        }
        public async Task<BankDto> Add(BankCreateOrUpdateDto bankCreateOrUpdateDto)
        {
            var bank = ObjectMapper.Map<BankCreateOrUpdateDto, Bank>(bankCreateOrUpdateDto);
            var entity = await _bankRepository.InsertAsync(bank, autoSave: true);
            return ObjectMapper.Map<Bank, BankDto>(entity);
        }

        public async Task<BankDto> Update(BankCreateOrUpdateDto bankCreateOrUpdateDto)
        {
            var bank = await Validation(bankCreateOrUpdateDto.Id, bankCreateOrUpdateDto);
            bank.Title= bankCreateOrUpdateDto.Title;
            bank.PhoneNumber= bankCreateOrUpdateDto.PhoneNumber;
            bank.Url= bankCreateOrUpdateDto.Url;
            await _bankRepository.UpdateAsync(bank, autoSave: true);
            return await GetById(bank.Id);
            
        }
        public async Task<BankDto> GetById(int id)
        {
            var bank = await Validation(id, null);
            var attachments = await _attachmentService.GetList(AttachmentEntityEnum.Bank, new List<int>() { id },new List<AttachmentEntityTypeEnum>());
            var bankDto= ObjectMapper.Map<Bank, BankDto>(bank);
            bankDto.Attachments= ObjectMapper.Map<List<AttachmentDto>, List<AttachmentViewModel>>(attachments);
            return bankDto;
        }
        public async Task<bool> Delete(int id)
        {
            await Validation(id, null);
            await _bankRepository.DeleteAsync(x => x.Id == id);
            await _attachmentService.DeleteByEntityId(AttachmentEntityEnum.Bank, id);
            return true;
        }

        private async Task<Bank> Validation(int id, BankCreateOrUpdateDto bankCreateOrUpdateDto)
        {
            var bank = (await _bankRepository.GetQueryableAsync()).AsNoTracking()
                .FirstOrDefault(x => x.Id == id);
            if (bank is null)
            {
                throw new UserFriendlyException(OrderConstant.BankNotFound, OrderConstant.BankNotFoundId);
            }
            return bank;
        }

        public async Task<bool> UploadFile(UploadFileDto uploadFile)
        {
            var bank = await Validation(uploadFile.Id, null);
            await _attachmentService.UploadFile(AttachmentEntityEnum.Bank, uploadFile);
            return true;
        }

        public Task DeleteAdvocayUserFromBank(string nationalCode)
        {
            throw new NotImplementedException();
        }

        public Task<List<AdvocacyUsersFromBankWithCompanyDto>> GetAdvocacyUserByCompanyId()
        {
            throw new NotImplementedException();
        }

        public Task<AdvocacyUserFromBankExportDto> InquiryAdvocacyUserReport(string nationalCode)
        {
            throw new NotImplementedException();
        }

        public Task SaveAdvocacyUsersFromBank(List<AdvocacyUsersFromBankDto> advocacyUsersFromBankDto)
        {
            throw new NotImplementedException();
        }


        public Task<AdvocacyUserFromBankDto> CheckAdvocacy(string NationalCode)
        {
            throw new NotImplementedException();
        }
    }
}
