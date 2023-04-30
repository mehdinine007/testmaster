using System;
using Microsoft.EntityFrameworkCore;
using Volo.Abp;
using OrderManagement.Domain;

namespace OrderManagement.EfCore;

public static class OrderManagementDbContextModelCreatingExtensions
{
    public static void ConfigureOrderManagement(
        this ModelBuilder builder,
        Action<OrderManagementModelBuilderConfigurationOptions> optionsAction = null)
    {
        Check.NotNull(builder, nameof(builder));

        var options = new OrderManagementModelBuilderConfigurationOptions();

        optionsAction?.Invoke(options);

        builder.Entity<CustomerOrder>(entity =>
        {
            entity.ToTable(nameof(CustomerOrder));

            entity.HasIndex(co => new { co.SaleDetailId, co.UserId })
                .HasFilter($"{nameof(CustomerOrder.IsDeleted)} = 0 and " +
                $"{nameof(CustomerOrder.PriorityId)} IS NOT NULL and " +
                $"{nameof(CustomerOrder.OrderStatus)} =" + (int)OrderStatusType.RecentlyAdded)
                .IsUnique();

            entity.HasIndex(co => new { co.SaleId, co.UserId, co.OrderStatus, co.PriorityId })
                .HasFilter($"{nameof(CustomerOrder.IsDeleted)} = 0 and " +
                $"{nameof(CustomerOrder.PriorityId)} IS NOT NULL and " +
                $"{nameof(CustomerOrder.OrderStatus)} =" + (int)OrderStatusType.RecentlyAdded)
                .IsUnique();

            entity//emkan sabte dota priority dar yek sale nabashe
                .HasIndex(co => new { co.UserId, co.OrderStatus })
                .HasFilter($"{nameof(CustomerOrder.IsDeleted)} = 0  ");

            entity.
                HasIndex(co => new { co.SaleId, co.UserId, co.OrderStatus })
                .HasFilter($"{nameof(CustomerOrder.IsDeleted)} = 0 and " +
                $"{nameof(CustomerOrder.PriorityId)} IS NULL and " +
                $"{nameof(CustomerOrder.OrderStatus)} =" + (int)OrderStatusType.RecentlyAdded)
                .IsUnique();

            //entity.HasOne<SaleDetail>(x => x.SaleDetail)
            //    .WithMany(x => x.CustomerOrders)
            //    .HasForeignKey(x => x.SaleDetailId)
            //    .OnDelete(DeleteBehavior.NoAction)
        });
    }
}