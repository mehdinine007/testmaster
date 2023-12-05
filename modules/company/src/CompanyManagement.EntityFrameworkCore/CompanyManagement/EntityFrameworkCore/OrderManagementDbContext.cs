using CompanyManagement.Domain.CompanyManagement;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace CompanyManagement.EfCore.CompanyManagement.EntityFrameworkCore;

[ConnectionStringName("OrderManagement")]
public class OrderManagementDbContext : AbpDbContext<OrderManagementDbContext>, IOrderManagementDbContex
{
    public OrderManagementDbContext(DbContextOptions<OrderManagementDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<CompaniesCustomer>(entity =>
        {
            entity.ToTable(nameof(CompaniesCustomer), t => t.ExcludeFromMigrations());
            entity.HasNoKey();
        });
    }

    public DbSet<CompaniesCustomer> CompaniesCustomer { get; set; }
}
