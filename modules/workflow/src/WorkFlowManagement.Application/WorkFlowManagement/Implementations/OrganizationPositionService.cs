using Microsoft.EntityFrameworkCore;
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
    public class OrganizationPositionService : ApplicationService, IOrganizationPositionService
    {
        private readonly IRepository<OrganizationPosition, int> _organizationPositionRepository;
        private readonly IRepository<OrganizationChart, int> _organizationChartRepository;
        public OrganizationPositionService(IRepository<OrganizationPosition, int> organizationPositionRepository, IRepository<OrganizationChart, int> organizationChartRepository)
        {
            _organizationPositionRepository = organizationPositionRepository;
            _organizationChartRepository = organizationChartRepository;
        }


        public async Task<OrganizationPositionDto> Add(OrganizationPositionCreateOrUpdateDto organizationPositionCreateOrUpdateDto)
        {
            await Validation(null, organizationPositionCreateOrUpdateDto);
            var organizationPosition = ObjectMapper.Map<OrganizationPositionCreateOrUpdateDto, OrganizationPosition>(organizationPositionCreateOrUpdateDto);
            var entity = await _organizationPositionRepository.InsertAsync(organizationPosition, autoSave: true);
            return ObjectMapper.Map<OrganizationPosition, OrganizationPositionDto>(entity);
        }

        public async Task<bool> Delete(int id)
        {
            var organizationPosition = await Validation(id, null);
            await _organizationPositionRepository.DeleteAsync(id);
            return true;
        }

        public async Task<OrganizationPositionDto> GetById(int id)
        {
            var organizationPosition = await Validation(id, null);
            var organizationPositionDto = ObjectMapper.Map<OrganizationPosition, OrganizationPositionDto>(organizationPosition);
            return organizationPositionDto;
        }

        public async Task<List<OrganizationPositionDto>> GetList(int organizationChartId)
        {
            var organizationPosition = (await _organizationPositionRepository.GetQueryableAsync()).Where(x => x.OrganizationChartId == organizationChartId).Include(x => x.OrganizationChart).ToList();
            var organizationPositionDto = ObjectMapper.Map<List<OrganizationPosition>, List<OrganizationPositionDto>>(organizationPosition);
            return organizationPositionDto;
        }

        public async Task<OrganizationPositionDto> Update(OrganizationPositionCreateOrUpdateDto organizationPositionCreateOrUpdateDto)
        {
            var organizationPosition = await Validation(organizationPositionCreateOrUpdateDto.Id, organizationPositionCreateOrUpdateDto);
            organizationPosition.OrganizationChartId = organizationPositionCreateOrUpdateDto.OrganizationChartId;
            organizationPosition.PersonId = organizationPositionCreateOrUpdateDto.PersonId;
            organizationPosition.Status = organizationPositionCreateOrUpdateDto.Status;
            organizationPosition.StartDate = organizationPositionCreateOrUpdateDto.StartDate;
            organizationPosition.EndDate = organizationPositionCreateOrUpdateDto.EndDate;
            var entity = await _organizationPositionRepository.UpdateAsync(organizationPosition);
            return ObjectMapper.Map<OrganizationPosition, OrganizationPositionDto>(entity);
        }

        private async Task<OrganizationPosition> Validation(int? id, OrganizationPositionCreateOrUpdateDto organizationPositionCreateOrUpdateDto)
        {
            var currentTime = DateTime.Now;
            var organizationPosition = new OrganizationPosition();
            var organizationPositionQuery = (await _organizationPositionRepository.GetQueryableAsync()).Include(x => x.OrganizationChart);
            if (id != null)
            {
                organizationPosition = organizationPositionQuery.FirstOrDefault(x => x.Id == id);
                if (organizationPosition is null)
                {
                    throw new UserFriendlyException(WorkFlowConstant.OrganizationPositionNotFound, WorkFlowConstant.OrganizationPositionNotFoundId);
                }
            }
            if (organizationPositionCreateOrUpdateDto != null)
            {
                if (organizationPositionCreateOrUpdateDto.EndDate is not null)
                    if (organizationPositionCreateOrUpdateDto.EndDate < organizationPositionCreateOrUpdateDto.StartDate)
                        throw new UserFriendlyException(WorkFlowConstant.InvalidEndDate, WorkFlowConstant.InvalidEndDateId);




                var organizationChart = (await _organizationChartRepository.GetQueryableAsync()).FirstOrDefault(x => x.Id == organizationPositionCreateOrUpdateDto.OrganizationChartId);
                if (organizationChart is null)
                {
                    throw new UserFriendlyException(WorkFlowConstant.OrganizationChartNotFound, WorkFlowConstant.OrganizationChartNotFoundId);
                }
                else
                {
                    if (organizationChart.OrganizationType == OrganizationTypeEnum.SinglePosition)
                    {
                        var checkDuplicate = organizationPositionQuery.FirstOrDefault(x => x.OrganizationChartId == organizationPositionCreateOrUpdateDto.OrganizationChartId && x.Id != organizationPositionCreateOrUpdateDto.Id);
                        if (checkDuplicate is not null)
                            throw new UserFriendlyException(WorkFlowConstant.OrganizationPositionDuplicateNotFound, WorkFlowConstant.OrganizationPositionDuplicateNotFoundId);
                    }
                }

            }
            return organizationPosition;
        }

    }
}
