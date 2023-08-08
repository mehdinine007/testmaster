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

        DbSet<OrderStatusInquiry> OrderStatusInquiry { get; set; }
    }
}