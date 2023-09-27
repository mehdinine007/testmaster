using UserManagement.Application.Contracts.Models;
using UserManagement.Domain.UserManagement.Bases;
using Volo.Abp.Application.Services;

namespace UserManagement.Application.Contracts.Services;

public interface IBaseInformationService : IApplicationService
{
    void RegistrationValidationWithoutCaptcha(RegistrationValidationDto input);

    Task<bool> CheckWhiteListAsync(WhiteListEnumType whiteListEnumType, string Nationalcode);
    Task<UserGrpcDto> GetUserByIdAsync(string userId);
    Task RegistrationValidation(RegistrationValidationDto input);
}
