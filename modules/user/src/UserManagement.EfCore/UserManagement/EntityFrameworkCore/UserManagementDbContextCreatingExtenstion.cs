using Volo.Abp;
using Microsoft.EntityFrameworkCore;
using UserManagement.EfCore.UserManagement.EntityFrameworkCore;
using UserManagement.Domain.UserManagement.Bases;
using UserManagement.Domain.UserManagement.Authorization.Users;
using System.Reflection.Emit;

namespace UserManagement.EfCore.EntityFrameworkCore
{
    public static class UserManagementDbContextCreatingExtenstion
    {
        public static void ConfigureUserManagement(this ModelBuilder builder,
             Action<UserManagementModelBuilderConfigurationOptions> optionsAction = null)
        {
            Check.NotNull(builder, nameof(builder));

            var options = new UserManagementModelBuilderConfigurationOptions();

            optionsAction?.Invoke(options);

            builder.Entity<WhiteList>()
                .HasIndex(u => u.NationalCode);
            //.HasFilter($"{nameof(CustomerOrder)} = 0");


            builder.Entity<WhiteList>()
                .HasIndex(u => new { u.NationalCode, u.WhiteListType });
            //.HasFilter($"{nameof(OrderManagement.Domain.CustomerOrder.IsDeleted)} = 0");
            builder.Entity<UserSQL>()
                .ToTable("AbpUsers");
            builder.Entity<UserSQL>(b =>
                {
                    b.Property(e => e.MongoId).HasMaxLength(64);
                    b.Property(e => e.Roles).HasMaxLength(256);
                    b.Ignore(e => e.Roles);
                });
             


              


        }
    }
}
