using IFG.Core.Infrastructures.TokenAuth;
using Volo.Abp.Application.Services;

namespace OrderManagement.Application.Contracts.OrderManagement.Services;

public interface IAuthenticateAppService : IApplicationService
{
    Task<AuthenticateResultModel> Authenticate(AuthenticateReqDto input);
}