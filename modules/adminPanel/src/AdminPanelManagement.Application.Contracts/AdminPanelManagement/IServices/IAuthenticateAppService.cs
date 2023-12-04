using CompanyManagement.Application.Contracts;
using IFG.Core.Infrastructures.TokenAuth;
using Volo.Abp.Application.Services;

namespace AdminPanelManagement.Application.Contracts.AdminPanelManagement.IServices
{
    public interface IAuthenticateAppService: IApplicationService
    {

        Task<AuthenticateResultModel> Authenticate(AuthenticateReqDto model);
    }
}
