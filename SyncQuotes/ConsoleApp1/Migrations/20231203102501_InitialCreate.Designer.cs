﻿// <auto-generated />
using System;
using StoreCoinMarket;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace StoreCoinMarket.Migrations
{
    [DbContext(typeof(CoinDbContext))]
    [Migration("20231203102501_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.14")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("ConsoleApp1.Coin", b =>
                {
                    b.Property<int>("CoinId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Symbol")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("CoinId");

                    b.ToTable("Coins");
                });

            modelBuilder.Entity("ConsoleApp1.CoinHistory", b =>
                {
                    b.Property<DateTime>("LastUpdate")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("CirculatingSupply")
                        .HasColumnType("int");

                    b.Property<int>("CoinId")
                        .HasColumnType("int");

                    b.Property<double>("MarketCap")
                        .HasColumnType("double");

                    b.Property<int>("MaxSupply")
                        .HasColumnType("int");

                    b.Property<double>("PercentChange1h")
                        .HasColumnType("double");

                    b.Property<double>("PercentChange24h")
                        .HasColumnType("double");

                    b.Property<double>("PercentChange30d")
                        .HasColumnType("double");

                    b.Property<double>("PercentChange7d")
                        .HasColumnType("double");

                    b.Property<double>("Price")
                        .HasColumnType("double");

                    b.Property<int>("TotalSupply")
                        .HasColumnType("int");

                    b.Property<double>("Volume24h")
                        .HasColumnType("double");

                    b.Property<double>("VolumeChange24h")
                        .HasColumnType("double");

                    b.HasKey("LastUpdate");

                    b.HasIndex("CoinId");

                    b.ToTable("CoinHistorys");
                });

            modelBuilder.Entity("ConsoleApp1.CoinHistory", b =>
                {
                    b.HasOne("ConsoleApp1.Coin", null)
                        .WithMany("History")
                        .HasForeignKey("CoinId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ConsoleApp1.Coin", b =>
                {
                    b.Navigation("History");
                });
#pragma warning restore 612, 618
        }
    }
}
