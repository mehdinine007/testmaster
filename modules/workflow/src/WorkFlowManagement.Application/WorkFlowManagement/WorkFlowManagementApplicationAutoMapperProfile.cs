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
            CreateMap<Role, RoleDto>()
                .ForMember(x => x.Security, c => c.MapFrom(m => !string.IsNullOrWhiteSpace(m.Security) ? JsonConvert.DeserializeObject<List<int>>(m.Security) : null))
         .ReverseMap()
          .ForMember(x => x.Security, c => c.MapFrom(m => JsonConvert.SerializeObject(m.Security)));
            CreateMap<Role, RoleCreateOrUpdateDto>()
       .ForMember(x => x.Security, c => c.MapFrom(m => !string.IsNullOrWhiteSpace(m.Security) ? JsonConvert.DeserializeObject<List<int>>(m.Security) : null))
       .ReverseMap()
         .ForMember(x => x.Security, c => c.MapFrom(m => JsonConvert.SerializeObject(m.Security)));

            CreateMap<RoleOrganizationChart, RoleOrganizationChartCreateOrUpdateDto>()
            .ReverseMap();
            CreateMap<RoleOrganizationChart, RoleOrganizationChartDto>()
           .ReverseMap();

            CreateMap<Scheme, SchemeDto>()
          .ReverseMap();
            CreateMap<Scheme, SchemeCreateOrUpdateDto>()
      .ReverseMap();

            CreateMap<Activity, ActivityDto>()
     .ReverseMap();

            CreateMap<Activity, ActivityCreateOrUpdateDto>()
               .ReverseMap();




        }
    }
}
