using Microsoft.EntityFrameworkCore;
using PaymentManagement.Domain.Models;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace PaymentManagement.EntityFrameworkCore
{
    [ConnectionStringName("PaymentManagement")]
    public interface IPaymentManagementDbContext : IEfCoreDbContext
    {
        DbSet<Account> Account { get; }
        DbSet<Customer> Customer { get; }
        DbSet<Payment> Payment { get; }
        DbSet<PaymentLog> PaymentLog { get; }
        DbSet<PaymentStatus> PaymentStatus { get; }
        DbSet<Psp> Psp { get; }
        DbSet<PspAccount> PspAccount { get; }
    }
}