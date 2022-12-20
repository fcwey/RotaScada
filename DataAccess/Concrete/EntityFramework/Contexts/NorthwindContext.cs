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

        //MalKabul 
        public DbSet<User> Users { get; set; }  
        public DbSet<OperationClaim> OperationClaims { get; set; }
        public DbSet<UserOperationClaim> UserOperationClaims { get; set; }

        public DbSet<Servis> Servisler { get; set; }
        public DbSet<Bolum> Bolumler { get; set; }
        public DbSet<BolumIslem> BolumIslemler { get; set; }
        public DbSet<Malzeme> Malzemeler { get; set; }
        public DbSet<MalzemeGrup> MalzemeGrups { get; set; }
        public DbSet<Islem> Islemler { get; set; }
        public DbSet<AktifIslem> AktifIslemler { get; set; }
        public DbSet<Fiyat> Fiyatlar { get; set; }
        public DbSet<Musteri> Musteriler { get; set; }
        public DbSet<MusteriFiyat> musteriFiyats { get; set; }
    }
}
