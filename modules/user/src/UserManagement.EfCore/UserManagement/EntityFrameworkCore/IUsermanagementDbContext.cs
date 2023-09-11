using Microsoft.EntityFrameworkCore;
using UserManagement.Domain.UserManagement.Bases;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace UserManagement.EfCore.EntityFrameworkCore
{
    [ConnectionStringName("UserManagement")]
    public interface IUsermanagementDbContext : IEfCoreDbContext
    {
        DbSet<WhiteList> WhiteList { get; set; }
    }
}
