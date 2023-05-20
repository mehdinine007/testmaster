using Microsoft.EntityFrameworkCore;
using PaymentManagement.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace PaymentService.Host.EntityFrameworkCore
{
    public class PaymentServiceMigrationDbContext : AbpDbContext<PaymentServiceMigrationDbContext>
    {
        public PaymentServiceMigrationDbContext(
            DbContextOptions<PaymentServiceMigrationDbContext> options
            ) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ConfigurePaymentManagement();
        }
    }
}
