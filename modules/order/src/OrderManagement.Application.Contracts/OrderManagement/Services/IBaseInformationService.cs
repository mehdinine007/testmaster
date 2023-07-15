using OrderManagement.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace OrderManagement.Application.Contracts.Services
{
    public interface IBaseInformationService : IApplicationService
    {
        List<CompanyDto> GetCompanies();

        Task<List<CarTipDto>> GetCarTipsByCompanyId(int companyId);
        Task RegistrationValidation(RegistrationValidationDto input);
        List<PublicDto> GetProvince();
        List<PublicDto> GetCities(int ProvienceId);
        void CheckBlackList(int esaleTypeId);
        Task CheckAdvocacyPrice(decimal MinimumAmountOfProxyDeposit);
        void RegistrationValidationWithoutCaptcha(RegistrationValidationDto input);
        void CheckWhiteList(WhiteListEnumType whiteListEnumType, string Nationalcode = "");

        Task<UserDto> GrpcTest();

        Task<List<AgencyDto>> GetAgencies(Guid saleDetailUid);
       // Task<List<AgencyDto>> GetAgencies();
        Task<List<ESaleTypeDto>> GetSaleTypes();
        Task ClearCache(string prefix);
    }
}
