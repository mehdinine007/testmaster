using AutoMapper;
using IFG.Core.Utility.Tools;
using Newtonsoft.Json;
using WorkFlowManagement.Application.Contracts.WorkFlowManagement.Dtos;

namespace AdminPanelManagement.Application
{
    public class AdminPanelManagementApplicationAutoMapperProfile : Profile
    {
        public AdminPanelManagementApplicationAutoMapperProfile()
        {
           
     //       CreateMap<Activity, ActivityDto>()
     //.ForMember(x => x.TypeTitle, c => c.MapFrom(m => m.Type != 0 ? EnumHelper.GetDescription(m.Type) : ""))
     //.ForMember(x => x.EntityTitle, c => c.MapFrom(m => m.Entity != 0 ? EnumHelper.GetDescription(m.Entity) : ""))
     //.ForMember(x => x.FlowTypeTitle, c => c.MapFrom(m => m.FlowType != 0 ? EnumHelper.GetDescription(m.FlowType) : ""))
     //.ReverseMap();

            

        }
    }
}
