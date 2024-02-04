using Microsoft.EntityFrameworkCore;
using PaymentManagement.Domain;
using PaymentManagement.Domain.Models;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace PaymentManagement.EntityFrameworkCore
{
    [ConnectionStringName("PaymentManagement")]
    public class PaymentManagementDbContext : AbpDbContext<PaymentManagementDbContext>, IPaymentManagementDbContext
    {
        public static string TablePrefix { get; set; } = PaymentManagementConsts.DefaultDbTablePrefix;

        public static string Schema { get; set; } = PaymentManagementConsts.DefaultDbSchema;

        public DbSet<Account> Account { get; set; }
        public DbSet<Customer> Customer { get; set; }
        public DbSet<Payment> Payment { get; set; }
        public DbSet<PaymentLog> PaymentLog { get; set; }
        public DbSet<PaymentStatus> PaymentStatus { get; set; }
        public DbSet<Psp> Psp { get; set; }
        public DbSet<PspAccount> PspAccount { get; set; }

        public PaymentManagementDbContext(DbContextOptions<PaymentManagementDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigurePaymentManagement(options =>
            {
                //options.TablePrefix = TablePrefix;
                //options.Schema = Schema;
            });

            builder.Entity<PaymentLog>().HasIndex(p => p.Message);
        }
    }
}