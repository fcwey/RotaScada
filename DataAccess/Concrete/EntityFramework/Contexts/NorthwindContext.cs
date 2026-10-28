using Core.Entities.Concrete;
using Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using System;
using System.Text.Json;

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
        // =========================================================================
        // OTOMATİK İŞLEM KAYDI (AUDIT LOG) INTERCEPTOR METOTLARI
        // =========================================================================

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var auditEntries = OnBeforeSaveChanges();
            var result = await base.SaveChangesAsync(cancellationToken);
            await OnAfterSaveChangesAsync(auditEntries);
            return result;
        }

        private List<AuditEntry> OnBeforeSaveChanges()
        {
            ChangeTracker.DetectChanges();
            var auditEntries = new List<AuditEntry>();

            foreach (var entry in ChangeTracker.Entries())
            {
                // AuditLog tablosunun kendisini ve değişmeyen verileri loglamıyoruz
                if (entry.Entity is AuditLog || entry.State == EntityState.Detached || entry.State == EntityState.Unchanged)
                    continue;

                var auditEntry = new AuditEntry
                {
                    TableName = entry.Entity.GetType().Name,
                    ActionType = entry.State.ToString()
                };

                foreach (var property in entry.Properties)
                {
                    if (property.IsTemporary) continue;

                    string propertyName = property.Metadata.Name;
                    if (property.Metadata.IsPrimaryKey())
                    {
                        auditEntry.KeyValues[propertyName] = property.CurrentValue;
                        continue;
                    }

                    switch (entry.State)
                    {
                        case EntityState.Added:
                            auditEntry.NewValues[propertyName] = property.CurrentValue;
                            break;

                        case EntityState.Deleted:
                            auditEntry.OldValues[propertyName] = property.OriginalValue;
                            break;

                        case EntityState.Modified:
                            if (property.IsModified)
                            {
                                auditEntry.OldValues[propertyName] = property.OriginalValue;
                                auditEntry.NewValues[propertyName] = property.CurrentValue;
                            }
                            break;
                    }
                }
                auditEntries.Add(auditEntry);
            }
            return auditEntries;
        }

        private async Task OnAfterSaveChangesAsync(List<AuditEntry> auditEntries)
        {
            if (auditEntries == null || auditEntries.Count == 0) return;

            foreach (var auditEntry in auditEntries)
            {
                AuditLogs.Add(new AuditLog
                {
                    ActionType = auditEntry.ActionType,
                    ModuleName = auditEntry.TableName,
                    Description = $"{auditEntry.TableName} tablosunda {auditEntry.ActionType} işlemi yapıldı.",
                    OldValues = auditEntry.OldValues.Count == 0 ? null : JsonSerializer.Serialize(auditEntry.OldValues),
                    NewValues = auditEntry.NewValues.Count == 0 ? null : JsonSerializer.Serialize(auditEntry.NewValues),
                    Timestamp = DateTime.Now,
                    IsSuccess = true
                });
            }
            await base.SaveChangesAsync();
        }
    }
    // Audit İşlemi İçin Yardımcı Veri Taşıyıcı Sınıf
    internal class AuditEntry
    {
        public string TableName { get; set; } = string.Empty;
        public string ActionType { get; set; } = string.Empty;
        public Dictionary<string, object?> KeyValues { get; } = new();
        public Dictionary<string, object?> OldValues { get; } = new();
        public Dictionary<string, object?> NewValues { get; } = new();
    }
}