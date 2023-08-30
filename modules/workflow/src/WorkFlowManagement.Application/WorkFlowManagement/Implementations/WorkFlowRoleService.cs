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
    public class WorkFlowRoleService : ApplicationService, IWorkFlowRoleService
    {
        private readonly IRepository<WorkFlowRole, int> _workFlowRoleRepository;
        public WorkFlowRoleService(IRepository<WorkFlowRole, int> workFlowRoleRepository)
        {
            _workFlowRoleRepository = workFlowRoleRepository;
        }

        public async Task<WorkFlowRoleDto> Add(WorkFlowRoleCreateOrUpdateDto workFlowRoleCreateOrUpdateDto)
        {
            await Validation(null, workFlowRoleCreateOrUpdateDto);
            var workFlowRole = ObjectMapper.Map<WorkFlowRoleCreateOrUpdateDto, WorkFlowRole>(workFlowRoleCreateOrUpdateDto);
            var entity = await _workFlowRoleRepository.InsertAsync(workFlowRole, autoSave: true);
            return ObjectMapper.Map<WorkFlowRole, WorkFlowRoleDto>(entity);

        }

        public async Task<bool> Delete(int id)
        {
            var workFlowRole = await Validation(id, null);
            await _workFlowRoleRepository.DeleteAsync(id);
            return true;
        }

        public async Task<WorkFlowRoleDto> GetById(int id)
        {
            var workFlowRole = await Validation(id, null);
            var workFlowRoleDto = ObjectMapper.Map<WorkFlowRole, WorkFlowRoleDto>(workFlowRole);
            return workFlowRoleDto;
        }

        public async Task<List<WorkFlowRoleDto>> GetList()
        {
            var workFlowRole = (await _workFlowRoleRepository.GetQueryableAsync()).ToList();
            var workFlowRoleDto = ObjectMapper.Map<List<WorkFlowRole>, List<WorkFlowRoleDto>>(workFlowRole);
            return workFlowRoleDto;
        }

        public async Task<WorkFlowRoleDto> Update(WorkFlowRoleCreateOrUpdateDto workFlowRoleCreateOrUpdateDto)
        {
            var workFlowRole = await Validation(workFlowRoleCreateOrUpdateDto.Id, workFlowRoleCreateOrUpdateDto);
            workFlowRole.Status = workFlowRoleCreateOrUpdateDto.Status;
            workFlowRole.Title = workFlowRoleCreateOrUpdateDto.Title;
            workFlowRole.Security = JsonConvert.SerializeObject(workFlowRoleCreateOrUpdateDto.Security);
            var entity = await _workFlowRoleRepository.UpdateAsync(workFlowRole);
            return ObjectMapper.Map<WorkFlowRole, WorkFlowRoleDto>(entity);
        }

        private async Task<WorkFlowRole> Validation(int? id, WorkFlowRoleCreateOrUpdateDto workFlowRoleCreateOrUpdateDto)
        {
            var organizationChartQuery = await _workFlowRoleRepository.GetQueryableAsync();
            WorkFlowRole workFlowRole = new WorkFlowRole();
            if (id is not null)
            {
                 workFlowRole = organizationChartQuery.FirstOrDefault(x => x.Id == id);
                if (workFlowRole is null)
                {
                    throw new UserFriendlyException(WorkFlowConstant.WorkFlowRoleNotFound, WorkFlowConstant.WorkFlowRoleNotFoundId);
                }
            }

            return workFlowRole;
        }
    }
}
