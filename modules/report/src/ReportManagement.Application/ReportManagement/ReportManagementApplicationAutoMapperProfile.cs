using AutoMapper;
using Newtonsoft.Json;
using ReportManagement.Application.Contracts.ReportManagement.Dtos;
using ReportManagement.Domain.ReportManagement;

namespace ReportManagement.Application
{
    public class ReportManagementApplicationAutoMapperProfile : Profile
    {
        public ReportManagementApplicationAutoMapperProfile()
        {
            CreateMap<Widget, WidgetDto>()
                  //.ForMember(x => x.Fields, c => c.MapFrom(m => !string.IsNullOrWhiteSpace(m.Fields) ? JsonConvert.DeserializeObject<List<FieldsDto>>(m.Fields) : null))
                   .ForMember(x => x.Condition, c => c.MapFrom(m => !string.IsNullOrWhiteSpace(m.Condition) ? JsonConvert.DeserializeObject<List<ConditionDto>>(m.Condition) : null))
          .ReverseMap()
           //.ForMember(x => x.Fields, c => c.MapFrom(m => JsonConvert.SerializeObject(m.Fields)))
           .ForMember(x => x.Condition, c => c.MapFrom(m => JsonConvert.SerializeObject(m.Condition)));
            CreateMap<Widget, WidgetCreateOrUpdateDto>()
                .ForMember(x => x.Fields, c => c.MapFrom(m => !string.IsNullOrWhiteSpace(m.Fields) ? JsonConvert.DeserializeObject<List<FieldsDto>>(m.Fields) : null))
                   .ForMember(x => x.Condition, c => c.MapFrom(m => !string.IsNullOrWhiteSpace(m.Condition) ? JsonConvert.DeserializeObject<List<ConditionDto>>(m.Condition) : null))
         .ReverseMap()
          .ForMember(x => x.Fields, c => c.MapFrom(m => JsonConvert.SerializeObject(m.Fields)))
           .ForMember(x => x.Condition, c => c.MapFrom(m => JsonConvert.SerializeObject(m.Condition)));
            CreateMap<Dashboard, DashboardDto>()
         .ReverseMap();
            CreateMap<Dashboard, DashboardCreateOrUpdateDto>()
       .ReverseMap();
            CreateMap<DashboardWidget, DashboardWidgetDto>()
     .ReverseMap();

            CreateMap<DashboardWidget, DashboardWidgetCreateOrUpdateDto>()
     .ReverseMap();



        }
    }
}
