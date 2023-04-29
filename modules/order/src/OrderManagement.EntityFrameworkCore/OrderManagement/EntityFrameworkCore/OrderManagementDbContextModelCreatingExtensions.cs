using System;
using Microsoft.EntityFrameworkCore;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace OrderManagement.EntityFrameworkCore
{
    public static class OrderManagementDbContextModelCreatingExtensions
    {
        public static void ConfigureOrderManagement(
            this ModelBuilder builder,
            Action<OrderManagementModelBuilderConfigurationOptions> optionsAction = null)
        {
            Check.NotNull(builder, nameof(builder));

            var options = new OrderManagementModelBuilderConfigurationOptions();

            optionsAction?.Invoke(options);
            
            builder.Entity<Order>(b =>
            {
                b.ToTable(options.TablePrefix + "Orders", options.Schema);

                b.ConfigureConcurrencyStamp();
                b.ConfigureExtraProperties();
                b.ConfigureAudited();

                b.Property(x => x.Code).IsRequired().HasMaxLength(OrderConsts.MaxCodeLength);
                b.Property(x => x.Name).IsRequired().HasMaxLength(OrderConsts.MaxNameLength);
                b.Property(x => x.ImageName).HasMaxLength(OrderConsts.MaxImageNameLength);

                b.HasIndex(q => q.Code);
                b.HasIndex(q => q.Name);
            });
        }
    }
}