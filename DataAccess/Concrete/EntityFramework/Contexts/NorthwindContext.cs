using Core.Entities.Concrete;
using Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete.EntityFramework.Contexts
{
    public class NorthwindContext : DbContext
    {

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=Northwind;Trusted_Connection=true");
          //  Data Source = (localdb)\MSSQLLocalDB; Initial Catalog = RfMalzemeKabul; Integrated Security = True; Connect Timeout = 30; Encrypt = False; TrustServerCertificate = False; ApplicationIntent = ReadWrite; MultiSubnetFailover = False
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=RfMalzemeKabul;Trusted_Connection=true", builder =>
            {
                builder.EnableRetryOnFailure(0, TimeSpan.FromSeconds(10), null);
            });
            base.OnConfiguring(optionsBuilder);
        }
       // Nortwind Deneme
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }



       // SCADA Tabloları
        public DbSet<Alarm> Alarms { get; set; }
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<Batch> Batches { get; set; }
        public DbSet<ProcessDataLog> ProcessDataLogs { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }






        //MalKabul 
        public DbSet<User> Users { get; set; }  
        public DbSet<OperationClaim> OperationClaims { get; set; }
        public DbSet<UserOperationClaim> UserOperationClaims { get; set; }




        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ProcessDataLog Index Yapılandırması (Historian Hızlı Trend Sorguları İçin)
            modelBuilder.Entity<ProcessDataLog>()
                .HasIndex(p => p.Timestamp)
                .HasDatabaseName("IX_ProcessDataLog_Timestamp");

            modelBuilder.Entity<ProcessDataLog>()
                .HasIndex(p => p.BatchId)
                .HasDatabaseName("IX_ProcessDataLog_BatchId");

            // Precision ve Veri Tipleri
            modelBuilder.Entity<Recipe>()
                .Property(r => r.TargetTemperature)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Recipe>()
                .Property(r => r.HeatingRampRate)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Recipe>()
                .Property(r => r.GasFlowSetPoint)
                .HasPrecision(18, 2);
        }
    }
}
