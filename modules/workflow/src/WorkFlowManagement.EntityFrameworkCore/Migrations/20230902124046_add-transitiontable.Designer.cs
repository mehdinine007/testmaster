﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Volo.Abp.EntityFrameworkCore;
using WorkFlowManagement.EntityFrameworkCore;

#nullable disable

namespace WorkFlowManagement.EntityFrameworkCore.Migrations
{
    [DbContext(typeof(WorkFlowManagementDbContext))]
    [Migration("20230902124046_add-transitiontable")]
    partial class addtransitiontable
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("_Abp_DatabaseProvider", EfCoreDatabaseProvider.SqlServer)
                .HasAnnotation("ProductVersion", "7.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("WorkFlowManagement.Domain.WorkFlowManagement.Activity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("datetime2")
                        .HasColumnName("CreationTime");

                    b.Property<Guid?>("CreatorId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("CreatorId");

                    b.Property<Guid?>("DeleterId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("DeleterId");

                    b.Property<DateTime?>("DeletionTime")
                        .HasColumnType("datetime2")
                        .HasColumnName("DeletionTime");

                    b.Property<int>("Entity")
                        .HasColumnType("int");

                    b.Property<int>("FlowType")
                        .HasColumnType("int");

                    b.Property<bool>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false)
                        .HasColumnName("IsDeleted");

                    b.Property<DateTime?>("LastModificationTime")
                        .HasColumnType("datetime2")
                        .HasColumnName("LastModificationTime");

                    b.Property<Guid?>("LastModifierId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("LastModifierId");

                    b.Property<int>("SchemeId")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("SchemeId");

                    b.ToTable("Activities", "Flow");
                });

            modelBuilder.Entity("WorkFlowManagement.Domain.WorkFlowManagement.OrganizationChart", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Code")
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("datetime2")
                        .HasColumnName("CreationTime");

                    b.Property<Guid?>("CreatorId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("CreatorId");

                    b.Property<Guid?>("DeleterId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("DeleterId");

                    b.Property<DateTime?>("DeletionTime")
                        .HasColumnType("datetime2")
                        .HasColumnName("DeletionTime");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false)
                        .HasColumnName("IsDeleted");

                    b.Property<DateTime?>("LastModificationTime")
                        .HasColumnType("datetime2")
                        .HasColumnName("LastModificationTime");

                    b.Property<Guid?>("LastModifierId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("LastModifierId");

                    b.Property<int>("OrganizationType")
                        .HasColumnType("int");

                    b.Property<int?>("ParentId")
                        .HasColumnType("int");

                    b.Property<bool>("Status")
                        .HasColumnType("bit");

                    b.Property<string>("Title")
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.HasKey("Id");

                    b.HasIndex("ParentId");

                    b.ToTable("OrganizationCharts", "Flow");
                });

            modelBuilder.Entity("WorkFlowManagement.Domain.WorkFlowManagement.OrganizationPosition", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("datetime2")
                        .HasColumnName("CreationTime");

                    b.Property<Guid?>("CreatorId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("CreatorId");

                    b.Property<Guid?>("DeleterId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("DeleterId");

                    b.Property<DateTime?>("DeletionTime")
                        .HasColumnType("datetime2")
                        .HasColumnName("DeletionTime");

                    b.Property<DateTime?>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false)
                        .HasColumnName("IsDeleted");

                    b.Property<DateTime?>("LastModificationTime")
                        .HasColumnType("datetime2")
                        .HasColumnName("LastModificationTime");

                    b.Property<Guid?>("LastModifierId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("LastModifierId");

                    b.Property<int>("OrganizationChartId")
                        .HasColumnType("int");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Status")
                        .HasColumnType("bit");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("OrganizationChartId");

                    b.ToTable("OrganizationPositions", "Flow");
                });

            modelBuilder.Entity("WorkFlowManagement.Domain.WorkFlowManagement.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("datetime2")
                        .HasColumnName("CreationTime");

                    b.Property<Guid?>("CreatorId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("CreatorId");

                    b.Property<Guid?>("DeleterId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("DeleterId");

                    b.Property<DateTime?>("DeletionTime")
                        .HasColumnType("datetime2")
                        .HasColumnName("DeletionTime");

                    b.Property<bool>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false)
                        .HasColumnName("IsDeleted");

                    b.Property<DateTime?>("LastModificationTime")
                        .HasColumnType("datetime2")
                        .HasColumnName("LastModificationTime");

                    b.Property<Guid?>("LastModifierId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("LastModifierId");

                    b.Property<string>("Security")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Status")
                        .HasColumnType("bit");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Roles", "Flow");
                });

            modelBuilder.Entity("WorkFlowManagement.Domain.WorkFlowManagement.RoleOrganizationChart", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("datetime2")
                        .HasColumnName("CreationTime");

                    b.Property<Guid?>("CreatorId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("CreatorId");

                    b.Property<Guid?>("DeleterId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("DeleterId");

                    b.Property<DateTime?>("DeletionTime")
                        .HasColumnType("datetime2")
                        .HasColumnName("DeletionTime");

                    b.Property<bool>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false)
                        .HasColumnName("IsDeleted");

                    b.Property<DateTime?>("LastModificationTime")
                        .HasColumnType("datetime2")
                        .HasColumnName("LastModificationTime");

                    b.Property<Guid?>("LastModifierId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("LastModifierId");

                    b.Property<int>("OrganizationChartId")
                        .HasColumnType("int");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("OrganizationChartId");

                    b.HasIndex("RoleId");

                    b.ToTable("RoleOrganizationChart", "Flow");
                });

            modelBuilder.Entity("WorkFlowManagement.Domain.WorkFlowManagement.Scheme", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("datetime2")
                        .HasColumnName("CreationTime");

                    b.Property<Guid?>("CreatorId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("CreatorId");

                    b.Property<Guid?>("DeleterId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("DeleterId");

                    b.Property<DateTime?>("DeletionTime")
                        .HasColumnType("datetime2")
                        .HasColumnName("DeletionTime");

                    b.Property<bool>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false)
                        .HasColumnName("IsDeleted");

                    b.Property<DateTime?>("LastModificationTime")
                        .HasColumnType("datetime2")
                        .HasColumnName("LastModificationTime");

                    b.Property<Guid?>("LastModifierId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("LastModifierId");

                    b.Property<int>("Priority")
                        .HasColumnType("int");

                    b.Property<bool>("Status")
                        .HasColumnType("bit");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Schemes", "Flow");
                });

            modelBuilder.Entity("WorkFlowManagement.Domain.WorkFlowManagement.Transition", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ActivitySourceId")
                        .HasColumnType("int");

                    b.Property<int>("ActivityTargetId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("datetime2")
                        .HasColumnName("CreationTime");

                    b.Property<Guid?>("CreatorId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("CreatorId");

                    b.Property<Guid?>("DeleterId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("DeleterId");

                    b.Property<DateTime?>("DeletionTime")
                        .HasColumnType("datetime2")
                        .HasColumnName("DeletionTime");

                    b.Property<bool>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false)
                        .HasColumnName("IsDeleted");

                    b.Property<DateTime?>("LastModificationTime")
                        .HasColumnType("datetime2")
                        .HasColumnName("LastModificationTime");

                    b.Property<Guid?>("LastModifierId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("LastModifierId");

                    b.Property<int>("SchemeId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ActivitySourceId");

                    b.HasIndex("ActivityTargetId");

                    b.HasIndex("SchemeId");

                    b.ToTable("Transitions", "Flow");
                });

            modelBuilder.Entity("WorkFlowManagement.Domain.WorkFlowManagement.Activity", b =>
                {
                    b.HasOne("WorkFlowManagement.Domain.WorkFlowManagement.Scheme", "Scheme")
                        .WithMany("Activities")
                        .HasForeignKey("SchemeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Scheme");
                });

            modelBuilder.Entity("WorkFlowManagement.Domain.WorkFlowManagement.OrganizationChart", b =>
                {
                    b.HasOne("WorkFlowManagement.Domain.WorkFlowManagement.OrganizationChart", "Parent")
                        .WithMany("Childrens")
                        .HasForeignKey("ParentId")
                        .OnDelete(DeleteBehavior.ClientCascade);

                    b.Navigation("Parent");
                });

            modelBuilder.Entity("WorkFlowManagement.Domain.WorkFlowManagement.OrganizationPosition", b =>
                {
                    b.HasOne("WorkFlowManagement.Domain.WorkFlowManagement.OrganizationChart", "OrganizationChart")
                        .WithMany("OrganizationPositions")
                        .HasForeignKey("OrganizationChartId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("OrganizationChart");
                });

            modelBuilder.Entity("WorkFlowManagement.Domain.WorkFlowManagement.RoleOrganizationChart", b =>
                {
                    b.HasOne("WorkFlowManagement.Domain.WorkFlowManagement.OrganizationChart", "OrganizationChart")
                        .WithMany("RoleOrganizationCharts")
                        .HasForeignKey("OrganizationChartId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WorkFlowManagement.Domain.WorkFlowManagement.Role", "Role")
                        .WithMany("RoleOrganizationCharts")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("OrganizationChart");

                    b.Navigation("Role");
                });

            modelBuilder.Entity("WorkFlowManagement.Domain.WorkFlowManagement.Transition", b =>
                {
                    b.HasOne("WorkFlowManagement.Domain.WorkFlowManagement.Activity", "ActivitySource")
                        .WithMany("SourceTransitions")
                        .HasForeignKey("ActivitySourceId")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();

                    b.HasOne("WorkFlowManagement.Domain.WorkFlowManagement.Activity", "ActivityTarget")
                        .WithMany("TargetTransitions")
                        .HasForeignKey("ActivityTargetId")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();

                    b.HasOne("WorkFlowManagement.Domain.WorkFlowManagement.Scheme", "Scheme")
                        .WithMany("Transitions")
                        .HasForeignKey("SchemeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ActivitySource");

                    b.Navigation("ActivityTarget");

                    b.Navigation("Scheme");
                });

            modelBuilder.Entity("WorkFlowManagement.Domain.WorkFlowManagement.Activity", b =>
                {
                    b.Navigation("SourceTransitions");

                    b.Navigation("TargetTransitions");
                });

            modelBuilder.Entity("WorkFlowManagement.Domain.WorkFlowManagement.OrganizationChart", b =>
                {
                    b.Navigation("Childrens");

                    b.Navigation("OrganizationPositions");

                    b.Navigation("RoleOrganizationCharts");
                });

            modelBuilder.Entity("WorkFlowManagement.Domain.WorkFlowManagement.Role", b =>
                {
                    b.Navigation("RoleOrganizationCharts");
                });

            modelBuilder.Entity("WorkFlowManagement.Domain.WorkFlowManagement.Scheme", b =>
                {
                    b.Navigation("Activities");

                    b.Navigation("Transitions");
                });
#pragma warning restore 612, 618
        }
    }
}
