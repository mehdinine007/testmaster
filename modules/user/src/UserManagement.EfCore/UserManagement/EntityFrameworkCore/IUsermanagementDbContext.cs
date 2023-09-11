using Microsoft.EntityFrameworkCore;
using UserManagement.Domain.UserManagement.Advocacy;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace UserManagement.EfCore.EntityFrameworkCore
{
    [ConnectionStringName("UserManagement")]
    public interface IUsermanagementDbContext : IEfCoreDbContext
    {
        DbSet<AdvocacyUsers> AdvocacyUsers { get; set; }
    }
}
