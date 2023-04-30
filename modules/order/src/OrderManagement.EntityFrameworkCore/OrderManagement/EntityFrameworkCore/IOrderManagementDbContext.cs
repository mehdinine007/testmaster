using Microsoft.EntityFrameworkCore;
using OrderManagement.Domain;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace OrderManagement.EfCore
{
    [ConnectionStringName("OrderManagement")]
    public interface IOrderManagementDbContext : IEfCoreDbContext
    {
         DbSet<CustomerOrder> CustomerOrders { get; }
    }
}