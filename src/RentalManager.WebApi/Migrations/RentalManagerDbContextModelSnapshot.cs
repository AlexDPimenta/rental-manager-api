﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using RentalManager.WebApi.Persistence.Context;

#nullable disable

namespace RentalManager.WebApi.Migrations
{
    [DbContext(typeof(RentalManagerDbContext))]
    partial class RentalManagerDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("RentalManager.WebApi.Entities.Driver", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<DateTime>("BirthdayDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Cnpj")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<string>("LicenseCategory")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<string>("LicenseImage")
                        .IsRequired()
                        .HasColumnType("varchar(1500)");

                    b.Property<string>("LicenseNumber")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.HasKey("Id");

                    b.ToTable("Drivers", (string)null);
                });

            modelBuilder.Entity("RentalManager.WebApi.Entities.Lease", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("DriverId")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.Property<int>("DurationInDays")
                        .HasColumnType("integer");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("ExpectedEndDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("MotorCycleId")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.Property<DateTime?>("ReturnData")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("DriverId");

                    b.HasIndex("DurationInDays");

                    b.HasIndex("MotorCycleId");

                    b.ToTable("Leases", (string)null);
                });

            modelBuilder.Entity("RentalManager.WebApi.Entities.MotorCycle", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Model")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Plate")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<int>("Year")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("MotorCycles", (string)null);
                });

            modelBuilder.Entity("RentalManager.WebApi.Entities.Plan", b =>
                {
                    b.Property<int>("DurationInDays")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("DurationInDays"));

                    b.Property<decimal>("CostPerDay")
                        .HasColumnType("decimal(10, 2)");

                    b.HasKey("DurationInDays");

                    b.ToTable("Plans", (string)null);

                    b.HasData(
                        new
                        {
                            DurationInDays = 7,
                            CostPerDay = 30.00m
                        },
                        new
                        {
                            DurationInDays = 15,
                            CostPerDay = 28.00m
                        },
                        new
                        {
                            DurationInDays = 30,
                            CostPerDay = 22.00m
                        },
                        new
                        {
                            DurationInDays = 45,
                            CostPerDay = 20.00m
                        },
                        new
                        {
                            DurationInDays = 50,
                            CostPerDay = 18.00m
                        });
                });

            modelBuilder.Entity("RentalManager.WebApi.Entities.Lease", b =>
                {
                    b.HasOne("RentalManager.WebApi.Entities.Driver", "Driver")
                        .WithMany("Leases")
                        .HasForeignKey("DriverId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RentalManager.WebApi.Entities.Plan", "LeasePlan")
                        .WithMany()
                        .HasForeignKey("DurationInDays")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RentalManager.WebApi.Entities.MotorCycle", "MotorCycle")
                        .WithMany("Leases")
                        .HasForeignKey("MotorCycleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Driver");

                    b.Navigation("LeasePlan");

                    b.Navigation("MotorCycle");
                });

            modelBuilder.Entity("RentalManager.WebApi.Entities.Driver", b =>
                {
                    b.Navigation("Leases");
                });

            modelBuilder.Entity("RentalManager.WebApi.Entities.MotorCycle", b =>
                {
                    b.Navigation("Leases");
                });
#pragma warning restore 612, 618
        }
    }
}
