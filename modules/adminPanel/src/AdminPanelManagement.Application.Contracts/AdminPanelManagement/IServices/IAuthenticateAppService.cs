using AdminPanelManagement.Application.Contracts.AdminPanelManagement.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace AdminPanelManagement.Application.Contracts.AdminPanelManagement.IServices
{
    public interface IAuthenticateAppService: IApplicationService
    {

        Task<AuthenticateResultModel> Authenticate(AuthenticateReqDto model);
    }
}
