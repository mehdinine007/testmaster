using IFG.Core.Infrastructures.TokenAuth;
using Volo.Abp.Application.Services;

namespace CompanyManagement.Application.Contracts.CompanyManagement.IServices
{
    public interface IUserGrpcClientService : IApplicationService
    {
        Task<AuthenticateResponseDto> Athenticate(AuthenticateReqDto input);
    }
}
