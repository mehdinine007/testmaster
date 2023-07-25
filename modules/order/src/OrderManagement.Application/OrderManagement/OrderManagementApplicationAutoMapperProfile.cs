﻿using AutoMapper;
using Esale.Core.Utility.Tools;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using OrderManagement.Application.Contracts;
using OrderManagement.Application.Contracts.OrderManagement;
using OrderManagement.Application.Helpers;
using OrderManagement.Application.OrderManagement.Implementations;
using OrderManagement.Domain;
using OrderManagement.Domain.OrderManagement;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Volo.Abp.AutoMapper;
using Volo.Abp.ObjectMapping;

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

            CreateMap<Attachment, AttachmentDto>()
                .ReverseMap();
            CreateMap<Attachment, AttachmentViewModel>()
                .ForMember(x => x.FileName, c => c.MapFrom(m => m.Id + "." + m.FileExtension))
                .ForMember(x => x.Type, c => c.MapFrom(m => m.EntityType))
                .ForMember(x => x.TypeTitle, c => c.MapFrom(m => m.EntityType != 0 ? EnumHelper.GetDescription(m.EntityType) : ""))
                .ForMember(x => x.Content, c => c.MapFrom(m => !string.IsNullOrWhiteSpace(m.Content) ? JsonConvert.DeserializeObject<List<string>>(m.Content) : null))
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
                .ForMember(x => x.Attachments, c => c.MapFrom(m => m.Attachments))
               .ReverseMap();
            CreateMap<PspHandShakeRequest, PaymentHandShakeDto>();
            CreateMap<PaymentHandShakeViewModel, IpgApiResult>();
            CreateMap<PaymentResultViewModel, PspInteractionResult>();
            CreateMap<ProductAndCategory, ProductAndCategoryDto>()
                .ForMember(x => x.HasChild, opt => opt.MapFrom(x => x.Childrens.Any()))
                .ForMember(x => x.Attachments, opt => opt.Ignore())
                .ReverseMap()
                .IgnoreFullAuditedObjectProperties();

        }
    }
}
