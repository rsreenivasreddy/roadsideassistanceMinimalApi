﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RoadsideAssistance.Api.Data;

#nullable disable

namespace RoadsideAssistance.Api.data.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20230524034201_init")]
    partial class init
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("RoadsideAssistance.Api.Data.DomianEntities.Assistant", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CurrentGeoLocationId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<bool>("IsReserved")
                        .IsConcurrencyToken()
                        .HasColumnType("bit");

                    b.HasKey("Id")
                        .HasName("PK_Assistant_Id");

                    b.HasIndex("CurrentGeoLocationId");

                    b.ToTable("Assistants");
                });

            modelBuilder.Entity("RoadsideAssistance.Api.Data.DomianEntities.Customer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("VehicleMakeModel")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("VehicleVINNumber")
                        .IsRequired()
                        .HasMaxLength(17)
                        .HasColumnType("nvarchar(17)");

                    b.HasKey("Id")
                        .HasName("PK_Customer_Id");

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("RoadsideAssistance.Api.Data.DomianEntities.CustomerAssistant", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AssistantId")
                        .HasColumnType("int");

                    b.Property<int>("CustomerId")
                        .HasColumnType("int");

                    b.Property<int>("GeoLocationId")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset?>("ServiceCompleteDate")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateTimeOffset>("ServiceStartDate")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id")
                        .HasName("PK_CustomerAssistant_Id");

                    b.HasIndex("AssistantId");

                    b.HasIndex("CustomerId");

                    b.HasIndex("GeoLocationId");

                    b.ToTable("CustomerAssistants");
                });

            modelBuilder.Entity("RoadsideAssistance.Api.Data.DomianEntities.GeoLocation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.Property<double>("Latitude")
                        .HasColumnType("float");

                    b.Property<double>("Longitude")
                        .HasColumnType("float");

                    b.HasKey("Id")
                        .HasName("PK_GeoLocation_Id");

                    b.HasIndex("Longitude", "Latitude")
                        .IsUnique()
                        .HasDatabaseName("UC_GeoLocation_Longitude_Latitude");

                    b.ToTable("GeoLocations");
                });

            modelBuilder.Entity("RoadsideAssistance.Api.Data.DomianEntities.Assistant", b =>
                {
                    b.HasOne("RoadsideAssistance.Api.Data.DomianEntities.GeoLocation", "GeoLocation")
                        .WithMany("Assistants")
                        .HasForeignKey("CurrentGeoLocationId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired()
                        .HasConstraintName("FK_Assistant_GeoLocation_CurrentGeoLocationId");

                    b.Navigation("GeoLocation");
                });

            modelBuilder.Entity("RoadsideAssistance.Api.Data.DomianEntities.CustomerAssistant", b =>
                {
                    b.HasOne("RoadsideAssistance.Api.Data.DomianEntities.Assistant", "Assistant")
                        .WithMany("CustomerAssistants")
                        .HasForeignKey("AssistantId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired()
                        .HasConstraintName("FK_CustomerAssistant_Assistant_AssistantId");

                    b.HasOne("RoadsideAssistance.Api.Data.DomianEntities.Customer", "Customer")
                        .WithMany("CustomerAssistants")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired()
                        .HasConstraintName("FK_CustomerAssistant_Customer_CustomerId");

                    b.HasOne("RoadsideAssistance.Api.Data.DomianEntities.GeoLocation", "GeoLocation")
                        .WithMany("CustomerAssistants")
                        .HasForeignKey("GeoLocationId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired()
                        .HasConstraintName("FK_CustomerAssistant_GeoLocation_GeoLocationId");

                    b.Navigation("Assistant");

                    b.Navigation("Customer");

                    b.Navigation("GeoLocation");
                });

            modelBuilder.Entity("RoadsideAssistance.Api.Data.DomianEntities.Assistant", b =>
                {
                    b.Navigation("CustomerAssistants");
                });

            modelBuilder.Entity("RoadsideAssistance.Api.Data.DomianEntities.Customer", b =>
                {
                    b.Navigation("CustomerAssistants");
                });

            modelBuilder.Entity("RoadsideAssistance.Api.Data.DomianEntities.GeoLocation", b =>
                {
                    b.Navigation("Assistants");

                    b.Navigation("CustomerAssistants");
                });
#pragma warning restore 612, 618
        }
    }
}
