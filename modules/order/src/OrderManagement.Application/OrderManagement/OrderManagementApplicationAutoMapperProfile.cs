﻿using AutoMapper;
using OrderManagement.Application.Contracts;
using OrderManagement.Application.Contracts.OrderManagement;
using OrderManagement.Application.Helpers;
using OrderManagement.Application.OrderManagement.Implementations;
using OrderManagement.Domain;
using OrderManagement.Domain.OrderManagement;
using Volo.Abp.AutoMapper;

namespace OrderManagement
{
    public class OrderManagementApplicationAutoMapperProfile : Profile
    {
        public OrderManagementApplicationAutoMapperProfile()
        {
            CreateMap<AdvocacyUsersFromBank, AdvocacyUsersFromBankDto>()
                .ReverseMap();
            CreateMap<UserRejectionAdvocacy, UserRejectionAdvocacyDto>()
                .ReverseMap();
            CreateMap<AdvocacyUsersFromBank, AdvocacyUserFromBankExportDto>()
                .ForMember(x => x.ShebaNumber, opt => opt.MapFrom(y => y.shabaNumber))
                .ForMember(x => x.NationalCode, opt => opt.MapFrom(y => y.nationalcode));
            CreateMap<AdvocacyUsersFromBank, AdvocacyUsersFromBankWithCompanyDto>()
                .ReverseMap();
            CreateMap<Company, CompanyDto>()
                .ForMember(x => x.LogoInPageUrl, opt => opt.MapFrom(y => y.GalleryLogoInPage.ImageUrl))
                .ForMember(x => x.LogoUrl, opt => opt.MapFrom(y => y.GalleryLogo.ImageUrl))
                .ForMember(x => x.BannerUrl, opt => opt.MapFrom(y => y.GalleryBanner.ImageUrl));

            CreateMap<CarTip, CarTipDto>()
                .ForMember(x => x.CarImageUrls, opt => opt.Ignore()).ReverseMap();
            CreateMap<ExternalApiLogResult, ExternalApiLogResultDto>()
                .ReverseMap();
            CreateMap<ExternalApiLogResultDto, ExternalApiLogResult>()
                .ReverseMap();

            CreateMap<ExternalApiResponsLog, ExternalApiResponsLogDto>()
                .ReverseMap();

            CreateMap<ExternalApiResponsLogDto, ExternalApiResponsLog>()
                .ReverseMap();

            CreateMap<CustomerOrder, CustomerOrderDto>()
                .ForMember(x => x.OrderStatus, opt => opt.MapFrom(y => y.OrderStatus.GetDisplayName()))
                .ReverseMap();

            CreateMap<ESaleType, ESaleTypeDto>()
                .ReverseMap()
                .IgnoreFullAuditedObjectProperties();

            CreateMap<City, PublicDto>()
                .ReverseMap();
            CreateMap<Province, PublicDto>()
                .ReverseMap();

            CreateMap<SaleDetail, SaleDetailDto>()
                .ReverseMap()
                .IgnoreFullAuditedObjectProperties();
            CreateMap<SaleDetail, SaleDetailOrderDto>();
            CreateMap<Agency, AgencyDto>().ReverseMap();
            //CreateMap<ApiResult, HandShakeResultDto>();
            CreateMap<SaleDetail, CreateSaleDetailDto>().ReverseMap();
            CreateMap<AgencySaleDetail, AgencySaleDetailDto>().ReverseMap();
            CreateMap<AgencySaleDetail, AgencySaleDetailListDto>()
               .ForMember(x => x.AgencyName, opt => opt.MapFrom(y => y.Agency.Name))
               .ReverseMap();
            CreateMap<Color, ColorDto>()
                .ReverseMap();
            CreateMap<SaleSchema, SaleSchemaDto>()
               .ReverseMap();
            CreateMap<PspHandShakeRequest, PaymentHandShakeDto>();
            CreateMap<PaymentHandShakeViewModel, IpgApiResult>();
            CreateMap<PaymentResultViewModel, PspInteractionResult>();
        }
    }
}
