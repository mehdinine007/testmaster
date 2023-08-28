using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp;
using Microsoft.EntityFrameworkCore;
using WorkFlowManagement.Domain.WorkFlowManagement;
using WorkFlowManagement.Application.Contracts.WorkFlowManagement.IServices;
using WorkFlowManagement.Application.Contracts.WorkFlowManagement.Dtos;
using Volo.Abp.ObjectMapping;
using NPOI.SS.Formula.Functions;
using Core.Utility.Tools;
using WorkFlowManagement.Application.Contracts.WorkFlowManagement.Constants;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;

namespace WorkFlowManagement.Application.WorkFlowManagement.Implementations
{
    public class OrganizationChartService : ApplicationService, IOrganizationChartService
    {

        private readonly IRepository<OrganizationChart, int> _organizationChartRepository;
        public OrganizationChartService(IRepository<OrganizationChart, int> organizationChartRepository)
        {
            _organizationChartRepository = organizationChartRepository;
        }

        public async Task<OrganizationChartDto> GetById(int id)
        {
            var organizationChart = await Validation(id, null);
            var organizationChartDto = ObjectMapper.Map<OrganizationChart, OrganizationChartDto>(organizationChart);
            return organizationChartDto;
        }

        public async Task<OrganizationChartDto> Add(OrganizationChartCreateOrUpdateDto organizationChartCreateOrUpdateDto)
        {
            await Validation(null, organizationChartCreateOrUpdateDto);
            var _parentCode = "";
            var igResult = await _organizationChartRepository.GetQueryableAsync();
            int codeLength = 5;
            if (organizationChartCreateOrUpdateDto.ParentId.HasValue && organizationChartCreateOrUpdateDto.ParentId.Value > 0)
                _parentCode = igResult.FirstOrDefault(x => x.Id == organizationChartCreateOrUpdateDto.ParentId).Code;
            var _maxCode = igResult.Where(x => x.ParentId == organizationChartCreateOrUpdateDto.ParentId).Max(x => x.Code);
            if (string.IsNullOrWhiteSpace(_maxCode))
                _maxCode = "1";
            else _maxCode = (Convert.ToInt32(_maxCode.Substring(_maxCode.Length - codeLength)) + 1).ToString();
            _maxCode = _parentCode + StringHelper.Repeat(_maxCode, codeLength);
            var code = _maxCode;
            var organizationChart = ObjectMapper.Map<OrganizationChartCreateOrUpdateDto, OrganizationChart>(organizationChartCreateOrUpdateDto);
            organizationChart.Code = code;
            var entity = await _organizationChartRepository.InsertAsync(organizationChart, autoSave: true);
            return ObjectMapper.Map<OrganizationChart, OrganizationChartDto>(entity);
        }

        public async Task<OrganizationChartDto> Update(OrganizationChartCreateOrUpdateDto organizationChartCreateOrUpdateDto)
        {
            var OrganizationChart = await Validation(organizationChartCreateOrUpdateDto.Id, organizationChartCreateOrUpdateDto);
            OrganizationChart.Status = organizationChartCreateOrUpdateDto.Status;
            OrganizationChart.Title = organizationChartCreateOrUpdateDto.Title;
            var entity = await _organizationChartRepository.UpdateAsync(OrganizationChart);
            return ObjectMapper.Map<OrganizationChart, OrganizationChartDto>(entity);
        }

        public async Task<List<OrganizationChartDto>> GetList()
        {
            var organizationsChart = (await _organizationChartRepository.GetQueryableAsync()).Include(x => x.Childrens).Where(x=>x.ParentId==null).ToList();
            var organizationsChartDto = ObjectMapper.Map<List<OrganizationChart>, List<OrganizationChartDto>>(organizationsChart);
            return organizationsChartDto;
        }

        public async Task<bool> Delete(int id)
        {
            var organizationChart = await Validation(id, null);
            await _organizationChartRepository.DeleteAsync(id);
            return true;
        }

        private async Task<OrganizationChart> Validation(int? id, OrganizationChartCreateOrUpdateDto organizationChartDto)
        {
            var organizationChart = new OrganizationChart();
            var organizationChartQuery = await _organizationChartRepository.GetQueryableAsync();
            if (id != null)
            {
                organizationChart = organizationChartQuery.FirstOrDefault(x => x.Id == id);
                if (organizationChart is null)
                {
                    throw new UserFriendlyException(WorkFlowConstant.OrganizationChartNotFound, WorkFlowConstant.OrganizationChartNotFoundId);
                }
            }
            if (organizationChartDto != null)
            {
                if (organizationChartDto.ParentId != null)
                {
                    var parent = organizationChartQuery.FirstOrDefault(x => x.Id == organizationChartDto.ParentId);
                    if (parent is null)
                    {
                        throw new UserFriendlyException(WorkFlowConstant.OrganizationChartParentNotFound, WorkFlowConstant.OrganizationChartParentNotFoundId);
                    }
                }

            }


            var duplicateTitle = organizationChartQuery.Where(x => x.Title == organizationChartDto.Title && x.ParentId == organizationChartDto.ParentId).FirstOrDefault();
            if (duplicateTitle is not null)
            {
                throw new UserFriendlyException(WorkFlowConstant.OrganizationChartDuplicate, WorkFlowConstant.OrganizationChartDuplicateId);
            }

            return organizationChart;
        }
    }
}
