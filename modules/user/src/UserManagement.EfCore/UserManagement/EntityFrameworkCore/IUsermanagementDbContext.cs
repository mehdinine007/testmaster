using Microsoft.EntityFrameworkCore;
using UserManagement.Domain.UserManagement.Advocacy;
using UserManagement.Domain.UserManagement.Bases;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using UserManagement.Domain.UserManagement;

namespace UserManagement.EfCore.EntityFrameworkCore
{
    [ConnectionStringName(UserDbConsts.EfConnectionStrinName)]
    public interface IUsermanagementDbContext : IEfCoreDbContext
    {
        DbSet<WhiteList> WhiteList { get; set; }
        DbSet<AdvocacyUsers> AdvocacyUsers { get; set; }

        DbSet<AdvocacyUsersFromBank> AdvocacyUsersFromBank { get; set; }

        DbSet<UserRejectionFromBank> UserRejectionFromBank { get; set; }
    }
}
