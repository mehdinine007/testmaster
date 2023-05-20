using AutoMapper;
using PaymentManagement.Application.Contracts;
using PaymentManagement.Domain.Models;

namespace PaymentManagement.Application
{
    public class PaymentManagementApplicationAutoMapperProfile : Profile
    {
        public PaymentManagementApplicationAutoMapperProfile()
        {
            CreateMap<PspAccount, PspAccountDto>()
                .ForMember(dest => dest.Psp, x => x.MapFrom(src => src.Psp.Title))
                .ForMember(dest => dest.AccountName, x => x.MapFrom(src => src.Account.AccountName))
                .ReverseMap()
                .ForPath(dest => dest.Psp.Title, x => x.Ignore())
                .ForPath(dest => dest.Account.AccountName, x => x.Ignore());
        }
    }
}
