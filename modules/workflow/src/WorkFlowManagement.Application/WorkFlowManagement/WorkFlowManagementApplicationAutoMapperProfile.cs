using AutoMapper;
using Esale.Core.Utility.Tools;
using WorkFlowManagement.Application.Contracts.WorkFlowManagement.Dtos;
using WorkFlowManagement.Domain.WorkFlowManagement;

namespace WorkFlowManagement.Application
{
    public class WorkFlowManagementApplicationAutoMapperProfile : Profile
    {
        public WorkFlowManagementApplicationAutoMapperProfile()
        {
            CreateMap<OrganizationChart, OrganizationChartDto>()
               .ForMember(x => x.TypeTitle, c => c.MapFrom(m => m.OrganizationType != 0 ? EnumHelper.GetDescription(m.OrganizationType) : ""));
            CreateMap<OrganizationChart, OrganizationChartCreateOrUpdateDto>()
             .ReverseMap();

            CreateMap<OrganizationPosition, OrganizationPositionDto>()
            .ReverseMap();
            CreateMap<OrganizationPosition,OrganizationPositionCreateOrUpdateDto>()
          .ReverseMap();
        }
    }
}
