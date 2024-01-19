using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;


// creation modele
// Add-Migration InitialCreate
// creation bdd
// Update-Database

// mise à jour modele ( puis update-database )
// Add-Migration AddBlogCreatedTimestamp


namespace StoreCoinMarket
{
    public class CoinDbContext : DbContext
    {
        public DbSet<Coin> Coins { get; set; }
        public DbSet<CoinHistory> CoinHistorys { get; set; }

        public string DbPath { get; }

        private Settings _settings;

        public CoinDbContext()
        {
            DbPath = "";
            _settings = singletonSettings.Instance.GetSettings();
        }

        public CoinDbContext(DbContextOptions<CoinDbContext> options) : base(options)
        {
            DbPath = "";
            _settings = singletonSettings.Instance.GetSettings();
        }

        /*
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer(@"Server=localhost\SQLEXPRESS;Database=crypto_currency;Trusted_Connection=True;TrustServerCertificate=True");
        }
        */

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var str = $"Server={_settings.MariaDbHost};" +
                     $"Database={_settings.MariaDbDatabase};" +
                     $"Uid={_settings.MariaDbUser};" +
                     $"Pwd={_settings.MariaDbDPassword};";
            optionsBuilder.UseMySql(str
                , ServerVersion.AutoDetect(str)
            );
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Coin>()
                .HasKey(_ => _.CoinId);
                    
             

            modelBuilder.Entity<CoinHistory>()
            .HasOne(_ => _.Coin)
            .WithMany(_ => _.History)
            .HasForeignKey(_ => _.CoinId);
        }
    }

    public class Coin
    {
        [Key]
        public int CoinId { get; set; }
        public String Name { get; set; }
        public String Symbol { get; set; }

        //[AllowNull]
        //public String Slug { get; set; }

        public List<CoinHistory> History { get; }

    }

    public class CoinHistory
    {
        public int CoinId { get; set; }
        [Key]
        public DateTime LastUpdate { get; set; }
        public int CirculatingSupply { get; set; }

        public int TotalSupply { get; set; }    

        public int MaxSupply { get; set; }  

        public Double Price {  get; set; }  

        public double Volume24h { get; set; }

        public double VolumeChange24h { get; set; }
        public double PercentChange1h { get; set; }
        public double PercentChange24h { get; set; }

        public double PercentChange7d { get; set; }

        public double PercentChange30d { get; set; }

        public double MarketCap { get; set; }

        public virtual Coin Coin { get; set; }

    }
}
