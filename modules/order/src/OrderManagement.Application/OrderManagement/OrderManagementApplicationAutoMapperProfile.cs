﻿using AutoMapper;
using IFG.Core.Utility.Tools;
using Newtonsoft.Json;
using OrderManagement.Application.Contracts;
using OrderManagement.Application.Contracts.Dtos;
using OrderManagement.Application.Contracts.OrderManagement;
using OrderManagement.Application.Contracts.OrderManagement.Models;
using OrderManagement.Domain;
using OrderManagement.Domain.OrderManagement;
using OrderManagement.Domain.Shared.OrderManagement.Enums;
using OrderManagement.Domain.OrderManagement.MongoWrite;
using System;
using System.Collections.Generic;
using System.Linq;
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
                .ForMember(x => x.OrderStatusDescription, opt => opt.MapFrom(y => y.OrderStatus.GetDisplayName()))
                .ForMember(x => x.OrderStatus, opt => opt.MapFrom(y => (int)y.OrderStatus))
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
            CreateMap<Attachment, AttachFileDto>()
                .ForMember(x => x.Content, c => c.MapFrom(m => !string.IsNullOrWhiteSpace(m.Content) ? JsonConvert.DeserializeObject<List<string>>(m.Content) : null))
                .ForMember(x => x.AttachmentId, c => c.MapFrom(m => m.Id))
                .ReverseMap()
                .ForMember(x => x.Id, c => c.MapFrom(m => m.AttachmentId));
            CreateMap<UploadFileDto, AttachFileDto>()
                .ForMember(x => x.EntityType, c => c.MapFrom(m => m.Type))
                .ReverseMap();
            CreateMap<Attachment, AttachmentUpdateDto>()
                .ReverseMap()
                .ForMember(x => x.EntityType, c => c.MapFrom(m => m.Type))
                .ForMember(x => x.Content, c => c.MapFrom(m => m.Content != null ? JsonConvert.SerializeObject(m.Content) : null));
            CreateMap<AttachmentDto, AttachmentViewModel>()
                .ForMember(x => x.FileName, c => c.MapFrom(m => m.Id + "." + m.FileExtension))
                .ForMember(x => x.Type, c => c.MapFrom(m => m.EntityType))
                .ForMember(x => x.TypeTitle, c => c.MapFrom(m => m.EntityType != 0 ? EnumHelper.GetDescription(m.EntityType) : ""))
                .ForMember(x => x.LocationTitle, c => c.MapFrom(m => m.Location != 0 ? EnumHelper.GetDescription(m.Location) : ""))
                .ForMember(x => x.Content, c => c.MapFrom(m => !string.IsNullOrWhiteSpace(m.Content) ? JsonConvert.DeserializeObject<List<string>>(m.Content) : null))
                     .ForMember(x => x.DeviceTitle, c => c.MapFrom(m => EnumHelper.GetDescription(m.Device)))
                .ReverseMap();

            CreateMap<SiteStructure, SiteStructureDto>()
                .ForMember(x => x.TypeTitle, c => c.MapFrom(m => m.Type != 0 ? EnumHelper.GetDescription(m.Type) : ""))
                .ReverseMap();

            CreateMap<SiteStructure, SiteStructureAddOrUpdateDto>()
                .ReverseMap();

            CreateMap<ChartStructure, ChartStructureDto>()
                .ForMember(x => x.Categories, c => c.MapFrom(m => !string.IsNullOrEmpty(m.Categories) ? JsonConvert.DeserializeObject<List<CategoryData>>(m.Categories) : null))
                .ForMember(x => x.Series, c => c.MapFrom(m => !string.IsNullOrEmpty(m.Series) ? JsonConvert.DeserializeObject<List<ChartSeriesData>>(m.Series) : null))
                .ForMember(x => x.TypeTitle, c => c.MapFrom(m => m.Type != 0 ? EnumHelper.GetDescription(m.Type) : ""))
                .ReverseMap();
            CreateMap<ChartStructure, ChartStructureCreateOrUpdateDto>()
                .ReverseMap()
                .ForMember(x => x.Categories, c => c.MapFrom(m => JsonConvert.SerializeObject(m.Categories)))
                .ForMember(x => x.Series, c => c.MapFrom(m => JsonConvert.SerializeObject(m.Series)));


            CreateMap<Property, PropertyDto>()
                .ForMember(x => x.TypeTitle, c => c.MapFrom(m => m.Type != 0 ? EnumHelper.GetDescription(m.Type) : ""))
                .ReverseMap();
            CreateMap<PropertyCategory, PropertyCategoryDto>()
                .ReverseMap();
            CreateMap<PropertyCategoryWrite, PropertyCategoryDto>()
                .ReverseMap();

            CreateMap<PropertyCategory, ProductPropertyCategoryDto>()
                .ReverseMap();

            CreateMap<ProductProperty, ProductPropertyDto>()
                .ReverseMap();
            CreateMap<ProductPropertyWrite, ProductPropertyDto>()
                .ReverseMap();

            CreateMap<DropDownItem, DropDownItemDto>()
                .ReverseMap();

            CreateMap<SaleDetail, SaleDetailDto>()
                 .ForMember(x => x.EsaleName, c => c.MapFrom(m => m.ESaleTypeId == 1 ? EnumHelper.GetDisplayName(ESaleTypeEnums.NormalSale) : m.ESaleTypeId == 2 ? EnumHelper.GetDisplayName(ESaleTypeEnums.YouthSale) : m.ESaleTypeId == 3 ? EnumHelper.GetDisplayName(ESaleTypeEnums.WornOutSale) : ""))
                 .ForMember(x => x.SaleTitle, c => c.MapFrom(m => m.SaleSchema.Title))
                .ForMember(o => o.SalePlanDescription, opt => opt.MapFrom(y => System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(y.SalePlanDescription))))
                .ReverseMap()
                .IgnoreFullAuditedObjectProperties();
            CreateMap<SaleDetail, SaleDetailOrderDto>();
            CreateMap<Agency, AgencyDto>()
                 .ForMember(x => x.ProvinceTitle, c => c.MapFrom(o => o.Province.Name))
                 .ForMember(x => x.CityTitle, c => c.MapFrom(o => o.City.Name))
                 .ForMember(x => x.AgencyTypeTitle, c => c.MapFrom(m => m.AgencyType != 0 ? EnumHelper.GetDescription(m.AgencyType) : ""))
                .ReverseMap();
            CreateMap<Agency, AgencyCreateDto>().ReverseMap();
            CreateMap<Agency, AgencyCreateOrUpdateDto>().ReverseMap();
            CreateMap<AgencyCreateDto, AgencyCreateOrUpdateDto>().ReverseMap();

            //CreateMap<ApiResult, HandShakeResultDto>();
            CreateMap<SaleDetail, CreateSaleDetailDto>()
                .ReverseMap()
                .ForMember(x => x.SalePlanDescription, opt => opt.MapFrom(y => System.Text.Encoding.UTF8.GetString(System.Convert.FromBase64String(y.SalePlanDescription))));

            CreateMap<SaleDetail, SaleDetailForDropDownDto>()
                .ReverseMap()
                .IgnoreFullAuditedObjectProperties();


            CreateMap<AgencySaleDetail, AgencySaleDetailDto>().ReverseMap();
            CreateMap<AgencySaleDetail, AgencySaleDetailListDto>()
               .ForMember(x => x.AgencyName, opt => opt.MapFrom(y => y.Agency.Name))
               .ReverseMap();
            CreateMap<Color, ColorDto>()
                .ReverseMap();
            CreateMap<SaleSchema, SaleSchemaDto>()
                .ForMember(o => o.Description, opt => opt.MapFrom(y => System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(y.Description))))
               .ReverseMap();
            CreateMap<PspHandShakeRequest, PaymentHandShakeDto>();
            CreateMap<PaymentHandShakeViewModel, IpgApiResult>();
            CreateMap<PaymentResultViewModel, PspInteractionResult>();
            CreateMap<ProductAndCategory, ProductAndCategoryDto>()
                .ForMember(x => x.HasChild, opt => opt.MapFrom(x => x.Childrens.Any()))
                .ForMember(x => x.Attachments, opt => opt.Ignore())
                .ReverseMap()
                .IgnoreFullAuditedObjectProperties();
            CreateMap<ProductAndCategory, ProductAndCategoryWithChildDto>();
            //.ForMember(x => x.ProductAndCategoryWithChilds)


            CreateMap<ProductAndCategory, ProductAndCategoryViewModel>().ReverseMap();
            CreateMap<ProductLevel, ProductLevelDto>().ReverseMap();
            CreateMap<ProductLevel, ProductAndCategoryAddDto>().ReverseMap();
            CreateMap<OrderStatusInquiry, OrderStatusInquiryDto>()
                .ReverseMap()
                .IgnoreFullAuditedObjectProperties();
            CreateMap<OrderStatusInquiry, OrderStatusInquiryResultDto>()
                .ForMember(x => x.OrderDeliveryStatusDescription, opt => opt.Ignore())
                .ForMember(x => x.AvailableDeliveryStatusList, opt => opt.Ignore())
                .ForMember(x => x.RejectionDate, opt => opt.Ignore());
            CreateMap<CreateSaleSchemaDto, SaleSchema>()
               .ForMember(x => x.Description, opt => opt.MapFrom(y => System.Text.Encoding.UTF8.GetString(System.Convert.FromBase64String(y.Description))))
                .ReverseMap();
            CreateMap<ProductAndCategoryCreateDto, ProductAndCategory>();
            CreateMap<ProductAndCategory, ProductAndSaleDetailListDto>().ReverseMap();
            CreateMap<SaleDetail, SaleDetailListDto>()
                .ForMember(x => x.EsaleTypeName, opt => opt.MapFrom(y => y.ESaleType.SaleTypeName))
                .ReverseMap();
            CreateMap<CarClass, CarClassDto>().ReverseMap();
            CreateMap<CarClass, CarClassCreateDto>().ReverseMap();
            CreateMap<Questionnaire, QuestionnaireTreeDto>();
            CreateMap<Questionnaire, QuestionnaireDto>()
                .ForMember(x => x.Attachments, opt => opt.Ignore())
                .ReverseMap()
                .IgnoreFullAuditedObjectProperties();
            CreateMap<Question, FullQuestionDto>();
            CreateMap<QuestionAnswer, QuestionAnswerDto>();
            CreateMap<SubmittedAnswer, SubmittedAnswerDto>();

            CreateMap<AttachFileDto, AttachmentUpdateDto>()
              .ReverseMap();
            CreateMap<BankDto, Bank>()
              .ReverseMap();

            CreateMap<Announcement, AnnouncementDto>()
                .ForMember(o => o.Content, opt => opt.MapFrom(y => System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(y.Content))))
                .ForMember(o => o.Description, opt => opt.MapFrom(y => System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(y.Description))))
                .ForMember(o => o.Notice, opt => opt.MapFrom(y => System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(y.Notice))))
                .ForMember(x => x.StatusTitle, c => c.MapFrom(m => DateTime.Now < m.FromDate ? EnumHelper.GetDescription(AnnouncementStatusEnum.Awaiting) : DateTime.Now >= m.FromDate && DateTime.Now <= m.ToDate ? EnumHelper.GetDescription(AnnouncementStatusEnum.Publishing) : DateTime.Now > m.ToDate ? EnumHelper.GetDescription(AnnouncementStatusEnum.Expired) : ""))
                .ForMember(x => x.Status, c => c.MapFrom(m => DateTime.Now < m.FromDate ? AnnouncementStatusEnum.Awaiting : DateTime.Now >= m.FromDate && DateTime.Now <= m.ToDate ? AnnouncementStatusEnum.Publishing : DateTime.Now > m.ToDate ? AnnouncementStatusEnum.Expired : 0))
                .ReverseMap()
                .IgnoreFullAuditedObjectProperties();

            CreateMap<CreateAnnouncementDto, Announcement>()
                .ForMember(x => x.Content, opt => opt.MapFrom(y => System.Text.Encoding.UTF8.GetString(System.Convert.FromBase64String(y.Content))))
                .ForMember(x => x.Description, opt => opt.MapFrom(y => System.Text.Encoding.UTF8.GetString(System.Convert.FromBase64String(y.Description))))
                .ForMember(x => x.Notice, opt => opt.MapFrom(y => System.Text.Encoding.UTF8.GetString(System.Convert.FromBase64String(y.Notice))));


            CreateMap<Bank, BankCreateOrUpdateDto>().ReverseMap();
            CreateMap<Organization, OrganizationAddOrUpdateDto>().ReverseMap();
            CreateMap<Organization, OrganizationDto>().ReverseMap();

            CreateMap<UnAuthorizedUser, UnregisteredUserInformation>()
                .ForMember(x => x.SmsCode, opt => opt.Ignore())
                .ReverseMap()
                .IgnoreFullAuditedObjectProperties();

            CreateMap<Priority, PriorityDto>().ReverseMap();
            CreateMap<QuestionRelationship, QuestionRelationshipDto>().ReverseMap();

            CreateMap<QuestionGroup, QuestionGroupDto>().ReverseMap();
            CreateMap<Advertisement, AdvertisementDto>().ReverseMap();
            CreateMap<Advertisement, AdvertisementCreateOrUpdateDto>().ReverseMap();
            CreateMap<AdvertisementDetail, AdvertisementDetailDto>()
                .ForMember(o => o.Description, opt => opt.MapFrom(y => System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(y.Description))))
                .ReverseMap();

            CreateMap<AdvertisementDetail, AdvertisementDetailCreateOrUpdateDto>().ReverseMap()
                  .ForMember(x => x.Description, opt => opt.MapFrom(y => System.Text.Encoding.UTF8.GetString(System.Convert.FromBase64String(y.Description))))
                  .ReverseMap();

            CreateMap<SaleDetailAllocation, SaleDetailAllocationDto>()
                .ReverseMap()
                .IgnoreFullAuditedObjectProperties();
            CreateMap<SaleDetailAllocation, SaleDetailAllocationCreateOrUpdateDto>()
               .ReverseMap();
            CreateMap<SeasonAllocation, SeasonAllocationDto>()
               .ForMember(x => x.SeasonTitle, c => c.MapFrom(m => m.SeasonId != 0 ? EnumHelper.GetDescription(m.SeasonId) : ""))
                .ReverseMap();

            CreateMap<SeasonAllocation, SeasonAllocationUpdateDto>().ReverseMap();
            CreateMap<SeasonAllocation, SeasonAllocationCreateDto>().ReverseMap();
        }
    }
}
