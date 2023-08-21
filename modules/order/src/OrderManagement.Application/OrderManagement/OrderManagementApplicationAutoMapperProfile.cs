using AutoMapper;
using Esale.Core.Utility.Tools;
using Newtonsoft.Json;
using OrderManagement.Application.Contracts;
using OrderManagement.Application.Contracts.OrderManagement;
using OrderManagement.Application.Contracts.OrderManagement.Models;
using OrderManagement.Domain;
using OrderManagement.Domain.OrderManagement;
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
            CreateMap<Attachment, AttachFileDto>()
                .ReverseMap();
            CreateMap<Attachment, AttachmentUpdateDto>()
                .ReverseMap();
            CreateMap<AttachmentDto, AttachmentViewModel>()
                .ForMember(x => x.FileName, c => c.MapFrom(m => m.Id + "." + m.FileExtension))
                .ForMember(x => x.Type, c => c.MapFrom(m => m.EntityType))
                .ForMember(x => x.TypeTitle, c => c.MapFrom(m => m.EntityType != 0 ? EnumHelper.GetDescription(m.EntityType) : ""))
                .ForMember(x => x.LocationTitle, c => c.MapFrom(m => m.Location != 0 ? EnumHelper.GetDescription(m.Location) : ""))
                .ForMember(x => x.Content, c => c.MapFrom(m => !string.IsNullOrWhiteSpace(m.Content) ? JsonConvert.DeserializeObject<List<string>>(m.Content) : null))
                .ReverseMap();

            CreateMap<SiteStructure, SiteStructureDto>()
                .ForMember(x => x.TypeTitle, c => c.MapFrom(m => m.Type != 0 ? EnumHelper.GetDescription(m.Type) : ""))
                .ReverseMap();

            CreateMap<SiteStructure, SiteStructureAddOrUpdateDto>()
                .ReverseMap();

            CreateMap<ChartStructure, ChartStructureDto>()
                .ForMember(x => x.Categories, c => c.MapFrom(m => !string.IsNullOrEmpty(m.Categories) ? JsonConvert.DeserializeObject<List<string>>(m.Categories) : null))
                .ForMember(x => x.Series, c => c.MapFrom(m => !string.IsNullOrEmpty(m.Series) ? JsonConvert.DeserializeObject<List<ChartSeriesData>>(m.Series) : null))
                .ReverseMap();

            CreateMap<Property, PropertyDto>()
                .ForMember(x => x.TypeTitle, c => c.MapFrom(m => m.Type != 0 ? EnumHelper.GetDescription(m.Type) : ""))
                .ReverseMap();
            CreateMap<PropertyCategory, PropertyCategoryDto>()
                .ReverseMap();
            CreateMap<ProductProperty, ProductPropertyDto>()
                .ReverseMap();
            CreateMap<DropDownItem, DropDownItemDto>()
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
            CreateMap<ProductAndCategory, ProductAndCategoryDto>()
                .ForMember(x => x.HasChild, opt => opt.MapFrom(x => x.Childrens.Any()))
                .ForMember(x => x.Attachments, opt => opt.Ignore())
                .ReverseMap()
                .IgnoreFullAuditedObjectProperties();
            CreateMap<ProductAndCategory, ProductAndCategoryWithChildDto>();
            //.ForMember(x => x.ProductAndCategoryWithChilds)


            CreateMap<ProductAndCategory, ProductAndCategoryViewModel>().ReverseMap();
            CreateMap<ProductLevel, ProductLevelDto>().ReverseMap();
            CreateMap<OrderStatusInquiry, OrderStatusInquiryDto>()
                .ReverseMap()
                .IgnoreFullAuditedObjectProperties();
            CreateMap<OrderStatusInquiry, OrderStatusInquiryResultDto>()
                .ForMember(x => x.OrderDeliveryStatusDescription, opt => opt.Ignore())
                .ForMember(x => x.AvailableDeliveryStatusList, opt => opt.Ignore())
                .ForMember(x => x.RejectionDate, opt => opt.Ignore());
            CreateMap<CreateSaleSchemaDto, SaleSchema>().ReverseMap();
            CreateMap<ProductAndCategoryCreateDto, ProductAndCategory>();
            CreateMap<ProductAndCategory, ProductAndSaleDetailListDto>().ReverseMap();
            CreateMap<SaleDetail, SaleDetailListDto>()
                .ForMember(x => x.EsaleTypeName, opt => opt.MapFrom(y => y.ESaleType.SaleTypeName))
                .ReverseMap();
            CreateMap<CarClass, CarClassDto>().ReverseMap();
            CreateMap<CarClass, CarClassCreateDto>().ReverseMap();
            CreateMap<Questionnaire, QuestionnaireTreeDto>();
            CreateMap<Question, FullQuestionDto>();
            CreateMap<QuestionAnswer, QuestionAnswerDto>();
            CreateMap<SubmittedAnswer, SubmittedAnswerDto>();
            CreateMap<AttachFileDto, AttachmentUpdateDto>()
              .ReverseMap();
            CreateMap<BankDto, Bank>()
              .ReverseMap();
            CreateMap<Announcement, AnnouncementDto>()
                .ReverseMap()
                .IgnoreFullAuditedObjectProperties();
            CreateMap<CreateAnnouncementDto, Announcement>();
              
       
            CreateMap<Bank, BankCreateOrUpdateDto>().ReverseMap();

            

        }
    }
}
