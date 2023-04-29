using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace OrderManagement.EntityFrameworkCore
{
    [ConnectionStringName("OrderManagement")]
    public interface IOrderManagementDbContext : IEfCoreDbContext
    {
         DbSet<Order> Orders { get; }
    }
}