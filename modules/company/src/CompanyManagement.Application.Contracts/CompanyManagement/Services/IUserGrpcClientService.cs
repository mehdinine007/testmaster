using CompanyManagement.Application.Contracts.CompanyManagement.Dtos;
using Volo.Abp.Application.Services;

namespace CompanyManagement.Application.Contracts.CompanyManagement.IServices
{
    public interface IUserGrpcClientService : IApplicationService
    {
        Task<AuthenticateResponseDto> Athenticate(AuthenticateReqDto input);
    }
}
