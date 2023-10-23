
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using WorkFlowManagement.Application.Contracts.WorkFlowManagement.Dtos;

namespace WorkFlowManagement.Application.Contracts.WorkFlowManagement.IServices
{
    public interface IUserGrpcClientService:IApplicationService
    {
        Task<UserDto> GetUserById(Guid userId);
        Task<AuthenticateResponseDto> Athenticate(AuthenticateReqDto input);



    }
}
