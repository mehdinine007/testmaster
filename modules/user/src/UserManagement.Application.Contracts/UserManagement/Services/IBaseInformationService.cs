using UserManagement.Application.Contracts.Models;
using UserManagement.Domain.UserManagement.Bases;
using Volo.Abp.Application.Services;

namespace UserManagement.Application.Contracts.Services;

public interface IBaseInformationService : IApplicationService
{
    Task RegistrationValidationWithoutCaptchaAsync(RegistrationValidationDto input);

    Task<bool> CheckWhiteListAsync(WhiteListEnumType whiteListEnumType, string Nationalcode);
}
