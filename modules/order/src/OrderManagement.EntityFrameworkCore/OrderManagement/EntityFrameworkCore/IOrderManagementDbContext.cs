using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using OrderManagement.Domain;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace OrderManagement.EfCore
{
    [ConnectionStringName("OrderManagement")]
    public interface IOrderManagementDbContext : IEfCoreDbContext
    {
        DbSet<CustomerOrder> CustomerOrders { get; set; }

        DbSet<SaleDetail> SaleDetails { get; set; }

        DbSet<Bank> Banks { get; set; }

        DbSet<AdvocacyUsersFromBank> AdvocacyUsersFromBanks { get; }

        DbSet<AdvocacyUsers> AdvocacyUsers { get; set; }

        DbSet<UserRejectionFromBank> UserRejectionFromBank { get; set; }

        DbSet<Company> Companies { get; set; }

        DbSet<CarFamily> CarFamilies { get; set; }

        DbSet<CarType> CarTypes { get; set; }

        DbSet<CarTip> CarTip { get; set; }

        DbSet<CarTip_Gallery_Mapping> CarTip_Gallery_Mappings { get; set; }

        DbSet<Season_Company_CarTip> Season_Company_CarTip { get; set; }

        DbSet<ESaleType> ESaleTypes { get; set; }

        DbSet<Year> Year { get; set; }

        DbSet<Season> Seasons { get; set; }

        DbSet<WhiteList> WhiteLists { get; set; }

        DbSet<ExternalApiLogResult> ExternalApiLogResults { get; set; }

        DbSet<ExternalApiResponsLog> ExternalApiResponsLogs { get; set; }

        DbSet<Logs> Logs { get; set; }
    }
}