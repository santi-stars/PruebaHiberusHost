﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PruebaHiberusHost;

#nullable disable

namespace PruebaHiberusHost.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20231016231835_Inicialv1.2")]
    partial class Inicialv12
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.12")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("PruebaHiberusHost.Entities.ExchangeRate", b =>
                {
                    b.Property<string>("FromCurrency")
                        .HasColumnType("nvarchar(450)")
                        .HasColumnOrder(0)
                        .HasAnnotation("Relational:JsonPropertyName", "fromCurrency");

                    b.Property<string>("ToCurrency")
                        .HasColumnType("nvarchar(450)")
                        .HasColumnOrder(1)
                        .HasAnnotation("Relational:JsonPropertyName", "toCurrency");

                    b.Property<decimal>("Rate")
                        .HasColumnType("decimal(18,2)")
                        .HasAnnotation("Relational:JsonPropertyName", "rate");

                    b.HasKey("FromCurrency", "ToCurrency");

                    b.ToTable("ExchangeRates");
                });

            modelBuilder.Entity("PruebaHiberusHost.Entities.Sum", b =>
                {
                    b.Property<string>("SKU")
                        .HasColumnType("nvarchar(450)");

                    b.Property<decimal>("TotalSum")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("SKU");

                    b.ToTable("Sums");

                    b.HasAnnotation("Relational:JsonPropertyName", "sum");
                });

            modelBuilder.Entity("PruebaHiberusHost.Entities.Transaction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("Relational:JsonPropertyName", "id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(18,2)")
                        .HasAnnotation("Relational:JsonPropertyName", "amount");

                    b.Property<string>("Currency")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasAnnotation("Relational:JsonPropertyName", "currency");

                    b.Property<string>("SKU")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)")
                        .HasAnnotation("Relational:JsonPropertyName", "sku");

                    b.HasKey("Id");

                    b.HasIndex("SKU");

                    b.ToTable("Transactions");
                });

            modelBuilder.Entity("PruebaHiberusHost.Entities.Transaction", b =>
                {
                    b.HasOne("PruebaHiberusHost.Entities.Sum", "Sum")
                        .WithMany("Transactions")
                        .HasForeignKey("SKU")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Sum");
                });

            modelBuilder.Entity("PruebaHiberusHost.Entities.Sum", b =>
                {
                    b.Navigation("Transactions");
                });
#pragma warning restore 612, 618
        }
    }
}
