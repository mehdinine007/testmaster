using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace UserManagement.EfCore.EntityFrameworkCore
{
    [ConnectionStringName("UserManagement")]
    public interface IUsermanagementDbContext : IEfCoreDbContext
    {

    }
}
