using AutoMapper;
using WorkFlowManagement.Application.Contracts.WorkFlowManagement.Dtos;
using WorkFlowManagement.Domain.WorkFlowManagement;

namespace WorkFlowManagement.Application
{
    public class WorkFlowManagementApplicationAutoMapperProfile : Profile
    {
        public WorkFlowManagementApplicationAutoMapperProfile()
        {
            CreateMap<OrganizationChart, OrganizationChartDto>()
               .ReverseMap();
            CreateMap<OrganizationChart, OrganizationChartCreateOrUpdateDto>()
             .ReverseMap();
        }
    }
}
