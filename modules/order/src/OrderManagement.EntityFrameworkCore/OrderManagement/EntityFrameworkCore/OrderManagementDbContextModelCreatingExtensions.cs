﻿using System;
using Microsoft.EntityFrameworkCore;
using Volo.Abp;
using OrderManagement.Domain;
using OrderManagement.Domain.Bases;
using Volo.Abp.EntityFrameworkCore.Modeling;
using OrderManagement.Domain.OrderManagement;
using IFG.Core.DataAccess;
using OrderManagement.Domain.Shared;

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

        builder.Entity<SaleSchema>(
            entity => entity.ToTable(nameof(SaleSchema))
            );

        builder.Entity<Season>(entity => entity.ToTable(nameof(Season)));

        builder.Entity<CustomerOrder>(entity =>
        {

            entity.ConfigureFullAudited();
            entity.ConfigureSoftDelete();
            entity.ToTable(nameof(CustomerOrder));

            entity.HasOne<SeasonAllocation>(x => x.SeasonAllocation)
                .WithMany(x => x.CustomerOrders)
                .HasForeignKey(x => x.SeasonAllocationId);
            entity.HasIndex(u => u.SeasonAllocationId);
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
            entity.HasIndex(p => p.TrackingCode)
                .IsUnique()
                .HasFilter($"{nameof(CustomerOrder.IsDeleted)} = 0 ");
        });

        builder.Entity<SaleDetail>(entity =>
        {
            entity.ToTable(nameof(SaleDetail));


            entity.HasIndex(x => x.UID)
                .IsUnique();
            entity.Property(x => x.Visible)
                .HasDefaultValue(true);
            entity.Property(x => x.CompanySaleId)
               .HasMaxLength(20);
            entity.HasOne<SaleSchema>(x => x.SaleSchema)
                .WithMany(x => x.SaleDetails)
                .HasForeignKey(x => x.SaleId);
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

        builder.Entity<Season_Product_Category>(entity =>
        {
            entity.ToTable(/*nameof(Season_Product_Category)*/"Season_Company_CarTip");
            entity.HasOne<ESaleType>(x => x.ESaleType)
                .WithMany(x => x.SeasonCompanyCarTip)
                .HasForeignKey(x => x.EsaleTypeId);

            entity.HasOne<Season>(x => x.Season)
                .WithMany(x => x.SeasonCompanyCarTip)
                .HasForeignKey(x => x.SeasonId);

            entity.HasOne<ProductAndCategory>(x => x.Category)
                .WithMany(x => x.CategorySeason)
                .HasForeignKey(x => x.CategoryId);

            entity.HasOne<ProductAndCategory>(x => x.Product)
                .WithMany(x => x.ProductSeason)
                .HasForeignKey(x => x.ProductId);
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

            entity.AddEnumChangeTracker<OrderRejectionTypeReadOnly, OrderRejectionType>();
        });

        builder.Entity<OrderStatusTypeReadOnly>(entity =>
        {
            entity.ToTable(nameof(OrderStatusTypeReadOnly));

            entity.AddEnumChangeTracker<OrderStatusTypeReadOnly, OrderStatusType>();
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

            entity.HasOne<Province>(x => x.Province)
                .WithMany(x => x.Agencies)
                .HasForeignKey(x => x.ProvinceId);
            entity.HasOne<City>(x => x.City)
                .WithMany(x => x.Agencies)
                .HasForeignKey(x => x.CityId)
              .OnDelete(DeleteBehavior.ClientCascade);
        });

        builder.Entity<AgencySaleDetail>(entity =>
        {
            entity.ToTable(nameof(AgencySaleDetail));

            entity.HasOne<Agency>(x => x.Agency)
                .WithMany(x => x.AgencySaleDetails)
                .HasForeignKey(x => x.AgencyId);

            entity.HasOne<SaleDetail>(x => x.SaleDetail)
                .WithMany(x => x.AgencySaleDetails)
                .HasForeignKey(x => x.SaleDetailId);
        });


        builder.Entity<SaleDetailCarColor>(entity =>
        {
            entity.ToTable(nameof(SaleDetailCarColor));

            entity.HasOne<Color>(x => x.Color)
                .WithMany(x => x.SaleDetailCarColor)
                .HasForeignKey(x => x.ColorId);

            entity.HasOne<SaleDetail>(x => x.SaleDetail)
                .WithMany(x => x.SaleDetailCarColors)
                .HasForeignKey(x => x.SaleDetailId);
        });

        builder.Entity<ProductAndCategory>(entity =>
        {
            entity.ToTable(nameof(ProductAndCategory));

            entity.HasOne<ProductAndCategory>(x => x.Parent)
                .WithMany(x => x.Childrens)
                .HasForeignKey(x => x.ParentId)
                .OnDelete(DeleteBehavior.ClientCascade);

            entity.Property(x => x.Code)
                .HasMaxLength(250);

            entity.Property(x => x.Title)
                .HasMaxLength(250);
        });

        builder.Entity<ProductAndCategoryType_ReadOnly>(entity =>
        {
            entity.ToTable(nameof(ProductAndCategoryType_ReadOnly));
            entity.AddEnumChangeTracker<ProductAndCategoryType_ReadOnly, ProductAndCategoryType>();
        });

        builder.Entity<ProductAndCategory>(entity =>
        {
            entity.ToTable(nameof(ProductAndCategory));
            entity.HasOne<ProductLevel>(x => x.ProductLevel)
                .WithMany(x => x.ProductAndCategories)
                .HasForeignKey(x => x.ProductLevelId);
            entity.HasOne<Organization>(x => x.Organization)
                .WithMany(x => x.ProductAndCategories)
                .HasForeignKey(x => x.OrganizationId)
                .IsRequired();
        });
        builder.Entity<AdvertisementDetail>(entity =>
        {
            entity.ToTable(nameof(AdvertisementDetail));
            entity.HasOne<Advertisement>(x => x.Advertisement)
                .WithMany(x => x.AdvertisementDetails)
                .HasForeignKey(x => x.AdvertisementId);
            entity.Property(x => x.Title)
            .IsRequired().HasMaxLength(100);

        });
        builder.Entity<Advertisement>(entity =>
        {
            entity.Property(x => x.Title)
            .IsRequired().HasMaxLength(100);

        });


        builder.Entity<OrderStatusInquiry>(entity =>
        {
            entity.ToTable(nameof(OrderStatusInquiry));

            entity.HasOne<ProductAndCategory>(x => x.CompanyCategory)
                .WithMany(x => x.OrderStatusInquiries)
                .HasForeignKey(x => x.CompanyId);
            entity.Property(x => x.FullAmountPaid)
                .HasColumnType("decimal(18,4)");
        });

        builder.Entity<OrderDeliveryStatusTypeReadOnly>(entity =>
        {
            entity.ToTable(nameof(OrderDeliveryStatusTypeReadOnly));
            entity.AddFullEnumChangeTracker<OrderDeliveryStatusTypeReadOnly, OrderDeliveryStatusType>();
        });

        builder.Entity<Questionnaire>(entity =>
        {
            entity.ToTable(nameof(Questionnaire));

            entity.Property(x => x.Title)
                .HasMaxLength(50);
            entity.Property(x => x.Description)
                .HasMaxLength(250);
        });

        builder.Entity<QuestionTypeReadOnly>(entity =>
        {
            entity.ToTable(nameof(QuestionTypeReadOnly));
            entity.AddEnumChangeTracker<QuestionTypeReadOnly, QuestionType>();
        });

        builder.Entity<Question>(entity =>
        {
            entity.ToTable(nameof(Question));
            entity.HasIndex(x => x.QuestionGroupId)
            .IsUnique(false);
            entity.HasOne<Questionnaire>(x => x.Questionnaire)
                .WithMany(x => x.Questions)
                .HasForeignKey(x => x.QuestionnaireId);

            entity.HasOne<QuestionGroup>(x => x.QuestionGroup)
                .WithOne(x => x.Question)
                .HasForeignKey<Question>(x => x.QuestionGroupId)
                .OnDelete(DeleteBehavior.NoAction);

            entity.Property(x => x.Title)
                .HasMaxLength(50);
        });
        builder.Entity<QuestionGroup>(entity =>
        {
            entity.ToTable(nameof(QuestionGroup));

            entity.HasOne<Questionnaire>(x => x.Questionnaire)
                .WithMany(x => x.QuestionGroups)
                .HasForeignKey(x => x.QuestionnaireId)
                .OnDelete(DeleteBehavior.NoAction);

            entity.Property(x => x.Title)
                    .HasMaxLength(50);
        });
        builder.Entity<QuestionAnswer>(entity =>
        {
            entity.ToTable(nameof(QuestionAnswer));

            entity.HasOne<Question>(x => x.Question)
                .WithMany(x => x.Answers)
                .HasForeignKey(x => x.QuestionId);

            entity.HasOne<Questionnaire>(x => x.Questionnaire)
                .WithMany(x => x.Answers)
                .HasForeignKey(x => x.QuestionnaireId);
            entity.Property(x => x.Description)
            .IsRequired().HasMaxLength(250);
            entity.Property(x => x.Hint)
            .IsRequired().HasMaxLength(250);
            entity.Property(x => x.CustomValue)
            .IsRequired().HasMaxLength(250);
        });

        builder.Entity<SubmittedAnswer>(entity =>
        {
            entity.ToTable(nameof(SubmittedAnswer));

            entity.HasOne<Question>(x => x.Question)
                .WithMany(x => x.SubmittedAnswers)
                .HasForeignKey(x => x.QuestionId);

            entity.HasOne<QuestionAnswer>(x => x.QuestionAnswer)
                .WithMany(x => x.SubmittedAnswers)
                .HasForeignKey(x => x.QuestionAnswerId);
            entity.Property(x => x.CustomAnswerValue)
                .HasMaxLength(250);
        });

        builder.Entity<Announcement>(entity =>
        {
            entity.ToTable(nameof(Announcement));
            entity.HasOne<ProductAndCategory>(x => x.Company)
                .WithOne(x => x.Announcement)
                .HasForeignKey<Announcement>(x => x.CompanyId)
                .HasPrincipalKey<ProductAndCategory>(x => x.Id);
        });

        builder.Entity<QuestionnaireTypeReadOnly>(entity =>
        {
            entity.ToTable(nameof(QuestionnaireTypeReadOnly));
            entity.AddEnumChangeTracker<QuestionnaireTypeReadOnly, QuestionnaireType>();
        });

        builder.Entity<UnAuthorizedUser>(entity =>
        {
            entity.ToTable(nameof(UnAuthorizedUser));

            entity.HasOne(x => x.Questionnaire)
                .WithMany(x => x.UnAuthorizedUsers)
                .HasForeignKey(x => x.QuestionnaireId);

            entity.Property(x => x.Age)
                .HasColumnType("Date");
        });


        builder.Entity<GenderTypeReadOnly>(entity =>
        {
            entity.ToTable(nameof(GenderTypeReadOnly));
            entity.AddEnumChangeTracker<GenderTypeReadOnly, GenderType>();
        });
        builder.Entity<Organization>(entity =>
        {
            entity.ToTable(nameof(Organization));
        });

        builder.Entity<SaleProcessTypeReadOnly>(entity =>
        {
            entity.ToTable(nameof(SaleProcessTypeReadOnly));
            entity.AddEnumChangeTracker<SaleProcessTypeReadOnly, SaleProcessType>();
        });

        builder.Entity<Priority>(entity =>
        {
            entity.ToTable("PriorityList");
            entity.HasIndex(x => x.NationalCode, "IX_PriorityList_NationalCode");
        });

        builder.Entity<CustomerPriority>(entity =>
        {
            entity.ToTable(nameof(CustomerPriority));

            entity.HasIndex(x => x.Uid, "IX_CustomerPriority_Uid");
        });



        builder.Entity<QuestionRelationship>(entity =>
        {
            entity.ToTable(nameof(QuestionRelationship));

            entity.HasOne<Question>(x => x.Question)
                    .WithMany(x => x.QuestionRelationships)
                    .HasForeignKey(x => x.QuestionId)
                    .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne<QuestionAnswer>(x => x.QuestionAnswer)
                   .WithMany(x => x.questionRelationships)
                   .HasForeignKey(x => x.QuestionAnswerId);
        });

        builder.Entity<OperatorEnumReadOnly>(entity =>
        {
            entity.ToTable(nameof(OperatorEnumReadOnly));
            entity.AddEnumChangeTracker<OperatorEnumReadOnly, OperatorFilterEnum>();
        });

        builder.Entity<SaleDetailAllocation>(entity =>
        {
            entity.ToTable(nameof(SaleDetailAllocation));

            entity.HasOne<SeasonAllocation>(x => x.SeasonAllocation)
                .WithMany(x => x.SaleDetailAllocations)
                .HasForeignKey(x => x.SeasonAllocationId);

            entity.HasOne<SaleDetail>(x => x.SaleDetail)
                .WithMany(x => x.SeasonCompanyProducts)
                .HasForeignKey(x => x.SaleDetailId);
        });
    }
}
