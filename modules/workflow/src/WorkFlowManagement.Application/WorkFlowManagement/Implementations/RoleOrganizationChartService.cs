using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Math.EC.Rfc7748;
using System.ComponentModel.DataAnnotations;
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
    public class RoleOrganizationChartService : ApplicationService, IRoleOrganizationChartService
    {
        private readonly IRepository<RoleOrganizationChart, int> _roleOrganizationChartRepository;
        private readonly IRepository<Role, int> _roleRepository;
        private readonly IRepository<OrganizationChart, int> _organizationChartRepository;
        public RoleOrganizationChartService(IRepository<RoleOrganizationChart, int> roleOrganizationChartRepository, IRepository<Role, int> roleRepository, IRepository<OrganizationChart, int> organizationChartRepository)
        {
            _roleOrganizationChartRepository = roleOrganizationChartRepository;
            _organizationChartRepository = organizationChartRepository;
            _roleRepository = roleRepository;
        }

        public async Task<RoleOrganizationChartDto> Add(RoleOrganizationChartCreateOrUpdateDto roleOrganizationChartCreateOrUpdateDto)
        {
            await Validation(null, roleOrganizationChartCreateOrUpdateDto);
            var roleOrganizationChart = ObjectMapper.Map<RoleOrganizationChartCreateOrUpdateDto, RoleOrganizationChart>(roleOrganizationChartCreateOrUpdateDto);
            var entity = await _roleOrganizationChartRepository.InsertAsync(roleOrganizationChart, autoSave: true);
            return ObjectMapper.Map<RoleOrganizationChart, RoleOrganizationChartDto>(entity);
        }

        public async Task<bool> Delete(int id)
        {
            var roleOrganizationChart = await Validation(id, null);
            await _roleOrganizationChartRepository.DeleteAsync(id);
            return true;
        }

        public async Task<RoleOrganizationChartDto> GetById(int id)
        {
            var roleOrganizationChart = await Validation(id, null);
            var roleOrganizationChartDto = ObjectMapper.Map<RoleOrganizationChart, RoleOrganizationChartDto>(roleOrganizationChart);
            return roleOrganizationChartDto;
        }

        public async Task<List<RoleOrganizationChartDto>> GetList()
        {
            var roleOrganizationChart = (await _roleOrganizationChartRepository.GetQueryableAsync()).Include(x=>x.Role).Include(x => x.OrganizationChart).ToList();
            var roleOrganizationChartDto = ObjectMapper.Map<List<RoleOrganizationChart>, List<RoleOrganizationChartDto>>(roleOrganizationChart);
            return roleOrganizationChartDto;
        }

        public async Task<RoleOrganizationChartDto> Update(RoleOrganizationChartCreateOrUpdateDto roleOrganizationChartCreateOrUpdateDto)
        {
            var roleOrganizationChart = await Validation(roleOrganizationChartCreateOrUpdateDto.Id, roleOrganizationChartCreateOrUpdateDto);
            roleOrganizationChart.RoleId = roleOrganizationChartCreateOrUpdateDto.RoleId;
            roleOrganizationChart.OrganizationChartId = roleOrganizationChartCreateOrUpdateDto.OrganizationChartId;

            var entity = await _roleOrganizationChartRepository.UpdateAsync(roleOrganizationChart);
            return ObjectMapper.Map<RoleOrganizationChart, RoleOrganizationChartDto>(entity);
        }


        private async Task<RoleOrganizationChart> Validation(int? id, RoleOrganizationChartCreateOrUpdateDto roleOrganizationChartCreateOrUpdateDto)
        {
            var roleOrganizationChart = new RoleOrganizationChart();
            var roleOrganizationChartQuery = (await _roleOrganizationChartRepository.GetQueryableAsync()).Include(x => x.Role).Include(x => x.OrganizationChart);
            if (id != null)
            {
                roleOrganizationChart = roleOrganizationChartQuery.FirstOrDefault(x => x.Id == id);
                if (roleOrganizationChart is null)
                {
                    throw new UserFriendlyException(WorkFlowConstant.OrganizationChartNotFound, WorkFlowConstant.OrganizationChartNotFoundId);
                }

            }

            if (roleOrganizationChartCreateOrUpdateDto != null)
            {
                var organizationChartQuery = await _organizationChartRepository.GetQueryableAsync();
                var organizationChart = organizationChartQuery.FirstOrDefault(x => x.Id == roleOrganizationChartCreateOrUpdateDto.OrganizationChartId);
                if (organizationChart is null)
                {
                    throw new UserFriendlyException(WorkFlowConstant.OrganizationChartNotFound, WorkFlowConstant.OrganizationChartNotFoundId);
                }

                var roleQuery = await _roleRepository.GetQueryableAsync();

                var role = roleQuery.FirstOrDefault(x => x.Id == roleOrganizationChartCreateOrUpdateDto.RoleId);
                if (role is null)
                {
                    throw new UserFriendlyException(WorkFlowConstant.RoleNotFound, WorkFlowConstant.RoleNotFoundId);
                }


            }
            return roleOrganizationChart;
        }
    }
}