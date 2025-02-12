﻿using AutoMapper;
using AutoMapper;
using IFG.Core.Utility.Tools;
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
                  .ForMember(x => x.FullName, c => c.MapFrom(m => m.Person.Title))
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
     .ForMember(x => x.TypeTitle, c => c.MapFrom(m => m.Type != 0 ? EnumHelper.GetDescription(m.Type) : ""))
     .ForMember(x => x.EntityTitle, c => c.MapFrom(m => m.Entity != 0 ? EnumHelper.GetDescription(m.Entity) : ""))
     .ForMember(x => x.FlowTypeTitle, c => c.MapFrom(m => m.FlowType != 0 ? EnumHelper.GetDescription(m.FlowType) : ""))
     .ReverseMap();

            CreateMap<Activity, ActivityCreateOrUpdateDto>()
               .ReverseMap();
            CreateMap<Transition, TransitionDto>().ReverseMap();
            CreateMap<Transition, TransitionCreateOrUpdateDto>()
               .ReverseMap();

            CreateMap<ActivityRole, ActivityRoleDto>()
              .ReverseMap();

            CreateMap<ActivityRole, ActivityRoleCreateOrUpdateDto>()
             .ReverseMap();
            CreateMap<Process, ProcessDto>()
                .ForMember(x => x.StateTitle, c => c.MapFrom(m => m.State != 0 ? EnumHelper.GetDescription(m.State) : ""))
                .ForMember(x => x.StatusTitle, c => c.MapFrom(m => m.Status != 0 ? EnumHelper.GetDescription(m.Status) : ""))
                 .ForMember(x => x.FullName, c => c.MapFrom(m => m.Person.Title))

            .ReverseMap();
            CreateMap<Process, ProcessCreateOrUpdateDto>()
            .ReverseMap();


            CreateMap<Inbox, InboxDto>()
               .ForMember(x => x.StateTitle, c => c.MapFrom(m => m.State != 0 ? EnumHelper.GetDescription(m.State) : ""))
               .ForMember(x => x.InboxStatusTilte, c => c.MapFrom(m => m.Status != 0 ? EnumHelper.GetDescription(m.Status) : ""))
                .ForMember(x => x.FullName, c => c.MapFrom(m => m.Person.Title))
               .ReverseMap();

            CreateMap<Inbox, InboxCreateOrUpdateDto>()
            .ReverseMap();

            CreateMap<Person, PersonCreateOrUpdateDto>()
          .ReverseMap();
            CreateMap<Person, PersonDto>()
       .ReverseMap();

        }
    }
}
