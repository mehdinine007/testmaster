using Microsoft.EntityFrameworkCore;
using UserManagement.Domain.UserManagement.Advocacy;
using UserManagement.Domain.UserManagement.Bases;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using UserManagement.Domain.UserManagement;
using UserManagement.Domain.UserManagement.CompanyService;

namespace UserManagement.EfCore.EntityFrameworkCore
{
    [ConnectionStringName(UserDbConsts.EfConnectionStrinName)]
    public interface IUsermanagementDbContext : IEfCoreDbContext
    {
        DbSet<WhiteList> WhiteList { get; set; }
        DbSet<AdvocacyUsers> AdvocacyUsers { get; set; }

        DbSet<AdvocacyUsersFromBank> AdvocacyUsersFromBank { get; set; }

        DbSet<UserRejectionFromBank> UserRejectionFromBank { get; set; }
        DbSet<ClientsOrderDetailByCompany> ClientsOrderDetailByCompany { get; set; }
        DbSet<CompanyPaypaidPrices> CompanyPaypaidPrices { get; set; }
        DbSet<UserDataAccess> UserDataAccess { get; set; }
    }
}
