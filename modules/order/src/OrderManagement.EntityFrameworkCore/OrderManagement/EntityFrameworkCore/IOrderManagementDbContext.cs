using Volo.Abp.AuditLogging;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using OrderManagement.Domain;
using OrderManagement.Domain.Bases;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using OrderManagement.Domain.OrderManagement;

namespace OrderManagement.EfCore
{
    [ConnectionStringName("OrderManagement")]
    public interface IOrderManagementDbContext : IEfCoreDbContext
    {
        DbSet<SiteStructure> SiteStructures { get; set; }
        DbSet<Attachment> Attachments { get; set; }
        DbSet<CustomerOrder> CustomerOrders { get; set; }

        DbSet<SaleDetail> SaleDetails { get; set; }

        DbSet<Bank> Banks { get; set; }

        DbSet<AdvocacyUsersFromBank> AdvocacyUsersFromBanks { get; }

        DbSet<AdvocacyUser> AdvocacyUsers { get; set; }

        DbSet<UserRejectionFromBank> UserRejectionFromBank { get; set; }

        DbSet<Company> Companies { get; set; }

        DbSet<CarFamily> CarFamilies { get; set; }

        DbSet<CarType> CarTypes { get; set; }

        DbSet<CarTip> CarTip { get; set; }

        DbSet<CarTip_Gallery_Mapping> CarTip_Gallery_Mappings { get; set; }

        DbSet<Season_Product_Category> Season_Company_CarTip { get; set; }

        DbSet<ESaleType> ESaleTypes { get; set; }

        DbSet<Year> Year { get; set; }

        DbSet<Season> Seasons { get; set; }

        DbSet<WhiteList> WhiteLists { get; set; }

        DbSet<ExternalApiLogResult> ExternalApiLogResults { get; set; }

        DbSet<ExternalApiResponsLog> ExternalApiResponsLogs { get; set; }

        DbSet<Logs> Logs { get; set; }

        DbSet<CarMakerBlackList> CarMakerBlackLists { get; set; }

        DbSet<City> Cities { get; set; }

        DbSet<Province> Provinces { get; set; }

        DbSet<PreSale> PreSales { get; set; }

        DbSet<Gallery> Gallery { get; set; }

        DbSet<UserRejectionAdvocacy> UserRejectionAdvocacies { get; set; }

        DbSet<OrderRejectionTypeReadOnly> OrderRejectionTypeReadOnly { get; set; }

        DbSet<OrderStatusTypeReadOnly> OrderStatusTypeReadOnly { get; set; }

        DbSet<SaleSchema> SaleSchema { get; set; }

        DbSet<Agency> Agency { get; set; }

        DbSet<AgencySaleDetail> AgencySaleDetailMap { get; set; }

        DbSet<ProductAndCategory> ProductAndCategory { get; set; }

        DbSet<ProductAndCategoryType_ReadOnly> ProductAndCategoryType_ReadOnly { get; set; }
        DbSet<ProductLevel> ProductLevel { get; set; }


        DbSet<OrderStatusInquiry> OrderStatusInquiry { get; set; }

        DbSet<OrderDeliveryStatusTypeReadOnly> OrderDeliveryStatusTypeReadOnly { get; set; }
        DbSet<CarClass> CarClass { get; set; }

        DbSet<Questionnaire> Questionnaire { get; set; }

        DbSet<Question> Question { get; set; }

        DbSet<QuestionAnswer> QuestionAnswer { get; set; }

        DbSet<SubmittedAnswer> SubmittedAnswer { get; set; }

        DbSet<QuestionTypeReadOnly> QuestionTypeReadOnly { get; set; }
        DbSet<Announcement> Announcement { get; set; }
        DbSet<ChartStructure> ChartStructure { get; set; }
        DbSet<QuestionnaireTypeReadOnly> QuestionnaireTypeReadOnly { get; set; }

        DbSet<UnAuthorizedUser> UnAuthorizedUser { get; set; }

        DbSet<GenderTypeReadOnly> GenderTypeReadOnly { get; set; }

        DbSet<SaleProcessTypeReadOnly> SaleProcessTypeReadOnly { get; set; }
        DbSet<Organization> Organization { get; set; }
        DbSet<Priority> Priority { get; set; }

        DbSet<CustomerPriority> CustomerPriority { get; set; }
        DbSet<QuestionRelationship> QuestionRelationship { get; set; }


        DbSet<QuestionGroup> QuestionGroup { get; set; }
        DbSet<AdvertisementDetail> AdvertisementDetail { get; set; }
        DbSet<Advertisement> Advertisement { get; set; }

        DbSet<SaleDetailAllocation> SeasonCompanyProduct { get; set; }
    }
}