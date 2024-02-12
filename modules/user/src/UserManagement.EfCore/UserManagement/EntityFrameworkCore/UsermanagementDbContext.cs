using Microsoft.EntityFrameworkCore;
using UserManagement.Domain.UserManagement;
using UserManagement.Domain.UserManagement.Advocacy;
using UserManagement.Domain.UserManagement.Authorization.Users;
using UserManagement.Domain.UserManagement.Bases;
using UserManagement.Domain.UserManagement.CompanyService;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace UserManagement.EfCore.EntityFrameworkCore
{
    [ConnectionStringName(UserDbConsts.EfConnectionStrinName)]
    public class UsermanagementDbContext : AbpDbContext<UsermanagementDbContext>, IUsermanagementDbContext
    {
        public UsermanagementDbContext(DbContextOptions<UsermanagementDbContext> options) : base(options)
        {
        }

        public DbSet<AdvocacyUsers> AdvocacyUsers { get; set; }

        public DbSet<WhiteList> WhiteList { get; set; }

        public DbSet<AdvocacyUsersFromBank> AdvocacyUsersFromBank { get; set; }

        public DbSet<UserRejectionFromBank> UserRejectionFromBank { get; set; }
        public DbSet<ClientsOrderDetailByCompany> ClientsOrderDetailByCompany { get; set; }
        public DbSet<CompanyPaypaidPrices> CompanyPaypaidPrices { get; set; }
        public DbSet<UserSQL> UserSQL { get; set; }
        public DbSet<UserDataAccess> UserDataAccess { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ConfigureAuditLogging();
            builder.ConfigureUserManagement();
        }
    }
}
