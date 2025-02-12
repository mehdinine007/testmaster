﻿using Volo.Abp;
using Microsoft.EntityFrameworkCore;
using UserManagement.EfCore.UserManagement.EntityFrameworkCore;
using UserManagement.Domain.UserManagement.Bases;
using UserManagement.Domain.UserManagement.Authorization.Users;
using System.Reflection.Emit;
using UserManagement.Domain.UserManagement.CompanyService;
using UserManagement.Domain.UserManagement;

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
                    b.Ignore(e => e.EditMode);
                    b.Property(t => t.UID).HasColumnType("UniqueIdentifier");


                });





            builder.Entity<CompanyPaypaidPrices>(entity =>
            {
                entity.ToTable(nameof(CompanyPaypaidPrices));
            });
            builder.Entity<ClientsOrderDetailByCompany>(entity =>
            {
                entity.ToTable(nameof(ClientsOrderDetailByCompany));

                entity.Property(x => x.NationalCode)
                    .HasMaxLength(10);

                entity.Property(x => x.SaleType)
                    .HasMaxLength(150);

                entity.Property(x => x.Vin)
                    .HasMaxLength(50);

                entity.Property(x => x.BodyNumber)
                    .HasMaxLength(50);

                entity.Property(x => x.CarDesc)
                    .HasMaxLength(250);
            });

            builder.Entity<UserDataAccess>(entity =>
            {
                entity.ToTable(nameof(UserDataAccess));
                entity.Property(x => x.Nationalcode).HasMaxLength(10);
                entity.HasIndex(x => x.Nationalcode);
            });


        }
    }
}
