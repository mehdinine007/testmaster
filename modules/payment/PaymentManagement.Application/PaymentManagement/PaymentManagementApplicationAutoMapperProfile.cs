using AutoMapper;
using PaymentManagement.Application.Contracts.Dtos;
using PaymentManagement.Domain.Models;
using Volo.Abp.AutoMapper;

namespace PaymentManagement.Application
{
    public class PaymentManagementApplicationAutoMapperProfile : Profile
    {
        public PaymentManagementApplicationAutoMapperProfile()
        {
            CreateMap<Payment, PaymentDto>().ReverseMap().IgnoreFullAuditedObjectProperties();
        }
    }
}
