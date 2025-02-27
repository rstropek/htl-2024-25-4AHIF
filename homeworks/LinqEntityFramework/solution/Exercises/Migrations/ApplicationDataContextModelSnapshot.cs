﻿// <auto-generated />
using System;
using Exercises;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Exercises.Migrations
{
    [DbContext(typeof(ApplicationDataContext))]
    partial class ApplicationDataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "9.0.2");

            modelBuilder.Entity("Exercises.Customer", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("CompanyName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("CountryIsoCode")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int?>("ParentCustomerID")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Region")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.HasIndex("ParentCustomerID");

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("Exercises.OrderHeader", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("CustomerID")
                        .HasColumnType("INTEGER");

                    b.Property<string>("DeliveryCountryIsoCode")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Incoterm")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateOnly>("OrderDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("PaymentTerms")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.HasIndex("CustomerID");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("Exercises.OrderLine", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("OrderID")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ProductCode")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Quantity")
                        .HasColumnType("INTEGER");

                    b.Property<double>("UnitPrice")
                        .HasColumnType("REAL");

                    b.HasKey("ID");

                    b.HasIndex("OrderID");

                    b.ToTable("OrderLines");
                });

            modelBuilder.Entity("Exercises.Customer", b =>
                {
                    b.HasOne("Exercises.Customer", "ParentCustomer")
                        .WithMany()
                        .HasForeignKey("ParentCustomerID");

                    b.Navigation("ParentCustomer");
                });

            modelBuilder.Entity("Exercises.OrderHeader", b =>
                {
                    b.HasOne("Exercises.Customer", "Customer")
                        .WithMany("Orders")
                        .HasForeignKey("CustomerID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("Exercises.OrderLine", b =>
                {
                    b.HasOne("Exercises.OrderHeader", "Order")
                        .WithMany("OrderLines")
                        .HasForeignKey("OrderID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Order");
                });

            modelBuilder.Entity("Exercises.Customer", b =>
                {
                    b.Navigation("Orders");
                });

            modelBuilder.Entity("Exercises.OrderHeader", b =>
                {
                    b.Navigation("OrderLines");
                });
#pragma warning restore 612, 618
        }
    }
}
