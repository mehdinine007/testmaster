using System;
using Microsoft.EntityFrameworkCore;
using Volo.Abp;
using OrderManagement.Domain;
using OrderManagement.Domain.Bases;
using System.Collections.Generic;
using System.Linq;
using OrderManagement.EfCore.Helpers;
using Volo.Abp.EntityFrameworkCore.Modeling;

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

        builder.Entity<PreSale>(entity => entity.ToTable(nameof(PreSale)));

        builder.Entity<SaleSchema>(entity => entity.ToTable(nameof(SaleSchema)));

        builder.Entity<Season>(entity => entity.ToTable(nameof(Season)));

        builder.Entity<CustomerOrder>(entity =>
        {

            entity.ConfigureFullAudited();
            entity.ConfigureSoftDelete();
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
            entity.Property(x => x.EngineNo)
                .HasMaxLength(20);
            entity.Property(x => x.ChassiNo)
                .HasMaxLength(20);
            entity.Property(x => x.Vin)
                .HasMaxLength(50);
            entity.Property(x => x.Vehicle)
                .HasMaxLength(50);
        });

        builder.Entity<SaleDetail>(entity =>
        {
            entity.ToTable(nameof(SaleDetail));


            entity.HasIndex(x => x.UID)
                .IsUnique();
            entity.Property(x => x.Visible)
                .HasDefaultValue(true);
        });

        builder.Entity<Bank>(entity => entity.ToTable(nameof(Bank)));

        builder.Entity<UserRejectionFromBank>(entity =>
        {
            entity.Property(x => x.nationalcode)
                .HasMaxLength(10);
            entity.Property(x => x.price)
                .HasColumnType("decimal(15)");
            entity.Property(x => x.shabaNumber)
               .HasMaxLength(26);
            entity.HasIndex(u => u.nationalcode)
              .HasFilter($"{nameof(UserRejectionFromBank.IsDeleted)} = 0");
        });

        builder.Entity<AdvocacyUser>(entity =>
        {
            entity.ToTable(nameof(AdvocacyUser));
        });

        builder.Entity<AdvocacyUsersFromBank>(entity =>
        {
            entity.ToTable(nameof(AdvocacyUsersFromBank));

            entity.HasIndex(co => co.nationalcode);
            entity.Property(x => x.nationalcode)
                .HasMaxLength(10);
            entity.Property(x => x.price)
                .HasColumnType("decimal(15)");
            entity.Property(x => x.shabaNumber)
                .HasMaxLength(26);

            entity.HasOne<Bank>(x => x.Bank)
                .WithMany(x => x.AdvocacyUsersFromBank)
                .HasForeignKey(x => x.BanksId)
                .IsRequired(false);
        });

        builder.Entity<Gallery>(entity =>
        {
            entity.ToTable(nameof(Gallery));
            entity.HasOne(x => x.CompanyLogo)
                .WithOne(x => x.GalleryLogo)
                .HasForeignKey<Company>(x => x.LogoId)
                .HasPrincipalKey<Gallery>(x => x.Id);

            entity.HasOne<Bank>(x => x.Bank)
                .WithOne(x => x.Gallery)
                .HasForeignKey<Bank>(x => x.LogoId);

            entity.HasOne<Company>(x => x.CompanyLogoInPage)
                .WithOne(x => x.GalleryLogoInPage)
                .HasForeignKey<Company>(x => x.LogoInPageId)
                .HasPrincipalKey<Gallery>(x => x.Id);

            entity.HasOne<Company>(x => x.CompanyBanner)
                .WithOne(x => x.GalleryBanner)
                .HasForeignKey<Company>(x => x.BannerId)
                .HasPrincipalKey<Gallery>(x => x.Id);
        });

        builder.Entity<Company>(entity =>
        {
            entity.ToTable(nameof(Company));
            entity.Property(x => x.Visible)
                .HasDefaultValue(true);
        });

        builder.Entity<CarFamily>(entity =>
        {
            entity.ToTable(nameof(CarFamily));
            entity.HasOne<Company>(x => x.Company)
                .WithMany(x => x.CarFamilies)
                .HasForeignKey(x => x.CompanyId);
        });

        builder.Entity<CarType>(entity =>
        {
            entity.ToTable(nameof(CarType));
            entity.HasOne<CarFamily>(x => x.CarFamily)
                .WithMany(x => x.CarTypes)
                .HasForeignKey(x => x.CarFamilyId);
        });

        builder.Entity<CarTip>(entity =>
        {
            entity.ToTable(nameof(CarTip));
            entity.HasOne<CarType>(x => x.CarType)
                .WithMany(x => x.CarTips)
                .HasForeignKey(x => x.CarTypeId);
        });

        builder.Entity<CarTip_Gallery_Mapping>(entity =>
        {
            entity.ToTable(nameof(CarTip_Gallery_Mapping));
            entity.HasOne<CarTip>(x => x.CarTip)
                .WithMany(x => x.CarTip_Gallery_Mappings)
                .HasForeignKey(x => x.CarTipId)
                .OnDelete(DeleteBehavior.ClientCascade);
            entity.HasOne<Gallery>(x => x.Gallery)
                .WithMany(x => x.CarTip_Gallery_Mappings)
                .HasForeignKey(x => x.GalleryId)
                .OnDelete(DeleteBehavior.ClientCascade);
        });

        builder.Entity<Season_Company_CarTip>(entity =>
        {
            entity.ToTable(nameof(Season_Company_CarTip));
            entity.HasOne<ESaleType>(x => x.ESaleType)
                .WithMany(x => x.SeasonCompanyCarTip)
                .HasForeignKey(x => x.EsaleTypeId);

            entity.HasOne<CarTip>(x => x.CarTip)
                .WithMany(x => x.SeasonCompanyCarTip)
                .HasForeignKey(x => x.CarTipId);

            entity.HasOne<Season>(x => x.Season)
                .WithMany(x => x.SeasonCompanyCarTip)
                .HasForeignKey(x => x.SeasonId);
        });

        builder.Entity<ESaleType>(entity => entity.ToTable(nameof(ESaleType)));

        builder.Entity<WhiteList>(entity =>
        {
            entity.HasIndex(u => u.NationalCode)
                .HasFilter($"{nameof(CustomerOrder.IsDeleted)} = 0");
            entity.HasIndex(u => new { u.NationalCode, u.WhiteListType })
                .HasFilter($"{nameof(CustomerOrder.IsDeleted)} = 0");
        });

        builder.Entity<ExternalApiLogResult>(entity =>
        {
            entity.ToTable(nameof(ExternalApiLogResult));
            entity.Property(x => x.NationalCode).HasMaxLength(10);
        });

        builder.Entity<ExternalApiResponsLog>(entity =>
        {
            entity.ToTable(nameof(ExternalApiResponsLog));
            entity.Property(x => x.NationalCode).HasMaxLength(10);
            entity.Property(x => x.ServiceName).HasMaxLength(100);
        });

        builder.Entity<CarMakerBlackList>(entity =>
        {
            entity.ToTable(nameof(CarMakerBlackList));
            entity.HasIndex(co => new { co.Nationalcode, co.EsaleTypeId });
        });

        builder.Entity<OrderRejectionTypeReadOnly>(entity =>
        {
            entity.ToTable(nameof(OrderRejectionTypeReadOnly));

            var records = Enum.GetValues(typeof(OrderRejectionType))
                .Cast<OrderRejectionType>()
                .OrderBy(x => (int)x)
                .Select(x => new Tuple<int, string, string>((int)x, x.ToString(), x.GetDisplayName()))
                .ToList();
            var dataToWrite = new List<OrderRejectionTypeReadOnly>(records.Count);
            for (var i = 0; i < records.Count; i++)
            {
                var current = records[i];
                dataToWrite.Add(new OrderRejectionTypeReadOnly(
                    id : i + 1,
                    orderRejectionCode : current.Item1,
                    orderRejectionTitleEn : current.Item2,
                    orderRejectionTitle : current.Item3
                ));
            }
            entity.HasData(dataToWrite);
        });

        builder.Entity<OrderStatusTypeReadOnly>(entity =>
        {
            entity.ToTable(nameof(OrderStatusTypeReadOnly));

            var statuses = Enum.GetValues(typeof(OrderStatusType))
                .Cast<OrderStatusType>()
                .OrderBy(x => (int)x)
                .Select(x => new Tuple<int, string, string>((int)x, x.ToString(), x.GetDisplayName()))
                .ToList();
            var dataToWrite = new List<OrderStatusTypeReadOnly>(statuses.Count);
            for (var i = 0; i < statuses.Count; i++)
            {
                var status = statuses[i];
                dataToWrite.Add(new OrderStatusTypeReadOnly(
                    id : i + 1,
                    orderStatusCode :status.Item1,
                    orderStatusTitleEn : status.Item2,
                    orderStatusTitle : status.Item3
                ));
            }
            entity.HasData(dataToWrite);
        });

        builder.Entity<UserRejectionAdvocacy>(entity =>
        {
            entity.ToTable(nameof(UserRejectionAdvocacy));
        });

        builder.Entity<WhiteList>(entity =>
        {
            entity.ToTable(nameof(WhiteList));
        });

        builder.Entity<Agency>(entity =>
        {
            entity.ToTable(nameof(Agency));

            entity.HasOne<City>(x => x.City)
                .WithMany(x => x.Agencies)
                .HasForeignKey(x => x.CityId);
        });

        builder.Entity<Agency_SaleDetail_Map>(entity =>
        {
            entity.ToTable(nameof(Agency_SaleDetail_Map));

            entity.HasOne<Agency>(x => x.Agency)
                .WithMany(x => x.Agency_SaleDetail_Maps)
                .HasForeignKey(x => x.AgencyId);

            entity.HasOne<SaleDetail>(x => x.SaleDetail)
                .WithMany(x => x.AgencySaleDetailMaps)
                .HasForeignKey(x => x.SaleDetailId);
        });
    }
}
