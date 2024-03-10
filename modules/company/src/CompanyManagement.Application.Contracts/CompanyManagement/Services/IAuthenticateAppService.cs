using IFG.Core.Infrastructures.TokenAuth;
using Volo.Abp.Application.Services;

namespace CompanyManagement.Application.Contracts.CompanyManagement.Services;


public interface IAuthenticateAppService : IApplicationService
{
    Task<AuthenticateResultModel> Authenticate(AuthenticateReqDto model);
}
