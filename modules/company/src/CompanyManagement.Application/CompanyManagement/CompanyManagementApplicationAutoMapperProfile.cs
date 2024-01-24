using AutoMapper;
using CompanyManagement.Application.Contracts;
using CompanyManagement.Application.Contracts.CompanyManagement;
using CompanyManagement.Application.Contracts.CompanyManagement.Dto.BankDtos;
using CompanyManagement.Application.Contracts.CompanyManagement.Dto.OldCarDtos;
using CompanyManagement.Domain.CompanyManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyManagement.Application.CompanyManagement
{
    public class CompanyManagementApplicationAutoMapperProfile : Profile
    {
        public CompanyManagementApplicationAutoMapperProfile()
        {
            CreateMap<ClientsOrderDetailByCompanyDto, ClientsOrderDetailByCompany>()
            .ForMember(u => u.OrderId, options => options.MapFrom(input => input.Id))
            .ForMember(u => u.Id, option => option.Ignore());
            CreateMap<PaypaidpriceDto, CompanyPaypaidPrices>();
            CreateMap<CompanyProductionDto, CompanyProduction>();
            CreateMap<TurnDateDto, CompanySaleCallDates>()
                .ForMember(x => x.ClientsOrderDetailByCompanyId, option => option.Ignore());
            //CreateMap<PaypaidpriceDto, CompanyPaypaidPrices>().ReverseMap();
            //CreateMap<TurnDateDto, CompanySaleCallDates>().ReverseMap();
            CreateMap<AdvocacyUsersFromBankDto, AdvocacyUsersFromBank>().ReverseMap();
            CreateMap<UserRejectionFromBankDto, UserRejectionFromBank>().ReverseMap();
            CreateMap<OldCarDto, OldCar>().ReverseMap();
            CreateMap<OldCarCreateDto, OldCar>().ReverseMap();
        }

    }
}
