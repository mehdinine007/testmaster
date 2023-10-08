using Microsoft.EntityFrameworkCore;
using OrderManagement.Domain.Bases;
using OrderManagement.Domain.OrderManagement;
using OrderManagement.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using OrderManagement.Domain.CompanyManagement;

namespace OrderManagement.EfCore.CompanyManagement.EntityFrameworkCore
{
    public static class CompanyManagementDbContextModelCreatingExtensions
    {
        public static void ConfigureCompanyManagement(this ModelBuilder builder,Action<CompanyManagementModelBuilderConfigurationOptions> optionsAction = null)
        {
            Check.NotNull(builder, nameof(builder));

            var options = new CompanyManagementModelBuilderConfigurationOptions();

            optionsAction?.Invoke(options);


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

        }
    }
}
