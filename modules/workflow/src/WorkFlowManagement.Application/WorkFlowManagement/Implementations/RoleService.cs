using crypto;
using Google.Protobuf.WellKnownTypes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;
using WorkFlowManagement.Application.Contracts.WorkFlowManagement.Constants;
using WorkFlowManagement.Application.Contracts.WorkFlowManagement.Dtos;
using WorkFlowManagement.Application.Contracts.WorkFlowManagement.IServices;
using WorkFlowManagement.Domain.Shared.WorkFlowManagement.Enums;
using WorkFlowManagement.Domain.WorkFlowManagement;

namespace WorkFlowManagement.Application.WorkFlowManagement.Implementations
{
    public class RoleService : ApplicationService, IRoleService
    {
        private readonly IRepository<Role, int> _roleRepository;
        public RoleService(IRepository<Role, int> roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<RoleDto> Add(RoleCreateOrUpdateDto roleCreateOrUpdateDto)
        {
            await Validation(null,roleCreateOrUpdateDto);
            var role = ObjectMapper.Map<RoleCreateOrUpdateDto, Role>(roleCreateOrUpdateDto);
            var entity = await _roleRepository.InsertAsync(role, autoSave: true);
            return ObjectMapper.Map<Role, RoleDto>(entity);

        }

        public async Task<bool> Delete(int id)
        {
            var role = await Validation(id, null);
            await _roleRepository.DeleteAsync(id);
            return true;
        }

        public async Task<RoleDto> GetById(int id)
        {
            var role = await Validation(id, null);
            var roleDto = ObjectMapper.Map<Role, RoleDto>(role);
            return roleDto;
        }

        public async Task<List<RoleDto>> GetList()
        {
            var role = (await _roleRepository.GetQueryableAsync()).ToList();
            var roleDto = ObjectMapper.Map<List<Role>, List<RoleDto>>(role);
            return roleDto;
        }

        public async Task<RoleDto> Update(RoleCreateOrUpdateDto roleCreateOrUpdateDto)
        {
            var role = await Validation(roleCreateOrUpdateDto.Id, roleCreateOrUpdateDto);
            role.Status = roleCreateOrUpdateDto.Status;
            role.Title = roleCreateOrUpdateDto.Title;
            role.Security = JsonConvert.SerializeObject(roleCreateOrUpdateDto.Security);
            var entity = await _roleRepository.UpdateAsync(role);
            return ObjectMapper.Map<Role, RoleDto>(entity);
        }

        private async Task<Role> Validation(int? id, RoleCreateOrUpdateDto roleCreateOrUpdateDto)
        {
            var organizationChartQuery = await _roleRepository.GetQueryableAsync();
            Role role = new Role();
            if (id is not null)
            {
                 role = organizationChartQuery.FirstOrDefault(x => x.Id == id);
                if (role is null)
                {
                    throw new UserFriendlyException(WorkFlowConstant.RoleNotFound, WorkFlowConstant.RoleNotFoundId);
                }
            }

            return role;
        }
    }
}
