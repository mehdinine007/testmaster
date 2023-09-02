using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using WorkFlowManagement.Application.Contracts.WorkFlowManagement.Dtos;

namespace WorkFlowManagement.Application.Contracts.WorkFlowManagement.IServices
{
    public interface IRoleService: IApplicationService
    {

        Task<RoleDto> GetById(int id);
        Task<RoleDto> Add(RoleCreateOrUpdateDto roleCreateOrUpdateDto);
        Task<RoleDto> Update(RoleCreateOrUpdateDto roleCreateOrUpdateDto);
        Task<List<RoleDto>> GetList();
        Task<bool> Delete(int id);
    }
}
