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
using WorkFlowManagement.Domain.WorkFlowManagement;

namespace WorkFlowManagement.Application.WorkFlowManagement.Implementations
{
    public class WorkFlowRoleChartService : ApplicationService, IWorkFlowRoleChartService
    {
        private readonly IRepository<WorkFlowRoleChart, int> _workFlowRoleChartRepository;
        public WorkFlowRoleChartService(IRepository<WorkFlowRoleChart, int> workFlowRoleChartRepository)
        {
            _workFlowRoleChartRepository = workFlowRoleChartRepository;
        }

        public async Task<WorkFlowRoleChartDto> Add(WorkFlowRoleChartCreateOrUpdateDto workFlowRoleChartCreateOrUpdateDto)
        {
            await Validation(null, workFlowRoleChartCreateOrUpdateDto);
            var workFlowRoleChart = ObjectMapper.Map<WorkFlowRoleChartCreateOrUpdateDto, WorkFlowRoleChart>(workFlowRoleChartCreateOrUpdateDto);
            var entity = await _workFlowRoleChartRepository.InsertAsync(workFlowRoleChart, autoSave: true);
            return ObjectMapper.Map<WorkFlowRoleChart, WorkFlowRoleChartDto>(entity);
        }

        public async Task<bool> Delete(int id)
        {
            var workFlowRoleChart = await Validation(id, null);
            await _workFlowRoleChartRepository.DeleteAsync(id);
            return true;
        }

        public async Task<WorkFlowRoleChartDto> GetById(int id)
        {
            var workFlowRoleChart = await Validation(id, null);
            var workFlowRoleChartDto = ObjectMapper.Map<WorkFlowRoleChart, WorkFlowRoleChartDto>(workFlowRoleChart);
            return workFlowRoleChartDto;
        }

        public async Task<List<WorkFlowRoleChartDto>> GetList()
        {
            var workFlowRoleChart = (await _workFlowRoleChartRepository.GetQueryableAsync()).ToList();
            var workFlowRoleChartDto = ObjectMapper.Map<List<WorkFlowRoleChart>, List<WorkFlowRoleChartDto>>(workFlowRoleChart);
            return workFlowRoleChartDto;
        }

        public async Task<WorkFlowRoleChartDto> Update(WorkFlowRoleChartCreateOrUpdateDto workFlowRoleChartCreateOrUpdateDto)
        {
            var workFlowRoleChart = await Validation(workFlowRoleChartCreateOrUpdateDto.Id, workFlowRoleChartCreateOrUpdateDto);
            workFlowRoleChart.WorkFlowRoleId = workFlowRoleChartCreateOrUpdateDto.WorkFlowRoleId;
            workFlowRoleChart.OrganizationChartId = workFlowRoleChartCreateOrUpdateDto.OrganizationChartId;
         
            var entity = await _workFlowRoleChartRepository.UpdateAsync(workFlowRoleChart);
            return ObjectMapper.Map<WorkFlowRoleChart, WorkFlowRoleChartDto>(entity);
        }


        private async Task<WorkFlowRoleChart> Validation(int? id, WorkFlowRoleChartCreateOrUpdateDto workFlowRoleChartCreateOrUpdateDto)
        {
            var workFlowRoleChart = new WorkFlowRoleChart();
            var workFlowRoleChartQuery = (await _workFlowRoleChartRepository.GetQueryableAsync());
            if (id != null)
            {
                workFlowRoleChart = workFlowRoleChartQuery.FirstOrDefault(x => x.Id == id);
                if (workFlowRoleChart is null)
                {
                    throw new UserFriendlyException(WorkFlowConstant.OrganizationChartNotFound, WorkFlowConstant.OrganizationChartNotFoundId);
                }
               
            }
            return workFlowRoleChart;
        }
    }
}