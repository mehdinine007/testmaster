using AutoMapper;
using Esale.Core.Utility.Tools;
using Newtonsoft.Json;
using WorkFlowManagement.Application.Contracts.WorkFlowManagement.Dtos;
using WorkFlowManagement.Domain.Shared.WorkFlowManagement.Enums;
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
            CreateMap<OrganizationPosition, OrganizationPositionCreateOrUpdateDto>()
          .ReverseMap();
            CreateMap<WorkFlowRole, WorkFlowRoleDto>()
                .ForMember(x => x.Security, c => c.MapFrom(m => !string.IsNullOrWhiteSpace(m.Security) ? JsonConvert.DeserializeObject<List<int>>(m.Security) : null))
         .ReverseMap()
          .ForMember(x => x.Security, c => c.MapFrom(m => JsonConvert.SerializeObject(m.Security)));
            CreateMap<WorkFlowRole, WorkFlowRoleCreateOrUpdateDto>()
       .ForMember(x => x.Security, c => c.MapFrom(m => !string.IsNullOrWhiteSpace(m.Security) ? JsonConvert.DeserializeObject<List<int>>(m.Security) : null))
       .ReverseMap()
         .ForMember(x => x.Security, c => c.MapFrom(m => JsonConvert.SerializeObject(m.Security)));
        
        }
    }
}
