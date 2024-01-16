using Abp.Domain.Uow;
using Abp.Runtime.Session;
using CompanyManagement.Application.Contracts.CompanyManagement.Constants.Permissions;
using CompanyManagement.Application.Contracts.CompanyManagement.Dto.BankDtos;
using CompanyManagement.Application.Contracts.CompanyManagement.Services;
using CompanyManagement.Domain.CompanyManagement;
using Esale.Share.Authorize;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace CompanyManagement.Application.CompanyManagement.Implementations
{
    public class BankAppService : ApplicationService, IBankAppService
    {
        private readonly IRepository<AdvocacyUsersFromBank, int> _advocacyUsersFromBank;
        private readonly IRepository<UserRejectionFromBank, int> _userRejectionFromBankFromBank;

        private readonly ICommonAppService _commonAppService;

        public BankAppService(IRepository<AdvocacyUsersFromBank, int> advocacyUsersFromBank, IRepository<UserRejectionFromBank, int> userRejectionFromBankFromBank
            , ICommonAppService commonAppService) 
        {

            _advocacyUsersFromBank = advocacyUsersFromBank;
            _userRejectionFromBankFromBank = userRejectionFromBankFromBank;
            _commonAppService = commonAppService;

        }

        [SecuredOperation(BankServicePermissionConstants.DeleteAdvocayUserFromBank)]
        public async  Task<bool> DeleteAdvocayUserFromBank(string nationalCode)
        {
         var userId=  _commonAppService.GetUserId();
         
            var ad = (await _advocacyUsersFromBank.GetQueryableAsync()).Select(x => new
            {
                x.nationalcode,
                x.UserId,
                x.Id,
                x.UserUid
            }).FirstOrDefault(x => x.nationalcode == nationalCode
            && x.UserUid == userId);
            if (ad == null)
            {
                throw new UserFriendlyException("رکورد مورد نظر یافت نشد");
            }
            await _advocacyUsersFromBank.DeleteAsync(ad.Id);
            return true;
        }
        [SecuredOperation(BankServicePermissionConstants.SaveUserRejectionFromBank)]
        public async Task<bool> SaveUserRejectionFromBank(UserRejectionFromBankDto userRejectionFromBankDto)
        {
            var userId = _commonAppService.GetUserId();
            UserRejectionFromBank userRejectionFromBank = ObjectMapper.Map<UserRejectionFromBankDto, UserRejectionFromBank>(userRejectionFromBankDto, new UserRejectionFromBank());
            userRejectionFromBank.UserUid = userId;
            await _userRejectionFromBankFromBank.InsertAsync(userRejectionFromBank);
            return true;
        }
        [SecuredOperation(BankServicePermissionConstants.InquiryUserRejectionFromBank)]
        public async Task<UserRejecgtionFromBankExportDto> InquiryUserRejectionFromBank(string nationalCode)
        {
            var userId = _commonAppService.GetUserId();
            var ad = (await _userRejectionFromBankFromBank.GetQueryableAsync())
                .OrderByDescending(x => x.Id).Select(x => new
                {
                    x.accountNumber,
                    x.nationalcode,
                    x.shabaNumber,
                    x.UserId,
                    x.price,
                    x.dateTime,
                    x.UserUid
                }).FirstOrDefault(x => x.nationalcode == nationalCode
                && x.UserUid == userId);
            if (ad == null)
            {
                return null;
            }
            else
            {
                UserRejecgtionFromBankExportDto advocacyUserFromBankExportDto = new UserRejecgtionFromBankExportDto();
                advocacyUserFromBankExportDto.NationalCode = ad.nationalcode;
                advocacyUserFromBankExportDto.ShebaNumber = ad.shabaNumber;
                advocacyUserFromBankExportDto.AccountNumber = ad.accountNumber;
                advocacyUserFromBankExportDto.Price = ad.price;
                advocacyUserFromBankExportDto.dateTime = ad.dateTime;

                return advocacyUserFromBankExportDto;

            }

        }
        [SecuredOperation(BankServicePermissionConstants.SaveAdvocacyUsersFromBank)]
        public async Task<bool> SaveAdvocacyUsersFromBank(List<AdvocacyUsersFromBankDto> advocacyUsersFromBankDto)
        {
            var userId = _commonAppService.GetUserId();
            List<AdvocacyUsersFromBank> advocacyUsersFromBanks = new List<AdvocacyUsersFromBank>();
            UnitOfWorkOptions unitOfWorkOptions = new UnitOfWorkOptions();
            //unitOfWorkOptions.IsTransactional = false;
            //unitOfWorkOptions.Scope = System.Transactions.TransactionScopeOption.RequiresNew;
            //using (var unitOfWork = _unitOfWorkManager.Begin(unitOfWorkOptions))
            //{

            //  _advocacyUsersFromBank.Delete(x => x.UserId == userId);
            advocacyUsersFromBanks = ObjectMapper.Map<List<AdvocacyUsersFromBankDto>, List<AdvocacyUsersFromBank>>(advocacyUsersFromBankDto, new List<AdvocacyUsersFromBank>());
            advocacyUsersFromBanks.ForEach(x =>
            {
                x.UserUid = userId;
            });
            await _advocacyUsersFromBank.InsertManyAsync(advocacyUsersFromBanks);
            CurrentUnitOfWork.SaveChangesAsync();
            //    unitOfWork.Complete();
            //}
            return true;
        }
      
      
    }
}
