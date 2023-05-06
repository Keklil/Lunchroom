﻿// <auto-generated />
using System;
using Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NetTopologySuite.Geometries;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Data.Migrations
{
    [DbContext(typeof(RepositoryContext))]
    [Migration("20230506114703_FixOwnsTypes")]
    partial class FixOwnsTypes
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.HasPostgresExtension(modelBuilder, "postgis");
            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Domain.Infrastructure.KitchenVerificationStamp", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<Guid>("CheckerId")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("VerificationTimestamp")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("CheckerId");

                    b.ToTable("KitchenVerificationStamps");
                });

            modelBuilder.Entity("Domain.Models.Dish", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("DishTypeId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("LunchSetId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("MenuId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("DishTypeId");

                    b.HasIndex("LunchSetId");

                    b.HasIndex("MenuId");

                    b.ToTable("Dishes");
                });

            modelBuilder.Entity("Domain.Models.DishType", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("DishTypes");
                });

            modelBuilder.Entity("Domain.Models.Group", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<Guid>("AdminId")
                        .HasColumnType("uuid");

                    b.Property<string>("OrganizationName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Referral")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid?>("SelectedKitchenId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("SettingsId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("AdminId");

                    b.HasIndex("SelectedKitchenId");

                    b.HasIndex("SettingsId");

                    b.ToTable("Groups");
                });

            modelBuilder.Entity("Domain.Models.GroupSettings", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Point>("Location")
                        .IsRequired()
                        .HasColumnType("geometry (point)");

                    b.HasKey("Id");

                    b.ToTable("GroupSettings");
                });

            modelBuilder.Entity("Domain.Models.Kitchen", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Inn")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsApproved")
                        .HasColumnType("boolean");

                    b.Property<string>("OrganizationName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid?>("SettingsId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("SettingsId");

                    b.ToTable("Kitchens");
                });

            modelBuilder.Entity("Domain.Models.KitchenSettings", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<TimeSpan>("LimitingTimeForOrder")
                        .HasColumnType("interval");

                    b.Property<int>("MenuFormat")
                        .HasColumnType("integer");

                    b.Property<int>("MenuUpdatePeriod")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("KitchenSettings");
                });

            modelBuilder.Entity("Domain.Models.LunchSet", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("MenuId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("MenuId");

                    b.ToTable("LunchSets");
                });

            modelBuilder.Entity("Domain.Models.Menu", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsReported")
                        .HasColumnType("boolean");

                    b.HasKey("Id");

                    b.ToTable("Menu");
                });

            modelBuilder.Entity("Domain.Models.Option", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<Guid>("DishId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("MenuId")
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<decimal>("Price")
                        .HasColumnType("numeric");

                    b.HasKey("Id");

                    b.HasIndex("DishId");

                    b.HasIndex("MenuId");

                    b.ToTable("Options");
                });

            modelBuilder.Entity("Domain.Models.Order", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<Guid>("CustomerId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("GroupId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("LunchSetId")
                        .HasColumnType("uuid");

                    b.Property<int>("LunchSetUnits")
                        .HasColumnType("integer");

                    b.Property<Guid>("MenuId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("OrderDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("OrderStatus")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("Payment")
                        .HasColumnType("boolean");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.HasIndex("MenuId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("Domain.Models.OrderOption", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<Guid>("OptionId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("OrderId")
                        .HasColumnType("uuid");

                    b.Property<int>("_optionUnits")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("OrderId");

                    b.ToTable("OrdersOptions");
                });

            modelBuilder.Entity("Domain.Models.User", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsEmailChecked")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Patronymic")
                        .HasColumnType("text");

                    b.Property<string>("Phone")
                        .HasColumnType("text");

                    b.Property<int>("Role")
                        .HasColumnType("integer");

                    b.Property<string>("Surname")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Domain.SecurityModels.EmailValidation", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("EmailValidations");
                });

            modelBuilder.Entity("GroupUser", b =>
                {
                    b.Property<Guid>("GroupsId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("MembersId")
                        .HasColumnType("uuid");

                    b.HasKey("GroupsId", "MembersId");

                    b.HasIndex("MembersId");

                    b.ToTable("GroupUser");
                });

            modelBuilder.Entity("KitchenUser", b =>
                {
                    b.Property<Guid>("KitchenId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("ManagersId")
                        .HasColumnType("uuid");

                    b.HasKey("KitchenId", "ManagersId");

                    b.HasIndex("ManagersId");

                    b.ToTable("KitchenUser");
                });

            modelBuilder.Entity("Domain.Infrastructure.KitchenVerificationStamp", b =>
                {
                    b.HasOne("Domain.Models.User", "Checker")
                        .WithMany()
                        .HasForeignKey("CheckerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Checker");
                });

            modelBuilder.Entity("Domain.Models.Dish", b =>
                {
                    b.HasOne("Domain.Models.DishType", null)
                        .WithMany("Dishes")
                        .HasForeignKey("DishTypeId");

                    b.HasOne("Domain.Models.LunchSet", null)
                        .WithMany("Dishes")
                        .HasForeignKey("LunchSetId");

                    b.HasOne("Domain.Models.Menu", null)
                        .WithMany("Dishes")
                        .HasForeignKey("MenuId");
                });

            modelBuilder.Entity("Domain.Models.Group", b =>
                {
                    b.HasOne("Domain.Models.User", "Admin")
                        .WithMany()
                        .HasForeignKey("AdminId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Domain.Models.Kitchen", null)
                        .WithMany()
                        .HasForeignKey("SelectedKitchenId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.HasOne("Domain.Models.GroupSettings", "Settings")
                        .WithMany()
                        .HasForeignKey("SettingsId");

                    b.OwnsOne("Domain.Models.PaymentInfo", "PaymentInfo", b1 =>
                        {
                            b1.Property<Guid>("GroupId")
                                .HasColumnType("uuid");

                            b1.Property<string>("Description")
                                .HasColumnType("text");

                            b1.Property<string>("Link")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.Property<string>("Qr")
                                .HasColumnType("text");

                            b1.HasKey("GroupId");

                            b1.ToTable("PaymentInfo", (string)null);

                            b1.WithOwner()
                                .HasForeignKey("GroupId");
                        });

                    b.Navigation("Admin");

                    b.Navigation("PaymentInfo");

                    b.Navigation("Settings");
                });

            modelBuilder.Entity("Domain.Models.Kitchen", b =>
                {
                    b.HasOne("Domain.Models.KitchenSettings", "Settings")
                        .WithMany()
                        .HasForeignKey("SettingsId");

                    b.OwnsOne("Domain.Models.Contacts", "Contacts", b1 =>
                        {
                            b1.Property<Guid>("KitchenId")
                                .HasColumnType("uuid");

                            b1.Property<string>("Email")
                                .HasColumnType("text");

                            b1.Property<string>("Phone")
                                .HasColumnType("text");

                            b1.HasKey("KitchenId");

                            b1.ToTable("Contacts", (string)null);

                            b1.WithOwner()
                                .HasForeignKey("KitchenId");
                        });

                    b.Navigation("Contacts")
                        .IsRequired();

                    b.Navigation("Settings");
                });

            modelBuilder.Entity("Domain.Models.KitchenSettings", b =>
                {
                    b.OwnsMany("Domain.Models.ShippingArea", "ShippingAreas", b1 =>
                        {
                            b1.Property<Guid>("KitchenSettingsId")
                                .HasColumnType("uuid");

                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("integer");

                            NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b1.Property<int>("Id"));

                            b1.Property<Polygon>("Polygon")
                                .IsRequired()
                                .HasColumnType("geometry (polygon)");

                            b1.HasKey("KitchenSettingsId", "Id");

                            b1.ToTable("ShippingArea");

                            b1.WithOwner()
                                .HasForeignKey("KitchenSettingsId");
                        });

                    b.Navigation("ShippingAreas");
                });

            modelBuilder.Entity("Domain.Models.LunchSet", b =>
                {
                    b.HasOne("Domain.Models.Menu", null)
                        .WithMany("LunchSets")
                        .HasForeignKey("MenuId");
                });

            modelBuilder.Entity("Domain.Models.Option", b =>
                {
                    b.HasOne("Domain.Models.Dish", "Dish")
                        .WithMany()
                        .HasForeignKey("DishId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Models.Menu", null)
                        .WithMany("Options")
                        .HasForeignKey("MenuId");

                    b.Navigation("Dish");
                });

            modelBuilder.Entity("Domain.Models.Order", b =>
                {
                    b.HasOne("Domain.Models.User", null)
                        .WithMany()
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Domain.Models.Menu", null)
                        .WithMany()
                        .HasForeignKey("MenuId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();
                });

            modelBuilder.Entity("Domain.Models.OrderOption", b =>
                {
                    b.HasOne("Domain.Models.Order", null)
                        .WithMany("Options")
                        .HasForeignKey("OrderId");
                });

            modelBuilder.Entity("GroupUser", b =>
                {
                    b.HasOne("Domain.Models.Group", null)
                        .WithMany()
                        .HasForeignKey("GroupsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Models.User", null)
                        .WithMany()
                        .HasForeignKey("MembersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("KitchenUser", b =>
                {
                    b.HasOne("Domain.Models.Kitchen", null)
                        .WithMany()
                        .HasForeignKey("KitchenId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Models.User", null)
                        .WithMany()
                        .HasForeignKey("ManagersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Domain.Models.DishType", b =>
                {
                    b.Navigation("Dishes");
                });

            modelBuilder.Entity("Domain.Models.LunchSet", b =>
                {
                    b.Navigation("Dishes");
                });

            modelBuilder.Entity("Domain.Models.Menu", b =>
                {
                    b.Navigation("Dishes");

                    b.Navigation("LunchSets");

                    b.Navigation("Options");
                });

            modelBuilder.Entity("Domain.Models.Order", b =>
                {
                    b.Navigation("Options");
                });
#pragma warning restore 612, 618
        }
    }
}
