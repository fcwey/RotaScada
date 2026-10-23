using Core.Entities.Concrete;
using Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using System;

namespace DataAccess.Concrete.EntityFramework.Contexts
{
    public class NorthwindContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=RfMalzemeKabul;Trusted_Connection=true", builder =>
            {
                builder.EnableRetryOnFailure(0, TimeSpan.FromSeconds(10), null);
            });
            base.OnConfiguring(optionsBuilder);
        }

        // Northwind & Deneme Tabloları
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }

        // SCADA Tabloları
        public DbSet<Alarm> Alarms { get; set; }
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<RecipeStep> RecipeSteps { get; set; } //  (20 Adımlı Reçete Detayları)
        public DbSet<Batch> Batches { get; set; }
        public DbSet<ProcessDataLog> ProcessDataLogs { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }

        // Mal Kabul ve Kimlik Tabloları
        public DbSet<User> Users { get; set; }
        public DbSet<OperationClaim> OperationClaims { get; set; }
        public DbSet<UserOperationClaim> UserOperationClaims { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 1. ProcessDataLog Index Yapılandırması (Historian Hızlı Trend Sorguları İçin)
            modelBuilder.Entity<ProcessDataLog>()
                .HasIndex(p => p.Timestamp)
                .HasDatabaseName("IX_ProcessDataLog_Timestamp");

            modelBuilder.Entity<ProcessDataLog>()
                .HasIndex(p => p.BatchId)
                .HasDatabaseName("IX_ProcessDataLog_BatchId");

            // 2. Reçete ve Adım (1 - N) İlişki Yapılandırması
            modelBuilder.Entity<RecipeStep>()
                .HasOne(s => s.Recipe)
                .WithMany(r => r.Steps)
                .HasForeignKey(s => s.RecipeId)
                .OnDelete(DeleteBehavior.Cascade);

            // 3. Reçete İsim Uzunluk Kısıtı
            modelBuilder.Entity<Recipe>()
                .Property(r => r.RecipeName)
                .HasMaxLength(30)
                .IsRequired();
        }
    }
}