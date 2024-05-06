using UserManagement.Application.Contracts.Models;
using UserManagement.Domain.UserManagement.Bases;
using Volo.Abp.Application.Services;

namespace UserManagement.Application.Contracts.Services;

public interface IBaseInformationService : IApplicationService
{
    Task RegistrationValidationWithoutCaptcha(RegistrationValidationDto input);

    Task<bool> CheckWhiteListAsync(WhiteListEnumType whiteListEnumType, string Nationalcode);
    Task<UserGrpcDto> GetUserByIdAsync(string userId);
    Task<UserGrpcDto> GetUserByNationalCode(string nationalCode);
    Task<bool> RegistrationValidationAsync(RegistrationValidationDto input);
    Task<string> AddressInquiry(AddressInquiryDto input);
}
