using UserManagement.Application.Contracts.Models;
using Volo.Abp.Application.Services;

namespace UserManagement.Application.Contracts.Services;

public interface IBaseInformationService : IApplicationService
{
    void RegistrationValidationWithoutCaptcha(RegistrationValidationDto input);
}