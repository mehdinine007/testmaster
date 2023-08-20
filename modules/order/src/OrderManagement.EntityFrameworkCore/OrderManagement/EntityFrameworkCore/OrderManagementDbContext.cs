using Microsoft.EntityFrameworkCore;
using OrderManagement.Domain;
using OrderManagement.Domain.Bases;
using System.ComponentModel.DataAnnotations;
using System;
using Volo.Abp.AuditLogging;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;
using OrderManagement.Domain.OrderManagement;

namespace OrderManagement.EfCore
{
    [ConnectionStringName("OrderManagement")]
    public class OrderManagementDbContext : AbpDbContext<OrderManagementDbContext>, IOrderManagementDbContext
    {
        public DbSet<SiteStructure> SiteStructures { get; set; }
        public DbSet<Attachment> Attachments { get; set; }
        public static string TablePrefix { get; set; } = OrderManagementConsts.DefaultDbTablePrefix;

        public static string Schema { get; set; } = OrderManagementConsts.DefaultDbSchema;

        public DbSet<CustomerOrder> CustomerOrders { get; set; }

        public DbSet<SaleDetail> SaleDetails { get; set; }

        public DbSet<Bank> Banks { get; set; }

        public DbSet<AdvocacyUsersFromBank> AdvocacyUsersFromBanks { get; set; }

        public DbSet<AdvocacyUser> AdvocacyUsers { get; set; }

        public DbSet<UserRejectionFromBank> UserRejectionFromBank { get; set; }

        public DbSet<Company> Companies { get; set; }

        public DbSet<CarFamily> CarFamilies { get; set; }

        public DbSet<CarType> CarTypes { get; set; }

        public DbSet<CarTip> CarTip { get; set; }

        public DbSet<CarTip_Gallery_Mapping> CarTip_Gallery_Mappings { get; set; }

        public DbSet<Season_Product_Category> Season_Company_CarTip { get; set; }

        public DbSet<UserRejectionAdvocacy> UserRejectionAdvocacies { get; set; }

        public DbSet<ESaleType> ESaleTypes { get; set; }

        public DbSet<Year> Year { get; set; }

        public DbSet<Season> Seasons { get; set; }

        public DbSet<WhiteList> WhiteLists { get; set; }

        public DbSet<ExternalApiLogResult> ExternalApiLogResults { get; set; }

        public DbSet<ExternalApiResponsLog> ExternalApiResponsLogs { get; set; }

        public DbSet<Logs> Logs { get; set; }

        public DbSet<CarMakerBlackList> CarMakerBlackLists { get; set; }

        public DbSet<City> Cities { get; set; }
        public DbSet<Gallery> Gallery { get; set; }

        public DbSet<Province> Provinces { get; set; }

        public DbSet<PreSale> PreSales { get; set; }

        public DbSet<OrderRejectionTypeReadOnly> OrderRejectionTypeReadOnly { get; set; }

        public DbSet<OrderStatusTypeReadOnly> OrderStatusTypeReadOnly { get; set; }

        public DbSet<SaleSchema> SaleSchema { get; set; }

        public DbSet<Agency> Agency { get; set; }

        public DbSet<AgencySaleDetail> AgencySaleDetailMap { get; set; }

        public DbSet<Color> Color { get; set; }

        public DbSet<SaleDetailCarColor> SaleDetailCarColor { get; set; }

        public DbSet<ProductAndCategory> ProductAndCategory { get; set; }

        public DbSet<ProductAndCategoryType_ReadOnly> ProductAndCategoryType_ReadOnly { get; set; }
        public DbSet<ProductLevel> ProductLevel { get; set; }

        public DbSet<OrderStatusInquiry> OrderStatusInquiry { get; set; }

        public DbSet<OrderDeliveryStatusTypeReadOnly> OrderDeliveryStatusTypeReadOnly { get; set; }

        public DbSet<CarClass> CarClass { get; set; }
        public DbSet<Announcement> Announcement { get; set; }


        public DbSet<Questionnaire> Questionnaire { get; set; }

        public DbSet<Question> Question { get; set; }

        public DbSet<QuestionAnswer> QuestionAnswer { get; set; }

        public DbSet<SubmittedAnswer> SubmittedAnswer { get; set; }

        public DbSet<QuestionTypeReadOnly> QuestionTypeReadOnly { get; set; }

        public OrderManagementDbContext(DbContextOptions<OrderManagementDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //System.Diagnostics.Debugger.Launch();
            base.OnModelCreating(builder);

            builder.ConfigureAuditLogging();
            builder.ConfigureOrderManagement(options =>
            {
                //options.TablePrefix = TablePrefix;
                //options.Schema = Schema;
            });
        }
    }
}