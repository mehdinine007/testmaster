using AutoMapper;
using CompanyManagement.Application.Contracts;
using CompanyManagement.Application.Contracts.CompanyManagement;
using CompanyManagement.Application.Contracts.CompanyManagement.Dto;
using CompanyManagement.Application.Contracts.CompanyManagement.Dto.BankDtos;
using CompanyManagement.Application.Contracts.CompanyManagement.Dto.OldCarDtos;
using CompanyManagement.Domain.CompanyManagement;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyManagement.Application.CompanyManagement
{
    public class CompanyManagementApplicationAutoMapperProfile : Profile
    {
        PersianCalendar pc = new PersianCalendar();

        public CompanyManagementApplicationAutoMapperProfile()
        {
            CreateMap<ClientsOrderDetailByCompanyDto, ClientsOrderDetailByCompany>()
            .ForMember(u => u.OrderId, options => options.MapFrom(input => input.OrderId))
            //.ForMember(u => u.Id, option => option.Ignore())
            .ForMember(x => x.FactorYear, opt => opt.MapFrom(x => x.FactorDate == null ? 0 : pc.GetYear(x.FactorDate.Value)))
            .ForMember(x => x.FactorMonth, opt => opt.MapFrom(x => x.FactorDate == null ? 0 : pc.GetMonth(x.FactorDate.Value)))
            .ForMember(x => x.FactorDay, opt => opt.MapFrom(x => x.FactorDate == null ? 0 : pc.GetDayOfMonth(x.FactorDate.Value)))
            .ForMember(x => x.DeliveryYear, opt => opt.MapFrom(x => x.DeliveryDate == null ? 0 : pc.GetYear(x.DeliveryDate.Value)))
            .ForMember(x => x.DeliveryMonth, opt => opt.MapFrom(x => x.DeliveryDate == null ? 0 : pc.GetMonth(x.DeliveryDate.Value)))
            .ForMember(x => x.DeliveryDay, opt => opt.MapFrom(x => x.DeliveryDate == null ? 0 : pc.GetDayOfMonth(x.DeliveryDate.Value)))
            .ForMember(x => x.IntroductionYear, opt => opt.MapFrom(x => x.IntroductionDate == null ? 0 : pc.GetYear(x.IntroductionDate.Value)))
            .ForMember(x => x.IntroductionMonth, opt => opt.MapFrom(x => x.IntroductionDate == null ? 0 : pc.GetMonth(x.IntroductionDate.Value)))
            .ForMember(x => x.IntroductionDay, opt => opt.MapFrom(x => x.IntroductionDate == null ? 0 : pc.GetDayOfMonth(x.IntroductionDate.Value)));
            CreateMap<PaypaidPriceDto, CompanyPaypaidPrices>()
            .ForMember(x => x.TranYear, opt => opt.MapFrom(x =>  pc.GetYear(x.TranDate)))
            .ForMember(x => x.TranMonth, opt => opt.MapFrom(x => pc.GetMonth(x.TranDate)))
            .ForMember(x => x.TranDay, opt => opt.MapFrom(x =>   pc.GetDayOfMonth(x.TranDate)))
            .ReverseMap();
            CreateMap<CompanyProductionDto, CompanyProduction>();
            CreateMap<TurnDateDto, CompanySaleCallDates>()
                .ForMember(x => x.ClientsOrderDetailByCompanyId, option => option.Ignore())
                .ReverseMap();
            //CreateMap<PaypaidpriceDto, CompanyPaypaidPrices>().ReverseMap();
            //CreateMap<TurnDateDto, CompanySaleCallDates>().ReverseMap();
            CreateMap<AdvocacyUsersFromBankDto, AdvocacyUsersFromBank>().ReverseMap();
            CreateMap<UserRejectionFromBankDto, UserRejectionFromBank>().ReverseMap();
            CreateMap<OldCarDto, OldCar>().ReverseMap();
            CreateMap<OldCarCreateDto, OldCar>().ReverseMap();
            CreateMap<ClientOrderDetailDto, ClientsOrderDetailByCompany>().ReverseMap();
        }

    }
}
